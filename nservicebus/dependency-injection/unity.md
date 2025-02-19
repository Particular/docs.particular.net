---
title: Unity
summary: Details on how to Configure NServiceBus to use Unity for dependency injection.
component: Unity
reviewed: 2025-02-19
related:
 - samples/dependency-injection/unity
redirects:
 - nservicebus/containers/unity
---

include: container-deprecation-notice

NServiceBus can be configured to use [Unity](https://github.com/unitycontainer/unity) for dependency injection.


## Default usage

snippet: Unity


## Using an existing container

snippet: Unity_Existing

## DependencyLifecycle mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/) maps to [Unity lifetime managers](https://msdn.microsoft.com/en-us/library/ff660872.aspx#Anchor_0) as follows:

| `DependencyLifecycle`                                                                                             | Unity Equivalent                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/)                                | [`TransientLifetimeManager`](https://msdn.microsoft.com/en-us/library/microsoft.practices.unity.transientlifetimemanager.aspx)         |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/)                    | [`HierarchicalLifetimeManager`](https://msdn.microsoft.com/en-us/library/microsoft.practices.unity.hierarchicallifetimemanager.aspx) |
| [SingleInstance](/nservicebus/dependency-injection/)                                  | [`ContainerControlledLifetimeManager`](https://msdn.microsoft.com/en-us/library/microsoft.practices.unity.containercontrolledlifetimemanager.aspx)                          |


include: property-injection
