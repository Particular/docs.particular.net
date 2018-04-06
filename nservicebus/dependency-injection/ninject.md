---
title: Ninject
summary: Configure NServiceBus to use Ninject for dependency injection.
reviewed: 2017-05-23
component: Ninject
tags:
 - Dependency Injection
related:
 - samples/dependency-injection/ninject
 - nservicebus/dependency-injection/child-lifetime
redirects:
 - nservicebus/containers/ninject
---

NServiceBus can be configured to use [Ninject](http://www.ninject.org/) for dependency injection.


### Default Usage

snippet: Ninject


### Existing Instance

snippet: Ninject_Existing


partial: uow


### DependencyLifecycle Mapping

The [DependencyLifecycle](/nservicebus/dependency-injection/#dependency-lifecycle) maps to Ninject in the following way.

| DependencyLifecycle                                                                                             | Ninject Equivalent                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-call) | [Transient](https://github.com/ninject/ninject/wiki/Object-Scopes)         |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-unit-of-work)                    | [Singleton](https://github.com/ninject/ninject/wiki/Object-Scopes) within a [Named Scope](https://github.com/ninject/ninject.extensions.namedscope/wiki) per Unit of Work |
| [SingleInstance](/nservicebus/dependency-injection/#dependency-lifecycle-single-instance)                                  | [Singleton](https://github.com/ninject/ninject/wiki/Object-Scopes)                          |
