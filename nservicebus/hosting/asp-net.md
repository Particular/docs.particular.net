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

ASP.NET 6 introduced a new hosting API with the [ASP.NET minimal APIs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0). The [NServiceBus.Extensions.Hosting package](/nservicebus/hosting/extensions-hosting.md) is fully compatible with the minimal API's `WebApplication` host:

snippet: asp-net-minimal-host-endpoint

## Generic Host

Starting with ASP.NET 3, the [NServiceBus Generic Host support](/nservicebus/hosting/extensions-hosting.md) provides integration with ASP.NET Core applications:

snippet: asp-net-generic-host-endpoint

## Reading application settings

NServiceBus uses code-based configuration. To use values from `appsettings.json` or other sources, read them via `IConfiguration` and pass them to the NServiceBus configuration API.

snippet: extensions-host-appsettings

Both `WebApplication.CreateBuilder()` and `Host.CreateApplicationBuilder()` automatically load configuration from:

- `appsettings.json`
- `appsettings.{Environment}.json` (e.g., `appsettings.Development.json`)
- Environment variables

No additional setup is required to enable these sources.

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
