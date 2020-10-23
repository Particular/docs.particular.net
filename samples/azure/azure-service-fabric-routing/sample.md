---
title: Service Fabric Partition-Aware Routing
summary: Implementing partition-aware routing for services hosted inside a Service Fabric cluster.
reviewed: 2020-10-24
component: Core
related:
- transports/azure-service-bus
---

This sample demonstrates how the NServiceBus API can be used to implement partition-aware routing for services hosted inside a Service Fabric cluster. It takes advantage of [routing system extensibility points](/nservicebus/messaging/routing-extensibility.md) and [custom pipeline behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md) to support various types of NServiceBus communication patterns. It is assumed that the NServiceBus users are able to define mapping between message type and service partition for each message. It is also assumed that `send local`, `timeout` and `reply` messages are partition-affine i.e. should be processed in the context of originating partition. The sample consists of services hosted inside and outside the Service Fabric and enables proper communication between the two.


## Prerequisites

 1. Strong understanding of Service Fabric [Reliable Services](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-quick-start). 
 1. Service Fabric [development environment setup](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-get-started) with dev cluster configured to **run on 5 nodes** (sample does not run with 1 node).
 1. Have *Service Fabric Tools* component added to load the `.sfproj` project via the *Visual Studio Installer*.
 1. An Azure Service Bus namespace that can be used for communication between the instances.
 1. A **system environment variable** named "AzureServiceBus.ConnectionString" set to the connection string of the Azure Service Bus namespace. The connection string must provide Manage rights.

NOTE: A Service Fabric cluster runs under the Network Service account and only reads **system environment variables**. Make sure the environment variable "AzureServiceBus.ConnectionString" is defined as a system environment variable and is not user-scoped.

NOTE: This sample makes use of Service Fabric's recommended instrumentation technology, [Event Tracing for Windows](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-diagnostics-how-to-monitor-and-diagnose-services-locally) (ETW), to provide sample output from the services running within the Service Fabric cluster. If diagnostic messages from the sample do not output in the Visual Studio Diagnostic Events window, it may be necessary to add `MyCompany-ServiceFabricRouting-ZipCodeVoteCount` and `MyCompany-ServiceFabricRouting-CandidateVoteCount` to the list of known [ETW providers](https://stackoverflow.com/a/35347603/2672802).


## Scenario

The scenario used in this sample covers a voting system. In this voting system the cast votes are counted by candidate. The endpoint responsible for counting candidate votes subscribes to an event published when votes are cast.

Additionally, the system also counts the total number of votes cast in each zip code. In order to achieve this, the candidate voting endpoint issues a `request` to the zip code counting endpoint to track the zip code. The zip code counting endpoint will `reply` back with the intermediate results.

When the election is closed, the candidate vote counting endpoint will `publish` the results per candidate and report them using Service Fabric's diagnostics infrastructure ([ETW Event Viewer windows](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-diagnostics-how-to-monitor-and-diagnose-services-locally#view-service-fabric-system-events-in-visual-studio)).

After the counting time expires, using a `timeout`, the zip code counting endpoint `sends a local command` to report the statistics per zip code.

The sample assumes that:

 * There are only 2 candidates in the election, called "John" and "Abby",
 * Zip codes are integers in the range of 0 to 99000.

This simplifies partition id value calculation. In a real world scenario a hash function could be used to perform mapping from arbitrary input types.

```mermaid
sequenceDiagram

participant v as Voter
participant cv as CandidateVoteCounts
participant zc as ZipCodeCounts
participant cvt as 1min Timeout
participant zct as 1min Timeout

v->> cv: 1. Publish VotePlaced

cv ->> zc: 2. Send
zc -->> cv: 3. Reply

v->> cv: 4. Send ElectionClosed

cv ->>+ cvt: 5. Request Timeout
cvt-->>- cv: 6. Reply
cv ->> cv: 7. Publish

zc ->>+ zct: 8. Request Timeout
zct -->>- zc: 9. Reply
zc ->> zc: 10. Publish
```


### Trade offs and known limitations

The scenario has been set up to show the different types of communication pattern that can occur in a partitioned solution: `send`, `send local`, `publish/subscribe`, `request/reply`, `timeout`.

The downside of the focus on communication patterns is that the saga design is not ideal for a real voting system. The saga is likely to experience contention, which may result in concurrency exceptions and retries impacting system performance.

For logging purposes, a simple static logger exposes specific log statements for the routing part. It is recommended to use a dedicated package to emit ETW logging information.


## Solution structure

The solution contains the following projects:

 * **Contracts**: Contains message definitions that are shared among projects.
 * **Shared**: Contains the receiver-side distribution and the sender-side distribution code.
 * **Voter**: This is a console application that simulates casting of votes. It is hosted outside of the Service Fabric cluster.
 * **CandidateVoteCount**: Service Fabric service with the logic to count votes by candidate while the votes come in. It also instructs the `ZipCodeVoteCount` endpoint to track votes by zip code. It will report the intermediate results as well as the final results when the election is closed.
 * **ZipCodeVoteCount**: Service Fabric service with the logic to count the votes by zip code in the background. It will report the results when the allowed counting period is over.
 * **ServiceFabricRouting**: [Service Fabric application](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-application-model) containing a description of services it will run when deployed into the Service Fabric cluster.


## Cluster partitioning

`CandidateVoteCount` is a [stateful service](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-introduction) that uses a `NamedPartition` [partitioning scheme](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-concepts-partitioning). Each candidate has its own partition, resulting in one called "John" and another called "Abby".

The `ZipCodeVoteCount` is a stateful service that uses a `UniformInt64Partition` partitioning scheme with the `PartitionCount` set to 3, the `LowKey` set to 0 and the `HighKey` set to 99000. This configuration ensures that the partition is split into three well-known ranges (0 -> 32999), (33000 -> 65999), (66000 -> 98999).


## Routing

The default NServiceBus routing approach cannot be used as-is with Service Fabric stateful services. Stateful services assume business data partitioning. A message must be routed to a specific replica (instance) of a stateful service that can handle the message data. E.g. for each `PlaceVote` message associated with a cast vote, the message should be routed to the partition associated with the preferred candidate, "John" or "Abby". Similarly, to count the votes per zip code the `TrackZipCode` message must be routed to the partition that is responsible for the range to which the zip code belongs.

Example:

 * Vote for John; cast in 88701; should result in a `PlaceVote` message routed to the named partition "John", followed by a `TrackZipCode` message routed to the range partition that is assigned 66000 through 98999.
 * Vote for Abby; cast in 36458; should result in a `PlaceVote` message routed to the named partition "Abby", followed by a `TrackZipCode` message routed to the range partition that is assigned 33000 through 65999.
 * Vote for John; cast in 12789; should result in a `PlaceVote` message routed to the named partition "John", followed by a `TrackZipCode` message routed to the range partition that is assigned 0 through 32999.

Partition-aware behavior is achieved by combining sender-side distribution (a built-in NServiceBus feature) with a few modifications to the message processing pipeline.

The remainder of this document will focus on the different techniques that can be used to configure these distribution strategies, either manually or automatically, to achieve full partition-aware routing.


## Partitioned endpoint configuration

Endpoint instances hosted with stateful service replicas must be uniquely addressable by the partition key associated with each replica. Possible keys are defined by the `NamedPartition` and `Int64RangePartition` info obtained from Service Fabric. The unique partition keys are used to route messages to the correct partitions.

Endpoints are configured as _partitioned endpoints_ by calling an extension method on `EndpointConfiguration`:

snippet: ApplyPartitionConfigurationToEndpoint-ZipCodeVoteCount


### Local sends

All local sends are handled by the `PartitionAwareDistributionStrategy`. For each locally sent message, the `partition-key` header value is set to the local partition key and the message is routed to the queue associated with the local partition.


### Replies

When replying, an endpoint routes the reply message to the endpoint that initiated the conversation. The requester is responsible for properly setting the reply-to header before sending out the request. For a partitioned endpoint this implies that it sets the reply-to header to its instance-specific queue instead of the shared queue. This functionality is covered by the `HardcodeReplyToAddressToLogicalAddress` behavior.


## Receiver-side distribution

Receiver-side distribution validates if a partitioned endpoint can process an incoming message or should forward it to the appropriate partition instead. Partition validation is performed by inspecting message headers and the message body.

A partitioned endpoint can be configured to check that an incoming message should be processed locally. If not, the message is forwarded to the correct partition.


### Message header inspection

Every incoming message has its `partition-key` header value inspected by the `DistributeMessagesBasedOnHeader` behavior. If the value specified in the header is matching the receiver's partition key, then the message is processed. Otherwise, the message is forwarded to the appropriate partition specified by the header value. If the partition key value indicates an unknown partition, the message is forwarded to the error queue.

If the `partition-key` header does not exist, the pipeline execution continues to the *Message body inspection* step.


### Message body inspection

If a message's `partition-key` header has not been set, then the message body is used to determine the partition key value. The `DistributeMessagesBasedOnPayload` behavior determines the partition value using the mapping function provided via the configuration API. The mapping function inspects the message data and returns the appropriate partition key value for the message. The inspection logic can raise a `PartitionMappingFailedException` exception if the partition key for the endpoint cannot be determined. For a partition key that is not local, the message is forwarded to the appropriate partition. 

Once the partition key value has been determined, the forwarding/processing decision is made in the same way as in the *Header inspection* step.

NOTE: `PartitionMappingFailedException` is considered an [unrecoverable exception](/nservicebus/recoverability/custom-recoverability-policy.md) and the message will be moved to the error queue immediately.


### Control message forwarding

When an endpoint instance receives a control message representing either the [subscribe or unsubscribe intent](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-messageintent), the message is forwarded to all other partitions.


### Configuration

To enable receiver-side distribution for a specific endpoint, provide:

 * endpoint discriminators that are based on Service Fabric partition keys
 * a mapping function per incoming message type that maps incoming messages to a partition key

Use this code to enable receiver-side distribution:

snippet: ConfigureReceiverSideDistribution-CandidateVoteCount


## Sender-side distribution

Receiver-side distribution addresses forwarding messages that arrive to the incorrect partition. Forwarding received messages introduces some overhead though. To remove the overhead on the receiver side the Sender Side Distribution approach can be used to distribute messages to the correct endpoint instances based on Service Fabric partitioning information.

Sender-side distribution can be applied to endpoints hosted inside Service Fabric by using the partition information of the stateful services. This is suitable for endpoints hosted inside the cluster that need to send messages to other endpoints hosted in the cluster. For endpoints hosted outside of the cluster, access to Service Fabric APIs is not required and partitioning information can simply be provided by the developer.

Sender-side distribution works in the following way:

 1. A mapping function is applied when dispatching messages. This mapping function is intended to select a partition key based on business criteria. In this example it's either the candidate name or the zip code of the voter, depending on the message type and destination endpoint.
 1. The result of this mapping, a partition key, is then added as a `partition-key` header to the outgoing message. This ensures that its value doesn't have to be calculated on the receiver side again and no receiver side distribution will occur.
 1. With determined destination, the message is sent to the instance specific queue directly.


### Partition-aware distribution strategy

The sender-side distribution feature adds a [custom distribution strategy](/transports/msmq/sender-side-distribution.md#mapping-physical-endpoint-instances-message-distribution) `PartitionAwareDistributionStrategy` into the outgoing pipeline, which is responsible for selecting a destination queue for each message sent to a specific endpoint. When a destination is to be selected for a given outgoing message, the mapping function is applied to obtain the partition key value. The message has its `partition-key` header value set and the partition specific queue is selected as a destination address.


### Configuration

Sender-side distribution is configured by providing partition information for a given endpoint and ensuring each of these partitions are uniquely addressable on the sender. In addition, a mapping function is required for each message type which can inspect the message data and determine the correct partition key for the message.

An example of configuring sender-side distribution on an endpoint external to the Service Fabric cluster sending to a partitioned endpoint in the cluster using named partitioning:

snippet: configuresendersiderouting-voter

An example of configuring sender-side distribution on an endpoint within the Service Fabric cluster sending to a partitioned endpoint in the cluster using ranged partitioning:

snippet: ConfigureSenderSideRouting-CandidateVoteCount


### Message-driven publish/subscribe

The sample can be used with message-driven publish/subscribe transports such as Azure Storage Queue transport as well. The sample works out of the box but will heavily rely on receiver-side distribution to add the required partition keys to the header.


## Optimization strategies

| **Scenario**                                                   | **Strategy**                            |
|----------------------------------------------------------------|-----------------------------------------|
|Send to a partitioned endpoint                                  | Partition-aware sender-side distribution|
|Send Local in a partitioned endpoint                            | Partition-Aware sender-side distribution for local endpoint|
|Directing the reply to myself, a partitioned endpoint           | Reply override behavior, header copying behavior on the replier|
|Directing the reply to a different partitioned endpoint         | Extension method on SendOptions|
|Publish a message to a partitioned endpoint using native pub/sub| Broker forwards to the correct key partition based on the header key value<sup>1</sup> |
|Publish a message to a partitioned endpoint using non native pub/sub| Receiver forwards to the correct key partition based on the header key value<sup>2</sup>|
|Request a timeout in a saga                                     | Partition-aware sender-side distribution for local endpoint|

<sup>1</sup> native distribution is not currently supported 

<sup>2</sup> message-driven pub/sub distribution on the publisher side is not currently supported 
