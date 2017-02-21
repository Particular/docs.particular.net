---
title: Azure Service Fabric Partition Aware Routing
reviewed: 2017-02-14
component: Core
related:
- nservicebus/azure-service-bus
---


## Prerequisites

1. Azure Service Fabric [dev cluster](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-get-started) running configured with 5 nodes.
2. An Azure Service Bus namespace that can be used for communication between the instances.
3. A machine level environment variable called "AzureServiceBus.ConnectionString" that contains the manage connectionstring to the Azure Service Bus namespace.

Note: Make sure the environment variable "AzureServiceBus.ConnectionString" is defined at the machine level. Because a Service Fabric cluster runs on the Network Service account, it has no access to user level variables.

## Scenario

The scenario used in this sample covers a voting system. In this voting system the casted votes are counted by candidate. The casted votes are `sent` from the voter client to the endpoint responsible for counting candidate votes.

Next to this the system also counts the total number of votes casted in each zip code. In order to achieve this the candidate voting endpoint issues a `request` to the zip code counting endpoint to track the zip code. The zip code counting endpoint will send a `reply` back with the intermediary results.

When election is closed, the candidate vote counting endpoint will `publish` the results per candidate and report them on the Service Fabric diagnostics infrastructure.

After the counting time expires, using a `timeout`, the zip code counting endpoint `sends a local command` to report the statistics per zip code.

For sake of simplicity, there are only 2 candidates in the election, called "John" and "Abby". Zip codes are always assumed to be valid integers with a length up to 5 digits in the range (0 to 99000).

### trade offs and known limitations

The scenario has been set up to show the different kinds of communication that can occur in a partitioned solution: `send`, `send local`, `publish/subscribe`, `request/reply`, `timeout`.

The downside of the focus on the communication patterns is that the saga design is less then ideal for a real voting system. There will be quite some contention on the saga data, which may result in concurrency exceptions and a few retries impacting performance of the system.

## Solution structure

The solution contains the following projects:

 * Contracts: This project contains message definitions that are shared between the projects in the solution.
 * Shared: Contains the receiver side distribution code as well as the sender side distribution code. 
 * CandidateVoteCount: This Service Fabric service contains the logic to count the votes by candidate while the votes come in. It also sends these votes to the `ZipCodeVoteCount` endpoint for tracking by zip code, it will report the intermediate results as well as the full results when the election is closed.
 * ZipCodeVoteCount: This Service Fabric service contains the logic to count the votes by zip code in the background. It will report the results when the allowed counting period is over.
 * ServiceFabricRouting: This is the Service Fabric deployment project, it describes how the Service Fabric application and service types will be configured.
 * Voter: This is a console application that simulates casting of votes. It is hosted outside of the Service Fabric cluster.

## Cluster partitioning

The CandidateVoteCount is a statefull service that uses a `NamedPartition` partitioning scheme. Each candidate has it's own partition, so there is one called "John" and another called "Abby". 

The ZipCodeVoteCount is a statefull service that uses a `UniformInt64Partition` partitioning scheme with the `PartitionCount` set to 3, the `LowKey` set to 0 and the `HighKey` set to 99000. This configuration ensure that the partition is split into 3 well known ranges (0 -> 32999), (33000 -> 65999) , (66000 -> 98999).

## Routing

The default NServiceBus routing approach cannot be used as-is with Service Fabric [statefull services](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-concepts-partitioning). Statefull services assume business data partitioning and as a result a message must be delivered to the instance that manages that specific partition. E.g. for each `PlaceVote` message associated with a casted vote, the message should be routed to the partition associated with voted candidate "John" or "Abby". Similar, to count the votes per zip code the sender needs to ensure that the `TrackZipCode` message ends up on the partition that is responsible for the range in which the zip code belongs.

Example:

- Vote for John; casted in 88701; should result in a `PlaceVote` message routed to named partition "John", followed by a `TrackZipCode` message routed to range partition (66000 -> 98999).
- Vote for Abby; casted in 36458; should result in a `PlaceVote` message routed to named partition "Abby", followed by a `TrackZipCode` message routed to range partition (33000 -> 65999).
- Vote for John; casted in 12789; should result in a `PlaceVote` message routed to named partition "John", followed by a `TrackZipCode` message routed to range partition (0 -> 32999).

and so on.

This behavior can be achieved by combinding NServiceBus' built-in sender side distribution feature and a few modiciations to the processing pipeline to make it partition aware.

The remainder of this document will focus on the different techniques that can be used to configure these distribution strategies, either manually or automatically, to achieve full partition aware routing. 

## Receiver Side Distribution

A partitioned endpoint can be configured to check that an incoming message should be processed locally. If it is not the case, the message is forwarded to a correct remote partition.

Partition validation is performed at the level of message headers and message body.

### Header inspection

Every incoming message has its `partition-key` header value inspected by `DistributeMessagesBasedOnHeader` behavior. If the value specified in the header is equal to the receiver's parition, then message processing continues. Otherwise, the message is forwarded to the remote partition specified by the header value. If the partition key is wrongly assigned - the specified partition does not exist, the message is moved to the error queue.

If the `partition-key` header does not exist, the pipeline execution continues moving the message to the *Message body inspection* step.

NOTE: `PartitionMappingFailedException` is configured as an unrecoverable exception. Whenever such an exception is raised the message that triggered the exception will be moved to the configured error queue. For more information refer to unrecoverable exceptions documentation page.

### Message body inspection

If the value of `partition-key` can't be extracted from a header value, it's determined on the basis of the message body. `DistributeMessagesBasedOnPayload` behavior determines the partition value using the mapping function provided by a user via configuration API. The calculated value is added as the `partition-key` header.

The forwarding/processing decision is made in the same way as in *Header inspection* step.

### Control message forwarding

When an endpoint instance receives a control message representing [either Subscribe or Unsubscribe intent](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-messageintent), the message is forwarded it to all other partitions.

### Configuration

To enable receiver side distribution in a specific endpoint two arguments are provided:
- endpoint discriminators that are based on Service Fabric partitions 
- mapping function that maps an incoming message of any type to a partition key value

The configuration is applied by calling an extension method on `EndpointConfiguration`:

snippet: ConfigureReceiverSideDistribution-CandidateVoteCount

In many low-throughput scenarios Receiver Side Distribution might be enough to get started. If higher throughput needs to be achieved it might be desired to route directly to the partition that is the ulimate destination of a command. In those cases the below described Sender Side Distribution can be used in combination with Receiver Side Distribution.

## Sender Side Distribution

Receiver Side Distribution addresses forwarding messages that arrive to an endpoint instance that is different from the destined one. Forwarding them introduces some overhead though. To remove the overhead on the receiver side; Sender Side Distribution can be used to distribute messages to the correct endpoint instance based on Service Fabric partitioning information.

Sender Side Distribution can be applied to endpoints hosted inside Service Fabric by using the available partition information on the stateful context or for endpoints outside the cluster that need to send messages into endpoints hosted inside the cluster. Endpoints that are hosted outside the cluster don't need access to Service Fabric specific APIs. The endpoints only need to know how the partition keys influence the instance specific mapping.

The Sender Side Distribution works the following way:
1. A mapping function is applied when dispatching messages. This mapping function is intended to select an partition key based on business criteria. In this example it's either the candidate name or the zip code of the voter, depending on the destination endpoint.
2. The result of this mapping, a selected partition key, is then added as a `partition-key` header to the sent message. This ensures that its value doesn't have to be calculated on the receiver side again and no receiver side distribution will occur. 
3. As the correct destination instance is now know, given there can only be one master endpoint per Service Fabric partition, the message can be sent to the instance specific queue directly.

### Partition aware distribution strategy

The Sender Side Distribution feature plugs a `PartitionAwareDistributionStrategy` into the outgoing pipeline, which is responsible for selecting a destination queue for each message sent to a specific endpoint. When a destination is to be selected for a given message, the mapping function is applied to obtain a partition key value. The message has it's `partition-key` header value set and the instance specific queue is selected as a destination address.

### Local sends

For local sends, the system already knows the `partition-key` header value and destination up front, so in this case no mapping needs to be specified.

### Reply 

When replying, an endpoint routes the reply message to the endpoint that initiated the conversation. It's the responsibility of the requestor to properly set the reply to header before sending out the request. For a partitioned endpoint this implies that it sets the reply to header to its instance specific queue instead of the shared queue. This functionality is covered by the `HardcodeReplyToAddressToLogicalAddress` behavior. This code is logically acting as part of sender side distribution, but as it only needs to be used for partitioned endpoints it's actually the receiver side distribution that sets it up.

### Configuration

Sender Side Distribution is configured by teaching the system which partitions exist for a given endpoint and make sure that each of these partitions is uniquely addressable on the receiver side.

snippet: ConfigureLocalPartitions-CandidateVoteCount

In order to send to specific partitions on the destination side, it is required to specificy the mapping function between a business property on the message to a partition key for each sent message type.

snippet: ConfigureSenderSideRouting-CandidateVoteCount