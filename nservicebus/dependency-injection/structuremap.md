---
title: StructureMap
summary: Details on how to Configure NServiceBus to use StructureMap for dependency injection.
component: StructureMap
reviewed: 2020-02-19
tags:
 - Dependency Injection
redirects:
 - nservicebus/containers/structuremap
related:
 - samples/dependency-injection/structuremap
---


NServiceBus can be configured to use [StructureMap](https://structuremap.github.io/) for dependency injection via the [`NServiceBus.Extensions.Hosting`](/nservicebus/hosting/extensions-hosting) or the [`NServiceBus.Extensions.DependencyInjection`](https://docs.particular.net/nservicebus/dependency-injection/nservicebus-dependencyinjection) package.

### DependencyLifecycle Mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/) maps to [StructureMap lifecycles](https://structuremap.github.io/object-lifecycle/supported-lifecycles/) as follows:

| `DependencyLifecycle`                                                                                             | StructureMap lifecycle                                                                        |
|-----------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/) | [AlwaysUnique](https://structuremap.github.io/object-lifecycle/supported-lifecycles/#sec1)     |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/)                    | [ContainerScoped](https://structuremap.github.io/object-lifecycle/supported-lifecycles/#sec3) |
| [SingleInstance](/nservicebus/dependency-injection/)                                  | [Singleton](https://structuremap.github.io/object-lifecycle/supported-lifecycles/#sec2)        |
