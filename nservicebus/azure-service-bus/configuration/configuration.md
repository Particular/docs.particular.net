---
title: Full Azure Service Bus Configuration API
summary: List of all Azure Service Bus Transport configuration settings
tags:
- Azure
- Cloud
- Configuration
---

## Full Configuration API

The full configuration API can be accessed from the `UseTransport<AzureServiceBusTransport>()` extension method and provides low level access to almost all aspects of the transport's behavior.
 
## Configuring The Topology
 
A topology defines what the underlying layout of Azure Service Bus messaging entities looks like, which ones are used and how they relate to each other. There are 2 topologies built in into the transport `EndpointOrientedTopology` and `ForwardingTopology`.

 * `UseTopology<T>()`: Specifies which topology should be used by the transport, instantiating it using the default constructor.
 * `UseTopology<T>(Func<T>)`: Specifies which topology should be used by the transport, instantiating it using a factory method.
 * `UseTopology<T>(T)`: Specifies which topology should be used by the transport, instance created by the calling code.
 
### Endpoint Oriented Topology

The endpoint oriented topology defines a queue and a topic per endpoint. This model causes the side effect that subscribing endpoints also need to know which publishing endpoints are responsible for publishing a given event type. The following configuration settings allow to define this relationship.

 * `RegisterPublisherForType(string, Type)`: Registers the publishing endpoint for a given type.
 * `RegisterPublisherForAssembly(string, Assembly)`: Registers the publishing endpoint for all types in an assembly.

### Forwarding Topology

The forwarding topology defines a queue per endpoint and a shared set of topics to do the publishing. The Azure Service Bus forwarding feature is used to forward messages from subscriptions on the shared set of topics to the input queue of each endpoint. As forwarding cannot be used in conjunction with partitioning, NServiceBus will perform the partitioning instead using a bundle of topics. 

 * `NumberOfEntitiesInBundle(int)`: The number of topics in the bundle, defaults to 2.
 * `BundlePrefix(string)`: The prefix used in the entity name to differentiate entities from the bundle from other entities.
 
## Controlling Entities

The topology will create the entities that it needs based on the following settings: 

 * `Queues()`: Settings for queues.
 * `Topics()`: Settings for topics.
 * `Subscriptions()`: Settings for subscriptions.

### Queues

The following settings are available to define how queues should be created.

 * `MaxSizeInMegabytes(SizeInMegabytes)`: The size of the queue, in megabytes.
 * `MaxDeliveryCount(int)`: Sets the maximum delivery count, defaults to 10.
 * `LockDuration(TimeSpan)`: The period of time that Azure Service Bus will lock a message before trying to redeliver it, defaults to 30 seconds.
 * `ForwardDeadLetteredMessagesTo(string)`: Forward all dead lettered message to the specified entity.
 * `ForwardDeadLetteredMessagesTo(Func<string, bool>, string)`: Forward all dead lettered message to the specified entity if the given condition is true (allows to exclude forwarding dead letters on the error queue for example).
 * `DefaultMessageTimeToLive(TimeSpan)`: The maximum age of a message, defaults to TimeSpan.MaxValue.
 * `EnableDeadLetteringOnMessageExpiration(bool)`: Messages that expire will be dead lettered, defaults to false.
 * `EnableExpress(bool)`: Enables express mode, defaults to false.
 * `EnableExpress(Func<string, bool>, string)`: Enables express mode when the given condition is true.
 * `AutoDeleteOnIdle(TimeSpan)`: Auto deletes the entity if it hasn't been used for the given time frame.
 * `EnablePartitioning(bool)`: Enables entity partitioning, defaults to false.
 * `EnableBatchedOperations(bool)`: Enables server side batched operations, defaults to true.
 * `RequiresDuplicateDetection(bool)`: Specifies whether the queue should perform duplicate detection, defaults to false.
 * `DuplicateDetectionHistoryTimeWindow(TimeSpan)`: The time period in which duplicate detection should occur. 
 * `SupportOrdering(bool)`: Best effort message ordering on the queue, defaults to false.
 * `DescriptionFactory(Func<string, ReadOnlySettings, QueueDescription>)`: A factory method that allows to create a QueueDescription object from the Azure Service Bus SDK. Use this factory method to override any (future) setting that is not supported by the Queues API.

### Topics
 
The following settings are available to define how topics should be created.

 * `MaxSizeInMegabytes(SizeInMegabytes)`: The size of the topic, in megabytes.
 * `DefaultMessageTimeToLive(TimeSpan)`: The maximum age of a message, defaults to TimeSpan.MaxValue. 
 * `AutoDeleteOnIdle(TimeSpan)`: Auto deletes the entity if it hasn't been used for the given time frame.
 * `EnableBatchedOperations(bool)`: Enables server side batched operations, defaults to true.
 * `EnableExpress(bool)`: Enables express mode, defaults to false.
 * `EnableExpress(Func<string, bool>, bool)`: Enables express mode when the given condition is true.
 * `EnableFilteringMessagesBeforePublishing(bool)`: Enables message filtering before they are published, defaults to false.
 * `EnablePartitioning(bool)`: Enables entity partitioning, defaults to false.
 * `RequiresDuplicateDetection(bool)`: Specifies whether the topic should perform duplicate detection, defaults to false.
 * `DuplicateDetectionHistoryTimeWindow(TimeSpan)`: The time period in which duplicate detection should occur. 
 * `SupportOrdering(bool)`: Best effort message ordering on the topic, defaults to false.
 * `DescriptionFactory(Func<string, ReadOnlySettings, TopicDescription>)`: A factory method that allows to create a TopicDescription object from the Azure Service Bus SDK. Use this factory method to override any (future) setting that is not supported by the Topics API.

### Subscriptions

The following settings are available to define how subscriptions should be created.
 
 * `DefaultMessageTimeToLive(TimeSpan)`: The maximum age of a message, defaults to TimeSpan.MaxValue. 
 * `EnableBatchedOperations(bool)`: Enables server side batched operations, defaults to true.
 * `EnableDeadLetteringOnFilterEvaluationExceptions(bool)`: Dead letters messages when a filter evaluation doesn't match, defaults to false.
 * `EnableDeadLetteringOnMessageExpiration(bool)`: Dead letters messages when they expire.
 * `ForwardDeadLetteredMessagesTo(string)`: Forward all dead lettered message to the specified entity.
 * `ForwardDeadLetteredMessagesTo(Func<string, bool>, string)`: Forward all dead lettered message to the specified entity if the given condition is true.
 * `LockDuration(TimeSpan)`: The period of time that Azure Service Bus will lock a message before trying to redeliver it, defaults to 30 seconds.
 * `MaxDeliveryCount(int)`: Sets the maximum delivery count, defaults to 10.
 * `AutoDeleteOnIdle(TimeSpan)`: Auto deletes the entity if it hasn't been used for the given time frame.
 * `DescriptionFactory(Func<string, string, ReadOnlySettings, SubscriptionDescription>)`: A factory method that allows to create a SubscriptionDescription object from the Azure Service Bus SDK. Use this factory method to override any (future) setting that is not supported by the Subscription API.
 
## Controlling Connectivity

The following settings determine how NServiceBus will connect to Azure Service Bus.
 
 * `NumberOfClientsPerEntity(int)`: NServiceBus maintains a small pool of receive and send clients for each entity, this setting determines how big that pool is. Defaults to 5.
 * `ConnectivityMode(ConnectivityMode)`: Determines how NServiceBus connects to Azure Service Bus, using TCP or HTTP. Defaults to TCP. 
 * `BrokeredMessageBodyType(SupportedBrokeredMessageBodyTypes)`: Controls how the body of a brokered will be serialized as a byte array or as a stream. Defaults to byte array.
 * `MessagingFactories()`: Provides access to settings of MessagingFactory instances used under the hood. These settings will automatically apply to all MessageReceiver and MessageSender instances created by the MessagingFactory.
 * `MessageReceivers()`: Provides access to the settings of the MessageReceiver instances.
 * `MessageSenders()`: Provides access to the settings of the MessageSender instances.

### Messaging Factories

Messaging factories are the heart of connectivity management in the Azure Service Bus SDK, each messaging factory maintains a TCP connection with the broker and creates MessageSender and MessageReceiver instances for that connection. This implies that all senders and receivers created by the same factory, use the same underlying TCP connection and inherit the settings configured at the messaging factory level. The following settings allow to control the messaging factories.

 * `NumberOfMessagingFactoriesPerNamespace(int)`: NServiceBus maintains a small pool of messaging factories per namespace, this setting determines the size of the pool. Defaults to 5.
 * `RetryPolicy(RetryPolicy)`: Determines how entities should respond on transient connectivity failures. Defaults to `RetryPolicy.Default`, which is an exponential retry under the hood.
 * `BatchFlushInterval(TimeSpan)`: This setting controls the batching behavior for message senders. They will buffer send operations during this time frame and send all messages at once. Defaults to 0.5 seconds. Specify TimeSpan.Zero to turn batching off.
 * `MessagingFactorySettingsFactory(Func<string, MessagingFactorySettings>)`: This factory method allows to override creation of messaging factories.
 
### Message Receivers

The following settings determine how NServiceBus will receive messages from Azure Service Bus entities.

 * `ReceiveMode(ReceiveMode)`: Determines part of the 'transactional behavior'. `ReceiveMode.PeekLock` ensure that messages will be only removed from the queue after processing is done. If processing fails, or takes to long, the message will reappear on the queue for re-processing. Basically emulating transactional rollback. `ReceiveMode.ReceiveAndDelete` will delete the message immediately after receive, meaning there is no rollback when an exception occurs. Defaults to `ReceiveMode.PeekLock`.
 * `PrefetchCount(int)`: This setting will make the receiver prefetch messages on receive operations, and acts like client side batching. Defaults to 200, meaning that each receive operation will pull in another 200 messages.
 * `RetryPolicy(RetryPolicy)`: Determines how the receiver should respond on transient connectivity failures. Defaults to `RetryPolicy.Default`, which is an exponential retry under the hood.
 * `AutoRenewTimeout(TimeSpan)`: When using `ReceiveMode.PeekLock`, the broker will lock a received message for a period specified as `LockDuration(TimeSpan)` on the entity. If processing doesn't end within this time period, a message will reappear on the entity and will be reprocessed. For long running operations this might imply that the operation will be executed multiple times in parallel, which is usually undesirable. This setting allows to extend the lock automatically when processing hasn't finished yet. Ensure that the `TimeSpan` specified here is a little smaller than the `TimeSpan` specified as `LockDuration`.

### Message Senders

The following settings determine how NServiceBus will send messages to Azure Service Bus entities.

 * `RetryPolicy(RetryPolicy)`: Determines how the sender should respond on transient connectivity failures. Defaults to `RetryPolicy.Default`, which is an exponential retry under the hood.
 * `BackOffTimeOnThrottle(TimeSpan)`: 'As A Service' products will tell there clients to back off under heavy load and so will Azure Service Bus. This setting specifies how long NServiceBus will wait to try sending again in these occurances. Defaults to 10 seconds. Note, by default this backoff period will increase exponentially during retries. 
 * `RetryAttemptsOnThrottle(int)`: Defines how many times the transport should attempt to retry a send operation when being operated before throwing an exception.
 * `MaximumMessageSizeInKilobytes(int)`: Specifies how large messages (and batches of messages) can be. This can differ between different types of namespaces. The default value of 256 aligns with Standard namespaces.
 * `MessageSizePaddingPercentage(int)`: As specified in the [msdn documentation](https://msdn.microsoft.com/en-us/library/microsoft.servicebus.messaging.brokeredmessage.size.aspx), the Size property on a BrokeredMessage object will only provide accurate values after sending a message, which turns it pretty useless to compute a maximum batch size for batching. Therefore NServiceBus implements it's own batch size computation. Part of this computation requires a guesstimate of how much overhead will be caused by serialization of the brokered message properties. This percentage represents that guesstimate and defaults to 5%.
 * `OversizedBrokeredMessageHandler<T>(T)`: This setting allows to override the transports behavior when a single message exceeds the maximum message size. The default behavior is represented by `ThrowOnOversizedBrokeredMessages` and will throw a `MessageTooLargeException`, informing to consider using the Databus feature instead.

## Transactional behavior

Under certain conditions, the Azure Service Bus transport can guarantee transactional behavior by combining the send operations and completion of the receive operation in one atomic operation. These conditions include that the source and destination are in the same namespace, that messages are received using `ReceiveMode.PeekLock` and that the transport explicitly sends messages via the receive queue. The latter can be configured using:

  * `SendViaReceiveQueue(bool)`: Uses the receive queue to dispatch outgoing messages when possible. Defaults to true.

## Physical Addressing Logic

Next to managing connectivity and the layout of messaging entities, another important task of the transport is to figure out how these entities are named and where they can actually be found in the (seemingly infinite) cloud. This is referred to as the 'Physical Addressing Logic', and has the responsibility to turn logical endpoint names into physical addresses of Azure Service Bus entities. The following configuration sections allow to redefine this aspect of the transport.

 * `UseNamespaceNamesInsteadOfConnectionStrings()`: Causes the transport to pass around namespace names instead of raw connection strings in brokered message body headers.
 * `Validation()`: Provides access to the settings that determine how entity names are validated.
 * `Sanitization()`: Provides access to the settings that determine how invalid entity names are cleaned up to become valid.
 * `Individualization()`: Provides access to the settings that determine how entity names are modified when usedin different consumption models.
 * `NamespacePartitioning()`: Provides access to the settings that determine how entities are partitioned across namespaces.
 * `Composition()`: Provides access to the settings that determine how entities are composed inside a single namespace.
 
### Validation

These settings determine how entity names are validated. These settings should correspond to the actual validation rules in place in the Azure Service Bus namespace configured. Different types of namespaces have different implementations and these implementations have changed over time as well (without notice and sometimes confusing/outdated msdn documentation). The default settings align with recently created Standard namespaces.

 * `UseQueuePathMaximumLength(int)`: The maximum length of a queue path (path = name + namespace hierarchy), defaults to 260.
 * `UseTopicPathMaximumLength(int)`: The maximum length of a topic path (path = name + namespace hierarchy), defaults to 260.
 * `UseSubscriptionPathMaximumLength(int)`: The maximum length of a subscription path (path = name), defaults to 50.
 * `UseStrategy<T>()`: An implementation of `IValidationStrategy` that validates the entity path. Following implementations exist:
	 * `EntityNameValidationV6Rules`: allows letters, numbers, periods (.), hyphens (-), and underscores (-)
	 * `EntityNameValidationRules` (default): allows letters, numbers, periods (.), hyphens (-), underscores (-) and slashes (/) for queues and topics, no slashes allowed for subscriptions.

### Sanitization

Sanitization refers to the cleanup logic, that turns invalid entity names into valid ones.

 * `UseStrategy<T>()`: An implementation of `ISanitizationStrategy` that handles invalid entity names. The following implementations exist:
	 * `AdjustmentSanitizationV6`: removes invalid characters according to `EntityNameValidationV6Rules`, uses MD5 hashing to reduce the length of an entity name if the maximum length is exceeded.
	 * `AdjustmentSanitization` (default): removes invalid characters according to `EntityNameValidationRules`, uses SHA1 hashing to reduce the length of an entity name if the maximum length is exceeded.
	 * `ThrowOnFailingSanitization`: throws an `EndpointValidationException` if the name is invalid.

### Individualization

Individualization refers to the logic that modifies an entity name so that the entity can be differentiated (or not) between different instances of the same endpoint.

 * `UseStrategy<T>()`: An implementation of `IIndividualizationStrategy` that modifies an endpoint name to become unique per endpoint instance. Following implementations exist:
	 * `CoreIndividualization` (default): Makes no modifications, and relies on the individualization logic as defined in the NServiceBus core framework.
	 * `DiscriminatorBasedIndividualization`: modifies the name of the endpoint by appending a discriminator value to the end.
 
### Namespace Partitioning

The settings that determine how entities are partitioned across namespaces.

 * `AddNamespace(string, string)`: Adds a namespace (name and connection string) to the list of namespaces used by the namespace partitioning strategy.
 * `UseStrategy<T>`: An implementation of `INamespacePartitioningStrategy` that determines how entities are distributed across namespaces. The following strategies exist:
	 * `SingleNamespacePartitioning` (default): assumes all entities are in the same namespace.
	 * `FailOverNamespacePartitioning`: assumes all entities are in the primary and secondary namespaces, where only the primary is in use by default. The secondary will function as a fallback in case of problems with the primary.
	 * `RoundRobinNamespacePartitioning`: assumes all entities are in all namespaces, all namespaces are in use but one at a time in a round robin fashing.
	 * `ReplicatedNamespacePartitioning`: assumes all entities are in all namespaces, all namespaces are in use, messages are replicated over all namespaces.
	 * `ShardedNamespacePartitioning`: assumes all entities are in all namespaces, all namespaces are in use, messages are sent to a single namespace based on a sharding rule.

### Composition

The settings that determine how entities are composed inside a single namespace.

 * `UseStrategy<T>()`: An implementation of `ICompositionStrategy` that determines how an entity is positioned inside a namespace hierarchy. The following implementations exist:
	 * `FlatComposition`: The entity is in the root of the namespace.
	 * `HierarchyComposition`: The entity is in a namespace hierarchy, at the path generated by the path generator.