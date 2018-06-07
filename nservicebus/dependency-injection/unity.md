---
title: Unity
summary: Details on how to Configure NServiceBus to use Unity for dependency injection. Includes usage examples as well as lifecycle mappings.
component: Unity
reviewed: 2018-06-06
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
| [InstancePerCall](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-call)                                | [Transient Lifetime Manager](https://msdn.microsoft.com/en-us/library/microsoft.practices.unity.transientlifetimemanager.aspx)         |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-unit-of-work)                    | [Hierarchical Lifetime Manager](https://msdn.microsoft.com/en-us/library/microsoft.practices.unity.hierarchicallifetimemanager.aspx) |
| [SingleInstance](/nservicebus/dependency-injection/#dependency-lifecycle-single-instance)                                  | [Container Controlled Lifetime Manager](https://msdn.microsoft.com/en-us/library/ff660872.aspx#Anchor_0)                          |

### Unity Dependency Breaking Changes

Breaking changes in Unity will be handled as follows:

- Changes that require a change to the `NServiceBus.Unity` API (e.g. changes to `IUnityContainer`) will be released in a new major version of `NServiceBus.Unity`.
- Changes that do not require a change to the `NServiceBus.Unity` API will be released as a patch release to the latest minor version of `NServiceBus.Unity`.
- If Unity releases a new major version, it will be supported in a new minor release of `NServiceBus.Unity`.