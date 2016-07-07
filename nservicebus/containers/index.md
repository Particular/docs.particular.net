---
title: Containers
summary: NServiceBus automatically registers components, user-implemented handlers, and sagas.
tags:
- Dependency Injection
- IOC
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

- [Autofac](autofac.md)
- [Ninject](ninject.md)
- [CastleWindsor](castlewindsor.md)
- [StructureMap](structuremap.md)
- [Spring](spring.md)
- [Unity](unity.md)


## Using an existing container

The above pages all have examples of how to pass in an instance of an existing container. This is useful to make use of the full features of the container and share the DI behavior with NServiceBus.


### Endpoint resolution

Note that the instance of `IBus` (in Version 3-5) is scoped for the lifetime of the container. Hence to resolve `IBus` and then dispose of it the endpoint will stop processing messages. Note that all NServiceBus services, including `IBus`, will be injected into the passed in container instance. As such there is no need to register these instances at configuration time. In Version 6 `IEndpointInstance` needs to be registered to be properly resolved.


### Cleanup

NOTE: In Version 6 `IEndpointInstance` is not `IDisposable`.
When using an external container normally the bus instance is not disposed of manually. If `IBus.Dispose()` is called that would indirectly trigger the container to dispose lifetime scope.

NOTE: Do NOT call `IBus.Dispose` when using an external container, instead call dispose in the container during shutdown.


## Plugging in the container

If a specific container is not already supported, then create a plugin using the `IContainer` abstraction. Once this is created and registered, NServiceBus will use the custom container to look up its own dependencies.

snippet:CustomContainers
