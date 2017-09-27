---
title: Unity
summary: Details on how to Configure NServiceBus to use Unity for dependency injection. Includes usage examples as well as lifecycle mappings.
component: Unity
reviewed: 2016-11-28
tags:
 - Dependency Injection
related:
 - samples/dependency-injection/unity
redirects:
 - nservicebus/containers/unity
---


NServiceBus can be configured to use [Unity](https://github.com/unitycontainer/unity) for dependency injection.


### Default Usage

snippet: Unity


### Existing Instance

snippet: Unity_Existing


### DependencyLifecycle Mapping

The [DependencyLifecycle](/nservicebus/dependency-injection/#dependency-lifecycle) map to Unity in the following way.

| DependencyLifecycle                                                                                             | Unity Equivalent                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/#dependency-lifecycle-instancepercall)                                | [Transient Lifetime Manager](https://msdn.microsoft.com/en-us/library/microsoft.practices.unity.transientlifetimemanager.aspx)         |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/#dependency-lifecycle-instanceperunitofwork)                    | [Hierarchical Lifetime Manager](https://msdn.microsoft.com/en-us/library/microsoft.practices.unity.hierarchicallifetimemanager.aspx) |
| [SingleInstance](/nservicebus/dependency-injection/#dependency-lifecycle-singleinstance)                                  | [Container Controlled Lifetime Manager](https://msdn.microsoft.com/en-us/library/ff660872.aspx#Anchor_0)                          |
