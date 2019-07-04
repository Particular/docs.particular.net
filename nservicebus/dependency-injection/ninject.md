---
title: Ninject
summary: Configure NServiceBus to use Ninject for dependency injection.
reviewed: 2018-12-05
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


### Default usage

snippet: Ninject


### Using an existing kernel

snippet: Ninject_Existing


## Unit of work

Its is possible to bind to use an _Unit of Work_ scope, which corresponds to the `DependencyLifecycle.InstancePerUnitOfWork` lifecycle, when registering components with `configuration.RegisterComponents(...)`.

In essence, bindings using _Unit of Work_ scope

 * will be instantiated only once per transport Message
 * will be disposed when message processing finishes

Bind the services in _Unit of Work_ scope using:

snippet: NinjectUnitOfWork

Services using `InUnitOfWorkScope()` can only be injected into code which is processing messages. To inject the service somewhere else (e.g. because of an user interaction) define conditional bindings:

snippet: NinjectConditionalBindings


### DependencyLifecycle Mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/#dependency-lifecycle) maps to [Ninject object scopes](https://github.com/ninject/ninject/wiki/Object-Scopes) as follows:

| `DependencyLifecycle`                                                                                             | Ninject object scope                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-call) | [Transient](https://github.com/ninject/ninject/wiki/Object-Scopes)         |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-unit-of-work)                    | [Singleton](https://github.com/ninject/ninject/wiki/Object-Scopes) within a [Named Scope](https://github.com/ninject/ninject.extensions.namedscope/wiki) per Unit of Work |
| [SingleInstance](/nservicebus/dependency-injection/#dependency-lifecycle-single-instance)                                  | [Singleton](https://github.com/ninject/ninject/wiki/Object-Scopes)                          |
