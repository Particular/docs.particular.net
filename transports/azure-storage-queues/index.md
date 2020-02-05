---
title: Azure Storage Queues Transport
summary: Using Azure Storage Queues as a message transport
component: ASQ
tags:
 - Azure
related:
 - samples/azure/storage-queues
reviewed: 2018-06-18
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
|Pub/Sub                    |Message driven
|Timeouts                   |Native (Requires Storage Table)
|Large message bodies       |Data bus
|Scale-out             |Competing consumer
|Scripted Deployment        |Not supported
|Installers                 |Mandatory

## Configuring the endpoint

To use Azure Storage Queues as the underlying transport configure it as follows:

snippet: AzureStorageQueueTransportWithAzure

Then set up appropriate [connection strings](/transports/azure-storage-queues/configuration.md#connection-strings) and consider using aliases to make them [more secure](/transports/azure-storage-queues/configuration.md#connection-strings-using-aliases-for-connection-strings-to-storage-accounts).

Note: When using Azure Storage Queues transport, a serializer must be configured explicitly [by the `UseSerializer` API](/nservicebus/serialization/).
