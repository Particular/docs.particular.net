---
title: Simple Injector
summary: Configure NServiceBus to use Simple Injector for dependency injection.
component: SimpleInjector
reviewed: 2017-06-04
tags:
 - Dependency Injection
related:
 - samples/dependency-injection/simpleinjector
redirects:
 - nservicebus/containers/simpleinjector
---


NServiceBus can be configured to use [Simple Injector](https://simpleinjector.org) for dependency injection.


### Default Usage

snippet: simpleinjector


### Existing Instance

snippet: simpleinjector_Existing


### DependencyLifecycle Mapping

The [DependencyLifecycle](/nservicebus/dependency-injection/#dependency-lifecycle) map to Simple Injector in the following way.

| DependencyLifecycle                                                                                             | Simple Injector Equivalent                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-call)                                | [Transient Lifestyle](http://simpleinjector.readthedocs.io/en/latest/lifetimes.html#transient)         |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-unit-of-work)                    | [Per Execution Context Scope Lifestyle](http://simpleinjector.readthedocs.io/en/latest/lifetimes.html#perexecutioncontextscope) |
| [SingleInstance](/nservicebus/dependency-injection/#dependency-lifecycle-single-instance)                                  | [Singleton Lifestyle](http://simpleinjector.readthedocs.io/en/latest/lifetimes.html#singleton)                          |