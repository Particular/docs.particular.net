---
title: Containers
summary: NServiceBus automatically registers components, handlers, and sagas.
component: Core
reviewed: 2016-08-26
tags:
 - Dependency Injection
redirects:
 - nservicebus/containers
related:
- samples/containers
---

Services and state are managed by Containers and Dependency Injection. NServiceBus automatically registers all its components as well as user-implemented handlers and sagas so that all instancing modes and wiring are done correctly by default and without errors. NServiceBus has a built-in container (currently an [ILMerged](https://github.com/Microsoft/ILMerge) version of [Autofac](https://autofac.org/)) but it can be replaced by any other container.


## Dependency Lifecycle

There are three modes of 


### InstancePerCall

A new instance will be returned for each call.


#### Registration

snippet: InstancePerCall


#### Delegate Registration

snippet: DelegateInstancePerCall


### InstancePerUnitOfWork

The instance will be singleton for the duration of the [unit of work](/nservicebus/pipeline/unit-of-work.md). In practice this means the processing of a single transport message.


#### Registration

snippet: InstancePerUnitOfWork


#### Delegate Registration

snippet: DelegateInstancePerUnitOfWork


### SingleInstance

The same instance will be returned each time.


#### Registration

snippet: SingleInstance


#### Delegate Registration

snippet: DelegateSingleInstance


#### Register Single Instance

snippet: RegisterSingleton



## Supported Containers

Support for other containers is provided via custom integrations.

 * [Autofac](autofac.md)
 * [CastleWindsor](castlewindsor.md)
 * [Ninject](ninject.md)
 * [SimpleInjector](simpleinjector.md)
 * [Spring](spring.md)
 * [StructureMap](structuremap.md)
 * [Unity](unity.md)


## Using an existing container

The above pages all have examples of how to pass in an instance of an existing container. This is useful to make use of the full features of the container and share the DI behavior with NServiceBus.


partial: content


## Plugging in the container

If a specific container is not already supported, then create a plugin using the `IContainer` abstraction. Once this is created and registered, NServiceBus will use the custom container to look up its own dependencies.

partial: custom