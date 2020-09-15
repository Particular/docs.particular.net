---
title: NServiceBus.Extensions.Hosting
summary: NServiceBus integration with Microsoft.Extensions.Hosting
component: Extensions.Hosting
reviewed: 2020-08-18
related:
 - samples/hosting/generic-host
 - samples/dependency-injection/aspnetcore
 - samples/netcore-reference
---

## Configuration

An NServiceBus endpoint can be hosted within the generic host with the `UseNServiceBus` extension method:

snippet: extensions-host-configuration

This code will register the endpoint with the hosting infrastructure and automatically start and stop it based on the host's application lifetime.

WARNING: `UseNServiceBus` must be specified before any other service (e.g. `ConfigureWebHostDefaults`) which requires access to the `IMessageSession`. Incorrect usage results in a `System.InvalidOperationException` with the following message `The message session can't be used before NServiceBus is started. Place UseNServiceBus() on the host builder before registering any hosted service (e.g. services.AddHostedService<HostedServiceAccessingTheSession>()) or the web host configuration (e.g. builder.ConfigureWebHostDefaults) if hosted services or controllers require access to the session`.

## Logging integration

NServiceBus logging is automatically configured to use the [logging configured](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging) for the [generic host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host); no NServiceBus specific logging configuration is needed.

WARNING: [NServiceBus.Extensions.Logging](/nservicebus/logging/extensions-logging.md) or [NServiceBus.MicrosoftLogging.Hosting](https://www.nuget.org/packages/NServiceBus.MicrosoftLogging.Hosting) should not be used.

## Dependency injection integration

NServiceBus endpoints hosted as part of the generic host automatically use the provided `IServiceCollection` and `IServiceProvider` dependency injection infrastructure. Message handlers can resolve dependencies which are registered in the `IServiceCollection`.

`UseNServiceBus` automatically registers an `IMessageSession` with the container which can be resolved from the `IServiceProvider` or via dependency injection during runtime.

### Configure custom containers

Custom dependency injection containers may be configured using `IWebHostBuilder.UseServiceProviderFactory`. NServiceBus automatically uses the host's dependency injection container. Refer to the container's documentation for further details.
