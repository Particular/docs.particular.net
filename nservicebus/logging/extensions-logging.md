---
title: NServiceBus.Extensions.Logging
summary: Logging to Microsoft.Extensions.Logging
reviewed: 2019-03-03
component: Extensions.Logging
tags:
- Logging
related:
- samples/logging/extensions-logging
---

Support for writing all NServiceBus log entries via the Common logging abstractions [Microsoft.Extensions.Logging](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging) as explained in [Logging in .NET Core and ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/).

Via this common logging abstraction it is possible to log to various logging providers. Some third-party frameworks can perform semantic logging, also known as structured logging.

## Compatiblity

Although NServiceBus has native support for various logging frameworks like log4net, NLog, CommonLogging, EventSourceLogging is it recommended to use these frameworks via Microsoft.Extensions.Logging for new projects.

At the time of this writing Microsoft.Extensions.Logging can be used to replace our following providers:

- [Common.Logging](common-logging.md) (Only if the configured provider in Common.Logging is supported by Microsoft.Extensions.Logging)
- [EventSource](eventsourcelogging.md)
- [Log4net](log4net.md)
- [NLog](nlog.md)

## Supported providers

For an up-to-date list please read the Microsoft.Extensions.Logging fundamentals documentation, specifically the listed [built-in](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/#built-in-logging-providers) and [thirdparty](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/#third-party-logging-providers) providers.

## Usage

Configure NServiceBus to use Microsoft.Extensions.Logging

snippet: ExtensionsLogging
