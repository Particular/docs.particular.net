---
title: Autofac
summary: Details on how to Configure NServiceBus to use Autofac for dependency injection.
component: Autofac
reviewed: 2020-02-19
tags:
 - Dependency Injection
related:
 - samples/dependency-injection/autofac
redirects:
 - nservicebus/containers/autofac
---

NServiceBus can be configured to use Autofac](https://autofac.org/) for dependency injection via the [`NServiceBus.Extensions.Hosting`](/nservicebus/hosting/extensions-hosting) or the [`NServiceBus.Extensions.DependencyInjection`](https://docs.particular.net/nservicebus/dependency-injection/nservicebus-dependencyinjection) package.

WARNING: Although it is possible to update the container after passing it to NServiceBus using the `ContainerBuilder.Update` method, from Autofac 4.2.1 onwards that method [is marked as obsolete](https://github.com/autofac/Autofac/issues/811). It is recommended not to use this method to update the container after it has been passed to NServiceBus.


### DependencyLifecycle mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/) maps to [Autofac instance scopes](https://docs.autofac.org/en/latest/lifetime/instance-scope.html#instance-scope) as follows:

| `DependencyLifecycle`                                                                                             | Autofac instance scope                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/) | [Instance Per Dependency](https://docs.autofac.org/en/latest/lifetime/instance-scope.html#instance-per-dependency)         |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/)                    | [Instance Per Lifetime Scope](https://docs.autofac.org/en/latest/lifetime/instance-scope.html#instance-per-lifetime-scope) |
| [SingleInstance](/nservicebus/dependency-injection/)                                  | [Single Instance](https://docs.autofac.org/en/latest/lifetime/instance-scope.html#single-instance)                          |
