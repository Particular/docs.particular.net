---
title: NServiceBus.Extensions.Hosting
summary: NServiceBus package-based integration with Microsoft.Extensions.Hosting
component: Extensions.Hosting
reviewed: 2026-05-05
related:
 - samples/hosting/generic-host
 - samples/dependency-injection/aspnetcore
---

> [!NOTE]
> For new development, prefer [hosting with Microsoft.Extensions.Hosting](/nservicebus/hosting/core-hosting.md) using the built-in `AddNServiceBusEndpoint` integration on `IServiceCollection`. The **NServiceBus.Extensions.Hosting** package's `UseNServiceBus` hosting method described on this page is deprecated starting with NServiceBus 10.2.0. See the [upgrade guide](/nservicebus/upgrades/extensions-hosting-4to5.md) for migration details.

## Configuration

An NServiceBus endpoint can be hosted in the .NET Generic Host using the `UseNServiceBus` extension method:

snippet: extensions-host-configuration

This registers the endpoint with the hosting infrastructure and starts/stops it automatically with the application's lifetime.

> [!WARNING]
> Call `UseNServiceBus` **before** registering any component that needs `IMessageSession`. Placing it later can cause a `System.InvalidOperationException`:
>
> > The message session can't be used before NServiceBus is started. Place `UseNServiceBus()` on the host builder before registering any hosted service (e.g. `services.AddHostedService<HostedServiceAccessingTheSession>()`) or the web host configuration (e.g. `builder.ConfigureWebHostDefaults`) if hosted services or controllers require access to the session.

## Reading application settings

NServiceBus is configured in code. Values such as endpoint names, connection strings, and feature flags can be sourced from `appsettings.json` or any other configuration provider by reading them via `IConfiguration` and passing them to the NServiceBus configuration API.

snippet: extensions-host-appsettings

The Generic Host automatically loads configuration from:

- `appsettings.json`
- `appsettings.{Environment}.json` (for example, `appsettings.Development.json`)
- Environment variables

No additional setup is required to enable these sources.

### Connection strings

Transport and persistence connection strings are typically stored in the `ConnectionStrings` section of `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Transport": "host=localhost;username=guest;password=guest"
  }
}
```

Read them with `IConfiguration.GetConnectionString("Transport")` and pass the result to the transport configuration:

snippet: extensions-host-connection-string

The exact transport API depends on the transport package in use; see the documentation for the [transport](/transports/) being configured.

### Strongly-typed settings

For more complex configuration, bind a settings class to a configuration section using the [.NET options pattern](https://learn.microsoft.com/en-us/dotnet/core/extensions/options) and use the bound values during endpoint setup:

```json
{
  "NServiceBus": {
    "EndpointName": "Sales",
    "MaxConcurrency": 8
  }
}
```

snippet: extensions-host-options-pattern

### Other configuration sources

Because the entry point is `IConfiguration`, any configuration provider works without additional integration, including [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets), [Azure Key Vault](https://learn.microsoft.com/en-us/azure/key-vault/general/overview), and AWS Systems Manager Parameter Store. See the [.NET configuration providers documentation](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration-providers) for the full list.

> [!NOTE]
> Endpoint configuration values are read once at startup. The reload-on-change behavior provided by `IOptionsMonitor<T>` does not apply to endpoint configuration after the endpoint has started.

### Azure Functions and other hosts

The same `IConfiguration`-based pattern applies when hosting in [Azure Functions](/nservicebus/hosting/azure-functions-service-bus/). `ServiceBusTriggeredEndpointConfiguration` reads values from the Functions host's `IConfiguration` (and falls back to environment variables), so settings flow through `local.settings.json` in development and through Function app settings in Azure. See the [Azure Functions configuration reference](/nservicebus/hosting/azure-functions-service-bus/#configuration) for the keys recognized by the Azure Service Bus integration, including connection-string and identity-based connection options.

When connecting endpoints across transports using the [NServiceBus.Transport.Bridge](/nservicebus/bridge/), the same `HostBuilderContext.Configuration` access is available during bridge setup; see [Bridge configuration](/nservicebus/bridge/configuration.md).

## Logging integration

NServiceBus logging is automatically wired to the host's logging pipeline. No NServiceBus-specific logging configuration is required. See [.NET logging](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging) and the [Generic Host](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host).

> [!WARNING]
> Do **not** use [NServiceBus.Extensions.Logging](/nservicebus/logging/extensions-logging.md) or [NServiceBus.MicrosoftLogging.Hosting](https://www.nuget.org/packages/NServiceBus.MicrosoftLogging.Hosting) with `NServiceBus.Extensions.Hosting`.

## Dependency injection integration

When hosted in the Generic Host, NServiceBus uses the application's `IServiceCollection` / `IServiceProvider`. Message handlers can resolve services registered in `IServiceCollection`.

`UseNServiceBus` also registers an `IMessageSession` that can be resolved from the container or injected where needed at runtime.

### Configure custom containers

To use a third-party container, configure it with `HostBuilder.UseServiceProviderFactory(...)`. NServiceBus will automatically use the host’s container. Refer to your container’s documentation for details.

partial: shutdown-timeout

## Stopping the endpoint

With the Generic Host, the `IEndpointInstance` used to stop the endpoint is not exposed directly. To shut down gracefully, request application shutdown via [`IHostApplicationLifetime`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihostapplicationlifetime). See [Generic Host application lifetime](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host#ihostapplicationlifetime) for more information.

## Installers

Avoid always running [NServiceBus installers](/nservicebus/operations/installers.md) via `.EnableInstallers()` as it adds startup time and may require elevated permissions.

Instead, run installers explicitly in a dedicated “setup” mode:

```csharp
var isSetup = args.Contains("-s") || args.Contains("/s");

if (isSetup)
{
    // Installers are useful in development. Consider disabling in production.
    // https://docs.particular.net/nservicebus/operations/installers
    // endpointConfiguration.EnableInstallers();

    await Installer.Setup(endpointConfiguration);
    return;
}

// Continue and eventually invoke:
// await host.RunAsync();
