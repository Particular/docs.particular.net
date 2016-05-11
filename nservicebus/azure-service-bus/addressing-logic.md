---
title: Azure Service Bus Transport Addressing Logic
summary: Azure ServiceBus Transport Addressing Logic
tags:
- Azure
- Cloud
- Configuration
---

## Physical Addressing Logic

One of the responsibilities of the transport is determining the names and physical location of entities in the underlying physical infrastructure. This is achieved by turning logical endpoint names into physical addresses of the Azure Service Bus entities, which is called *Physical Addressing Logic*.

Prior to version 7 there was no explicit layer for handling this kind of logic, most assumptions such as assumptions on length limitations, legal characters, differences between paths and names etc... were sprinkled across the transport.

Over time, subtle variations started to show in these assumptions as changes took place inside the implementation of certain namespace types. For example `Mixed` namespaces allowed paths and names up 290 characters, while `Messaging` namespaces are capped at 260. Next to that, customers also figured out ways to game the system (for valid reasons), like embedding `'/'` characters in endpoint names to create subfolders in the Azure Service Bus namespace registration system.

### Version 6 and below

In older versions some of these changes got exposed over time as lambda expressions and became collectively known as the [Naming Conventions](/nservicebus/azure-service-bus/naming-conventions.md#version-6-and-below-naming-conventions)

### Version 7

In version 7 the configuration API allows to modify all behaviours and assumptions related to the addressing logic. This article will mainly focus on describing the layers inside the addressing logic and will show how to replace parts if further changes should occur, for a full list of out of the box options refer to [Full Configuration API](/nservicebus/azure-service-bus/configuration/configuration.md).

### Addressing layers

The following layers are found in the addressing logic:

 * Validation: Determines how entity names and paths are validated.
 * Sanitization: Determines how invalid entity names are cleaned up.
 * Individualization: Determines how entity names are modified when used in different consumption modes.
 * NamespacePartitioning: Determines how entities are partitioned across namespaces.
 * Composition: Determines how entities are hierarchically composed inside a single namespace.
 
### Validation

The validation layer is represented by an implementation of `IValidationStrategy`. 

Out of the box there are 2 validation strategies
 * `EntityNameValidationV6Rules`: allows letters, numbers, periods (.), hyphens (-), and underscores (-)
 * `EntityNameValidationRules` (default): allows letters, numbers, periods (.), hyphens (-), underscores (-) and slashes (/) for queues and topics, no slashes allowed for subscriptions.

The default implementation of this strategy can be replaced by using the configuration API:

snippet:swap-validation-strategy

#### Implementing a custom validation strategy

Implementing a custom validation strategy requires a class that implements `IValidationStrategy`. This interface contains one method, called `IsValid`, which provides access to the entity path in case the entity is a queue or topic (or name if the entity is a subscription), and returns a boolean indicating if that name is invalids. The entity type is passed in as second parameter and can be used to differentiate between queues, topics and subscription.

If the implementation of a validation strategy requires configuration settings, these settings can be accessed by letting NServiceBus inject the `ReadOnlySettings` into the constructor of the validation strategy.

snippet:custom-validation-strategy

#### Extending the configuration API for custom validation settings

In order to allow configuration of the custom validation strategy it is advised to create an extension method at the `AzureServiceBusValidationSettings` extension point, returned by `.Validation()`, in the NServiceBus configuration API. This provides access to the settings, in which the value can be registered using a well known key.

snippet:custom-validation-strategy-extension

### Sanitization

The sanitization layer is represented by an implementation of `ISanitizationStrategy`. 

Out of the box there are 3 sanitization strategies
 * `AdjustmentSanitizationV6`: removes invalid characters according to `EntityNameValidationV6Rules`, uses MD5 hashing to reduce the length of an entity name if the maximum length is exceeded.
 * `AdjustmentSanitization` (default): removes invalid characters according to `EntityNameValidationRules`, uses SHA1 hashing to reduce the length of an entity name if the maximum length is exceeded.
 * `ThrowOnFailingSanitization`: throws an `EndpointValidationException` if the name is invalid.

The default implementation of this strategy can be replaced by using the configuration API:

snippet:swap-sanitization-strategy

#### Implementing a custom sanitization strategy

Implementing a custom validation strategy requires a class that implements `ISanitizationStrategy`. This interface contains one method, called `Sanitize`, which provides access to the entity path in case the entity is a queue or topic (or name if the entity is a subscription), and returns a string that should contain a cleaned up version of the entity path passed in. The entity type is passed in as second parameter and can be used to differentiate between queues, topics and subscription. Note that the `Sanitize` method is only called if the active validation strategy's `IsValid` method returned false.

If the implementation of a sanitization strategy requires configuration settings, these settings can be accessed by letting NServiceBus inject the `ReadOnlySettings` into the constructor of the strategy.

snippet:custom-sanitization-strategy

#### Extending the configuration API for custom sanitization settings

In order to allow configuration of the custom sanitization strategy it is advised to create an extension method at the `AzureServiceBusSanitizationSettings` extension point , returned by `.Sanitization()`,in the NServiceBus configuration API. This provides access to the settings, in which the value can be registered using a well known key.

snippet:custom-sanitization-strategy-extension

### Individualization

The individualization layer is represented by an implementation of `IIndividualizationStrategy`. 

Out of the box, there are 2 individualization strategies
 * `CoreIndividualization` (default): Makes no modifications, and relies on the individualization logic as defined in the NServiceBus core framework.
 * `DiscriminatorBasedIndividualization`: modifies the name of the endpoint by appending a discriminator value to the end.

The default implementation of this strategy can be replaced by using the configuration API:

snippet:swap-individualization-strategy
 
#### Implementing a custom individualization strategy

Implementing a custom individualization strategy requires a class that implements `IIndividualizationStrategy`. This interface contains one method, called `Individualize`, which provides access to the current endpoint name, and returns a string that should contain a unique representation of the current endpoint instance. Note that, depending on other settings, the endpoint name passed in may differ already from the one originally configured.

If the implementation of a individualization strategy requires configuration settings, these settings can be accessed by letting NServiceBus inject the `ReadOnlySettings` into the constructor of the strategy.

snippet:custom-individualization-strategy

#### Extending the configuration API for custom individualization settings

In order to allow configuration of the custom individualization strategy it is advised to create an extension method at the `AzureServiceBusIndividualizationSettings` extension point , returned by `.Individualization()`,in the NServiceBus configuration API. This provides access to the settings, in which the value can be registered using a well known key.

snippet:custom-individualization-strategy-extension

### Namespace Partitioning

The namespace partitioning layer is represented by an implementation of `INamespacePartitioningStrategy`. 

Out of the box there are 3 namespace partitioning strategies
 * `SingleNamespacePartitioning` (default): All entities are in a single namespace.
 * `FailOverNamespacePartitioning`: assumes all entities are in the primary and secondary namespaces, where only the primary is in use by default. The secondary will function as a fallback in case of problems with the primary.
 * `RoundRobinNamespacePartitioning`: assumes all entities are in all namespaces, all namespaces are in use but one at a time in a round robin fashion.

The default implementation of this strategy can be replaced by using the configuration API:

snippet:swap-namespace-partitioning-strategy

#### Implementing a custom namespace partitioning strategy

Implementing a custom namespace partitioning strategy requires a class that implements `INamespacePartitioningStrategy`. This interface contains one method, called `GetNamespaces`, which provides access to the `PartitioningIntent` for which namespaces are requested and returns a set of namespaces, represented by the `RuntimeNamespaceInfo` class, that should be used for the specified intent.

The `PartitioningIntent` is to be interpreted as follows:
* `Creating`: returns a set of namespaces in which entities should be created at startup, most likely all namespaces.
* `Receiving`: returns a set of namespaces to which the transport should listen, very likely to be all namespaces as well.
* `Sending`: returns a set of namespaces to which the next send operation should go. In this operation it's very important to differentiate between `NamespaceMode.Active` and `NamespaceMode.Passive`. The transport will send the message to **ALL** active namespaces and if one of those send operation fails it will attempt to execute that operation on one of the passive namespaces returned.

If the implementation of a namespace partitioning strategy requires configuration settings, these settings can be accessed by letting NServiceBus inject the `ReadOnlySettings` into the constructor of the strategy.

snippet:custom-namespace-partitioning-strategy

#### Extending the configuration API for custom partitioning settings

In order to allow configuration of the custom namespace partitioning strategy it is advised to create an extension method at the `AzureServiceBusNamespacePartitioningSettings` extension point , returned by `.NamespacePartitioning()`,in the NServiceBus configuration API. This provides access to the settings, in which the value can be registered using a well known key.

snippet:custom-namespace-partitioning-strategy-extension

### Composition

The composition layer is represented by an implementation of `ICompositionStrategy`.

Out of the box there are 2 composition strategies
 * `FlatComposition`: The entity is in the root of the namespace.
 * `HierarchyComposition`: The entity is in a namespace hierarchy, at the location generated by the path generator.

The default implementation of this strategy can be replaced by using the configuration API:

snippet:swap-composition-strategy

#### Implementing a custom composition strategy

Implementing a custom composition strategy requires a class that implements `ICompositionStrategy`. This interface contains one method, called `GetEntityPath`, which provides access to an entity name as well as the type of the entity in question, and returns a string that should contain an entity path representing the location of the entity inside an azure servicebus namespace. Note that only queues and topics support positioning in a namespace hierarchy, subscriptions will always sit underneath the topic that they are being subscribed too, so only prefix the name with path information for `EntityType.Queue` and `EntityType.Topic`.

If the implementation of a composition strategy requires configuration settings, these settings can be accessed by letting NServiceBus inject the `ReadOnlySettings` into the constructor of the strategy.

snippet:custom-composition-strategy

#### Extending the configuration API for custom composition settings

In order to allow configuration of the custom composition partitioning strategy it is advised to create an extension method at the `AzureServiceBusCompositionSettings` extension point , returned by `.Composition()`,in the NServiceBus configuration API. This provides access to the settings, in which the value can be registered using a well known key.

snippet:custom-composition-strategy-extension