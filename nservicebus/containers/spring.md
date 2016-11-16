---
title: Spring
summary: Configure NServiceBus to use Spring as a container.
component: Spring
reviewed: 2016-03-17
tags:
- Dependency Injection
---


NServiceBus can be configured to use [Spring](http://www.springframework.net/) as a dependency injection container.


### Default Usage

snippet:Spring


### Existing Container Instance

snippet:Spring_Existing


### DependencyLifecycle Mapping

The way that the NServiceBus.Spring adapter is implemented means that the [dependency lifecycle](/nservicebus/containers/#dependency-lifecycle)'s of NServiceBus do not map directly to Spring Object Scopes. Almost all of the lifecycles have been implemented using a custom type regsitrations on top of a [Generic Application Context](http://springframework.net/docs/1.1-RC1/sdk/1.1/html/Spring.Core~Spring.Context.Support.GenericApplicationContext.html).