---
title: Upgrade Version 10 to 11
summary: Instructions on how to upgrade NServiceBus from version 10 to version 11.
reviewed: 2025-09-03
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 10
 - 11
---

include: upgrade-major

## Self-hosted endpoints

With the ubiquity of the .NET Generic Host as the entry point for an application's hosting, dependency injection, and logging needs, it no longer makes sense to self-host NServiceBus endpoints using `Endpoint.Create()` or `Endpoint.Start()`. Instead, NServiceBus endpoints can be added to the `IServiceCollection` which will cause them to start along with the host's lifecycle.

Instead of:

```csharp
var endpointInstance = await Endpoint.Start(endpointConfiguration);

// or

var startableEndpoint = await Endpoint.Create(endpointConfiguration);
var endpointInstance = await startableEndpoint.Start();
```

…the endpoint can be started through the .NET Generic Host:

```csharp
var builder = Host.CreateApplicationBuilder();

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

var host = builder.Build();

await host.RunAsync();
```

In addition, the following APIs related to creating and starting endpoints with the self-hosting API are deprecated and no longer necessary when using the .NET Generic Host:

- `NServiceBus.Endpoint`
- `NServiceBus.Installer`
- `NServiceBus.IEndpointInstance`
- `NServiceBus.IStartableEndpoint`
- `NServiceBus.IStartableEndpointWithExternallyManagedContainer`

### Endpoint-specific dependency injection

The `RegisterCompoments(Action<IServiceCollection> registration)` method on `EndpointConfiguration` is obsolete and must be replaced. Originally this method was meant to allow dependency injection registrations when self-hosting, but is no longer necessary without self-hosted endpoints. It is better practice to manage dependency injection registrations using standard .NET idioms through the Generic Host.

Instead of:

```csharp
var endpointConfiguration = new EndpointConfiguration("EndpointName");
endpointConfiguration.RegisterComponents(registrations =>
{
    registrations.AddSingleton<EndpointSpecificService>();
});
```

…the service can be added to the global `IServiceCollection` when only one NServiceBus endpoint is defined, and the endpoint will resolve the dependency from the global collection:

```csharp
var endpointConfiguration = new EndpointConfiguration("EndpointName");

builder.Services.AddSingleton<EndpointSpecificService>();
builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
```

When multiple endpoints are hosted in the same process, each endpoint can receive its own configured dependency using [keyed services](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection#keyed-services), where the key matches the endpoint name by default:

```csharp
var salesConfig = new EndpointConfiguration("Sales");
var billingConfig = new EndpointConfiguration("Billing");

var salesDb = new DatabaseService("sales-db");
var billingDb = new DatabaseService("billing-db");

builder.Services.AddKeyedSingleton<DatabaseService>("Sales", salesDb);
builder.Services.AddKeyedSingleton<DatabaseService>("Billing", billingDb);

builder.Services.AddNServiceBusEndpoint(salesConfig, "Sales");
builder.Services.AddNServiceBusEndpoint(billingConfig, "Billing");
```

### Logging

TODO

Deprecated list:

- NServiceBus.Logging.DefaultFactory - use `IServiceCollection.Configure<RollingLoggerProviderOptions>()` instead
  - void Directory(string directory) - use `RollingLoggerProviderOptions.Directory` instead
  - void Level(LogLevel level) - use `RollingLoggerProviderOptions.LogLevel` instead
- NServiceBus.Logging.LogManager.Use<T>() where T : LoggingFactoryDefinition - Use Microsoft.Extensions.Logging instead
- NServiceBus.Logging.LogManager.UseFactory(ILoggerFactory loggerFactory) - Use Microsoft.Extensions.Logging instead
- NServiceBus.Logging.LoggingFactoryDefinition
