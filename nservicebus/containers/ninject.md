---
title: Ninject
summary: Configure NServiceBus to use Ninject as a container.
reviewed: 2017-05-23
component: Ninject
tags:
- Dependency Injection
related:
- samples/containers/ninject
- nservicebus/containers/child-containers
---

NServiceBus can be configured to use [Ninject](http://www.ninject.org/) as a dependency injection container.


### Default Usage

snippet: Ninject


### Existing Container Instance

snippet: Ninject_Existing


partial: uow


### DependencyLifecycle Mapping

The [DependencyLifecycle](/nservicebus/containers/#dependency-lifecycle) maps to Ninject in the following way.

| DependencyLifecycle                                                                                             | Ninject Equivalent                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/containers/#dependency-lifecycle-instancepercall) | [Transient](https://github.com/ninject/ninject/wiki/Object-Scopes)         |
| [InstancePerUnitOfWork](/nservicebus/containers/#dependency-lifecycle-instanceperunitofwork)                    | [Singleton](https://github.com/ninject/ninject/wiki/Object-Scopes) within a [Named Scope](https://github.com/ninject/ninject.extensions.namedscope/wiki) per Unit of Work |
| [SingleInstance](/nservicebus/containers/#dependency-lifecycle-singleinstance)                                  | [Singleton](https://github.com/ninject/ninject/wiki/Object-Scopes)                          |
