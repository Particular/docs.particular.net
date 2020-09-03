---
title: Spring
summary: Details on how to Configure NServiceBus to use Spring for dependency injection.
component: Spring
reviewed: 2020-08-17
related:
 - samples/dependency-injection/spring
redirects:
 - nservicebus/containers/spring
---

include: container-deprecation-notice

NServiceBus can be configured to use [Spring](https://www.springframework.net/) for dependency injection.

### Default usage

snippet: Spring

### Using an existing context

snippet: Spring_Existing

### DependencyLifecycle Mapping

The [NServiceBus dependency lifecycle modes](/nservicebus/dependency-injection/) do not map directly to Spring object scopes. Almost all of the lifecycle modes have been implemented using custom type registrations using a Spring [`GenericApplicationContext`](http://springframework.net/docs/1.3.2/api/net-2.0/html/topic1040.html).

include: property-injection
