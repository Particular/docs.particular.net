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


partial: uow

### Multi hosting

Sometimes you want to host multiple endpoints in a single process. You cannot share a single Ninject kernel with multiple endpoint instances. You need to create a child container. Ninject supports this with the [Ninject.Extensions.ChildKernel](https://github.com/ninject/Ninject.Extensions.ChildKernel) extension. Before you pass the kernel to NServiceBus you need to do `new ChildKernel(parentKernel)` and pass this new kernel instance.


### DependencyLifecycle Mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/#dependency-lifecycle) maps to [Ninject object scopes](https://github.com/ninject/ninject/wiki/Object-Scopes) as follows:

| `DependencyLifecycle`                                                                                             | Ninject object scope                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-call) | [Transient](https://github.com/ninject/ninject/wiki/Object-Scopes)         |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-unit-of-work)                    | [Singleton](https://github.com/ninject/ninject/wiki/Object-Scopes) within a [Named Scope](https://github.com/ninject/ninject.extensions.namedscope/wiki) per Unit of Work |
| [SingleInstance](/nservicebus/dependency-injection/#dependency-lifecycle-single-instance)                                  | [Singleton](https://github.com/ninject/ninject/wiki/Object-Scopes)                          |
