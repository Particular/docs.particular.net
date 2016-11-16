---
title: Autofac
summary: Configure NServiceBus to use Autofac as a container.
component: Autofac
reviewed: 2016-03-17
tags:
- Dependency Injection
---


NServiceBus can be configured to use [Autofac](https://autofac.org/) as a dependency injection container.


### Default Usage

snippet:Autofac


### Existing Container Instance

snippet:Autofac_Existing


### DependencyLifecycle Mapping

The [DependencyLifecycle](/nservicebus/containers/#dependency-lifecycle) map to Autofac in the following way.

| DependencyLifecycle                                                                                             | Autofac Equivalent                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](https://docstest.particular.net/nservicebus/containers/#dependency-lifecycle-instancepercall) | [Instance Per Dependency](http://docs.autofac.org/en/latest/lifetime/instance-scope.html#instance-per-dependency)         |
| [InstancePerUnitOfWork](/nservicebus/containers/#dependency-lifecycle-instanceperunitofwork)                    | [Instance Per Lifetime Scope](http://docs.autofac.org/en/latest/lifetime/instance-scope.html#instance-per-lifetime-scope) |
| [SingleInstance](/nservicebus/containers/#dependency-lifecycle-singleinstance)                                  | [SingleInstance](http://docs.autofac.org/en/latest/lifetime/instance-scope.html#single-instance)                          |