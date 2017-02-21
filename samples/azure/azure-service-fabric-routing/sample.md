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

## Scenario

The scenario used in this sample covers a voting system. In this voting system the casted votes are counted by candidate. The casted votes are `sent` from the voter client to the endpoint responsible for counting candidate votes.

Next to this the system also counts the total number of votes casted in each zip code. In order to achieve this the candidate voting endpoint issues a `request` to the zip code counting endpoint to track the zip code. The zip code counting endpoint will send a `reply` back with the intermediary results.

When election is closed, the candidate vote counting endpoint will `publish` the results per candidate and report them on the Service Fabric diagnostics infrastructure.

After the counting time expires, using a `timeout`, the zip code counting endpoint `sends a local command` to report the statistics per zip code.

For sake of simplicity, there are only 2 candidates in the election, called "John" and "Abby". Zip codes are always assumed to be valid integers with a length up to 5 digits in the range (00000 to 99000).

### trade offs and known limitations

The scenario has been set up to show the different kinds of communication that can occur in a partitioned solution: `send`, `send local`, `publish/subscribe`, `request\reply`, `timeout`.

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

The ZipCodeVoteCount is a statefull service that uses a `UniformInt64Partition` partitioning scheme with the `PartitionCount` set to 3, the `LowKey` set to 0 and the `HighKey` set to 99000. This configuration ensure that the partition is split into 3 well known ranges (00000 -> 32999), (33000 -> 65999) , (66000 -> 98999).

## Routing

The conventional NServiceBus routing approach cannot be used as-is with Service Fabric [statefull services](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-concepts-partitioning) as processing should be taking place on the partition strictly associated with the partition data. For each `PlaceVote` message associated with a casted vote, the message should be routed to the partition associated with voted candidate "John" or "Abby". Similar, to count the votes per zip code the sender needs to ensure that the `TrackZipCode` message ends up on the partition that is responsible for the range in which the zip code belongs.

Example:

- Vote for John; casted in 88701; should result in a `PlaceVote` message routed to partition "John", followed by a `TrackZipCode` message routed to range (66000 -> 98999).
- Vote for Abby; casted in 36458; should result in a `PlaceVote` message routed to partition "Abby", followed by a `TrackZipCode` message routed to range (33000 -> 65999).
- Vote for John; casted in 12789; should result in a `PlaceVote` message routed to partition "John", followed by a `TrackZipCode` message routed to range (00000 -> 32999).

and so on.

This behavior can be achieved however by using NServiceBus' sender and receiver side distribution features and a few modifications to the NServiceBus processing pipeline to make it partition aware.

The remainder of this document will focus on the different techniques that can be used to configure these distribution strategies, either manually or automatically, to achieve full partition aware routing. 

## Receiver Side Distribution

Every partitioned endpoint is configured to check whether an incoming message should be processed on a partition it runs on. If it is not the case, the message is forwarded to the proper partition. Details of Receiver Side Distribution are described below.

### Header inspection

Every incoming message has its `partition-key` header value inspected by `DistributeMessagesBasedOnHeader` behavior. If the value specified in this header is equal to the receiver's parition, then a regular message processing occurs. Otherwise, the receiver forwards the message to the right partition. If the partition key is wrongly assigned - the specified partition does not exist, the message is moved to the error queue.

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

snippet: ReceiverSideRoutingCandidateVoteCount

## Sender Side Distribution

Receiver Side Distribution addresses forwarding messages that arrive to an endpoint instance that is different from the destined one. Forwarding them introduces some overhead though. To remove the overhead on the receiver side; Sender Side Distribution can be used to distribute messages to the correct endpoint instance based on Service Fabric partitioning information.

The Sender Side Distribution works in a following way. It applies the mapping function when dispatching messages. The result of the mapping, a selected partition, is added as the `partition-key` header. This ensures that its value doesn't have to be calculated on the receiver side again. Additionally, to remove the need of forwarding on the receiver side, the message is sent to the instance specific queue.

### Partition aware distribution strategy

Sender Side Distribution introduces an overload for `DistributionStrategy` called `PartitionAwareDistributionStrategy`. When a destination is selected for a given message, the mapping function is applied to obtain a discriminator value. The message has `partition-key` value set and is routed to the instance specific queue.

### Reply 

When replying, an endpoint routes the reply message to the endpoint that initiated conversation. For a partitioned endpoint, it could require forwarding messages between endpoint instances. This is required to properly handle messages that need access to a paritioned state. Therefore, is much easier to ensure that all the `ReplyTo` headers will be instance specific. For sagas, it means that replies will arrive right to the correct partition. The rest of messages will be routed in the same way. Handling them does not require access to the parititoned state, so they can be processed on any instance.

### Configuration

The Sender Side Distribution is configured by augmenting the distributing messages for a specific endpoint. The first step of configuring it is setting a new PartitionAwareDistributionStrategy aware of partitions of the destined endpoint. The second, to register this endpoints' instances. This ensures, that from now on, all messages sent to this endpoint will be routed in the partition-aware manner.

snippet: ConfigureSenderSideDistributionCandidateVoteCount