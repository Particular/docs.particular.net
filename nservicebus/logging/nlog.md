---
title: NLog
summary: Logging to NLog
reviewed: 2024-08-20
component: NLog
related:
- samples/logging/nlog-custom
---

> [!WARNING]
> NServiceBus.NLog is obsolete. NServiceBus is now providing support for logging libraries through the Microsoft.Extensions.Logging. Please see [Logging in .NET Core and ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/) for further details.

partial: usage


## Filtering

NServiceBus can write a significant amount of information to the log. To limit this information use the filtering features of the underlying logging framework.

For example to limit log output to a specific namespace.

Here is a code configuration example for adding a [Rule](https://github.com/nlog/NLog/wiki/Configuration-file#rules).

snippet: NLogFiltering

partial: nlog-exception-data
