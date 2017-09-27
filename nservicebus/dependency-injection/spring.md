---
title: Spring
summary: Details on how to Configure NServiceBus to use Spring for dependency injection. Includes usage examples as well as lifecycle mappings.
component: Spring
reviewed: 2016-11-28
tags:
 - Dependency Injection
related:
 - samples/dependency-injection/spring
redirects:
 - nservicebus/containers/spring
---


NServiceBus can be configured to use [Spring](http://www.springframework.net/) for dependency injection.


### Default Usage

snippet: Spring


### Existing Instance

snippet: Spring_Existing


### DependencyLifecycle Mapping

The way that the NServiceBus.Spring adapter is implemented means that the [dependency lifecycle](/nservicebus/dependency-injection/#dependency-lifecycle)'s of NServiceBus do not map directly to Spring Object Scopes. Almost all of the lifecycles have been implemented using a custom type regsitrations on top of a [Generic Application Context](http://springframework.net/docs/1.1-RC1/sdk/1.1/html/Spring.Core~Spring.Context.Support.GenericApplicationContext.html).