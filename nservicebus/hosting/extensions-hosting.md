---
title: NServiceBus.Extensions.Hosting
summary: NServiceBus integration with Microsoft.Extensions.Hosting
component: Extensions.Hosting
reviewed: 2025-08-19
related:
 - samples/hosting/generic-host
 - samples/dependency-injection/aspnetcore
---

## Configuration

An NServiceBus endpoint can be hosted in the .NET Generic Host using the `UseNServiceBus` extension method:

snippet: extensions-host-configuration

This registers the endpoint with the hosting infrastructure and starts/stops it automatically with the application's lifetime.

> [!WARNING]
> Call `UseNServiceBus` **before** registering any component that needs `IMessageSession`. Placing it later can cause a `System.InvalidOperationException`:
>
> > The message session can't be used before NServiceBus is started. Place `UseNServiceBus()` on the host builder before registering any hosted service (e.g. `services.AddHostedService<HostedServiceAccessingTheSession>()`) or the web host configuration (e.g. `builder.ConfigureWebHostDefaults`) if hosted services or controllers require access to the session.

## Logging integration

NServiceBus logging is automatically wired to the host's logging pipeline. No NServiceBus-specific logging configuration is required. See [.NET logging](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging) and the [Generic Host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host).

> [!WARNING]
> Do **not** use [NServiceBus.Extensions.Logging](/nservicebus/logging/extensions-logging.md) or [NServiceBus.MicrosoftLogging.Hosting](https://www.nuget.org/packages/NServiceBus.MicrosoftLogging.Hosting) with `NServiceBus.Extensions.Hosting`.

## Dependency injection integration

When hosted in the Generic Host, NServiceBus uses the application's `IServiceCollection` / `IServiceProvider`. Message handlers can resolve services registered in `IServiceCollection`.

`UseNServiceBus` also registers an `IMessageSession` that can be resolved from the container or injected where needed at runtime.

### Using a custom DI container

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
