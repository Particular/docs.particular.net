---
title: Ninject
summary: Configure NServiceBus to use Ninject as a container.
reviewed: 2016-03-17
tags:
- Dependency Injection
related:
- nservicebus/containers/child-containers
---


NServiceBus can be configured to use [Ninject](http://www.ninject.org/) as a dependency injection container.


## Usage


### Pull in the NuGets

https://www.nuget.org/packages/NServiceBus.Ninject/

    Install-Package NServiceBus.Ninject


### The Code


#### Default Usage

snippet:Ninject


#### Existing Container Instance

snippet:Ninject_Existing


## Unit of work

In addition to that, the `NServiceBus.Ninject` NuGet package allows bindings to use an _Unit of Work_ scope which corresponds to the `DependencyLifecycle.InstancePerUnitOfWork` lifecycle when registering components with `configuration.RegisterComponents(...)`.

In essence, bindings using _Unit of Work_ scope

 * will be instantiated only once per transport Message
 * will be disposed when message processing finishes

Bind the services in _Unit of Work_ scope using:

snippet:NinjectUnitOfWork

Services using `InUnitOfWorkScope()` can only be injected into code which is processing messages. To inject the service somewhere else (e.g. because of an user interaction) define conditional bindings:

snippet:NinjectConditionalBindings
