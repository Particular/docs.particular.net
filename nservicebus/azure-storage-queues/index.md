---
title: Azure Storage Queues Transport
summary: Using Azure Storage Queues as transport
tags:
- Azure
related:
 - samples/azure/storage-queues
---

In some environments it is not possible or recommended to rely heavily on distributed transactions to ensure reliability and consistency. Therefore in environments such as very large cloud networks or hybrid networks using MSMQ is not the best idea. In those scenarios a good alternative is Azure Storage Queues.

NOTE: As part of the Azure support for NServiceBus, one can choose between two transports provided by the Azure platform Azure Storage Queues and Azure Service Bus. Each of these two options has separate features, capabilities, and usage characteristics. A detailed comparison and discussion of when to select which is beyond the scope of this document. To help decide which option best suits the application's needs, review the Azure article [Azure Queues and Azure Service Bus Queues - Compared and Contrasted](https://azure.microsoft.com/en-us/documentation/articles/service-bus-azure-and-service-bus-queues-compared-contrasted/).

Azure Queue storage is a service hosted on the Azure platform, used for storing large numbers of messages that can be accessed from anywhere in the world via authenticated calls using HTTP or HTTPS.

 * The main advantage of this service is that it offers a highly reliable and very cheap queuing service ($0.1 per million messages). A single message can be up to 64 KB in size, and a queue can keep millions of messages, up to the total capacity limit of the storage account (200 TB). Furthermore it is capable to emulate local transactions using it's queue Peek-Lock mechanism.
 * The main disadvantages of this service is latency introduced by remoteness and the fact that it only supports HTTP based communication.


## How to enable the transport

Firstly, reference `NServiceBus.Azure.Transports.WindowsAzureStorageQueues` NuGet package.

```
PM> Install-Package NServiceBus.Azure.Transports.WindowsAzureStorageQueues
```

Then use the Configuration API to set up NServiceBus, by specifying `.UseTransport<T>()` to override the default transport:

snippet:AzureStorageQueueTransportWithAzure


## Setting the connection string

The default way to set the connection string is using the .NET provided `connectionStrings` configuration section in app.config or web.config, with the name `NServicebus\Transport`:

snippet:AzureStorageQueueConnectionStringFromAppConfig

Note that multiple connection string formats apply when working with Azure storage services. When running against the emulated environment the format is `UseDevelopmentStorage=true`, but when running against a cloud hosted storage account the format is `DefaultEndpointsProtocol=https;AccountName=myAccount;AccountKey=myKey;`

For more details refer to [Configuring Azure Connection Strings](https://azure.microsoft.com/en-us/documentation/articles/storage-configure-connection-string/) document.


## Securing the connection string

Accidental leaking of sensitive information in the connection string could occur if an error occurs when communicating across untrusted boundaries, or if that information is logged to an unsecured log file. In order to prevent this, Versions 7 and above of `NServiceBus.Azure.Transports.WindowsAzureStorageQueues` it is possible to create a logical name for each connection string, map that to the physical connection string, and only refer to the connection string by it's logical name. This means that only the logical name could be accidentaly leaked in the event of an error, or logging.

This feature can be enabledin Versions 7 and above by specifying `.UseAccountNamesInsteadOfConnectionStrings()` when configuring the `AzureStorageQueueTransport`.

snippet:AzureStorageQueueUseAccountNamesInsteadOfConnectionStrings

This feature is not available in Versions 6 and below.