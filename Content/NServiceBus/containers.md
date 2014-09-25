---
title: Containers
summary: NServiceBus automatically registers components, user-implemented handlers, and sagas.
tags: []
---

NServiceBus automatically registers all its components as well as user-implemented handlers and sagas so that all instancing modes and wiring are done correctly by default and without errors.

NServiceBus has a build in container (currently an ILMerged version of Autofac) but it can be replaced by any other container.

## Getting other containers

Other containers are available on nuget.

- http://www.nuget.org/packages/NServiceBus.Autofac/
- http://www.nuget.org/packages/NServiceBus.Ninject/
- http://www.nuget.org/packages/NServiceBus.CastleWindsor/
- http://www.nuget.org/packages/NServiceBus.StructureMap/
- http://www.nuget.org/packages/NServiceBus.Spring/
- http://www.nuget.org/packages/NServiceBus.Unity/

## Configuring NServiceBus to use other containers

### Version 4

<!-- import ContainersV4 -->

### Version 5

<!-- import ContainersV5 -->

## Plugging in your own container

### Version 4

 * Create a class that implements `IContainer`
 * Call `Configure.UsingContainer<T>()` in your configuration

<!-- import CustomContainersV4 -->

### Version 5

 * Create a class that implements `IContainer`
 * Create a class that implements `ContainerDefinition` and returns your `IContainer` implementation. Place this in the `NServiceBus` namespace for convenience to users.  
 * Register the container in the configuration with `.UseContainer<T>()`

<!-- import CustomContainersV5 -->
