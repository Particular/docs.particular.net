---
title: NServiceBus.Extensions.Hosting Upgrade Version 4 to 5
summary: How to upgrade NServiceBus.Extensions.Hosting from version 4 to 5
component: Extensions.Hosting
reviewed: 2026-05-22
isUpgradeGuide: true
upgradeGuideCoreVersions:
  - 10
---

NServiceBus 10.2 introduces built-in support for hosting endpoints in .NET Generic Host applications via `IServiceCollection.AddNServiceBusEndpoint`. This makes the `NServiceBus.Extensions.Hosting` package's `UseNServiceBus` extension methods unnecessary.

> [!NOTE]
> NServiceBus version 10.2 or later is required.

## UseNServiceBus is deprecated

The `UseNServiceBus` extension methods on `HostApplicationBuilder` and `IHostBuilder` have been deprecated. Replace calls to `UseNServiceBus` with `AddNServiceBusEndpoint` on the host's service collection, then remove the `NServiceBus.Extensions.Hosting` package reference.

### HostApplicationBuilder

Replace:

```csharp
var builder = Host.CreateApplicationBuilder();

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
```

with:

```csharp
var builder = Host.CreateApplicationBuilder();

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

var host = builder.Build();
```

### IHostBuilder

Replace:

```csharp
var host = Host.CreateDefaultBuilder()
    .UseNServiceBus(context =>
    {
        var endpointConfiguration = new EndpointConfiguration("MyEndpoint");
        return endpointConfiguration;
    })
    .Build();
```

with:

```csharp
var builder = Host.CreateApplicationBuilder();

var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

var host = builder.Build();
```

## Remove the package

After replacing `UseNServiceBus` with `AddNServiceBusEndpoint`, remove the `NServiceBus.Extensions.Hosting` package reference from the project. The `AddNServiceBusEndpoint` method is provided by the core `NServiceBus` package starting with version 10.2.
