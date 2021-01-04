---
title: Queue Configuration Section
summary: Configuring Azure Service Bus as transport
component: ASB
versions: '[5,7)'
reviewed: 2021-01-04
redirects:
 - transports/azure-service-bus/configuration/azureservicebusqueueconfig
---

include: legacy-asb-warning

## AzureServiceBusQueueConfig (Version 6 and below)

The default settings can be overridden by adding a configuration section called `AzureServiceBusQueueConfig` to the web.config or app.config files:

snippet: AzureServiceBusQueueConfig

The following values can be modified using this configuration setting:

NOTE: Most of these values are applied when a queue or topic is created and cannot be changed afterwards.

 * `ConnectionString`: Overrides the default "NServiceBus/Transport" connection string value.
 * `ConnectivityMode`: Allows to switch between HTTP and TCP based communication. Defaults to TCP. Useful behind a firewall.
 * `ServerWaitTime`: The transport uses long polling to communicate with the Azure Service Bus entities. This value specifies the amount of time, in seconds, the longpoll can take. Defaults to 300 seconds.
 * `BackoffTimeInSeconds`: The transport will back off linearly when no messages can be found on the queue to save money on the transaction operations. This value specifies how fast it will back off. Defaults to 10 seconds.
 * `LockDuration`: The Peek-Lock system supported by Azure Service Bus relies on a period of time that a message becomes locked/invisible after being read. If the processing unit fails to delete the message in the specified time, it will reappear on the queue so that another process can retry. This value is defined in milliseconds and defaults to 30000 (30 seconds).
 * `EnableBatchedOperations`: Specifies whether batching is enabled. Defaults to true.
 * `BatchSize`: The number of messages that the transport tries to pull at once from the queue. Defaults to 1000.
 * `MaxDeliveryCount`: Specifies the number of times a message can be delivered before being put on the dead letter queue. Defaults to 6 (so the NServiceBus first and second level retry mechanism gets preference).
 * `MaxSizeInMegabytes`: Specifies the size in MB. Defaults to 1024 (1GB). Allowed values are 1024, 2048, 3072, 4096 5120.
 * `RequiresDuplicateDetection`: Specifies whether exactly once delivery is enabled and therefore, duplicates need to be detected. Defaults to false, meaning that the same message can arrive multiple times.
 * `DuplicateDetectionHistoryTimeWindow`: Specifies the amount of time in milliseconds that the queue should perform duplicate detection. Defaults to 60,000 ms (1 minute).
 * `RequiresSession`: Specifies whether sessions are required. Defaults to false (NServiceBus makes no use of this feature).
 * `DefaultMessageTimeToLive`: Specifies the time that a message can stay in the queue without being delivered. Defaults to [int64.MaxValue](https://msdn.microsoft.com/en-us/library/system.int64.maxvalue.aspx).
 * `EnableDeadLetteringOnMessageExpiration`: Specifies whether messages should be moved to a dead letter queue upon expiration. Defaults to false (TTL is so large it wouldn't matter anyway). This assumes there have been no attempts to deliver the message. Errors on delivery will still send the message to the dead letter queue.
 * `EnableDeadLetteringOnFilterEvaluationExceptions`: Specifies whether messages should be moved to a dead letter queue upon filter evaluation exceptions. Defaults to false.
 * `EnablePartitioning`: Increase overall throughput by allowing to use multiple brokers/stores to handle queues and topics to overcome limitations of a single broker/store at increased cost. Partitioning does reduce number of queues or topics per namespace. Defaults to false.
 * `SupportOrdering`: Specifies whether queues and topics should enable support for message ordering. Defaults to true.

NOTE: AzureServiceBusQueueConfig is only available in Versions 6 and below. `QueueName` and `QueuePerInstance` are obsoleted since version 6. Instead, specify endpoint name and scale out option.

Defaults are just starting values. These values should be always measured and tested against the specific solution and adjusted accordingly.