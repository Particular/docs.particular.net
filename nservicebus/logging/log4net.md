---
title: Log4Net
summary: Logging to Log4Net
reviewed: 2026-05-27
component: Log4Net
---

> [!WARNING]
> NServiceBus.Log4Net is obsolete. NServiceBus now supports logging libraries through Microsoft.Extensions.Logging. See [Logging in .NET Core and ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/) for details.

## Logging integration into the host

When using NServiceBus 10.2 or later with the [.NET Generic Host](/nservicebus/hosting/core-hosting.md), configure Log4Net directly on the host builder. The bridge package is not required:

```csharp
var builder = Host.CreateApplicationBuilder();

builder.Logging.AddLog4Net();

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
```

For more information, see [Hosting with Microsoft.Extensions.Hosting](/nservicebus/hosting/core-hosting.md).

## Usage

snippet: Log4NetInCode


## Filtering

NServiceBus can write a significant amount of information to the log. To limit this information, use the filtering features of the underlying logging framework.

For example, to limit log output to a specific namespace.

The following code example adds a [filter](https://logging.apache.org/log4net/manual/configuration.html#filters).

### The Filter

snippet: Log4NetFilter


### Using the Filter

snippet: Log4NetFilterUsage


## Additional exception data

Starting with NServiceBus version 7.2, exceptions from failing message handlers might contain additional error information in the `Exception.Data` property. Log4Net does not log this information by default, but it can be configured to do so using a custom `PatternLayoutConverter`:

snippet: ExceptionDataConverter

The custom converter can then be registered and incorporated into the log layout:

snippet: RegisterConverter
