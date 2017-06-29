---
title: StructureMap
summary: Details on how to Configure NServiceBus to use StructureMap as a container. Includes usage examples as well as lifecycle mappings.
component: StructureMap
reviewed: 2017-06-29
tags:
- Dependency Injection
---


NServiceBus can be configured to use [StructureMap](https://structuremap.github.io/) as a dependency injection container.


### Default Usage

snippet: StructureMap


### Existing Container Instance

snippet: StructureMap_Existing



### DependencyLifecycle Mapping

The [DependencyLifecycle](/nservicebus/containers/#dependency-lifecycle) map to [StructureMap Lifecycle](http://structuremap.github.io/object-lifecycle/supported-lifecycles/) in the following way.

| DependencyLifecycle                                                                                             | StructureMap Lifecycle                                                                        |
|-----------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/containers/#dependency-lifecycle-instancepercall) | [AlwaysUnique](http://structuremap.github.io/object-lifecycle/supported-lifecycles/#sec1)     |
| [InstancePerUnitOfWork](/nservicebus/containers/#dependency-lifecycle-instanceperunitofwork)                    | [ContainerScoped](http://structuremap.github.io/object-lifecycle/supported-lifecycles/#sec3) |
| [SingleInstance](/nservicebus/containers/#dependency-lifecycle-singleinstance)                                  | [Singleton](http://structuremap.github.io/object-lifecycle/supported-lifecycles/#sec2)        |