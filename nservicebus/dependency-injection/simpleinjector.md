---
title: Simple Injector
summary: Configure NServiceBus to use Simple Injector for dependency injection.
component: SimpleInjector
reviewed: 2018-12-05
tags:
 - Dependency Injection
related:
 - samples/dependency-injection/simpleinjector
redirects:
 - nservicebus/containers/simpleinjector
---


NServiceBus can be configured to use [Simple Injector](https://simpleinjector.org) for dependency injection.


### Default usage

snippet: simpleinjector


### Using an existing container

snippet: simpleinjector_Existing


### DependencyLifecycle Mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/#dependency-lifecycle) maps to [Simple Injector lifestyles](https://simpleinjector.readthedocs.io/en/latest/lifetimes.html) as follows:

| `DependencyLifecycle`                                                                                             | Simple Injector lifestyle                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-call)                                | [Transient](http://simpleinjector.readthedocs.io/en/latest/lifetimes.html#transient)         |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-unit-of-work)                    | [Scoped](http://simpleinjector.readthedocs.io/en/latest/lifetimes.html#perexecutioncontextscope) |
| [SingleInstance](/nservicebus/dependency-injection/#dependency-lifecycle-single-instance)                                  | [Singleton](http://simpleinjector.readthedocs.io/en/latest/lifetimes.html#singleton)                          |