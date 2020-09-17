---
title: Externally managed Container Mode Usage
summary: A sample that uses NServiceBus externally managed container mode to configure a DI container.
component: Core
reviewed: 2020-09-17
related:
 - nservicebus/dependency-injection
---

### Configuring an endpoint to use ServiceProvider

The following code configures an endpoint with [externally managed mode](/nservicebus/dependency-injection/#externally-managed-mode) using Microsoft's dependency injection container.

snippet: ContainerConfiguration

### Injecting the dependency in the handler

Services registered with the `IServiceCollection` may be injected into message handlers using constructor injection:

snippet: InjectingDependency

### Injecting the message session into dependencies

The `IMessageSession` may be registered with the `IServiceCollection` so it can be injected as a dependency into other classes:

```csharp
serviceCollection.AddSingleton(p => endpointWithExternallyManagedContainer.MessageSession.Value);
```

snippet: InjectingMessageSession

NOTE: The `IMessageSession` may only be resolved from the `IServiceProvider` after the endpoint has been started.
