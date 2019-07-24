---
title: StructureMap
summary: Details on how to Configure NServiceBus to use StructureMap for dependency injection. Includes usage examples as well as lifecycle mappings.
component: StructureMap
reviewed: 2018-12-05
tags:
 - Dependency Injection
redirects:
 - nservicebus/containers/structuremap
related:
 - samples/dependency-injection/structuremap
---


NServiceBus can be configured to use [StructureMap](https://structuremap.github.io/) for dependency injection.


### Default usage

snippet: StructureMap


### Using an existing container

snippet: StructureMap_Existing



### DependencyLifecycle Mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/#dependency-lifecycle) maps to [StructureMap lifecycles](https://structuremap.github.io/object-lifecycle/supported-lifecycles/) as follows:

| `DependencyLifecycle`                                                                                             | StructureMap lifecycle                                                                        |
|-----------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-call) | [AlwaysUnique](https://structuremap.github.io/object-lifecycle/supported-lifecycles/#sec1)     |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-unit-of-work)                    | [ContainerScoped](https://structuremap.github.io/object-lifecycle/supported-lifecycles/#sec3) |
| [SingleInstance](/nservicebus/dependency-injection/#dependency-lifecycle-single-instance)                                  | [Singleton](https://structuremap.github.io/object-lifecycle/supported-lifecycles/#sec2)        |