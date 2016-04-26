---
title: Azure Service Bus Transport Addressing Logic
summary: Azure ServiceBus Transport Addressing Logic
tags:
- Azure
- Cloud
- Configuration
---


## Physical Addressing Logic

One of the responsibilities of the transport is determining the names and physical location of the entities. It is achieved by turning logical endpoint names into physical addresses of the Azure Service Bus entities, which is called *Physical Addressing Logic*.

Prior to version 7 there was no explicit layer for handling this kind of logic, most assumptions such as assumptions on length limitations, legal characters, differences between paths and names etc... were sprinkled across the transport.

Over time, little cracks started to show in these assumptions. Subtle changes took place inside the implementation of certain namespace types. For example `Mixed` namespaces allowed paths and names up 290 characters, while `Messaging` namespaces are capped at 260. Next to that, customers also figured out ways to game the system (for valid reasons), like embedding `'/'` characters in endpoint names to create subfolders in the Azure Service Bus namespace registration system.

### Version 6 and below

In older versions some of these cracking points got exposed (usually as hotfixes) as lambda functions and became collectively known as the [Naming Conventions](/nservicebus/azure-service-bus/naming-conventions.md#version-6-and-below-naming-conventions)

### Version 7

In version 7 the configuration API allows to modify all behaviours and assumptions related to the addressing logic. This article will mainly focus on how to replace the parts of the out of the box addressing logic, for a full list of out of the box options refer to [Full Configuration API](/nservicebus/azure-service-bus/configuration/configuration.md).

### Addressing layers

The following layers are available for replacement in the addressing logic:

 * Validation: Determines how entity names are validated and is represented by an implementation of `IValidationStrategy`.
 * Sanitization: Determines how invalid entity names are cleaned up, represented by an implementation of `ISanitizationStrategy`.
 * Individualization: Determines how entity names are modified when used in different consumption modes, represented by an implementation of `IIndividualizationStrategy`.
 * NamespacePartitioning: Determines how entities are partitioned across namespaces, represented by an implementation `INamespacePartinioningStrategy`.
 * Composition: Determines how entities are composed inside a single namespace, represented by an implementation of `ICompositionStrategy`.
 
### Validation

 * `UseStrategy<T>()`: An implementation of `IValidationStrategy` that validates the entity path. Following implementations exist:
	 * `EntityNameValidationV6Rules`: allows letters, numbers, periods (.), hyphens (-), and underscores (-)
	 * `EntityNameValidationRules` (default): allows letters, numbers, periods (.), hyphens (-), underscores (-) and slashes (/) for queues and topics, no slashes allowed for subscriptions.

### Sanitization

* `UseStrategy<T>()`: An implementation of `ISanitizationStrategy` that handles invalid entity names. The following implementations exist:
	 * `AdjustmentSanitizationV6`: removes invalid characters according to `EntityNameValidationV6Rules`, uses MD5 hashing to reduce the length of an entity name if the maximum length is exceeded.
	 * `AdjustmentSanitization` (default): removes invalid characters according to `EntityNameValidationRules`, uses SHA1 hashing to reduce the length of an entity name if the maximum length is exceeded.
	 * `ThrowOnFailingSanitization`: throws an `EndpointValidationException` if the name is invalid.

### Individualization

 * `UseStrategy<T>()`: An implementation of `IIndividualizationStrategy` that modifies an endpoint name to become unique per endpoint instance. Following implementations exist:
	 * `CoreIndividualization` (default): Makes no modifications, and relies on the individualization logic as defined in the NServiceBus core framework.
	 * `DiscriminatorBasedIndividualization`: modifies the name of the endpoint by appending a discriminator value to the end.
 
### Namespace Partitioning

 * `UseStrategy<T>`: An implementation of `INamespacePartitioningStrategy` that determines how entities are distributed across namespaces. The following strategies exist:
	 * `SingleNamespacePartitioning` (default): All entities are in a single namespace.
	 * `FailOverNamespacePartitioning`: assumes all entities are in the primary and secondary namespaces, where only the primary is in use by default. The secondary will function as a fallback in case of problems with the primary.
	 * `RoundRobinNamespacePartitioning`: assumes all entities are in all namespaces, all namespaces are in use but one at a time in a round robin fashion.
	 * `ReplicatedNamespacePartitioning`: assumes all entities are in all namespaces, all namespaces are in use, messages are replicated over all namespaces.
	 * `ShardedNamespacePartitioning`: assumes all entities are in all namespaces, all namespaces are in use, messages are sent to a single namespace based on a sharding rule.

### Composition

 * `UseStrategy<T>()`: An implementation of `ICompositionStrategy` that determines how an entity is positioned inside a namespace hierarchy. The following implementations exist:
	 * `FlatComposition`: The entity is in the root of the namespace.
	 * `HierarchyComposition`: The entity is in a namespace hierarchy, at the location generated by the path generator.