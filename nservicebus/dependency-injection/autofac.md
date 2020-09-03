---
title: Autofac
summary: Details on how to Configure NServiceBus to use Autofac for dependency injection. 
component: Autofac
reviewed: 2020-02-20
related:
 - samples/dependency-injection/autofac
redirects:
 - nservicebus/containers/autofac
---

include: container-deprecation-notice

partial: usage

## DependencyLifecycle mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/) maps to [Autofac instance scopes](https://docs.autofac.org/en/latest/lifetime/instance-scope.html#instance-scope) as follows:

| `DependencyLifecycle`                                                                                             | Autofac instance scope                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/) | [Instance Per Dependency](https://docs.autofac.org/en/latest/lifetime/instance-scope.html#instance-per-dependency)         |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/)                    | [Instance Per Lifetime Scope](https://docs.autofac.org/en/latest/lifetime/instance-scope.html#instance-per-lifetime-scope) |
| [SingleInstance](/nservicebus/dependency-injection/)                                  | [Single Instance](https://docs.autofac.org/en/latest/lifetime/instance-scope.html#single-instance)                          |


include: property-injection