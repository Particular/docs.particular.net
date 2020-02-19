---
title: Unity
summary: Details on how to Configure NServiceBus to use Unity for dependency injection. Includes usage examples as well as lifecycle mappings.
component: Unity
reviewed: 2018-12-05
tags:
 - Dependency Injection
related:
 - samples/dependency-injection/unity
redirects:
 - nservicebus/containers/unity
---

NServiceBus can be configured to use [Unity](https://github.com/unitycontainer/unity) for dependency injection via the [`NServiceBus.Extensions.Hosting`](/nservicebus/hosting/extensions-hosting) or the [`NServiceBus.Extensions.DependencyInjection`](https://docs.particular.net/nservicebus/dependency-injection/nservicebus-dependencyinjection) package.

### DependencyLifecycle mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/) maps to [Unity lifetime managers](https://msdn.microsoft.com/en-us/library/ff660872.aspx#Anchor_0) as follows:

| `DependencyLifecycle`                                                                                             | Unity Equivalent                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/)                                | [`TransientLifetimeManager`](https://msdn.microsoft.com/en-us/library/microsoft.practices.unity.transientlifetimemanager.aspx)         |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/)                    | [`HierarchicalLifetimeManager`](https://msdn.microsoft.com/en-us/library/microsoft.practices.unity.hierarchicallifetimemanager.aspx) |
| [SingleInstance](/nservicebus/dependency-injection/)                                  | [`ContainerControlledLifetimeManager`](https://msdn.microsoft.com/en-us/library/microsoft.practices.unity.containercontrolledlifetimemanager.aspx)                          |
