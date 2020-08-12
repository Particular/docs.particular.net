---
title: NServiceBus.Extensions.Logging
summary: Logging with Microsoft.Extensions.Logging
reviewed: 2020-03-20
component: Extensions.Logging
related:
- samples/logging/extensions-logging
---

The `NServiceBus.Extensions.Logging` package provides support for writing NServiceBus log entries via the logging abstractions, [Microsoft.Extensions.Logging](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging), as explained in [Logging in .NET Core and ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/).

With this common logging abstraction, it is possible to log to different logging providers. Some third-party frameworks can perform semantic logging, also known as structured logging.

NOTE: This package should only be used when configuring logging in a self-host model. If hosting with the [.NET Generic Host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host) use [NServiceBus.Extensions.Hosting](/nservicebus/hosting/extensions-hosting.md) package instead.

## Compatibility

Although NServiceBus has native support for logging frameworks like log4net, NLog, and CommonLogging, it is recommended to use Microsoft.Extensions.Logging with these frameworks for new projects.

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

For an up-to-date list, see the Microsoft.Extensions.Logging fundamentals documentation, specifically the [built-in](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/#built-in-logging-providers) and [third-party](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/#third-party-logging-providers) providers sections.

## Support for ILogger<TCategoryName>

Microsoft.Extensions.Logging abstractions provide a generic interface that allows a dependency injection-friendly way to use loggers. NServiceBus.Extensions.Logging does not register any resolver for type `ILogger<TCategoryName>`. Usage of `ILogger<TCategoryName>` requires the use an external dependency injection container like [NServiceBus.Extensions.DependencyInjection](/nservicebus/dependency-injection/extensions-dependencyinjection.md) that contains registrations for the Microsoft.Extensions.Logging abstractions.

## Usage

Configure NServiceBus to use Microsoft.Extensions.Logging

snippet: ExtensionsLogging
