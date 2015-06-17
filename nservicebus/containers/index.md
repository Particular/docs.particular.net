---
title: Containers
summary: NServiceBus automatically registers components, user-implemented handlers, and sagas.
tags: 
- Dependency Injection
- IOC
- ObjectBuilder
- Customization
- Disposing
- Container
redirects:
- nservicebus/containers
related:
- samples/containers
---

NServiceBus relies heavily on Containers and Dependency Injection to manage services and state.

NServiceBus automatically registers all its components as well as user-implemented handlers and sagas so that all instancing modes and wiring are done correctly by default and without errors.

NServiceBus has a built-in container (currently an ILMerged version of Autofac) but it can be replaced by any other container.

Using an external container is usefull when:

* A external container is already present and you want types that are registered in that container to be injected in your messages handlers or other types that NServiceBus creates internally.
* Have lifetime requirements on the created instances that are not supported by the  NServiceBus object builder.
* Not want to use the NServiceBus object builder because you are already familiar with other dependency injection frameworks.


## Registration behavior

When you pass an external container in the `busConfiguration.UseContainer<>` method NServiceBus will register the `IBus` instance in the container.

There is no need to register the `IBus` instance after the bus has been created.


## Disposing

### Bus instance

When using an external container you would normally not dispose the bus instance manually. If you would call `IBus.Dispose()` then you will indirectly trigger the container to dispose lifetime scope

NOTE: Do NOT call `IBus.Dispose` when using an external container.


### Container

When the injected container is disposed then the registered bus instance will be disposed too. This means that when the container is disposed the bus will be stopped and will not process messages.

### NServiceBus.Host

When using the NServiceBus host and injecting an external container then this container will be disposed when the host stops the bus


## Getting other containers

Other containers are available on NuGet.

- http://www.nuget.org/packages/NServiceBus.Autofac/
- http://www.nuget.org/packages/NServiceBus.Ninject/
- http://www.nuget.org/packages/NServiceBus.CastleWindsor/
- http://www.nuget.org/packages/NServiceBus.StructureMap/
- http://www.nuget.org/packages/NServiceBus.Spring/
- http://www.nuget.org/packages/NServiceBus.Unity/

## Configuring NServiceBus to use other containers

The following code demonstrates how to configure NServiceBus to use a container of your choosing. Each assumes you've already included the relevant NuGet package above.

<!-- import Containers --> 

## Plugging in your own container

If you have your own container that is not already supported by a NuGet package, you can create a plugin centering around the `IContainer` abstraction. Once this is created and registered, NServiceBus will use your custom container to look up its own dependencies.

<!-- import CustomContainers -->