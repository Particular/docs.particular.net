---
title: NServiceBus.Extensions.Logging
summary: Logging with Microsoft.Extensions.Logging
reviewed: 2026-05-27
component: Extensions.Logging
related:
- samples/logging/extensions-logging
- nservicebus/upgrades/extensions-logging-4to5
---

> [!NOTE]
> It is recommended to directly use the [`Microsoft.Extensions.Logging`](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/) package to log entries as it also supports semantic logging. Please see [Logging in .NET Core and ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/) for further details.

> The `NServiceBus.Extensions.Logging` package's `ExtensionsLoggerFactory` is deprecated starting with NServiceBus 10.2.0. When hosting with the [.NET Generic Host](/nservicebus/hosting/core-hosting.md) using `AddNServiceBusEndpoint`, NServiceBus natively uses `Microsoft.Extensions.Logging` and the bridge package is no longer required. See the [upgrade guide](/nservicebus/upgrades/extensions-logging-4to5.md) for migration details.

The `NServiceBus.Extensions.Logging` package provides support for writing NServiceBus log entries via the [Microsoft.Extensions.Logging](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/) abstractions. With this common logging abstraction, it is possible to log to different logging providers. Some third-party frameworks can perform semantic logging, also known as structured logging.

## Compatibility

Microsoft.Extensions.Logging can be used to replace the following providers:

- [Common.Logging](common-logging.md) (Only if the configured provider in Common.Logging is supported by Microsoft.Extensions.Logging)
- [Log4net](log4net.md)
- [NLog](nlog.md)

## Supported providers

NServiceBus supports the following logging frameworks via Microsoft.Extensions.Logging:

- elmah.io
- Gelf
- JSNLog
- KissLog.net
- Log4Net
- Loggr
- NLog
- Sentry
- Serilog
- Stackdriver

For an up-to-date list, see the Microsoft.Extensions.Logging fundamentals documentation, specifically the [built-in](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/#built-in-logging-providers) and [third-party](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/#third-party-logging-providers) providers sections.

## Support for ILogger<TCategoryName>

Microsoft.Extensions.Logging abstractions provide a generic interface that allows a dependency injection-friendly way to use loggers. NServiceBus.Extensions.Logging does not register any resolver for type `ILogger<TCategoryName>`. Usage of `ILogger<TCategoryName>` requires the use an external dependency injection container like [NServiceBus.Extensions.DependencyInjection](/nservicebus/dependency-injection/extensions-dependencyinjection.md) that contains registrations for the Microsoft.Extensions.Logging abstractions.

## Usage

Configure NServiceBus to use Microsoft.Extensions.Logging:

snippet: ExtensionsLogging
