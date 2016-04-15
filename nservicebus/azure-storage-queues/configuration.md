---
title: Azure Storage Queues Transport Configuration
summary: Using Azure Storage Queues as transport
tags:
- Azure
- Cloud
- ASQ
- Azure Storage Queues
---

## Configuration parameters

The Azure Storage Queues Transport can be configured using the following parameters:

 * `ConnectionString`: The default value is `UseDevelopmentStorage=true`.
 * `PeekInterval`: The amount of time that the transport waits before polling the input queue, in milliseconds. The default value is 50 ms.
 * `MaximumWaitTimeWhenIdle`: In order to save money on the transaction operations, the transport optimizes wait times according to the expected load. The transport will back off when no messages can be found on the queue. The wait time will be increased linearly, but it will never exceed the value specified here, in milliseconds. The default value is 1000 (i.e. 1 second).
 * `PurgeOnStartup`: Instructs the transport to remove any existing messages from the input queue on startup. The default value is `false`, i.e. messages are not removed when endpoint starts.
 * `MessageInvisibleTime`: The [visibilitytimeout mechanism](https://msdn.microsoft.com/en-us/library/azure/dd179474.aspx), supported by Azure Storage Queues, causes the message to become *invisible* after read for a specified period of time. If the processing unit fails to delete the message in the specified time, the message will reappear on the queue. Then another process can retry the message. The default value is 30000, in milliseconds (i.e. 30 seconds).
 * `BatchSize`: The number of messages that the transport tries to pull at once from the storage queue. The default value is 10. Depending on the expected load, the value should vary between 1 and 1000 (the maximum).

NOTE: `QueueName` and `QueuePerInstance` are obsoleted. Instead, use bus configuration object to specify the endpoint name and select a scale-out option.

Parameters' values can be configured in the following ways:


### Via the configuration API

In Versions 7 and below the default settings can be overriden only using configuration API:

snippet:AzureStorageQueueConfigCodeOnly


### Via the App.Config

In Versions 5 and 6 all settings can be overridden by adding to the `web.config` or the `app.config` files a configuration section called `AzureServiceBusQueueConfig`:

snippet:AzureStorageQueueConfig

Note that the connection string can be also configured by specifying a value for connection string called `NServiceBus/Transport`, however this value will be overriden if another is provided in `AzureServiceBusQueueConfig`:

snippet: AzureStorageQueueConnectionStringFromAppConfig


## Connection strings

Note that multiple connection string formats apply when working with Azure storage services. When running against the emulated environment the format is `UseDevelopmentStorage=true`, but when running against a cloud hosted storage account the format is `DefaultEndpointsProtocol=https;AccountName=myAccount;AccountKey=myKey;`

For more details refer to [Configuring Azure Connection Strings](https://azure.microsoft.com/en-us/documentation/articles/storage-configure-connection-string/) document.


### Securing connection strings

It is possible to accidentally leak sensitive information in the connection string if it's not properly secured. E.g. the information can be leaked if an error occurs when communicating across untrusted boundaries, or if the error information is logged to an unsecured log file.

In order to prevent it, `NServiceBus.Azure.Transports.WindowsAzureStorageQueues` Versions 7 and above allow for creating a logical name for each connection string. The name is mapped to the physical connection string, and connection strings are always reffered to by their logical name. In the event of an error or when logging only the logical name can be accidentally leaked.

This feature can be enabled by specifying `.UseAccountNamesInsteadOfConnectionStrings()` when configuring the `AzureStorageQueueTransport`:

snippet:AzureStorageQueueUseAccountNamesInsteadOfConnectionStrings

NOTE: This feature is not available in `NServiceBus.Azure.Transports.WindowsAzureStorageQueues` Versions 6 and below.
