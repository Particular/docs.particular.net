---
title: Azure Storage Queues Transport
summary: NServiceBus can use Azure storage queues to take advantage of their peek-lock mechanism in environments where one cannot rely on the DTC
tags:
- Azure
- Cloud
- Azure Storage Queues
- Transport
redirects:
 - nservicebus/using-azure-storage-queues-as-transport-in-nservicebus
related:
 - samples/azure/storage-queues
reviewed: 2016-03-07
---


In some environments it is not possible or recommended to rely heavily on distributed transactions to ensure reliability and consistency. Therefore in environments such as very large cloud networks or hybrid networks using MSMQ is not the best idea. In those scenarios a good alternative is Azure Storage Queues.

Azure Queue storage is a service hosted on the Azure platform, used for storing large numbers of messages that can be accessed from anywhere in the world via authenticated calls using HTTP or HTTPS.

- The main advantage of this service is that it offers a highly reliable and very cheap queuing service ($0.1 per million messages). A single message can be up to 64 KB in size, and a queue can keep millions of messages, up to the total capacity limit of the storage account (200 TB). Furthermore it is capable to emulate local transactions using it's queue Peek-Lock mechanism.
- The main disadvantages of this service is latency introduced by remoteness and the fact that it only supports HTTP based communication.

## How to enable the transport

Firstly, reference the assembly that contains the Azure storage queue transport definition. The recommended method is to add a `NServiceBus.Azure.Transports.WindowsAzureStorageQueues` NuGet package reference to the project.

```
PM> Install-Package NServiceBus.Azure.Transports.WindowsAzureStorageQueues
```

Then use the Configuration API to set up NServiceBus, by specifying `.UseTransport<T>()` to override the default transport:

snippet:AzureStorageQueueTransportWithAzure

When using one of the NServiceBus provided hosting processes, the `UseTransport<T>` should be called on the endpoint configuration. For example, for Azure role entrypoint host:

snippet:AzureStorageQueueTransportWithAzureHost

## Setting the connection string

The default way to set the connection string is using the .NET provided `connectionStrings` configuration section in app.config or web.config, with the name `NServicebus\Transport`:

snippet:AzureStorageQueueConnectionStringFromAppConfig

Note that multiple connection string formats apply when working with Azure storage services. When running against the emulated environment the format is `UseDevelopmentStorage=true`, but when running against a cloud hosted storage account the format is `DefaultEndpointsProtocol=https;AccountName=myAccount;AccountKey=myKey;`

For more details refer to [Configuring Azure Connection Strings](https://azure.microsoft.com/en-us/documentation/articles/storage-configure-connection-string/) document.

## Detailed configuration

The default settings can be overriden by adding a configuration section called `AzureServiceBusQueueConfig` to the web.config or app.config files:

snippet:AzureStorageQueueConfig

The following values can be modified using this configuration setting:

- `ConnectionString`: Overrides the default "NServiceBus/Transport" value and defaults to "UseDevelopmentStorage=true" if not set. It's recommended to set this value when specifying the configuration setting to prevent unexpected issues.
- `PeekInterval`: Represents the amount of time that the transport waits before polling the queue in milliseconds, defaults to 50 ms.
- `MaximumWaitTimeWhenIdle`: The transport will back of linearly when no messages can be found on the queue to save some money on the transaction operations, but it will never wait longer than the value specified here, also in milliseconds and defaults to 1000 (1 second)
- `PurgeOnStartup`: Instructs the transport to remove any existing messages from the queue on startup, defaults to false.
- `MessageInvisibleTime`: The Peek-Lock mechanism, supported by Azure storage queues relies on a period of time that a message becomes locked/invisible after being read. If the processing unit fails to delete the message in the specified time it will reappear on the queue so that another process can retry. This value is defined in milliseconds and defaults to 30000 (30 seconds).
- `BatchSize`: The number of messages that the transport tries to pull at once from the storage queue. Defaults to 10. Depending on the expected load, I would vary this value between 1 and 1000 (which is the limit)

NOTE: `QueueName` and `QueuePerInstance` are obsoleted. Instead, use bus configuration object to specify endpoint name and scale-out option.

## Transactions and delivery guarantees


### Version 6 and above
Azure Storage Queues Transport supports `ReceiveOnly` and `Unreliable` levels.

#### ReceiveOnly

The message is not removed from the queue directly after receive, but it's hidden for 30 seconds. That prevents other instances from picking it up. If the receiver fails to process the message withing that timeframe or explicitly abandons the message, then the message will become visible again. Other instances will be able to pick it up.

#### Unraliable (Transactions Disabled)

The message is deleted from the queue directly after receive operation completes, before it is processed.