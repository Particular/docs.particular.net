---
title: NLog
summary: Logging to NLog
reviewed: 2021-09-17
component: NLog
related:
- samples/logging/nlog-custom
---

> [!Note]
> It is recommended to directly use the [`Microsoft.Extensions.Logging`](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/) to log entries as it also supports semantic logging. Please see [Logging in .NET Core and ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/) for further details.

Support for writing all NServiceBus log entries to [NLog](https://nlog-project.org/).


partial: usage


## Filtering

NServiceBus can write a significant amount of information to the log. To limit this information use the filtering features of the underlying logging framework.

For example to limit log output to a specific namespace.

Here is a code configuration example for adding a [Rule](https://github.com/nlog/NLog/wiki/Configuration-file#rules).

snippet: NLogFiltering

partial: nlog-exception-data
