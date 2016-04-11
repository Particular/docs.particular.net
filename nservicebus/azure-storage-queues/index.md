---
title: Azure Storage Queues
summary: Using Azure Storage Queues as transport
tags:
- Azure
- Cloud
---

In some environments it is not possible or recommended to rely heavily on distributed transactions to ensure reliability and consistency. Therefore in environments such as very large cloud networks or hybrid networks using MSMQ is not the best idea. In those scenarios a good alternative is Azure Storage Queues.

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
