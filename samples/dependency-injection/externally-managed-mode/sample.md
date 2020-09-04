---
title: Externally managed container mode usage
summary: A sample that uses the NServiceBus externally managed container mode to configure a DI container.
component: Core
reviewed: 2020-09-04
related:
 - nservicebus/dependency-injection
---

### Configuring an endpoint to use ServiceProvider

The following code configures an endpoint with the [externally managed mode](/nservicebus/dependency-injection/#externally-managed-mode) using Microsoft's dependency injection container.

snippet: ContainerConfiguration

### Injecting the dependency in the handler

Services that have been registered with the `IServiceCollection` can be injected into message handlers via constructor injection:

snippet: InjectingDependency

### Injecting the message session into dependencies

The `IMessageSession` can be registered with the `IServiceCollection` so it can be injected as a dependency into other classes:

```csharp
serviceCollection.AddSingleton(p => endpointWithExternallyManagedContainer.MessageSession.Value);
```

snippet: InjectingMessageSession

NOTE: The `IMessageSession` can only be resolved from the `IServiceProvider` once the endpoint has been started.

