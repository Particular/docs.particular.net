s---
title: Azure Storage Queues Transport
summary: Using Azure Storage Queues as transport
component: ASQ
tags:
 - Azure
related:
 - samples/azure/storage-queues
reviewed: 2017-04-24
---

Azure Storage Queues is a service hosted on the Azure platform, used for storing large numbers of messages. The messages can be accessed from anywhere in the world via authenticated calls using HTTP or HTTPS.

Azure Storage Queues are designed for very large cloud networks or hybrid networks, providing a highly reliable and very cheap queuing service. A single message can be up to 64 KB in size and a queue can keep millions of messages, up to the total capacity limit of the storage account (200 TB). Furthermore, it is capable to emulate local transactions using it's queue Peek-Lock mechanism.

The main disadvantages of this service is latency introduced by remoteness and the fact that it only supports HTTP based communication.

include: azure-transports


## Configuring the endpoint

To use Azure Storage Queues as the underlying transport configure it in the following way:

snippet: AzureStorageQueueTransportWithAzure

Then set up appropriate [connection strings](/nservicebus/azure-storage-queues/configuration.md#connection-strings) and consider using aliase to make them [more secure](/nservicebus/azure-storage-queues/configuration.md#connection-strings-using-aliases-for-connection-strings-to-storage-accounts).
