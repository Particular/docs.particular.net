---
title: StructureMap
summary: Details on how to Configure NServiceBus to use StructureMap for dependency injection.
component: StructureMap
reviewed: 2020-02-20
redirects:
 - nservicebus/containers/structuremap
related:
 - samples/dependency-injection/structuremap
---

include: container-deprecation-notice

partial: usage

## DependencyLifecycle Mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/) maps to [StructureMap lifecycles](https://structuremap.github.io/object-lifecycle/supported-lifecycles/) as follows:

| `DependencyLifecycle`                                                                                             | StructureMap lifecycle                                                                        |
|-----------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/) | [AlwaysUnique](https://structuremap.github.io/object-lifecycle/supported-lifecycles/#sec1)     |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/)                    | [ContainerScoped](https://structuremap.github.io/object-lifecycle/supported-lifecycles/#sec3) |
| [SingleInstance](/nservicebus/dependency-injection/)                                  | [Singleton](https://structuremap.github.io/object-lifecycle/supported-lifecycles/#sec2)        |


include: property-injection