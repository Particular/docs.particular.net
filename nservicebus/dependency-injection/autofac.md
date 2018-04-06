---
title: Autofac
summary: Details on how to Configure NServiceBus to use Autofac for dependency injection. Includes usage examples as well as lifecycle mappings. 
component: Autofac
reviewed: 2017-02-02
tags:
 - Dependency Injection
related:
 - samples/dependency-injection/autofac
redirects:
 - nservicebus/containers/autofac
---


NServiceBus can be configured to use [Autofac](https://autofac.org/) for dependency injection.


### Default Usage

snippet: Autofac


### Existing Instance

snippet: Autofac_Existing

WARNING: As of Autofac 4.2.1, the [`ContainerBuilder.Update` method is marked as obsolete](https://github.com/autofac/Autofac/issues/811). In the future, updating the instance passed to NServiceBus may not be possible.


### DependencyLifecycle Mapping

The [DependencyLifecycle](/nservicebus/dependency-injection/#dependency-lifecycle) map to Autofac in the following way.

| DependencyLifecycle                                                                                             | Autofac Equivalent                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-call) | [Instance Per Dependency](http://docs.autofac.org/en/latest/lifetime/instance-scope.html#instance-per-dependency)         |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-unit-of-work)                    | [Instance Per Lifetime Scope](http://docs.autofac.org/en/latest/lifetime/instance-scope.html#instance-per-lifetime-scope) |
| [SingleInstance](/nservicebus/dependency-injection/#dependency-lifecycle-single-instance)                                  | [SingleInstance](http://docs.autofac.org/en/latest/lifetime/instance-scope.html#single-instance)                          |
