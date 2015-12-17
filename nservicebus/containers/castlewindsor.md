---
title: Castle Windsor
summary: How to configure NServiceBus to use Castle Windsor as a container.
tags:
- Dependency Injection
- IOC
- Castle Windsor
---


NServiceBus can be configured to use [CastleWindsor](https://github.com/castleproject/Windsor) as a dependency injection container.


## Usage


### Pull in the NuGets

http://www.nuget.org/packages/NServiceBus.CastleWindsor/

    Install-Package NServiceBus.CastleWindsor


### The Code


#### Default Usage

snippet:CastleWindsor


#### Existing Container Instance

snippet:CastleWindsor_Existing

