---
title: Castle Windsor
summary: Configure NServiceBus to use Castle Windsor as a container.
reviewed: 2016-03-17
tags:
- Dependency Injection
- IOC
---


NServiceBus can be configured to use [CastleWindsor](https://github.com/castleproject/Windsor) as a dependency injection container.


## Usage


### Pull in the NuGets

https://www.nuget.org/packages/NServiceBus.CastleWindsor/

    Install-Package NServiceBus.CastleWindsor


### The Code


#### Default Usage

snippet:CastleWindsor


#### Existing Container Instance

snippet:CastleWindsor_Existing
