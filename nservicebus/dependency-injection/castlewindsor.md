---
title: Castle Windsor
summary: Details on how to Configure NServiceBus to use Castle Windsor for dependency injection.
component: Castle
reviewed: 2020-02-20
related:
 - samples/dependency-injection/castle
redirects:
 - nservicebus/containers/castle
---

include: container-deprecation-notice

partial: usage

### DependencyLifecycle Mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/) maps to [Castle's `LifestyleType`](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md) as follows:


| `DependencyLifecycle`                                                                                             | `LifestyleType`                                                                           |
|-----------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/) | [Transient](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md#transient) |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/)                    | [Scoped](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md#scoped)       |
| [SingleInstance](/nservicebus/dependency-injection/)                                  | [Singleton](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md#singleton) |


include: property-injection