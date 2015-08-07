---
title: Using Ninject
summary: How to integrate your Ninject kernel and configure your bindings.
tags:
- Dependency Injection
- IOC
- Ninject
---
NServiceBus can easily be configured to use your existing Ninject Kernel (see [Containers](/nservicebus/containers)). In addition, the `NServiceBus.Ninject` NuGet package also allows your bindings to use an _Unit of Work_ scope which corresponds to the `DependencyLifecycle.InstancePerUnitOfWork` lifecycle when registering components with `configuration.RegisterComponents(...)` (read more about this in [Child Containers](child-containers.md)).

In essence, bindings using _Unit of Work_ scope
* will be instantiated only once per transport Message
* will be disposed when message processing finishes

You can bind your services in _Unit of Work_ scope using this:
<!-- import NinjectUnitOfWork -->

Services using `InUnitOfWorkScope()` can only be injected into code which is processing messages. If you want to inject your service somewhere else (e.g. because of an user interaction) you have to define conditional bindings:
<!-- import NinjectConditionalBindings -->
