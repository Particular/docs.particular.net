---
title: Autofac
summary: Configure NServiceBus to use Autofac as a container.
reviewed: 2016-03-17
tags:
- Dependency Injection
- IOC
- Autofac
---


NServiceBus can be configured to use [Autofac](http://autofac.org/) as a dependency injection container.


## Usage


### Pull in the NuGets

https://www.nuget.org/packages/NServiceBus.Autofac/

    Install-Package NServiceBus.Autofac


### The Code


#### Default Usage

snippet:Autofac


#### Existing Container Instance

snippet:Autofac_Existing