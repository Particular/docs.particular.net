---
title: Common.Logging
summary: Using the Common.Logging abstraction with NServiceBus
component: CommonLogging
reviewed: 2026-05-27
---

> [!NOTE]
> It is recommended to directly use the [`Microsoft.Extensions.Logging`](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/) to log entries as it also supports semantic logging. Please see [Logging in .NET Core and ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/) for further details.

## Logging integration into the host

When using NServiceBus 10.2 or later with the [.NET Generic Host](/nservicebus/hosting/core-hosting.md), use `Microsoft.Extensions.Logging` directly instead of Common.Logging. The bridge package is not required:

```csharp
var builder = Host.CreateApplicationBuilder();

builder.Logging.AddConsole();

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
```

For more information, see [Hosting with Microsoft.Extensions.Hosting](/nservicebus/hosting/core-hosting.md).

The `NServiceBus.CommonLogging` package provides support for writing NServiceBus log entries to [Common.Logging](https://github.com/net-commons/common-logging). Common.Logging provides a simple logging abstraction to switch between different logging implementations, similar to [Microsoft.Extensions.Logging](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging).

## Usage

snippet: CommonLoggingInCode
