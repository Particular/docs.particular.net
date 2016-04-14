---
title: Azure Storage Queues Transport Configuration
summary: Using Azure Storage Queues as transport
tags:
- Azure
- Cloud
---

Azure Storage Queues Transport can be configured using following parameters. Depending on the version, parameters' values can  be configured using a custom configuration section or code-first approach. Please find the description of the parameters below:

 * `ConnectionString`: Overrides the default "NServiceBus/Transport" value and defaults to "UseDevelopmentStorage=true" if not set. It's recommended to set this value when specifying the configuration setting to prevent unexpected issues.
 * `PeekInterval`: Represents the amount of time that the transport waits before polling the queue in milliseconds, defaults to 50 ms.
 * `MaximumWaitTimeWhenIdle`: The transport will back of linearly when no messages can be found on the queue to save some money on the transaction operations, but it will never wait longer than the value specified here, also in milliseconds and defaults to 1000 (1 second)
 * `PurgeOnStartup`: Instructs the transport to remove any existing messages from the queue on startup, defaults to false.
 * `MessageInvisibleTime`: The Peek-Lock mechanism, supported by Azure storage queues relies on a period of time that a message becomes locked/invisible after being read. If the processing unit fails to delete the message in the specified time it will reappear on the queue so that another process can retry. This value is defined in milliseconds and defaults to 30000 (30 seconds).
 * `BatchSize`: The number of messages that the transport tries to pull at once from the storage queue. Defaults to 10. Depending on the expected load, I would vary this value between 1 and 1000 (which is the limit)

NOTE: `QueueName` and `QueuePerInstance` are obsoleted. Instead, use bus configuration object to specify endpoint name and scale-out option.

### Via the App.Config

In Versions 5.0 to 6.x the default settings can be overridden by adding a configuration section called `AzureServiceBusQueueConfig` to the web.config or app.config files:

snippet:AzureStorageQueueConfig

### Via code-first

In Version 7.x the default setting can be overriden using only code

snippet:AzureStorageQueueConfigCodeOnly