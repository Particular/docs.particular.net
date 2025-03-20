---
title: Common.Logging
summary: Using the Common.Logging abstraction with NServiceBus
component: CommonLogging
reviewed: 2025-03-06
related:
- samples/logging/commonlogging
---

> [!NOTE]
> It is recommended to directly use the [`Microsoft.Extensions.Logging`](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/) to log entries as it also supports semantic logging. Please see [Logging in .NET Core and ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/) for further details.

The `NServiceBus.CommonLogging` package provides support for writing NServiceBus log entries to [Common.Logging](https://github.com/net-commons/common-logging). Common.Logging provides a simple logging abstraction to switch between different logging implementations, similar to [Microsoft.Extensions.Logging](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging).

## Usage

snippet: CommonLoggingInCode
