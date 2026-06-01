---
title: NServiceBus in ASP.NET
summary: Hosting NServiceBus in ASP.NET Core applications
reviewed: 2026-05-05
component: Extensions.Hosting
related:
 - nservicebus/hosting/web-application
 - samples/web
---

NServiceBus easily integrates with [ASP.NET Core](https://asp.net) applications.

## Minimal APIs

ASP.NET 6 introduced a new hosting API with the [ASP.NET minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0). The [NServiceBus.Extensions.Hosting package](/nservicebus/hosting/extensions-hosting.md) is fully compatible with the minimal API's `WebApplication` host:

snippet: asp-net-minimal-host-endpoint

## Generic Host

Starting with ASP.NET 3, the [NServiceBus Generic Host support](/nservicebus/hosting/extensions-hosting.md) provides integration with ASP.NET Core applications:

snippet: asp-net-generic-host-endpoint

## Reading application settings

NServiceBus is configured in code. Values such as endpoint names, connection strings, and feature flags can be sourced from `appsettings.json` or any other configuration provider by reading them via `IConfiguration` and passing them to the NServiceBus configuration API.

When using the Minimal API host, read configuration via `builder.Configuration`:

snippet: asp-net-minimal-host-appsettings

When using the Generic Host, read configuration via the `HostBuilderContext` (or `hostBuilder.Configuration`):

snippet: extensions-host-appsettings

Both `WebApplication.CreateBuilder()` and `Host.CreateApplicationBuilder()` automatically load configuration from:

- `appsettings.json`
- `appsettings.{Environment}.json` (for example, `appsettings.Development.json`)
- Environment variables

No additional setup is required to enable these sources. See [Reading application settings](/nservicebus/hosting/extensions-hosting.md#reading-application-settings) for guidance on connection strings, the options pattern, and other configuration providers.

## Dependency injection

Messages can be sent with NServiceBus using the `IMessageSession` API, which is available via the dependency injection container.

### Web API controllers

snippet: web-api-usage

### MVC controllers

snippet: mvc-controller-usage

### Razor Pages

snippet: razor-page-usage

## Older ASP.NET Core versions

For web applications using ASP.NET Core 2.x, the [Community.NServiceBus.WebHost](https://github.com/timbussmann/Community.NServiceBus.WebHost) community package provides integration with the legacy WebHost.
