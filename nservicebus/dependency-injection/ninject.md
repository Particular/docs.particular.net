---
title: Ninject
summary: Configure NServiceBus to use Ninject for dependency injection.
reviewed: 2020-07-28
component: Ninject
related:
 - samples/dependency-injection/ninject
 - nservicebus/dependency-injection/child-lifetime
redirects:
 - nservicebus/containers/ninject
---

include: container-deprecation-notice

NServiceBus can be configured to use [Ninject](http://www.ninject.org/) for dependency injection.


### Default usage

snippet: Ninject


### Using an existing kernel

snippet: Ninject_Existing


### Unit of work

Its is possible to bind to use an _Unit of Work_ scope, which corresponds to the `DependencyLifecycle.InstancePerUnitOfWork` lifecycle, when registering components with `configuration.RegisterComponents(...)`.

In essence, bindings using _Unit of Work_ scope

 * will be instantiated only once per transport Message
 * will be disposed when message processing finishes

Bind the services in _Unit of Work_ scope using:

snippet: NinjectUnitOfWork

Services using `InUnitOfWorkScope()` can only be injected into code which is processing messages. To inject the service somewhere else (e.g. because of an user interaction) define conditional bindings:

snippet: NinjectConditionalBindings

Funcs can only be injected when using the [ContextPreservation Ninject extension](https://github.com/ninject/Ninject.Extensions.ContextPreservation/).

snippet: NinjectContextPreservationFuncBinding

### Multi hosting

Multiple endpoints in a single process cannot share a single Ninject kernel. Each requires its own container instance or each requires its own child container. Ninject supports this with the [Ninject.Extensions.ChildKernel](https://github.com/ninject/Ninject.Extensions.ChildKernel) extension. Execute `new ChildKernel(parentKernel)` and pass this new kernel instance to NServiceBus.


### DependencyLifecycle Mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/) maps to [Ninject object scopes](https://github.com/ninject/ninject/wiki/Object-Scopes) as follows:

| `DependencyLifecycle`                                                                                             | Ninject object scope                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/) | [Transient](https://github.com/ninject/ninject/wiki/Object-Scopes)         |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/)                    | [Singleton](https://github.com/ninject/ninject/wiki/Object-Scopes) within a [Named Scope](https://github.com/ninject/ninject.extensions.namedscope/wiki) per Unit of Work |
| [SingleInstance](/nservicebus/dependency-injection/)                                  | [Singleton](https://github.com/ninject/ninject/wiki/Object-Scopes)                          |


include: property-injection