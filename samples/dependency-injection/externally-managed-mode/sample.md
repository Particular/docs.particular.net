---
title: Externally Managed Mode
summary: A sample that uses NServiceBus's externally managed mode feature to configure a DI container.
component: Core
reviewed: 2020-09-22
related:
 - nservicebus/dependency-injection
---

### Configuring the endpoint

The sample configures an endpoint to use [externally managed mode](/nservicebus/dependency-injection/#externally-managed-mode) with [Microsoft's dependency injection container](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection) and registers some dependencies.

snippet: ContainerConfiguration

### Injecting dependencies into handlers

Registered dependencies may be injected into message handlers using constructor injection:

snippet: InjectingDependency

### Injecting the message session into other types

When `IMessageSession` has been registered as shown above, it may be injected into other types using constructor injection:

snippet: InjectingMessageSession

NOTE: An `IMessageSession` may only be injected by the container _after the endpoint has been started_.
