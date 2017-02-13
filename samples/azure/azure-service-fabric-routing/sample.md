---
title: Azure Service Fabric Routing
reviewed: 2016-10-10
component: ASF
related:
- nservicebus/azure-service-bus
---


## Prerequisites

1. Azure Service Fabric [dev cluster](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-get-started) running.
1. [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/storage-use-emulator) running.
 

## Scenario

Voting system recording casted votes by candidate and zip code.
The sample will produce votes and record the processing intent int a storage table.
For each message associated with a casted vote, the messages should be routed to the partitions associated with voted candidate / voter zip code. Processing should be taking places on the partition strictly associated with the vote data.

Example:

- Vote for John; vote casted in 88701
- Vote for Abby; vote casted in 36458
- Vote for Abby; vote casted in 92555  
- Vote for John; vote casted in 12789
- Vote for Abby; vote casted in 20567
// some sort of distribution

At the end of the execution, the following results should be expected:

table: candidates
candidate | partition 
John          JONH
Abby          ABBY
Abby          ABBY
John          JONH
Abby          ABBY

(John: 2, Abby: 3)

table: voters_location
zip code | partition
92555       99000        
36458       66000
88701       99000
12789       33000
20567       33000


mapping for named service:
key = msg.Candidate

mapping for range service:
key = 00000..33000 => 33000
      34000..66000 => 66000
      67000..99000 => 99000


## Routing

The conventional routing with NSB cannot be used as-is with ASF [statefull/reliable services](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-concepts-partitioning).


### Partitioning (logical)

explain how business partitioning is done 


### Data Mapping (technical / code related)

mapping to partition keys (named and ranged)
