---
title: Autofac
summary: Details on how to Configure NServiceBus to use Autofac as a container. Includes usage examples as well as lifecycle mappings. 
component: Autofac
reviewed: 2017-02-02
tags:
- Dependency Injection
related:
- samples/containers/autofac
---


NServiceBus can be configured to use [Autofac](https://autofac.org/) as a dependency injection container.


### Default Usage

snippet: Autofac


### Existing Container Instance

snippet: Autofac_Existing

WARNING: As of Autofac 4.2.1, the [`ContainerBuilder.Update` method is marked as obsolete](https://github.com/autofac/Autofac/issues/811). In the future, updating the container passed to NServiceBus may not be possible.


### DependencyLifecycle Mapping

The [DependencyLifecycle](/nservicebus/containers/#dependency-lifecycle) map to Autofac in the following way.

| DependencyLifecycle                                                                                             | Autofac Equivalent                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/containers/#dependency-lifecycle-instancepercall) | [Instance Per Dependency](http://docs.autofac.org/en/latest/lifetime/instance-scope.html#instance-per-dependency)         |
| [InstancePerUnitOfWork](/nservicebus/containers/#dependency-lifecycle-instanceperunitofwork)                    | [Instance Per Lifetime Scope](http://docs.autofac.org/en/latest/lifetime/instance-scope.html#instance-per-lifetime-scope) |
| [SingleInstance](/nservicebus/containers/#dependency-lifecycle-singleinstance)                                  | [SingleInstance](http://docs.autofac.org/en/latest/lifetime/instance-scope.html#single-instance)                          |
