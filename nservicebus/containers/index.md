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

NServiceBus relies heavily on Containers and Dependency Injection to manage services and state.

NServiceBus automatically registers all its components as well as user-implemented handlers and sagas so that all instancing modes and wiring are done correctly by default and without errors.

NServiceBus has a built-in container (currently an ILMerged version of Autofac) but it can be replaced by any other container.


## Supported Containers

Support for other containers is provided via custom integrations.

 * [Autofac](autofac.md)
 * [CastleWindsor](castlewindsor.md)
 * [Ninject](ninject.md)
 * [Spring](spring.md)
 * [StructureMap](structuremap.md)
 * [Unity](unity.md)


## Using an existing container

The above pages all have examples of how to pass in an instance of an existing container. This is useful to make use of the full features of the container and share the DI behavior with NServiceBus.


partial: content


## Plugging in the container

If a specific container is not already supported, then create a plugin using the `IContainer` abstraction. Once this is created and registered, NServiceBus will use the custom container to look up its own dependencies.

partial: custom