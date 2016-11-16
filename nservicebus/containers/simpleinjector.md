---
title: Simple Injector
summary: Configure NServiceBus to use Simple Injector as a container.
component: SimpleInjector
reviewed: 2016-10-31
tags:
 - Dependency Injection
related:
 - samples/containers/simpleinjector
---


NServiceBus can be configured to use [Simple Injector](https://simpleinjector.org) as a dependency injection container.


### Default Usage

snippet:simpleinjector


### Existing Container Instance

snippet:simpleinjector_Existing


### DependencyLifecycle Mapping

The [DependencyLifecycle](/nservicebus/containers/#dependency-lifecycle) map to Simple Injector in the following way.

| DependencyLifecycle                                                                                             | Simple Injector Equivalent                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/containers/#dependency-lifecycle-instancepercall)                                | [Transient Lifestyle](http://simpleinjector.readthedocs.io/en/latest/lifetimes.html#transient)         |
| [InstancePerUnitOfWork](/nservicebus/containers/#dependency-lifecycle-instanceperunitofwork)                    | [Per Execution Context Scope Lifestyle](http://simpleinjector.readthedocs.io/en/latest/lifetimes.html#perexecutioncontextscope) |
| [SingleInstance](/nservicebus/containers/#dependency-lifecycle-singleinstance)                                  | [Singleton Lifestyle](http://simpleinjector.readthedocs.io/en/latest/lifetimes.html#singleton)                          |