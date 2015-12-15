---
title: Azure Service Bus as Transport
summary: NServiceBus can use Azure Service Bus to take advantage of its peek-lock mechanism in environments where one cannot rely on the DTC.
tags: 
- Azure
- Cloud
- Transport
- Concurrency
- Performance
- Batching
redirects:
 - nservicebus/using-azure-servicebus-as-transport-in-nservicebus
related:
 - samples/azure/azure-service-bus
---

In some environments such as very large cloud networks or hybrid network scenarios, it is not possible or recommended to rely heavily on the DTC, and thus on MSMQ, to ensure transactional behavior and retry in case of failures. A good alternative to MSMQ in this case is to use Azure Service Bus instead.

Azure Service Bus is messaging infrastructure that sits between applications, allowing them to exchange messages in a loosely coupled way for improved scale and resiliency. Service Bus Queues offer simple first in, first out guaranteed message delivery and supports a range of standard protocols (REST, AMQP, WS*) and APIs to put/pull messages on/off a queue. Service Bus Topics deliver messages to multiple subscriptions and easily fan out message delivery at scale to downstream systems.
 
- The main advantage of this service is that it offers a highly reliable and (relatively) low latency remote messaging infrastructure. A single queue message can be up to 256 KB in size, and a queue can contain many messages, up to 5GB in total. Furthermore it is capable of emulating local transactions using its queue peek-lock mechanism and has many built-in features that you (and NServiceBus) can take advantage of, such as message deduplication and deferred messages.
- The main disadvantage of this service is its dependency on TCP (if you want low latency), which may require you to open some outbound ports on your firewall. Additionally, the price may be steep, depending on your scenario ($1 per million messages).


## Enabling the Transport

First, ensure you're using Standard Messaging Tier for Azure Service Bus when creating your namespace at Azure portal.

Second, reference the assembly that contains the Azure Service Bus transport definition. The recommended method is to add a NuGet package reference to the `NServiceBus.Azure.Transports.WindowsAzureServiceBus` package to your project.

```
PM> Install-Package NServiceBus.Azure.Transports.WindowsAzureServiceBus
```

Then, use the Configuration API to set up NServiceBus, by specifying `.UseTransport<T>()` to override the default transport:

snippet:AzureServiceBusTransportWithAzure

Alternatively, when using one of the NServiceBus provided hosting processes, you should call the `UseTransport<T>` on the endpoint configuration. In the Azure role entrypoint host, for example, it looks like this:

snippet:AzureServiceBusTransportWithAzureHost


## Setting the Connection String

The default way to set the connection string is using the .NET provided `connectionStrings` configuration section in app.config or web.config, with the name `NServicebus\Transport`:

snippet:AzureServiceBusConnectionStringFromAppConfig

For more detail see [Configuration Connection Strings](https://msdn.microsoft.com/en-us/library/azure/jj149830.aspx)


## Configuring in Detail

If you need fine grained control on how the Azure Service Bus transport behaves, you can override the default settings by adding a configuration section called `AzureServiceBusQueueConfig` to your web.config or app.config files. For example:

snippet:AzureServiceBusQueueConfig

Using this configuration setting you can change the following values. NOTE: Most of these values are applied when a queue or topic is created and cannot be changed afterwards).

- `ConnectionString`: Overrides the default "NServiceBus/Transport" connectionstring value.
- `ConnectivityMode`: Allows you to switch between HTTP and TCP based communication. Defaults to TCP. Very useful when behind a firewall.
- `ServerWaitTime`: The transport uses longpolling to communicate with the Azure Service Bus entities. This value specifies the amount of time, in seconds, the longpoll can take. Defaults to 300 seconds. 
- `BackoffTimeInSeconds`: The transport will back off linearly when no messages can be found on the queue to save some money on the transaction operations. This value specifies how fast it will back off. Defaults to 10 seconds.
- `LockDuration`: The peek-lock system supported by Azure Service Bus relies on a period of time that a message becomes locked/invisible after being read. If the processing unit fails to delete the message by the specified time, it will reappear on the queue so that another process can retry. This value is defined in milliseconds and defaults to 30000 (30 seconds). 
- `EnableBatchedOperations`: Specifies whether batching is enabled. Defaults to true.
- `BatchSize`: The number of messages that the transport tries to pull at once from the queue. Defaults to 1000. 
- `MaxDeliveryCount`: Specifies the number of times a message can be delivered before being put on the dead letter queue. Defaults to 6 (so the NServiceBus first and second level retry mechanism gets preference).
- `MaxSizeInMegabytes`: Specifies the size in MB. Defaults to 1024 (1GB). Allowed values are 1024, 2048, 3072, 4096 5120.
- `RequiresDuplicateDetection`: Specifies whether exactly once delivery is enabled. Defaults to false, meaning that the same message can arrive multiple times.
- `DuplicateDetectionHistoryTimeWindow`: Specifies the amount of time in milliseconds that the queue should perform duplicate detection. Defaults to 60000 ms (1 minute).
- `RequiresSession`: Specifies whether sessions are required. Defaults to false (NServiceBus makes no use of this feature).
- `DefaultMessageTimeToLive`: Specifies the time that a message can stay in the queue without being delivered. Defaults to int64.MaxValue, which is roughly 10.000 days.
- `EnableDeadLetteringOnMessageExpiration`: Specifies whether messages should be moved to a dead letter queue upon expiration. Defaults to false (TTL is so large it wouldn't matter anyway). This assumes there have been no attempts to deliver. Errors on delivery will still put the message on the dead letter queue.
- `EnableDeadLetteringOnFilterEvaluationExceptions`: Specifies whether messages should be moved to a dead letter queue upon filter evaluation exceptions. Defaults to false.
- `EnablePartitioning`: Increase overall throughput by allowing to use multiple brokers/stores to handle queues and topics to overcome limitations of a single broker/store at increased cost. Partitioning does reduce number of queues or topics per namespace. Defaults to false.
- `SupportOrdering`: Specifies whether queues and topics should enable support for message ordering. Defaults to true.

NOTE: `QueueName` and `QueuePerInstance` are obsoleted. Instead, use bus configuration object to specify endpoint name and scale-out option.

Defaults are just starting values. You should always measure and test these values against your solution and adjust those accordingly.


## Transactions

NServiceBus AzureServiceBus transport relies on the underlying Azure ServiceBus library which requires the use of the `Serializable` isolation level (the most restrictive isolation level that does not permit `dirty reads`, `phantom reads` and `non repeatable reads`; will block any reader until the writer is committed [see this link](http://dotnetspeak.com/2013/04/transaction-isolation-levels-explained-in-details) for more information)

NServiceBus AzureServiceBus transport configuration is hard coded to `Serializable` isolation level to prevent users from overriding it.


## Scenarios

For any scenario provided in this document, you should always test it in environment as close to production as possible.


### CPU vs IO bound processing

There are several things to consider:
- `BatchSize`
- `LockDuration`
- `MaxDeliveryCount`

In scenario where handlers are CPU intense and have very little IO, it is advised to lower number of threads to one and have a bigger `BatchSize`. `LockDuration` and `MaxDeliveryCount` might require an adjustment to match the batch size taking in account number of messages that end up in the dead letter queue.

In scenario where handlers are IO intense, it is advised to set number of threads ([`MaximumConcurrencyLevel`](/nservicebus/operations/tuning.md) in NServiceBus) to 12 threads per logical core and `BatchSize` to a number of messages that takes to process, taking in account possible/measured single message processing time and IO latency. Try to start with a small `BatchSize` and through adjustment and measurement bring it up, adjusting accordingly `LockDuration` and `MaxDeliveryCount`.
