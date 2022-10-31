---
title: NServiceBus.Extensions.DependencyInjection Usage
summary: A sample that uses Microsoft's built-in dependency injection container
component: Extensions.DependencyInjection
reviewed: 2022-10-31
related:
 - nservicebus/dependency-injection
 - nservicebus/dependency-injection/extensions-dependencyinjection
---

### Configuring an endpoint to use ServiceProvider

The following code configures an endpoint with the [externally managed mode](/nservicebus/dependency-injection/#externally-managed-mode) using Microsoft's built-in dependency injection container:

snippet: ContainerConfiguration

### Injecting the message session into dependencies

snippet: InjectingMessageSession

### Injecting the dependency in the handler

snippet: InjectingDependency
