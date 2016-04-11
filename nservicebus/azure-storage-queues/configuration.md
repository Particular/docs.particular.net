---
title: Azure Storage Queues
summary: Using Azure Storage Queues as transport
tags:
- Azure
- Cloud
---

The default settings can be overridden by adding a configuration section called `AzureServiceBusQueueConfig` to the web.config or app.config files:

snippet:AzureStorageQueueConfig

The following values can be modified using this configuration setting:

 * `ConnectionString`: Overrides the default "NServiceBus/Transport" value and defaults to "UseDevelopmentStorage=true" if not set. It's recommended to set this value when specifying the configuration setting to prevent unexpected issues.
 * `PeekInterval`: Represents the amount of time that the transport waits before polling the queue in milliseconds, defaults to 50 ms.
 * `MaximumWaitTimeWhenIdle`: The transport will back of linearly when no messages can be found on the queue to save some money on the transaction operations, but it will never wait longer than the value specified here, also in milliseconds and defaults to 1000 (1 second)
 * `PurgeOnStartup`: Instructs the transport to remove any existing messages from the queue on startup, defaults to false.
 * `MessageInvisibleTime`: The Peek-Lock mechanism, supported by Azure storage queues relies on a period of time that a message becomes locked/invisible after being read. If the processing unit fails to delete the message in the specified time it will reappear on the queue so that another process can retry. This value is defined in milliseconds and defaults to 30000 (30 seconds).
 * `BatchSize`: The number of messages that the transport tries to pull at once from the storage queue. Defaults to 10. Depending on the expected load, I would vary this value between 1 and 1000 (which is the limit)

NOTE: `QueueName` and `QueuePerInstance` are obsoleted. Instead, use bus configuration object to specify endpoint name and scale-out option.
