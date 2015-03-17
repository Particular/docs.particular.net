---
title: Containers
summary: NServiceBus automatically registers components, user-implemented handlers, and sagas.
tags: 
- Dependency Injection
- IOC
redirects:
- nservicebus/containers
---

NServiceBus automatically registers all its components as well as user-implemented handlers and sagas so that all instancing modes and wiring are done correctly by default and without errors.

NServiceBus has a built-in container (currently an ILMerged version of Autofac) but it can be replaced by any other container.

## Getting other containers

Other containers are available on nuget.

- http://www.nuget.org/packages/NServiceBus.Autofac/
- http://www.nuget.org/packages/NServiceBus.Ninject/
- http://www.nuget.org/packages/NServiceBus.CastleWindsor/
- http://www.nuget.org/packages/NServiceBus.StructureMap/
- http://www.nuget.org/packages/NServiceBus.Spring/
- http://www.nuget.org/packages/NServiceBus.Unity/

## Configuring NServiceBus to use other containers

<!-- import Containers --> 

## Plugging in your own container

<!-- import CustomContainers -->

