---
title: Azure Storage Queues Transport
summary: Using Azure Storage Queues as a message transport
component: ASQ
related:
 - samples/azure/storage-queues
 - samples/azure/native-integration-asq
reviewed: 2020-11-12
redirects:
 - nservicebus/azure-storage-queues
---

Azure Storage Queues is a service hosted on the Azure platform, used for storing large numbers of messages. The messages can be accessed from anywhere in the world via authenticated calls using HTTP or HTTPS.

Azure Storage Queues are designed for very large cloud networks or hybrid networks, providing a highly reliable and inexpensive queuing service. A single message can be up to 64 KB in size and a queue can hold millions of messages, up to the total capacity limit of the storage account (200 TB). Furthermore, it is capable to emulate local transactions using its queue Peek-Lock mechanism.

The main disadvantages of this service is latency introduced by remoteness and the fact that it supports only HTTP-based communication.

include: azure-transports

## Transport at a glance

|Feature                    |   |  
|:---                       |---
|Transactions |None, ReceiveOnly (Message visibility timeout)
|Pub/Sub                    |Native (Requires Storage Table)
|Timeouts                   |Native (Requires Storage Table)
|Large message bodies       |Data bus
|Scale-out             |Competing consumer
|Scripted Deployment        |Not supported
|Installers                 |Mandatory

## Configuring the endpoint

partial: endpointconfig

Note: When using Azure Storage Queues transport, a serializer must be configured explicitly [by the `UseSerializer` API](/nservicebus/serialization/).
