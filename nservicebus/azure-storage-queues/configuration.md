---
title: Azure Storage Queues Transport Configuration
summary: Using Azure Storage Queues as transport
tags:
- Azure
- Cloud
- ASQ
- Azure Storage Queues
related:
- nservicebus/azure-storage-persistence/performance-tuning
---

## Configuration parameters

The Azure Storage Queues Transport can be configured using the following parameters:

#### ConnectionString 

Defaults: 
 * `UseDevelopmentStorage=true` in Versions 6 and below
 * none in Version 7


#### PeekInterval 

The amount of time that the transport waits before polling the input queue, in milliseconds. 

Defaults: 50 ms


 #### MaximumWaitTimeWhenIdle 

In order to save money on the transaction operations, the transport optimizes wait times according to the expected load. The transport will back off when no messages can be found on the queue. The wait time will be increased linearly, but it will never exceed the value specified here, in milliseconds. 

Defaults: 1000 ms (i.e. 1 second)


#### PurgeOnStartup 

Instructs the transport to remove any existing messages from the input queue on startup. 

Defaults: `false`, i.e. messages are not removed when endpoint starts.


#### MessageInvisibleTime 

The [visibilitytimeout mechanism](https://msdn.microsoft.com/en-us/library/azure/dd179474.aspx), supported by Azure Storage Queues, causes the message to become *invisible* after read for a specified period of time. If the processing unit fails to delete the message in the specified time, the message will reappear on the queue. Then another process can retry the message. 

Defaults: 30000 ms (i.e. 30 seconds)


#### BatchSize 

The number of messages that the transport tries to pull at once from the storage queue. Depending on the expected load, the value should vary between 1 and 32 (the maximum).

Defaults:
 * 10 in Version 6 and below 
 * 32 in Version 7


#### DegreeOfReceiveParallelism 

The number of parallel receive operations that the transport is issuing against the storage queue to pull messages out of it. 

Defaults: value is dynamically set based on the endpoints [message processing concurrency limit](/nservicebus/operations/tuning.md), using the following equation:
 
`Degree of parallelism = square root of MaxConcurrency (rounded)`

|`MaxConcurrency` | `DegreeOfReceiveParallelism` |
| :-: |:-:|
| 1 | 1 |
| 10 | 3 |
| 20 | 4 |
| 50 | 7 |
| 100 | 10 [default] |
| 200 | 14 |
| 1000 | 32 [max] |

Versions: 7 and above.

This means that `DegreeOfReceiveParallelism` message processing loops will receive up to the configured `BatchSize` number of messages in parallel. For example with the default `BatchSize` of 32 and the default degree of parallelism of 10 the transport will be able to receive 320 messages from the storage queue at the same time.
  

WARNING: Changing the value of `DegreeOfReceiveParallelism` will influence the total number of storage operations against Azure Storage Services and can result in higher costs.

WARNING: The values of `BatchSize` , `DegreeOfParallelism`, `Concurrency`, [ServicePointManager Settings](/nservicebus/azure-storage-persistence/performance-tuning.md) and the other parameters like `MaximumWaitTimeWhenIdle` have to be selected carefully in order to get the desired speed out of the transport while not exceeding [the boundaries](https://azure.microsoft.com/en-us/documentation/articles/azure-subscription-service-limits/#storage-limits) of the allowed number of operations per second.

NOTE: `QueueName` and `QueuePerInstance` are obsoleted. Instead, use bus configuration object to specify the endpoint name and select a scale-out option.

Parameters' values can be configured in the following ways:


### Via the configuration API

In Versions 7 and below the default settings can be overridden only using configuration API:

snippet:AzureStorageQueueConfigCodeOnly


### Via the App.Config

In Versions 5 and 6 all settings can be overridden by adding to the `web.config` or the `app.config` files a configuration section called `AzureServiceBusQueueConfig`:

snippet:AzureStorageQueueConfig

Note that the connection string can be also configured by specifying a value for connection string called `NServiceBus/Transport`, however this value will be overridden if another is provided in `AzureServiceBusQueueConfig`:

snippet: AzureStorageQueueConnectionStringFromAppConfig


## Connection strings

Note that multiple connection string formats apply when working with Azure storage services. When running against the emulated environment the format is `UseDevelopmentStorage=true`, but when running against a cloud hosted storage account the format is `DefaultEndpointsProtocol=https;AccountName=myAccount;AccountKey=myKey;`

For more details refer to [Configuring Azure Connection Strings](https://azure.microsoft.com/en-us/documentation/articles/storage-configure-connection-string/) document.


### Securing connection strings

It is possible to accidentally leak sensitive information in the connection string if it's not properly secured. E.g. the information can be leaked if an error occurs when communicating across untrusted boundaries, or if the error information is logged to an unsecured log file.

In order to prevent it, `NServiceBus.Azure.Transports.WindowsAzureStorageQueues` Versions 7 and above allow for creating a logical name for each connection string. The name is mapped to the physical connection string, and connection strings are always reffered to by their logical name. In the event of an error or when logging only the logical name can be accidentally leaked.

This feature can be enabled by specifying `.UseAccountNamesInsteadOfConnectionStrings()` when configuring the `AzureStorageQueueTransport`:

snippet:AzureStorageQueueUseAccountNamesInsteadOfConnectionStrings

NOTE: This feature is available in `NServiceBus.Azure.Transports.WindowsAzureStorageQueues` Versions 7 and above.


## Hashing algorithms

If a queue name is longer than [63 characters](https://msdn.microsoft.com/en-us/library/azure/dd179349.aspx), the Azure Storage Queues Transport uses a hashing algorithm to rename it. The default algorithm is `MD5`. In order to use `SHA1`, apply the following configuration:

snippet:AzureStorageQueueUseSha1

NOTE: This feature is available in `NServiceBus.Azure.Transports.WindowsAzureStorageQueues` Versions 7 and above.
