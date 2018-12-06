---
title: Autofac
summary: Details on how to Configure NServiceBus to use Autofac for dependency injection. Includes usage examples as well as lifecycle mappings. 
component: Autofac
reviewed: 2018-12-05
tags:
 - Dependency Injection
related:
 - samples/dependency-injection/autofac
redirects:
 - nservicebus/containers/autofac
---


NServiceBus can be configured to use [Autofac](https://autofac.org/) for dependency injection.


### Default usage

snippet: Autofac


### Using an existing container

snippet: Autofac_Existing

WARNING: Although it is possible to update the container after passing it to NServiceBus using the `ContainerBuilder.Update` method, from Autofac 4.2.1 onwards that method [is marked as obsolete](https://github.com/autofac/Autofac/issues/811). It is recommended not to use this method to update the container after it has been passed to NServiceBus.


### DependencyLifecycle mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/#dependency-lifecycle) maps to [Autofac instance scopes](http://docs.autofac.org/en/latest/lifetime/instance-scope.html#instance-scope) as follows:

| `DependencyLifecycle`                                                                                             | Autofac instance scope                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-call) | [Instance Per Dependency](http://docs.autofac.org/en/latest/lifetime/instance-scope.html#instance-per-dependency)         |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-unit-of-work)                    | [Instance Per Lifetime Scope](http://docs.autofac.org/en/latest/lifetime/instance-scope.html#instance-per-lifetime-scope) |
| [SingleInstance](/nservicebus/dependency-injection/#dependency-lifecycle-single-instance)                                  | [Single Instance](http://docs.autofac.org/en/latest/lifetime/instance-scope.html#single-instance)                          |
