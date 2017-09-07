---
title: Azure Storage Queues Transport Configuration
component: ASQ
tags:
- Azure
related:
- persistence/azure-storage/performance-tuning
reviewed: 2017-09-07
redirects:
 - nservicebus/azure-storage-queues/configuration
 ---


## Configuration parameters

The Azure Storage Queues Transport can be configured using the following parameters:


#### ConnectionString

partial: connectionstring


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

The [visibilitytimeout mechanism](https://docs.microsoft.com/en-us/rest/api/storageservices/get-messages), supported by Azure Storage Queues, causes the message to become *invisible* after read for a specified period of time. If the processing unit fails to delete the message in the specified time, the message will reappear on the queue. Then another process can retry the message.

Defaults: 30000 ms (i.e. 30 seconds)


#### BatchSize

The number of messages that the transport tries to pull at once from the storage queue. Depending on the expected load, the value should vary between 1 and 32 (the maximum).

Defaults:

 * 10 in Version 6 and below
 * 32 in Version 7


partial: serialization

partial: parallelismdegree

partial: config


## Connection strings

Note that multiple connection string formats apply when working with Azure storage services. When running against the emulated environment the format is `UseDevelopmentStorage=true`, but when running against a cloud hosted storage account the format is `DefaultEndpointsProtocol=https;AccountName=myAccount;AccountKey=myKey;`

For more details refer to [Configuring Azure Connection Strings](https://docs.microsoft.com/en-us/azure/storage/storage-configure-connection-string) document.

partial: aliases

partial: sanitization


## Serialization

Azure Storage Queues Transport changes the default serializer to JSON. The serializer can be changed using the [serialization API](/nservicebus/serialization).


partial: envelope