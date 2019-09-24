---
title: Castle Windsor
summary: Details on how to Configure NServiceBus to use Castle Windsor for dependency injection. Includes usage examples as well as lifecycle mappings.
component: Castle
reviewed: 2018-12-05
tags:
 - Dependency Injection
related:
 - samples/dependency-injection/castle
redirects:
 - nservicebus/containers/castle
---


NServiceBus can be configured to use [Castle Windsor](https://github.com/castleproject/Windsor) for dependency injection.


### Default usage

snippet: CastleWindsor


### Using an existing container

snippet: CastleWindsor_Existing


### DependencyLifecycle Mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/) maps to [Castle's `LifestyleType`](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md) as follows:


| `DependencyLifecycle`                                                                                             | `LifestyleType`                                                                           |
|-----------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/) | [Transient](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md#transient) |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/)                    | [Scoped](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md#scoped)       |
| [SingleInstance](/nservicebus/dependency-injection/)                                  | [Singleton](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md#singleton) |
