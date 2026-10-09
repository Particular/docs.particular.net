---
title: StructureMap
summary: Details on how to Configure NServiceBus to use StructureMap for dependency injection.
component: StructureMap
reviewed: 2026-10-09
redirects:
 - nservicebus/containers/structuremap
---

include: container-deprecation-notice

NServiceBus can be configured to use [StructureMap](https://structuremap.github.io/) for dependency injection.

> [!NOTE]
> StructureMap has been sunset and the `NServiceBus.StructureMap` repository is archived. The StructureMap maintainers recommend [Lamar](https://jasperfx.github.io/lamar/) instead. To use StructureMap with NServiceBus version 8 and later, see the [StructureMap section of the upgrade guide](/nservicebus/upgrades/7to8/dependency-injection.md#externally-managed-container-mode-migrating-to-externally-managed-mode-structuremap).


## Default usage

snippet: StructureMap


## Using an existing container

snippet: StructureMap_Existing

## DependencyLifecycle Mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/#service-registrations) maps to [StructureMap lifecycles](https://structuremap.github.io/object-lifecycle/supported-lifecycles/) as follows:

| `DependencyLifecycle`                                                                                             | StructureMap lifecycle                                                                        |
|-----------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/#service-registrations) | [AlwaysUnique](https://structuremap.github.io/object-lifecycle/supported-lifecycles/#alwaysunique)     |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/#service-registrations)                    | [ContainerScoped](https://structuremap.github.io/object-lifecycle/supported-lifecycles/#containerscoped) |
| [SingleInstance](/nservicebus/dependency-injection/#service-registrations)                                  | [Singleton](https://structuremap.github.io/object-lifecycle/supported-lifecycles/#singleton)        |


include: property-injection
