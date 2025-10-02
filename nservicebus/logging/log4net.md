---
title: Log4Net
summary: Logging to Log4Net
reviewed: 2024-08-20
component: Log4Net
---

> [!WARNING]
> NServiceBus.Log4Net is obsolete. NServiceBus is now providing support for logging libraries through the Microsoft.Extensions.Logging. Please see [Logging in .NET Core and ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/) for further details.

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
