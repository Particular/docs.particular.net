---
title: Externally Managed Mode
summary: A sample that uses the externally managed mode feature to configure a dependency injection container.
component: Core
versions: '[,10)'
reviewed: 2025-10-03
related:
 - nservicebus/dependency-injection
---

> [!WARNING]
> In NServiceBus version 10.2.0 and above, externally managed mode is no longer a relevant concept, as integration with the Microsoft .NET Host is integrated directly into NServiceBus.
>
> See the documentation on [hosting with Microsoft.Extensions.Hosting](/nservicebus/hosting/core-hosting.md) or the [NServiceBus version 10 to 11 upgrade guide](/nservicebus/upgrades/10to11/) for more information.

### Configuring the endpoint

This sample configures an endpoint to use [externally managed mode](/nservicebus/dependency-injection/#modes-of-operation-externally-managed-mode) with [Microsoft's dependency injection container](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection) and registers some dependencies.

snippet: ContainerConfiguration

### Injecting dependencies into handlers

Registered dependencies may be injected into message handlers using constructor injection:

snippet: InjectingDependency

### Injecting the message session into other types

When `IMessageSession` has been registered as shown above, it may be injected into other types using constructor injection:

snippet: InjectingMessageSession

> [!NOTE]
> An `IMessageSession` may only be injected by the container _after the endpoint has been started_.
