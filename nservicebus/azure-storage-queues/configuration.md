---
title: Azure Storage Queues Transport Configuration
component: ASQ
tags:
- Azure
related:
- nservicebus/azure-storage-persistence/performance-tuning
reviewed: 2016-10-21
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

The [visibilitytimeout mechanism](https://docs.microsoft.com/en-us/rest/api/storageservices/fileservices/Get-Messages), supported by Azure Storage Queues, causes the message to become *invisible* after read for a specified period of time. If the processing unit fails to delete the message in the specified time, the message will reappear on the queue. Then another process can retry the message.

Defaults: 30000 ms (i.e. 30 seconds)


#### BatchSize

The number of messages that the transport tries to pull at once from the storage queue. Depending on the expected load, the value should vary between 1 and 32 (the maximum).

Defaults:

 * 10 in Version 6 and below
 * 32 in Version 7


partial:serialization
#### DegreeOfReceiveParallelism

The number of parallel receive operations that the transport is issuing against the storage queue to pull messages out of it.

Defaults: In Versions 7 and above the value is dynamically calculated based on the endpoints [message processing concurrency limit](/nservicebus/operations/tuning.md), using the following equation:

```no-highlight
Degree of parallelism = square root of MaxConcurrency
```

|`MaxConcurrency` | `DegreeOfReceiveParallelism` |
| :-: |:-:|
| 1 | 1 |
| 10 | 3 |
| 20 | 4 |
| 50 | 7 |
| 100 | 10 [default] |
| 200 | 14 |
| 1000 | 32 [max] |

This means that `DegreeOfReceiveParallelism` message processing loops will receive up to the configured `BatchSize` number of messages in parallel. For example with the default `BatchSize` of 32 and the default degree of parallelism of 10 the transport will be able to receive 320 messages from the storage queue at the same time.

WARNING: Changing the value of `DegreeOfReceiveParallelism` will influence the total number of storage operations against Azure Storage Services and can result in higher costs.

WARNING: The values of `BatchSize` , `DegreeOfParallelism`, `Concurrency`, [ServicePointManager Settings](/nservicebus/azure-storage-persistence/performance-tuning.md) and the other parameters like `MaximumWaitTimeWhenIdle` have to be selected carefully in order to get the desired speed out of the transport while not exceeding [the boundaries](https://docs.microsoft.com/en-us/azure/azure-subscription-service-limits) of the allowed number of operations per second.

partial:config


## Connection strings

Note that multiple connection string formats apply when working with Azure storage services. When running against the emulated environment the format is `UseDevelopmentStorage=true`, but when running against a cloud hosted storage account the format is `DefaultEndpointsProtocol=https;AccountName=myAccount;AccountKey=myKey;`

For more details refer to [Configuring Azure Connection Strings](https://docs.microsoft.com/en-us/azure/storage/storage-configure-connection-string) document.


partial: aliasesAndHashing


## Serialization

Azure Storage Queues Transport changes the default serializer to JSON. The serializer can be changed using the [serialization API](/nservicebus/serialization).

## Custom envelope unwrapper

Azure Storage Queues lacks native header support. NServiceBus solves this by wrapping headers and message body in a custom envelope structure. This envelope is serialized using the configured [serializer](/nservicebus/serialization) for the endpoint before being sent.

Creating this envelope can cause uneeded complexity should headers not be needed for example in native integration scenarios. For this reason Version 7.1 and above support configuring a custom envelop unwrapper. 

The snippet below shows custom unwrapping logic that enables both NServiceBus formatted and plain JSON messages to be consumed.

snippet:CustomEnvelopeUnwrapper 