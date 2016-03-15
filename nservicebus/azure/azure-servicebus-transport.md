---
title: Azure Service Bus Transport
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
reviewed: 2016-03-07
---

In some environments it is not possible or recommended to rely heavily on distributed transactions to ensure reliability and consistency. Therefore in environments such as very large cloud networks or hybrid networks using MSMQ is not the best idea. In those scenarios a good alternative is Azure Service Bus.

[Azure Service Bus (ASB)](https://azure.microsoft.com/en-us/services/service-bus/) is a messaging service (broker) that sits between applications, allowing them to exchange messages in a loosely coupled fashion for improved scale and resiliency. Service Bus Queues offer simple first in, first out guaranteed message delivery and support a range of standard protocols (REST, AMQP, WS*) and APIs (to put/pull messages on/off the queue). Service Bus Topics deliver messages to multiple subscribers and easily use fan-out pattern to deliver messages to downstream systems.

 * The main advantage of ASB is that it offers a highly reliable and (relatively) low latency remote messaging infrastructure. A single message can be up to 256 KB in size, and a queue can keep many messages at once, up to 5 GB size in total. Furthermore it is capable of emulating local transactions using its queue peek-lock mechanism. NServiceBus is an abstraction over ASB. It takes advantage of ASB's built-in features, such as message deduplication and deferred messages, and provides higher-level, convenient API for programmers on top of ASB.
 * The main disadvantage of ASB is its dependency on TCP (for low latency), which may require opening some outbound ports on the firewall. Additionally, in some systems the price may get high ($1 per million messages).

Note: Publish/Subscribe and Timeouts (including message deferral) are supported natively by the ASB transport and do not use persistence.


## Enabling the Transport

Firstly, choose Standard or Premium Messaging Tier for Azure Service Bus when creating the namespace at Azure portal.

Secondly, reference `NServiceBus.Azure.Transports.WindowsAzureServiceBus` NuGet package.

```
PM> Install-Package NServiceBus.Azure.Transports.WindowsAzureServiceBus
```

Then use the Configuration API to set up NServiceBus, by specifying `.UseTransport<T>()` to override the default transport:

snippet:AzureServiceBusTransportWithAzure

When using one of the NServiceBus provided hosting processes, the `UseTransport<T>` should be called on the endpoint configuration. For example, for Azure role entrypoint host:

snippet:AzureServiceBusTransportWithAzureHost


## Setting the Connection String

The default way to set the connection string is using the .NET provided `connectionStrings` configuration section in app.config or web.config, with the name `NServicebus\Transport`:

snippet:AzureServiceBusConnectionStringFromAppConfig

For more details refer to [Configuration Connection Strings](https://azure.microsoft.com/en-us/documentation/articles/service-bus-dotnet-how-to-use-topics-subscriptions/#set-up-a-service-bus-connection-string) document.


## Detailed configuration

The default settings can be overridden by adding a configuration section called `AzureServiceBusQueueConfig` to the web.config or app.config files:

snippet:AzureServiceBusQueueConfig

The following values can be modified using this configuration setting:

NOTE: Most of these values are applied when a queue or topic is created and cannot be changed afterwards.

 * `ConnectionString`: Overrides the default "NServiceBus/Transport" connectionstring value.
 * `ConnectivityMode`: Allows to switch between HTTP and TCP based communication. Defaults to TCP. Very useful behind a firewall.
 * `ServerWaitTime`: The transport uses longpolling to communicate with the Azure Service Bus entities. This value specifies the amount of time, in seconds, the longpoll can take. Defaults to 300 seconds.
 * `BackoffTimeInSeconds`: The transport will back off linearly when no messages can be found on the queue to save some money on the transaction operations. This value specifies how fast it will back off. Defaults to 10 seconds.
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

NOTE: `QueueName` and `QueuePerInstance` are obsoleted. Instead, use bus configuration object to specify endpoint name and scale-out option.

Defaults are just starting values. These values should be always measured and tested against the specific solution and adjusted accordingly.


### BrokeredMessage body conventions

By default `BrokeredMessage` body is transmitted as a byte array. But for scenarios such as native integration, the body can be stored and retrieved using `Stream`. To specify how the `BrokeredMessage` body is stored and retrieved, override conventions provided by using `BrokeredMessageBodyConversion` class.

Outgoing message:

snippet: ASB-outgoing-message-convention

Incoming message:

snippet: ASB-incoming-message-convention


### Naming Conventions

To have a fine-grained control over entity names generated by the ASB transport, `NamingConventions` class exposes several conventions that can be overwritten to provide customization.

Entity sanitization:

snippet: ASB-NamingConventions-entity-sanitization

This sanitization allows forward slash `/` in queue and topic names unlike default sanitization convention used by Azure Service Bus transport.

Entities creation:

snippet: ASB-NamingConventions-entity-creation-conventions

WARNING: This is an advanced topic and requires full understanding of the topology.


## Transactions and delivery guarantees

NServiceBus Azure Service Bus transport relies on the underlying Azure Service Bus library which requires the use of the `Serializable` isolation level (the most restrictive isolation level that does not permit `dirty reads`, `phantom reads` and `non repeatable reads`; will block any reader until the writer is committed. For more information refer to [Transaction Isolation Levels Explained in Details](http://dotnetspeak.com/2013/04/transaction-isolation-levels-explained-in-details) article.

NServiceBus AzureServiceBus transport configuration is hard-coded to `Serializable` isolation level. Users can't override it.


### Version 6 and above

Azure Service Bus Transport supports `SendAtomicWithReceive`, `ReceiveOnly` and `Unreliable` levels.


#### SendAtomicWithReceive

Note: `SendAtomicWithReceive` level is supported only when destination and receive queues are in the same namespace.

The `SendAtomicWithReceive` guarantee is achieved by using `ViaEntityPath` property on outbound messages. It's value is set to the receiving queue.

If the `ViaEntityPath` is not empty, then messages will be added to the receive queue. The messages will be forwarded to their actual destination (inside the broker) only when the complete operation is called on the received brokered message. The message won't be forwarded if the lock duration limit is exceeded (30 seconds by default) or if the message is explicitly abandoned.


#### ReceiveOnly

The `ReceiveOnly` guarantee is based on the Azure Service Bus Peek-Lock mechanism. 

The message is not removed from the queue directly after receive, but it's hidden by default for 30 seconds. That prevents other instances from picking it up. If the receiver fails to process the message withing that timeframe or explicitly abandons the message, then the message will become visible again. Other instances will be able to pick it up.


#### Unreliable (Transactions Disabled)

When transactions are disabled then NServiceBus uses the [ASB's ReceiveAndDelete mode](https://msdn.microsoft.com/en-us/library/microsoft.servicebus.messaging.receivemode.aspx).

The message is deleted from the queue directly after receive operation completes, before it is processed.


## Scenarios

Every scenario provided in this document should always be tested it in environment as close to production as possible.


### CPU vs IO bound processing

There are several things to consider:

 - `BatchSize`
 - `LockDuration`
 - `MaxDeliveryCount`

In scenarios where handlers are CPU intense and have very little IO, it is advised to lower number of threads to one and have a bigger `BatchSize`. `LockDuration` and `MaxDeliveryCount` might require an adjustment to match the batch size taking in account number of messages that end up in the dead letter queue.

In scenario where handlers are IO intense, it is advised to set number of threads ([`MaximumConcurrencyLevel`](/nservicebus/operations/tuning.md) in NServiceBus) to 12 threads per logical core and `BatchSize` to a number of messages that takes to process, taking into account possible/measured single message processing time and IO latency. Start with a small `BatchSize` and through adjustment and measurement bring it up, adjusting accordingly `LockDuration` and `MaxDeliveryCount`.