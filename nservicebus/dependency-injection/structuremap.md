---
title: StructureMap
summary: Details on how to Configure NServiceBus to use StructureMap for dependency injection. Includes usage examples as well as lifecycle mappings.
component: StructureMap
reviewed: 2017-06-29
tags:
 - Dependency Injection
redirects:
 - nservicebus/containers/structuremap
related:
 - samples/dependency-injection/structuremap
---


NServiceBus can be configured to use [StructureMap](https://structuremap.github.io/) for dependency injection.


### Default Usage

snippet: StructureMap


### Existing Instance

snippet: StructureMap_Existing



### DependencyLifecycle Mapping

The [DependencyLifecycle](/nservicebus/dependency-injection/#dependency-lifecycle) map to [StructureMap Lifecycle](http://structuremap.github.io/object-lifecycle/supported-lifecycles/) in the following way.

| DependencyLifecycle                                                                                             | StructureMap Lifecycle                                                                        |
|-----------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-call) | [AlwaysUnique](http://structuremap.github.io/object-lifecycle/supported-lifecycles/#sec1)     |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-unit-of-work)                    | [ContainerScoped](http://structuremap.github.io/object-lifecycle/supported-lifecycles/#sec3) |
| [SingleInstance](/nservicebus/dependency-injection/#dependency-lifecycle-single-instance)                                  | [Singleton](http://structuremap.github.io/object-lifecycle/supported-lifecycles/#sec2)        |