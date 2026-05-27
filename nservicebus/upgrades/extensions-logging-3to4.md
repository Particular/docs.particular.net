---
title: NServiceBus.Extensions.Logging Upgrade Version 3 to 4
summary: How to upgrade NServiceBus.Extensions.Logging from version 3 to 4
component: Extensions.Logging
reviewed: 2026-05-27
isUpgradeGuide: true
upgradeGuideCoreVersions:
  - 10
---

NServiceBus 10.2 introduces built-in support for [Microsoft.Extensions.Logging](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/) when hosting endpoints via `IServiceCollection.AddNServiceBusEndpoint`. This makes the `NServiceBus.Extensions.Logging` package unnecessary. The recommended path is to migrate to the [.NET Generic Host](/nservicebus/hosting/core-hosting.md) with `AddNServiceBusEndpoint` and remove the bridge package.

> [!NOTE]
> NServiceBus version 10.2 or later is required.

## ExtensionsLoggerFactory is deprecated

The `ExtensionsLoggerFactory` class has been deprecated. When hosting with the [.NET Generic Host](/nservicebus/hosting/core-hosting.md) using `AddNServiceBusEndpoint`, NServiceBus automatically uses the host's configured `Microsoft.Extensions.Logging` infrastructure. Remove any call to `LogManager.UseFactory(new ExtensionsLoggerFactory(...))`, then remove the `NServiceBus.Extensions.Logging` package reference.

### Recommended: Use the Generic Host

When using `AddNServiceBusEndpoint`, no code changes are required beyond removing the obsolete configuration:

Replace:

```csharp
var builder = Host.CreateApplicationBuilder();

var loggerFactory = LoggerFactory.Create(logging =>
{
    logging.AddNLog();
});

LogManager.UseFactory(new ExtensionsLoggerFactory(loggerFactory));

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
```

with:

```csharp
var builder = Host.CreateApplicationBuilder();

builder.Logging.AddNLog();

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
```

For more information about hosting with `AddNServiceBusEndpoint`, see [Hosting with Microsoft.Extensions.Hosting](/nservicebus/hosting/core-hosting.md).

### Self-hosted endpoints

For endpoints that are not yet using the .NET Generic Host and still call `Endpoint.Start`, logging providers can be registered on the endpoint's service collection via `RegisterComponents` as a temporary bridge:

```csharp
var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

endpointConfiguration.RegisterComponents(services =>
{
    services.AddLogging(logging => logging.AddNLog());
});

var endpointInstance = await Endpoint.Start(endpointConfiguration);
```

> [!WARNING]
> `Endpoint.Start` and `RegisterComponents` are also deprecated in NServiceBus 10.2. Transition to the Generic Host with `AddNServiceBusEndpoint` as soon as possible.

## Remove the package

After removing any use of `ExtensionsLoggerFactory`, remove the `NServiceBus.Extensions.Logging` package reference from the project. The built-in `Microsoft.Extensions.Logging` integration is provided by the core `NServiceBus` package starting with version 10.2.
