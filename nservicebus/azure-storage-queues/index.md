---
title: Azure Storage Queues Transport
summary: Using Azure Storage Queues as transport
tags:
- Azure
related:
 - samples/azure/storage-queues
---

In some environments, such as very large cloud networks or hybrid networks, relying on distributed transactions to ensure reliability and consistency is not recommended or, in some cases, not even possible (refer to [Transactions in Azure](/nservicebus/azure/transactions.md#understanding-distributed-transactions-and-the-two-phase-commit-protocol) article to learn more). The Azure Storage Queues transport is designed specifically for such environments.

NOTE: As part of the Azure support for NServiceBus, one can choose between two transports provided by the Azure platform: [Azure Storage Queues](/nservicebus/azure-storage-queues/) and [Azure Service Bus](/nservicebus/azure-service-bus/). Each of them has different features, capabilities, and usage characteristics. A detailed comparison and discussion of when to select which is beyond the scope of this document. To help decide which option best suits the application's needs, refer to the  [Azure Queues and Azure Service Bus Queues - Compared and Contrasted](https://azure.microsoft.com/en-us/documentation/articles/service-bus-azure-and-service-bus-queues-compared-contrasted/) article.

Azure Queue storage is a service hosted on the Azure platform, used for storing large numbers of messages. The messages can be accessed from anywhere in the world via authenticated calls using HTTP or HTTPS.

 * The main advantage of this service is that it offers a highly reliable and very cheap queuing service ($0.1 per million messages). A single message can be up to 64 KB in size, and a queue can keep millions of messages, up to the total capacity limit of the storage account (200 TB). Furthermore, it is capable to emulate local transactions using it's queue Peek-Lock mechanism.
 * The main disadvantages of this service is latency introduced by remoteness and the fact that it only supports HTTP based communication.


## How to enable the transport

Reference `NServiceBus.Azure.Transports.WindowsAzureStorageQueues` NuGet package.

```
PM> Install-Package NServiceBus.Azure.Transports.WindowsAzureStorageQueues
```

Then use the Configuration API to set up NServiceBus, by specifying `.UseTransport<T>()` to override the default transport:

snippet:AzureStorageQueueTransportWithAzure

Then set up appropriate [connection strings](/nservicebus/azure-storage-queues/configuration.md#connection-strings), and ensure they are [secure](/nservicebus/azure-storage-queues/configuration.md#securing-the-connection-string) .