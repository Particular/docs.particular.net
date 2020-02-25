---
title: Unity
summary: Details on how to Configure NServiceBus to use Unity for dependency injection.
component: Unity
reviewed: 2020-02-20
tags:
 - Dependency Injection
related:
 - samples/dependency-injection/unity
redirects:
 - nservicebus/containers/unity
---

partial: usage

### DependencyLifecycle mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/) maps to [Unity lifetime managers](https://msdn.microsoft.com/en-us/library/ff660872.aspx#Anchor_0) as follows:

| `DependencyLifecycle`                                                                                             | Unity Equivalent                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/)                                | [`TransientLifetimeManager`](https://msdn.microsoft.com/en-us/library/microsoft.practices.unity.transientlifetimemanager.aspx)         |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/)                    | [`HierarchicalLifetimeManager`](https://msdn.microsoft.com/en-us/library/microsoft.practices.unity.hierarchicallifetimemanager.aspx) |
| [SingleInstance](/nservicebus/dependency-injection/)                                  | [`ContainerControlledLifetimeManager`](https://msdn.microsoft.com/en-us/library/microsoft.practices.unity.containercontrolledlifetimemanager.aspx)                          |

### Breaking changes in Unity

Breaking changes in Unity will be handled as follows:

- Changes that require a change to the `NServiceBus.Unity` API (e.g. changes to `IUnityContainer`) will be released in a new major version of `NServiceBus.Unity`.
- Changes that do not require a change to the `NServiceBus.Unity` API will be released as a patch release to the latest minor version of `NServiceBus.Unity`.
- If Unity releases a new major version, it will be supported in a new minor release of `NServiceBus.Unity`.
