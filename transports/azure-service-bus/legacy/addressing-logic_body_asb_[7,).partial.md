In Azure Service Bus transport version 7 and above, the configuration API allows modification of all behaviors and assumptions related to the addressing logic. This article will describe the different aspects of the addressing logic and will demonstrate how to replace parts if further changes should occur. For a full list of out of the box options, refer to [Full Configuration API](/transports/azure-service-bus/legacy/configuration/full.md).


## Addressing aspects

The addressing logic consists of:

 * **Sanitization**: How invalid entity names are cleaned up.
 * **Individualization**: How entity names are modified when used in different consumption modes.
 * **NamespacePartitioning**: How entities are partitioned across namespaces.
 * **Composition**: How entities are composed hierarchically within a single namespace.


## Sanitization

The sanitization aspect is represented by an implementation of `ISanitizationStrategy`.

Out of the box there are two sanitization strategies:

 * `ThrowOnFailingSanitization` (default): throws an exception if the name is invalid.
 * `ValidateAndHashIfNeeded`: removes invalid characters and hashes to reduce the length of an entity name if the maximum length is exceeded. By default, sanitization and hashing do nothing and [must be configured](/transports/azure-service-bus/legacy/sanitization.md#automated-sanitization).

The default implementation of this strategy can be replaced with the configuration API:

snippet: swap-sanitization-strategy


### Implementing a custom sanitization strategy

Creating a custom sanitization strategy requires a class that implements `ISanitizationStrategy`. This interface contains one method, `Sanitize`, which provides access to the entity path in case the entity is a queue or topic (or name if the entity is a subscription or rule), and returns a string that should contain a cleaned-up version of the entity path/name passed in. The entity type is passed in as the second parameter and is used to differentiate between queues, topics, and subscriptions if necessary.

If the implementation of a sanitization strategy requires configuration settings, these settings can be accessed by letting NServiceBus inject a `ReadOnlySettings` instance into the constructor of the strategy.

snippet: custom-sanitization-strategy


### Extending the configuration API for custom sanitization settings

In order to allow configuration of the custom sanitization strategy, create an extension method at the `AzureServiceBusSanitizationSettings` extension point, returned by `.Sanitization()`, in the NServiceBus configuration API. This provides access to the settings in which the value can be registered using a well-known key.

snippet: custom-sanitization-strategy-extension

## Individualization

The individualization aspect is represented by an implementation of `IIndividualizationStrategy`.

Out of the box, there are two individualization strategies:

 * `CoreIndividualization` (default): Makes no modifications, and relies on the individualization logic as defined in the NServiceBus core framework.
 * `DiscriminatorBasedIndividualization`: modifies the name of the endpoint by appending a discriminator value to the end.

The default implementation of this strategy can be replaced by using the configuration API:

snippet: swap-individualization-strategy


### Implementing a custom individualization strategy

Creating a custom individualization strategy requires a class that implements `IIndividualizationStrategy`. This interface contains one method, `Individualize`, which provides access to the current endpoint name, and returns a string that should contain a unique representation of the current endpoint instance. Note that depending on other settings, the endpoint name passed in may differ already from the one originally configured.

If the implementation of an individualization strategy requires configuration settings, these settings can be accessed by letting NServiceBus inject a `ReadOnlySettings` instance into the constructor of the strategy.

snippet: custom-individualization-strategy


### Extending the configuration API for custom individualization settings

In order to allow configuration of the custom individualization strategy, create an extension method at the `AzureServiceBusIndividualizationSettings` extension point, returned by `.Individualization()`, in the NServiceBus configuration API. This provides access to the settings in which the value can be registered using a well-known key.

snippet: custom-individualization-strategy-extension


## Namespace partitioning

Namespace partitioning is represented by an implementation of `INamespacePartitioningStrategy`.

Out of the box there are three namespace partitioning strategies:

 * `SingleNamespacePartitioning` (default): All entities are in a single namespace.
 * `FailOverNamespacePartitioning`: assumes all entities are in the primary and secondary namespaces, where only the primary is in use by default. The secondary will function as a fallback in case of problems with the primary.
 * `RoundRobinNamespacePartitioning`: assumes all entities are in all namespaces, all namespaces are in use but one at a time in a round robin fashion.

The default implementation of this strategy can be replaced by using the configuration API:

snippet: swap-namespace-partitioning-strategy


### Implementing a custom namespace partitioning strategy

Creating a custom namespace partitioning strategy requires a class that implements `INamespacePartitioningStrategy`. This interface contains one method, `GetNamespaces`, which provides access to the `PartitioningIntent` for which namespaces are requested and returns a set of namespaces, represented by the `RuntimeNamespaceInfo` class, that should be used for the specified intent.

The `PartitioningIntent` is to be interpreted as follows:

 * `Creating`: returns a set of namespaces in which entities should be created at startup, usually all namespaces.
 * `Receiving`: returns a set of namespaces to which the transport should listen, usually all namespaces as well.
 * `Sending`: returns a set of namespaces to which the next send/publish operation should go. In this operation it's important to differentiate between `NamespaceMode.Active` and `NamespaceMode.Passive`. The transport will send the message to **ALL** active namespaces and if one of those send operations fails it will attempt to execute that operation on one of the passive namespaces returned.

If the implementation of a namespace partitioning strategy requires configuration settings, these settings can be accessed by letting NServiceBus inject the `ReadOnlySettings` into the constructor of the strategy.

snippet: custom-namespace-partitioning-strategy


### Extending the configuration API for custom partitioning settings

In order to allow configuration of the custom namespace partitioning strategy, create an extension method at the `AzureServiceBusNamespacePartitioningSettings` extension point, returned by `.NamespacePartitioning()`, in the NServiceBus configuration API. This provides access to the settings in which the value can be registered using a well-known key.

snippet: custom-namespace-partitioning-strategy-extension


## Composition

The composition aspect is represented by an implementation of `ICompositionStrategy`.

Out of the box there are two composition strategies:

 * `FlatComposition` (default): The entity is in the root of the namespace.
 * `HierarchyComposition`: The entity is in a namespace hierarchy at the location generated by the path generator.

The default implementation of this strategy can be replaced by using the configuration API:

snippet: swap-composition-strategy


### Implementing a custom composition strategy

Creating a custom composition strategy requires a class that implements `ICompositionStrategy`. This interface contains one method, `GetEntityPath`, which provides access to an entity name as well as the type of the entity in question, and returns a string that should contain an entity path representing the location of the entity inside an Azure Service Bus namespace. Note that only queues and topics support positioning in a namespace hierarchy; subscriptions will always sit underneath the topic that they are being subscribed too, so only prefix the name with path information for `EntityType.Queue` and `EntityType.Topic`.

If the implementation of a composition strategy requires configuration settings, these settings can be accessed by letting NServiceBus inject the `ReadOnlySettings` into the constructor of the strategy.

snippet: custom-composition-strategy


### Extending the configuration API for custom composition settings

In order to allow configuration of the custom composition partitioning strategy, create an extension method at the `AzureServiceBusCompositionSettings` extension point, returned by `.Composition()`, in the NServiceBus configuration API. This provides access to the settings in which the value can be registered using a well-known key.

snippet: custom-composition-strategy-extension
