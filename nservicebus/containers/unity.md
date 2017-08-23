---
title: Unity
summary: Details on how to Configure NServiceBus to use Unity as a container. Includes usage examples as well as lifecycle mappings.
component: Unity
reviewed: 2016-11-28
tags:
- Dependency Injection
related:
- samples/containers/unity
---


NServiceBus can be configured to use [Unity](https://github.com/unitycontainer/unity) as a dependency injection container.


### Default Usage

snippet: Unity


### Existing Container Instance

snippet: Unity_Existing


### DependencyLifecycle Mapping

The [DependencyLifecycle](/nservicebus/containers/#dependency-lifecycle) map to Unity in the following way.

| DependencyLifecycle                                                                                             | Unity Equivalent                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/containers/#dependency-lifecycle-instancepercall)                                | [Transient Lifetime Manager](https://msdn.microsoft.com/en-us/library/microsoft.practices.unity.transientlifetimemanager.aspx)         |
| [InstancePerUnitOfWork](/nservicebus/containers/#dependency-lifecycle-instanceperunitofwork)                    | [Hierarchical Lifetime Manager](https://msdn.microsoft.com/en-us/library/microsoft.practices.unity.hierarchicallifetimemanager.aspx) |
| [SingleInstance](/nservicebus/containers/#dependency-lifecycle-singleinstance)                                  | [Container Controlled Lifetime Manager](https://msdn.microsoft.com/en-us/library/ff660872.aspx#Anchor_0)                          |
