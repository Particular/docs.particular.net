---
title: StructureMap
summary: Configuring NServiceBus to use StructureMap as a container.
component: StructureMap
reviewed: 2016-03-17
tags:
- Dependency Injection
---


NServiceBus can be configured to use [StructureMap](https://structuremap.github.io/) as a dependency injection container.


### Default Usage

snippet:StructureMap


### Existing Container Instance

snippet:StructureMap_Existing



### DependencyLifecycle Mapping

The [DependencyLifecycle](/nservicebus/containers/#dependency-lifecycle) map to [StructureMap Lifecycle](http://structuremap.github.io/object-lifecycle/supported-lifecycles/) in the following way.

| DependencyLifecycle                                                                                             | StructureMap Lifecycle                                                                        |
|-----------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------|
| [InstancePerCall](https://docstest.particular.net/nservicebus/containers/#dependency-lifecycle-instancepercall) | [AlwaysUnique](http://structuremap.github.io/object-lifecycle/supported-lifecycles/#sec1)     |
| [InstancePerUnitOfWork](/nservicebus/containers/#dependency-lifecycle-instanceperunitofwork)                    | [ContainerScoped](http://structuremap.github.io/object-lifecycle/supported-lifecycles/#sec3) |
| [SingleInstance](/nservicebus/containers/#dependency-lifecycle-singleinstance)                                  | [Singleton](http://structuremap.github.io/object-lifecycle/supported-lifecycles/#sec2)        |