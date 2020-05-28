---
title: Spring
summary: Details on how to Configure NServiceBus to use Spring for dependency injection. Includes usage examples as well as lifecycle mappings.
component: Spring
reviewed: 2018-12-05
related:
 - samples/dependency-injection/spring
redirects:
 - nservicebus/containers/spring
---


NServiceBus can be configured to use [Spring](https://www.springframework.net/) for dependency injection.


### Default usage

snippet: Spring


### Using an existing context

snippet: Spring_Existing


### DependencyLifecycle Mapping

The [NServiceBus dependency lifecycle modes](/nservicebus/dependency-injection/) do not map directly to Spring object scopes. Almost all of the lifecycle modes have been implemented using custom type registrations using a Spring [`GenericApplicationContext`](https://springframework.net/docs/1.1-RC1/sdk/1.1/html/Spring.Core~Spring.Context.Support.GenericApplicationContext.html).


include: property-injection