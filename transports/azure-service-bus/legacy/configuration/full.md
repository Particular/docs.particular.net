---
title: Full Configuration API
summary: List of all Azure Service Bus Transport configuration settings
component: ASB
versions: '[7,)'
redirects:
 - nservicebus/azure-service-bus/configuration/configuration
 - nservicebus/azure-service-bus/configuration/full
 - transports/azure-service-bus/configuration/full
reviewed: 2020-09-14
---

include: legacy-asb-warning


INFO: This document is intended for Versions 7 and above.

The full configuration API can be accessed from the `UseTransport<AzureServiceBusTransport>()` extension method and provides low level access to various aspects of the transport's behavior.


## Configuring the topology

A topology defines what the underlying layout of Azure Service Bus messaging entities looks like, specifically what entities are used and how they relate to each other. There are 2 built-in topologies: `EndpointOrientedTopology` and `ForwardingTopology`. For more information, refer to the [Topologies](/transports/azure-service-bus/legacy/topologies.md) article.

 * `UseForwardingTopology()`: Selects `ForwardingTopology` as the topology to be used by the transport.
 * `UseEndpointOrientedTopology()`: Selects `EndpointOrientedTopology` as the topology to be used by the transport.

### Forwarding topology

The [forwarding topology](/transports/azure-service-bus/legacy/topologies.md#versions-7-and-above-forwarding-topology) defines a queue per endpoint and a shared topic to do the publishing. Topic prefix can be configured using the following setting:

 * `BundlePrefix(string)`: The prefix used in the entity name to differentiate shared topic (bundle) from other topics.

### Endpoint-oriented topology

The [endpoint-oriented topology](/transports/azure-service-bus/legacy/topologies.md#versions-7-and-above-endpoint-oriented-topology) defines a queue and a topic per endpoint. It can be configured using the following settings:

 * `RegisterPublisherForType(string, Type)`: Registers the publishing endpoint for a given type.
 * `RegisterPublisherForAssembly(string, Assembly)`: Registers the publishing endpoint for all types in an assembly.

partial: migration


## Controlling entities

The topology will create entities it needs, based on the following settings:

 * `Queues()`: Settings for queues.
 * `Topics()`: Settings for topics.
 * `Subscriptions()`: Settings for subscriptions.


### Queues

The following settings are available to define how queues should be created:

 * `MaxSizeInMegabytes(SizeInMegabytes)`: The size of the queue, in megabytes. Defaults to 1,024 MB.
 * `LockDuration(TimeSpan)`: The period of time that Azure Service Bus will lock a message before trying to redeliver it. Defaults to 30 seconds.
 * `ForwardDeadLetteredMessagesTo(string)`: Forwards all dead lettered messages to the specified entity. This setting is set to off by default.
 * `ForwardDeadLetteredMessagesTo(Func<string, bool>, string)`: Forwards all dead lettered messages to the specified entity if the given condition is true (e.g. excluding forwarding dead lettered messages in the error queue). This setting is set to off by default.
 * `DefaultMessageTimeToLive(TimeSpan)`: The maximum age of a message. Defaults to `TimeSpan.MaxValue`.
 * `EnableDeadLetteringOnMessageExpiration(bool)`: Messages that expire will be dead lettered. Defaults to `false`.
 * `AutoDeleteOnIdle(TimeSpan)`: Automatically deletes the queue if it hasn't been used for the specified time period. By default the queue will not be automatically deleted.
 * `EnablePartitioning(bool)`: Enables partitioning. Defaults to `false`. For more information on partitioning refer to the [partitioned messaging entities](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-partitioning) article on MSDN.
 * `EnableBatchedOperations(bool)`: Enables server side batched operations. Defaults to `true`.
 * `RequiresDuplicateDetection(bool)`: Specifies whether the queue should perform native broker duplicate detection. Defaults to `false`.
 * `DuplicateDetectionHistoryTimeWindow(TimeSpan)`: The time period in which native broker duplicate detection should occur.
 * `SupportOrdering(bool)`: Best effort message ordering on the queue. Defaults to `false`.
partial: queues


### Topics

The following settings are available to define how topics should be created:

 * `MaxSizeInMegabytes(SizeInMegabytes)`: The size of the topic, in megabytes. Defaults to 1,024 MB.
 * `DefaultMessageTimeToLive(TimeSpan)`: The maximum age of a message. Defaults to `TimeSpan.MaxValue`.
 * `AutoDeleteOnIdle(TimeSpan)`: Automatically deletes the topic if it hasn't been used for the specified time period. By default the topic will not be automatically deleted.
 * `EnableBatchedOperations(bool)`: Enables server side batched operations. Defaults to `true`.
 * `EnableFilteringMessagesBeforePublishing(bool)`: Enables filtering messages before they are published, which validates that subscribers exist before a message is published. Defaults to `false`.
 * `EnablePartitioning(bool)`: Enables partitioning, defaults to `false`. For more information on partitioning refer to the [partitioned messaging entities](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-partitioning) article on MSDN.
 * `RequiresDuplicateDetection(bool)`: Specifies whether the topic should perform native broker duplicate detection. Defaults to `false`.
 * `DuplicateDetectionHistoryTimeWindow(TimeSpan)`: The time period in which native broker duplicate detection should occur.
 * `SupportOrdering(bool)`: Best effort message ordering on the topic. Defaults to `false`.
partial: topics


### Subscriptions

The following settings are available to define how subscriptions should be created:

 * `DefaultMessageTimeToLive(TimeSpan)`: The maximum age of a message. Defaults to `TimeSpan.MaxValue`.
 * `EnableBatchedOperations(bool)`: Enables server side batched operations. Defaults to `true`.
 * `EnableDeadLetteringOnFilterEvaluationExceptions(bool)`: Dead letters messages when a filter evaluation doesn't match. Defaults to `false`.
 * `EnableDeadLetteringOnMessageExpiration(bool)`: Dead letters messages when they expire.
 * `ForwardDeadLetteredMessagesTo(string)`: Forwards all dead lettered messages to the specified entity. This setting is set to off by default.
 * `ForwardDeadLetteredMessagesTo(Func<string, bool>, string)`: Forwards all dead lettered messages to the specified entity if the given condition is `true`. This setting is off by default.
 * `LockDuration(TimeSpan)`: The period of time that Azure Service Bus will lock a message before trying to redeliver it. Defaults to 30 seconds.
 * `AutoDeleteOnIdle(TimeSpan)`: Automatically deletes the subscription if it hasn't been used for the specified time period. By default the subscription will not be automatically deleted.
partial: subscriptions


## Controlling Connectivity

The following settings determine how NServiceBus will connect to Azure Service Bus:

 * `NumberOfClientsPerEntity(int)`: NServiceBus maintains a pool of receive and send clients for each entity. This setting determines how big that pool is. Defaults to `max(Number of logical processors, 2)`.
 * `ConnectivityMode(ConnectivityMode)`: Determines how NServiceBus connects to Azure Service Bus, using [TCP, HTTPS or HTTP](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.connectivitymode). Defaults to TCP.
 * `TransportType(TransportType)`: Determines what transport protocol NServiceBus is using for Azure Service Bus, `TransportType.NetMessaging` or `TransportType.Amqp`. Defaults to `TransportType.NetMessaging`.
 * `BrokeredMessageBodyType(SupportedBrokeredMessageBodyTypes)`: Controls how the body of a brokered message will be serialized, either as a byte array or as a stream. Defaults to byte array.
 * `MessagingFactories()`: Provides access to settings of the used native instances of the class `MessagingFactory`. These settings will automatically apply to all `MessageReceiver` and `MessageSender` instances created by the `MessagingFactory`.
 * `MessageReceivers()`: Provides access to the settings of the `MessageReceiver` instances.
 * `MessageSenders()`: Provides access to the settings of the `MessageSender` instances.


### Messaging Factories

Messaging factories are the heart of connectivity management in the Azure Service Bus SDK. Each messaging factory maintains a TCP connection with the broker and creates `MessageSender` and `MessageReceiver` instances for that connection. This implies that all senders and receivers created by the same factory use the same underlying TCP connection and inherit the settings configured at the messaging factory level.

The following settings allow to control the messaging factories:

 * `NumberOfMessagingFactoriesPerNamespace(int)`: NServiceBus maintains a pool of messaging factories per namespace, this setting determines the size of the pool. Defaults to `max(Number of logical processors, 2)`.
 * `RetryPolicy(RetryPolicy)`: Determines how entities should respond on transient connectivity failures. Defaults to `RetryPolicy.Default`, which is an exponential retry.
 * `MessagingFactorySettingsFactory(Func<string, MessagingFactorySettings>)`: This factory method allows to override creation of messaging factories.
partial: batchflushinterval


### Message Receivers

The following settings determine how NServiceBus will receive messages from Azure Service Bus entities:

 * `ReceiveMode(ReceiveMode)`: Determines *"transactional behavior"*. Choosing `ReceiveMode.PeekLock` ensures that messages will be removed from the queue only after the processing has completed. If processing fails, or takes too long, the message will reappear on the queue for reprocessing. This behavior emulates transactional rollback. `ReceiveMode.ReceiveAndDelete` will delete the message immediately after receive, meaning there is no rollback when an exception occurs. Defaults to `ReceiveMode.PeekLock`. To learn more about the supported transactional behaviors in the Azure Service Bus transport, refer to the [Transaction Support in Azure](/transports/azure-service-bus/legacy/transaction-support.md) article.
 * `AutoRenewTimeout(TimeSpan)`: When using `ReceiveMode.PeekLock`, the broker will lock a received message for a period specified as `LockDuration(TimeSpan)` on the entity. If processing doesn't end within this time period, a message will reappear on the entity and will be reprocessed. For long running operations this might imply that the operation will be executed multiple times in parallel, which is usually undesirable. This setting allows to extend the lock automatically when processing hasn't finished yet. The `TimeSpan` specified here refers to the maximum period that the lock can be extended for, so it should be bigger than the expected processing time. Note that it is not recommended to extend the lock duration, message processing should be fast by default.
 * `PrefetchCount(int)`: This setting will make the receiver prefetch messages on receive operations, acting like a client side batching mechanism. Defaults to 20 in `PeekLock` mode, meaning that each receive operation will pull in 20 messages at once. In `ReceiveAndDelete` mode prefetching is disabled by default and has to be enabled explicitly.
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

To learn more about the supported transactional behaviors in the Azure Service Bus transport, refer to the [Transaction Support in Azure](/transports/azure-service-bus/legacy/transaction-support.md) article.


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

`UseStrategy<T>()` can be used to customize the selected [sanitization](/transports/azure-service-bus/legacy/sanitization.md) strategy or completely replace it.


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
   * `FlatComposition`: The entity is in the root of the namespace. Default strategy.
   * `HierarchyComposition`: The entity is in a namespace hierarchy, at the location generated by the path generator. This composition strategy is idempotent.

Note: When implementing a custom composition strategy, idempotency needs to be taken into consideration to ensure an entity path is not altered when strategy is invoked more than once.
