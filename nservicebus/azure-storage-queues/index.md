---
title: Azure Storage Queues Transport
summary: Using Azure Storage Queues as transport
component: ASQ
tags:
 - Azure
 - ASQ
 - Azure Storage Queues
related:
 - samples/azure/storage-queues
reviewed: 2016-10-19
---

Azure Storage Queues is a service hosted on the Azure platform, used for storing large numbers of messages. The messages can be accessed from anywhere in the world via authenticated calls using HTTP or HTTPS.

Azure Storage Queues are designed for very large cloud networks or hybrid networks, providing a highly reliable and very cheap queuing service. A single message can be up to 64 KB in size and a queue can keep millions of messages, up to the total capacity limit of the storage account (200 TB). Furthermore, it is capable to emulate local transactions using it's queue Peek-Lock mechanism.

The main disadvantages of this service is latency introduced by remoteness and the fact that it only supports HTTP based communication.

NOTE: As part of the Azure support for NServiceBus, one can choose between two transports provided by the Azure platform: [Azure Storage Queues](/nservicebus/azure-storage-queues/) and [Azure Service Bus](/nservicebus/azure-service-bus/). Each of them has different features, capabilities, and usage characteristics. A detailed comparison and discussion of when to select which is beyond the scope of this document. To help decide which option best suits the application's needs, refer to the  [Azure Queues and Azure Service Bus Queues - Compared and Contrasted](https://azure.microsoft.com/en-us/documentation/articles/service-bus-azure-and-service-bus-queues-compared-contrasted/) article.


## Enable the transport

Then at configuration time set ASB as the transport:

snippet:AzureStorageQueueTransportWithAzure

Then set up appropriate [connection strings](/nservicebus/azure-storage-queues/configuration.md#connection-strings), and ensure they are [secure](/nservicebus/azure-storage-queues/configuration.md#connection-strings-using-aliases-for-connection-strings-to-storage-accounts).
