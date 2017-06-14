---
title: Full Configuration API
summary: List of all Azure Service Bus Transport configuration settings
component: ASB
versions: '[7,)'
tags:
- Azure
redirects:
- nservicebus/azure-service-bus/configuration/configuration
reviewed: 2017-06-08
---


INFO: This document is intended for Versions 7 and above.

The full configuration API can be accessed from the `UseTransport<AzureServiceBusTransport>()` extension method and provides low level access to various aspects of the transport's behavior.


## Configuring The Topology

A topology defines what the underlying layout of Azure Service Bus messaging entities looks like, specifically what entities are used and how they relate to each other. There are 2 built-in topologies: `EndpointOrientedTopology` and `ForwardingTopology`. For more information refer to the [Topologies](/nservicebus/azure-service-bus/topologies) article.

 * `UseForwardingTopology()`: Selects `ForwardingTopology` as the topology to be used by the transport.
 * `UseEndpointOrientedTopology()`: Selects `UseEndpointOrientedTopology` as the topology to be used by the transport.


### Endpoint Oriented Topology

The [Endpoint Oriented Topology](/nservicebus/azure-service-bus/topologies/#versions-7-and-above-endpoint-oriented-topology) defines a queue and a topic per endpoint. It can be configured using the following settings:

 * `RegisterPublisherForType(string, Type)`: Registers the publishing endpoint for a given type.
 * `RegisterPublisherForAssembly(string, Assembly)`: Registers the publishing endpoint for all types in an assembly.


## Controlling Entities

The topology will create entities it needs, based on the following settings:

 * `Queues()`: Settings for queues.
 * `Topics()`: Settings for topics.
 * `Subscriptions()`: Settings for subscriptions.


### Queues

The following settings are available to define how queues should be created:

 * `MaxSizeInMegabytes(SizeInMegabytes)`: The size of the queue, in megabytes. Defaults to 1,024 MB.
 * `MaxDeliveryCount(int)`: Sets the maximum delivery count, defaults to the number of Immediate Retries + 1. In case Immediate Retries are disabled, the transport will default `MaxDeliveryCount` to 10 attempts.
 * `LockDuration(TimeSpan)`: The period of time that Azure Service Bus will lock a message before trying to redeliver it, defaults to 30 seconds.
 * `ForwardDeadLetteredMessagesTo(string)`: Forward all dead lettered messages to the specified entity. This setting is off by default.
 * `ForwardDeadLetteredMessagesTo(Func<string, bool>, string)`: Forward all dead lettered messages to the specified entity if the given condition equals to `true` (e.g. it allows to exclude forwarding dead lettered messages on the error queue). This setting is off by default.
 * `DefaultMessageTimeToLive(TimeSpan)`: The maximum age of a message, defaults to `TimeSpan.MaxValue`.
 * `EnableDeadLetteringOnMessageExpiration(bool)`: Messages that expire will be dead lettered, defaults to `false`.
 * `EnableExpress(bool)`: Enables express mode, defaults to `false`. For more information refer to [MSDN](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.messaging.queuedescription#Microsoft_ServiceBus_Messaging_QueueDescription_EnableExpress)
 * `EnableExpress(Func<string, bool>, string)`: Enables express mode when the given condition is `true`.
 * `AutoDeleteOnIdle(TimeSpan)`: Automatically deletes the queue if it hasn't been used for the specified time period. By default the queue will not be automatically deleted.
 * `EnablePartitioning(bool)`: Enables partitioning, defaults to `false`. For more information on partitioning refer to the [Partitioned messaging entities](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-partitioning) article on MSDN.
 * `EnableBatchedOperations(bool)`: Enables server side batched operations, defaults to `true`.
 * `RequiresDuplicateDetection(bool)`: Specifies whether the queue should perform native broker duplicate detection, defaults to `false`.
 * `DuplicateDetectionHistoryTimeWindow(TimeSpan)`: The time period in which native broker duplicate detection should occur.
 * `SupportOrdering(bool)`: Best effort message ordering on the queue, defaults to `false`.
 * `DescriptionFactory(Func<string, ReadOnlySettings, QueueDescription>)`: A factory method that allows to create a `QueueDescription` object from the Azure Service Bus SDK. Use this factory method to override any (future) setting that is not supported by the Queues API.


### Topics

The following settings are available to define how topics should be created:

 * `MaxSizeInMegabytes(SizeInMegabytes)`: The size of the topic, in megabytes. Defaults to 1,024 MB.
 * `DefaultMessageTimeToLive(TimeSpan)`: The maximum age of a message, defaults to `TimeSpan.MaxValue`.
 * `AutoDeleteOnIdle(TimeSpan)`: Automatically deletes the topic if it hasn't been used for the specified time period. By default the topic will not be automatically deleted.
 * `EnableBatchedOperations(bool)`: Enables server side batched operations, defaults to `true`.
 * `EnableExpress(bool)`: Enables express mode, defaults to `false`. For more information refer to  the [TopicDescription.EnableExpress Property](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.messaging.topicdescription#Microsoft_ServiceBus_Messaging_TopicDescription_EnableExpress) documentation on MSDN.
 * `EnableExpress(Func<string, bool>, bool)`: Enables express mode when the given condition is `true`.
 * `EnableFilteringMessagesBeforePublishing(bool)`: Enables filtering messages before they are published, which validates that subscribers exist before a message is published. Defaults to `false`.
 * `EnablePartitioning(bool)`: Enables partitioning, defaults to `false`. For more information on partitioning refer to the [Partitioned messaging entities](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-partitioning) article on MSDN.
 * `RequiresDuplicateDetection(bool)`: Specifies whether the topic should perform native broker duplicate detection, defaults to `false`.
 * `DuplicateDetectionHistoryTimeWindow(TimeSpan)`: The time period in which native broker duplicate detection should occur.
 * `SupportOrdering(bool)`: Best effort message ordering on the topic, defaults to `false`.
 * `DescriptionFactory(Func<string, ReadOnlySettings, TopicDescription>)`: A factory method that allows to create a `TopicDescription` object from the Azure Service Bus SDK. Use this factory method to override any (future) setting that is not supported by the Topics API.


### Subscriptions

The following settings are available to define how subscriptions should be created:

 * `DefaultMessageTimeToLive(TimeSpan)`: The maximum age of a message, defaults to `TimeSpan.MaxValue`.
 * `EnableBatchedOperations(bool)`: Enables server side batched operations, defaults to `true`.
 * `EnableDeadLetteringOnFilterEvaluationExceptions(bool)`: Dead letters messages when a filter evaluation doesn't match, defaults to `false`.
 * `EnableDeadLetteringOnMessageExpiration(bool)`: Dead letters messages when they expire.
 * `ForwardDeadLetteredMessagesTo(string)`: Forwards all dead lettered messages to the specified entity. This setting is off by default.
 * `ForwardDeadLetteredMessagesTo(Func<string, bool>, string)`: Forwards all dead lettered messages to the specified entity if the given condition is `true`. This setting is off by default.
 * `LockDuration(TimeSpan)`: The period of time that Azure Service Bus will lock a message before trying to redeliver it, defaults to 30 seconds.
 * `MaxDeliveryCount(int)`: Sets the maximum delivery count, defaults to the number of Immediate Retries + 1. In case Immediate Retries are disabled, the transport will default `MaxDeliveryCount` to 10 attempts.  
 * `AutoDeleteOnIdle(TimeSpan)`: Automatically deletes the subscription if it hasn't been used for the specified time period. By default the subscription will not be automatically deleted.
 * `DescriptionFactory(Func<string, string, ReadOnlySettings, SubscriptionDescription>)`: A factory method that allows to create a `SubscriptionDescription` object from the Azure Service Bus SDK. Use this factory method to override any (future) setting that is not supported by the Subscription API.


## Controlling Connectivity

The following settings determine how NServiceBus will connect to Azure Service Bus:

 * `NumberOfClientsPerEntity(int)`: NServiceBus maintains a pool of receive and send clients for each entity. This setting determines how big that pool is. Defaults to 5.
 * `ConnectivityMode(ConnectivityMode)`: Determines how NServiceBus connects to Azure Service Bus, using [TCP, HTTPS or HTTP](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.connectivitymode). Defaults to TCP.
 * `TransportType(TransportType)`: Determines what transport protocol NServiceBus is using for Azure Service Bus, `TransportType.NetMessaging` or `TransportType.Amqp`. Defaults to `TransportType.NetMessaging`.
 * `BrokeredMessageBodyType(SupportedBrokeredMessageBodyTypes)`: Controls how the body of a brokered message will be serialized, either as a byte array or as a stream. Defaults to byte array.
 * `MessagingFactories()`: Provides access to settings of the used native instances of the class `MessagingFactory`. These settings will automatically apply to all `MessageReceiver` and `MessageSender` instances created by the `MessagingFactory`.
 * `MessageReceivers()`: Provides access to the settings of the `MessageReceiver` instances.
 * `MessageSenders()`: Provides access to the settings of the `MessageSender` instances.


### Messaging Factories

Messaging factories are the heart of connectivity management in the Azure Service Bus SDK. Each messaging factory maintains a TCP connection with the broker and creates `MessageSender` and `MessageReceiver` instances for that connection. This implies that all senders and receivers created by the same factory use the same underlying TCP connection and inherit the settings configured at the messaging factory level.

The following settings allow to control the messaging factories:

 * `NumberOfMessagingFactoriesPerNamespace(int)`: NServiceBus maintains a pool of messaging factories per namespace, this setting determines the size of the pool. Defaults to 5.
 * `RetryPolicy(RetryPolicy)`: Determines how entities should respond on transient connectivity failures. Defaults to `RetryPolicy.Default`, which is an exponential retry.
 * `BatchFlushInterval(TimeSpan)`: This setting controls the batching behavior for message senders. They will buffer send operations during this time frame and send all messages at once. Defaults to 0.5 seconds. Specify `TimeSpan.Zero` to turn batching off.
 * `MessagingFactorySettingsFactory(Func<string, MessagingFactorySettings>)`: This factory method allows to override creation of messaging factories.


### Message Receivers

The following settings determine how NServiceBus will receive messages from Azure Service Bus entities:

 * `ReceiveMode(ReceiveMode)`: Determines *"transactional behavior"*. Choosing `ReceiveMode.PeekLock` ensures that messages will be removed from the queue only after the processing has completed. If processing fails, or takes too long, the message will reappear on the queue for reprocessing. This behavior emulates transactional rollback. `ReceiveMode.ReceiveAndDelete` will delete the message immediately after receive, meaning there is no rollback when an exception occurs. Defaults to `ReceiveMode.PeekLock`. To learn more about the supported transactional behaviors in the Azure Service Bus transport, refer to the [Transaction Support in Azure](/nservicebus/azure-service-bus/transaction-support.md) article.
 * `AutoRenewTimeout(TimeSpan)`: When using `ReceiveMode.PeekLock`, the broker will lock a received message for a period specified as `LockDuration(TimeSpan)` on the entity. If processing doesn't end within this time period, a message will reappear on the entity and will be reprocessed. For long running operations this might imply that the operation will be executed multiple times in parallel, which is usually undesirable. This setting allows to extend the lock automatically when processing hasn't finished yet. The `TimeSpan` specified here refers to the maximum period that the lock can be extended for, so it should be bigger than the expected processing time. Note that it is not recommended to extend the lock duration, message processing should be fast by default.
 * `PrefetchCount(int)`: This setting will make the receiver prefetch messages on receive operations, acting like a client side batching mechanism. Defaults to 200 in `PeekLock` mode, meaning that each receive operation will pull in 200 messages at once. In `ReceiveAndDelete` mode prefetching is disabled by default and has to be enabled explicitly.
 * `RetryPolicy(RetryPolicy)`: Determines how the receiver should respond on transient connectivity failures. Defaults to `RetryPolicy.Default`, which is an exponential retry.


### Message Senders

The following settings determine how NServiceBus will send messages to Azure Service Bus entities:

 * `RetryPolicy(RetryPolicy)`: Determines how the sender should respond on transient connectivity failures. Defaults to `RetryPolicy.Default`, which is an exponential retry.
 * `BackOffTimeOnThrottle(TimeSpan)`: Similarly to all hosted services, Azure Service Bus indicates to clients to back off under heavy load. This setting specifies for how long NServiceBus will wait before trying to send a message again. Defaults to 10 seconds.
 * `RetryAttemptsOnThrottle(int)`: Defines for how many times the transport should attempt to retry a send operation before throwing an exception.
 * `MaximumMessageSizeInKilobytes(int)`: Specifies how large messages can be. This can differ between various types of namespaces. The default value of 256 aligns with Standard namespaces. Note that when message batching is enabled, this setting will also be applied as the maximum size of the batch.
 * `MessageSizePaddingPercentage(int)`: The Size property on a `BrokeredMessage` will provide accurate values only after the message was sent (for more details refer to the [BrokeredMessage.Size Property](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.messaging.brokeredmessage#Microsoft_ServiceBus_Messaging_BrokeredMessage_Size) documentation on MSDN). Therefore it can't be used to compute an accurate maximum batch size. NServiceBus has it's own batch size computation implementation, which requires providing an estimate for how much overhead (in %) will be caused by the serialization of the brokered message properties. The estimate is specified using `MessageSizePaddingPercentage`. Defaults to 5%.
 * `OversizedBrokeredMessageHandler<T>(T)`: This setting allows to override the transports behavior when a single message exceeds the maximum message size. The default behavior is represented by `ThrowOnOversizedBrokeredMessages` and will throw a `MessageTooLargeException`, suggesting to use the [Databus](/nservicebus/messaging/databus/).


## Transactional behavior

The Azure Service Bus transport can guarantee transactional behavior by combining the send operations and the completion of the receive operation in one atomic operation, providing that:

 * The source and destination are in the same namespace,
 * Messages are received using `ReceiveMode.PeekLock` mode,
 * The transport explicitly sends messages via the receive queue.
 * The destination entity does not excced its maximum size.


Enable the latter using the following configuration setting:

 * `SendViaReceiveQueue(bool)`: Uses the receive queue to dispatch outgoing messages when possible. Defaults to `true`.

 To learn more about the supported transactional behaviors in the Azure Service Bus transport, refer to the [Transaction Support in Azure](/nservicebus/azure-service-bus/transaction-support.md) article.

 
## Physical Addressing Logic

One of the responsibilities of the transport is determining the names and physical location of the entities. It is achieved by turning logical endpoint names into physical addresses of the Azure Service Bus entities, which is called *Physical Addressing Logic*. The following configuration settings allow to redefine this aspect of the transport:

 * `UseNamespaceNamesInsteadOfConnectionStrings()`: Causes the transport to pass around namespace names instead of raw connection strings in brokered message body headers.
 * `Sanitization()`: Provides access to the settings that determine how entity names are sanitized.
 * `Individualization()`: Provides access to the settings that determine how entity names are modified when used in different consumption modes.
 * `NamespacePartitioning()`: Provides access to the settings that determine how entities are partitioned across namespaces.
 * `Composition()`: Provides access to the settings that determine how entities are composed inside a single namespace.


### Sanitization

Sanitization refers to the cleanup logic that converts invalid entity names into valid ones. "Validation rules" are the individual logic blocks used to determine if entity names are valid. The rules implementations vary depending on the namespace type, and are changing over time (in some cases without notice and update of the [relevant MSDN documentation](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quotas)). The default settings align with the recently created Standard namespaces.

 * `UseQueuePathMaximumLength(int)`: The maximum length of a queue path (path = name + namespace hierarchy), defaults to 260.
 * `UseTopicPathMaximumLength(int)`: The maximum length of a topic path (path = name + namespace hierarchy), defaults to 260.
 * `UseSubscriptionPathMaximumLength(int)`: The maximum length of a subscription path (path = name), defaults to 50.
 * `UseRulePathMaximumLength(int)`: The maximum length of a rule path (path = name), defaults to 50.
 * `UseStrategy<T>()`: An implementation of `ISanitizationStrategy` that handles invalid entity names. The following implementations exist:
	 * `ThrowOnFailedValidation`: (default) throws an exception if the name is invalid.
	 * `ValidateAndHashIfNeeded`: allows customization of sanitization without creating a new strategy.

`UseStrategy<T>()` can be used to customize the selected [sanitization](/nservicebus/azure-service-bus/sanitization.md) strategy or completely replace it.


### Individualization

Individualization is the logic for modifying an entity name, to allow for differentiating between multiple instances of a single logical endpoint.

 * `UseStrategy<T>()`: An implementation of `IIndividualizationStrategy` that modifies an endpoint name to become unique per endpoint instance. Following implementations exist:
	 * `CoreIndividualization` (default): Makes no modifications, and relies on the individualization logic as defined in the NServiceBus core framework.
	 * `DiscriminatorBasedIndividualization`: modifies the name of the endpoint by appending a discriminator value to the end.


### Namespace Partitioning

The settings that determine how entities are partitioned across namespaces:

 * `AddNamespace(string, string)`: Adds a namespace (name and connection string) to the list of namespaces used by the namespace partitioning strategy.
 * `UseStrategy<T>`: An implementation of `INamespacePartitioningStrategy` that determines how entities are distributed across namespaces. The following strategies exist:
	 * `SingleNamespacePartitioning` (default): All entities are in a single namespace.
	 * `FailOverNamespacePartitioning`: assumes all entities are in the primary and secondary namespaces, where only the primary is in use by default. The secondary will function as a fallback in case of problems with the primary.
	 * `RoundRobinNamespacePartitioning`: assumes all entities are in all namespaces, all namespaces are in use but one at a time in a round robin fashion.


### Composition

The settings that determine how entities are composed inside a single namespace:

 * `UseStrategy<T>()`: An implementation of `ICompositionStrategy` that determines how an entity is positioned inside a namespace hierarchy. The following implementations exist:
	 * `FlatComposition`: The entity is in the root of the namespace.
	 * `HierarchyComposition`: The entity is in a namespace hierarchy, at the location generated by the path generator.
