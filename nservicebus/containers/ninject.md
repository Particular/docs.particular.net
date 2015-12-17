---
title: Ninject
summary: How to configure NServiceBus to use Ninject as a container.
tags:
- Dependency Injection
- IOC
- Ninject
---


NServiceBus can be configured to use [Ninject](http://www.ninject.org/) as a dependency injection container.


## Usage


### Pull in the NuGets

http://www.nuget.org/packages/NServiceBus.Ninject/

    Install-Package NServiceBus.Ninject


### The Code


#### Default Usage

snippet:Ninject


#### Existing Container Instance

snippet:Ninject_Existing


## Unit of work

In addition to that, the `NServiceBus.Ninject` NuGet package allows your bindings to use an _Unit of Work_ scope which corresponds to the `DependencyLifecycle.InstancePerUnitOfWork` lifecycle when registering components with `configuration.RegisterComponents(...)` (read more about this in [Child Containers](child-containers.md)).

In essence, bindings using _Unit of Work_ scope

* will be instantiated only once per transport Message
* will be disposed when message processing finishes

You can bind your services in _Unit of Work_ scope using:

snippet:NinjectUnitOfWork

Services using `InUnitOfWorkScope()` can only be injected into code which is processing messages. If you want to inject your service somewhere else (e.g. because of an user interaction) you have to define conditional bindings:

snippet:NinjectConditionalBindings
