---
title: Azure Service Fabric Partition Aware Routing
reviewed: 2017-02-14
component: Core
related:
- nservicebus/azure-service-bus
---


## Prerequisites

1. Azure Service Fabric [dev cluster](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-get-started) running and configured with 5 nodes.
2. An Azure Service Bus namespace that can be used for communication between the instances.
3. A system environment variable named "AzureServiceBus.ConnectionString" set to connection string to the Azure Service Bus namespace with Manage rights.

Note: Service Fabric cluster runs under Network Service account and only reads system environment variables. Make sure the environment variable "AzureServiceBus.ConnectionString" is defined as a system environment variable and not user-scoped.

## Scenario

The scenario used in this sample covers a voting system. In this voting system the casted votes are counted by candidate. The casted votes are `published` from the voter client to the endpoint responsible for counting candidate votes.

Next to this the system also counts the total number of votes casted in each zip code. In order to achieve this the candidate voting endpoint issues a `request` to the zip code counting endpoint to track the zip code. The zip code counting endpoint will `reply` back with the intermediary results.

When election is closed, the candidate vote counting endpoint will `publish` the results per candidate and report them using Service Fabric diagnostics infrastructure (ETW Event Viewer windows).

After the counting time expires, using a `timeout`, the zip code counting endpoint `sends a local command` to report the statistics per zip code.

For sake of simplicity, there are only 2 candidates in the election, called "John" and "Abby". Zip codes are always assumed to be valid integers with a length up to 5 digits in the range of 0 to 99000.

### Trade offs and known limitations

The scenario has been set up to show the different kinds of communication patterns that can occur in a partitioned solution: `send`, `send local`, `publish/subscribe`, `request/reply`, `timeout`.

The downside of the focus on the communication patterns is that the saga design is less then ideal for a real voting system. There will be quite some contention on the saga data, which may result in concurrency exceptions and a few retries impacting performance of the system.

## Solution structure

The solution contains the following projects:

 * Contracts: Contains message definitions that are shared among projects.
 * Shared: Contains the receiver side distribution and the sender side distribution code.
 * Voter: This is a console application that simulates casting of votes. It is hosted outside of the Service Fabric cluster. 
 * CandidateVoteCount: Service Fabric service with the logic to count votes by candidate while the votes come in. It also instructs the `ZipCodeVoteCount` endpoint to track votes by zip code. It will report the intermediate results as well as the final results when the election is closed.
 * ZipCodeVoteCount: Service Fabric service with the logic to count the votes by zip code in the background. It will report the results when the allowed counting period is over.
 * ServiceFabricRouting: [Service Fabric application](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-application-model) containing description of services it will run when deployed into Service Fabric cluster.
 
## Cluster partitioning

The `CandidateVoteCount` is a [statefull service](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-introduction) that uses a `NamedPartition` [partitioning scheme](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-concepts-partitioning). Each candidate has its own partition, so there is one called "John" and another called "Abby". 

The `ZipCodeVoteCount` is a statefull service that uses a `UniformInt64Partition` partitioning scheme with the `PartitionCount` set to 3, the `LowKey` set to 0 and the `HighKey` set to 99000. This configuration ensure that the partition is split into 3 well known ranges (0 -> 32999), (33000 -> 65999) , (66000 -> 98999).

## Routing

The default NServiceBus routing approach cannot be used as-is with Service Fabric statefull services. Statefull services assume business data partitioning. A message must be delivered to a replica (instance) of a statefull that can handle message data. E.g. for each `PlaceVote` message associated with a casted vote, the message should be routed to the partition associated with voted candidate, "John" or "Abby". Similar to that, to count the votes per zip code the `TrackZipCode` message needs to end up on the partition that is responsible for the range to which the zip code belongs.

Example:

- Vote for John; casted in 88701; should result in a `PlaceVote` message routed to named partition "John", followed by a `TrackZipCode` message routed to range partition (66000 -> 98999).
- Vote for Abby; casted in 36458; should result in a `PlaceVote` message routed to named partition "Abby", followed by a `TrackZipCode` message routed to range partition (33000 -> 65999).
- Vote for John; casted in 12789; should result in a `PlaceVote` message routed to named partition "John", followed by a `TrackZipCode` message routed to range partition (0 -> 32999).

and so on.

Partition aware behavior is achieved by combining NServiceBus built-in sender side distribution feature and a few modifications to the processing pipeline.

The remainder of this document will focus on the different techniques that can be used to configure these distribution strategies, either manually or automatically, to achieve full partition aware routing. 

## Partitioned Endpoint Configuration

Partitioned endpoints require specifying partitioning configuration by calling an extension method on `EndpointConfiguration`:

snippet: ConfigureLocalPartitions-ZipCodeVoteCount

This ensures that the endpoint will be uniquely addressable and that both `SendLocal` and `ReplyTo` operations result in messages sent to the proper originating partition.  

### Local sends

All local sends are handled by `PartitionAwareDistributionStrategy`. It sets the `partition-key` header value to local partition and routes the message to instance specific queue.

### Replies 

When replying, an endpoint routes the reply message to the endpoint that initiated the conversation. It's the responsibility of the requestor to properly set the reply to header before sending out the request. For a partitioned endpoint this implies that it sets the reply to header to its instance specific queue instead of the shared queue. This functionality is covered by the `HardcodeReplyToAddressToLogicalAddress` behavior.

## Receiver Side Distribution

A partitioned endpoint can be configured to check that an incoming message should be processed locally. If it is not the case, the message is forwarded to the correct partition.

Partition validation is performed by inspecting message headers and message body.

### Message headers inspection

Every incoming message has its `partition-key` header value inspected by `DistributeMessagesBasedOnHeader` behavior. If the value specified in the header is equal to the receiver`s partition, then message processing takes place. Otherwise, the message is forwarded to the appropriate partition specified by the header value. If the partition key is wrongly assigned - the specified partition does not exist, the message is moved to the error queue.

If the `partition-key` header does not exist, the pipeline execution continues moving the message to the *Message body inspection* step.

NOTE: `PartitionMappingFailedException` is configured as an unrecoverable exception. Whenever such an exception is raised the message that triggered the exception will be moved to the configured error queue. For more information refer to [unrecoverable exceptions documentation](/nservicebus/recoverability/custom-recoverability-policy.md).

### Message body inspection

If the value of `partition-key` can't be extracted from a header value, it's determined on the basis of the message body. `DistributeMessagesBasedOnPayload` behavior determines the partition value using the mapping function provided by a user via configuration API. The calculated value is added as the `partition-key` header.

The forwarding/processing decision is made in the same way as in *Header inspection* step.

### Control message forwarding

When an endpoint instance receives a control message representing either [Subscribe or Unsubscribe intent](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-messageintent), the message is forwarded to all other partitions.

### Configuration

To enable Receiver Side Distribution for a specific endpoint, two arguments need to be provided:
- Endpoint discriminators that are based on Service Fabric partition keys 
- Mapping function per incoming message type that maps incoming messages to a partition key

To enable Server Side Distribution, the following code is involved:

snippet: ConfigureReceiverSideDistribution-CandidateVoteCount

In many low-throughput scenarios Receiver Side Distribution might be enough to get started. To achieve a better performance, it would be better to route directly to the partitions from the sender. See Sender Side Distribution below and how it can be used in combination with Receiver Side Distribution.

## Sender Side Distribution

Receiver Side Distribution addresses forwarding messages that arrive to an endpoint instance that is different from the destined one. Forwarding them introduces some overhead though. To remove the overhead on the receiver side Sender Side Distribution can be used to distribute messages to the correct endpoint instances based on Service Fabric partitioning information.

Sender Side Distribution can be applied to endpoints hosted inside Service Fabric by using the partition information of the statefull services. This is suitable for endpoints hosted inside the cluster that need to send messages to other endpoints hosted in the cluster. For the endpoints hosted outside of the cluster access to Service Fabric APIs is not possible. Instead, partitioned destination endpoints need to be registered with message mapping for commands sent out.

The Sender Side Distribution works in the following way:

1. A mapping function is applied when dispatching messages. This mapping function is intended to select a partition key based on business criteria. In this example it's either the candidate name or the zip code of the voter, depending on the destination endpoint.
1. The result of this mapping, a selected partition key, is then added as a `partition-key` header to the sent message. This ensures that its value doesn't have to be calculated on the receiver side again and no receiver side distribution will occur. 
1. With determined destination instance, the message is sent to the instance specific queue directly.

### Partition aware distribution strategy

The Sender Side Distribution feature adds a [custom distribution strategy](/nservicebus/messaging/routing-extensibility.md#routing-apis-message-distribution) `PartitionAwareDistributionStrategy` into the outgoing pipeline, which is responsible for selecting a destination queue for each message sent to a specific endpoint. When a destination is to be selected for a given outgoing message, the mapping function is applied to obtain the partition key value. The message has its `partition-key` header value set and the instance specific queue is selected as a destination address.

### Configuration

Sender Side Distribution is configured by providing partitions information for a given endpoint and ensuring each of these partitions is uniquely addressable on the receiver side.

snippet: ConfigureLocalPartitions-CandidateVoteCount

In order to send to specific destination partitions, it is required to provide the a mapping function that connects a business property on the message to a partition key for each outgoing message type.

snippet: ConfigureSenderSideRouting-CandidateVoteCount

### Message Driven Pub/Sub

The sample can be used with Message Driven Pub/Sub transports such as Azure Storage Queue Transport as well. The sample works out of the box but will heavily rely on Receiver Side Distribution to add the required partition keys to the header. For higher throughput scenarios the sender side distribution code needs to be tweaked in the following way:

1. Add a `PartitionAwareDistributionStrategy` with scope `Publish` in the `SenderSideDistributionExtensions`

snippet: SenderSideDistributionExtensions-MessageDrivenPubSub

2. Make sure `VotePlaced` message has a partition mapping function on the sender side

snippet: ConfigureSenderSideRouting-MessageDrivenPubSub
