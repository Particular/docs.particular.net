---
title: Log4Net
summary: Logging to Log4Net
reviewed: 2021-09-08
component: Log4Net
related:
- samples/logging/log4net-custom
---
> [!Note]
> It is recommended to directly use the [`Microsoft.Extensions.Logging`](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/) to log entries as it also supports semantic logging. Please see [Logging in .NET Core and ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/) for further details.


Support for writing all NServiceBus log entries to [Log4Net](https://logging.apache.org/log4net/).


## Usage

snippet: Log4netInCode


## Filtering

NServiceBus can write a significant amount of information to the log. To limit this information use the filtering features of the underlying logging framework.

For example to limit log output to a specific namespace.

Here is a code configuration example for adding a [Filter](https://logging.apache.org/log4net/release/manual/configuration.html#filters).


### The Filter

snippet: Log4netFilter


### Using the Filter

snippet: Log4netFilterUsage


## Additional exception data

Starting from NServiceBus version 7.2, exceptions from failing message handlers might contain additional error information in the `Exception.Data` property. Log4Net does not log this information by default, but can be configured to do so using a custom `PatternLayoutConverter`:

snippet: ExceptionDataConverter

The custom converter can then be registered and incorporated into the log layout:

snippet: RegisterConverter
