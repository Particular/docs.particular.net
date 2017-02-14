---
title: Azure Service Fabric Partition Aware Routing
reviewed: 2017-02-14
component: Core
related:
- nservicebus/azure-service-bus
---


## Prerequisites

1. Azure Service Fabric [dev cluster](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-get-started) running.
2. An Azure Service Bus namespace that can be used for configuration.
3. A machine level environment variable called "AzureServiceBus.ConnectionString" that contains the manage connectionstring to the Azure Service Bus namespace.

## Scenario

The scenario used in this sample covers a voting system. In this voting system the casted votes are counted by candidate. Next to this the system also counts the total number of votes casted in each zip code. When voting is over, the system will publish the results and report them on the Service Fabric diagnostics infrastructure.

For sake of simplicity, there are only 2 candidates in the election, called "John" and "Abby". Zip codes are always assumed to be valid integers with a length up to 5 digits in the range (00000 to 99000).


## Solution structure

The solution contains the following projects:

 * Contracts: This project contains class that are shared by the other projects. These classes include message definitions as well as partition aware routing definitions that will be plugged into the NServiceBus pipeline.
 * CandidateVoteCount: This Service Fabric service contains the logic to count the votes by candidate while the votes come in. It also sends these votes to the `ZipCodeVoteCount` endpoint for tracking by zipcode and it will report the results when the voting period is over.
 * ZipCodeVoteCount: This Service Fabric service contains the logic to count the votes by zip code in the background. It will report the results when the allowed counting period is over.
 * ServiceFabricRouting: This is the Service Fabric deployment project, it describes how the Service Fabric application and service types will be configured.
 * Voter: This is a console application that allows to cast votes. It is hosted outside of the Service Fabric cluster.

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


### Configuring Sender Side Distribution

mapping to partition keys (named and ranged)



### Setting up Sender Side Distribution

mapping to partition keys (named and ranged)







