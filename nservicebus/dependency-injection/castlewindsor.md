---
title: Castle Windsor
summary: Details on how to Configure NServiceBus to use Castle Windsor for dependency injection.
component: Castle
reviewed: 2020-02-19
tags:
 - Dependency Injection
related:
 - samples/dependency-injection/castle
redirects:
 - nservicebus/containers/castle
---

NServiceBus can be configured to use [Castle Windsor](https://github.com/castleproject/Windsor) via the [`NServiceBus.Extensions.Hosting`](/nservicebus/hosting/extensions-hosting) or the [`NServiceBus.Extensions.DependencyInjection`](https://docs.particular.net/nservicebus/dependency-injection/nservicebus-dependencyinjection) package.

### DependencyLifecycle Mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/) maps to [Castle's `LifestyleType`](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md) as follows:

| `DependencyLifecycle`                                                                                             | `LifestyleType`                                                                           |
|-----------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/) | [Transient](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md#transient) |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/)                    | [Scoped](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md#scoped)       |
| [SingleInstance](/nservicebus/dependency-injection/)                                  | [Singleton](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md#singleton) |
