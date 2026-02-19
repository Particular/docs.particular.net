---
title: Logging
summary: NServiceBus logging
reviewed: 2026-02-19
component: Core
isLearningPath: true
redirects:
- nservicebus/logging-in-nservicebus
- nservicebus/logging/usage
- nservicebus/logging/message-contents
related:
- samples/logging
---

> [!NOTE]
> It is recommended to directly use the [`Microsoft.Extensions.Logging`](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/) to log entries as it also supports semantic logging. Please see [Logging in .NET Core and ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/) for further details.

## Default Logging

NServiceBus has a built-in logging mechanism that does not depend on any external libraries. While limited in terms of available log targets, this built-in mechanism is production-ready and offers defaults that are reasonable for most deployments. The built-in framework is available and used as default in all NServiceBus hosting modes (e.g. self-hosting or Windows Service). Regardless of whether the built-in logging or a custom logging library is used under the hood, the NServiceBus logging abstractions can be used for [logging in the user code](#writing-a-log-entry). By default NServiceBus has three log targets configured:


### Console

All `Info` (and above) messages are written to the current console if one is available in the hosting environment.

Errors will be written with `ConsoleColor.Red`. Warnings will be written with `ConsoleColor.DarkYellow`. All other messages will be written with `ConsoleColor.White`.


### Trace

All `Warn` (and above) messages are written to `Trace.WriteLine` and therefore can be forwarded to any [trace listener](https://docs.microsoft.com/en-us/dotnet/framework/debug-trace-profile/trace-listeners).


### Rolling file

All `Info` (and above) messages are written to file. NServiceBus maintains up to 10 log files, each up to 10 MB in size. When the current file becomes full, NServiceBus automatically switches to the next one. If all ten files are full, the oldest file is overwritten.

The default logging directory is `HttpContext.Current.Server.MapPath("~/App_Data/")` for websites and `AppDomain.CurrentDomain.BaseDirectory` for all other processes.

The default file name is `nsb_log_yyyy-MM-dd_N.txt`, where `N` is a sequence number for when the log file reaches the max size.


### Changing the defaults

The built-in logging mechanism allows customizing the logging directory and applying a global filter/threshold for log entries.


#### Changing the Logging Level

Each log entry is associated with a _level_ that describes how important and critical that entry is. The built-in levels are following (in order of increasing importance)

 * Debug
 * Info
 * Warn
 * Error
 * Fatal

Configuring the global threshold to one of the levels described above means that all messages below that level are discarded. For example setting the threshold value to `Warn` means that only `Warn`, `Error` and `Fatal` messages are written.

The `LogManager` class is the entry point for the logging configuration. If needed, it allows using custom logging integrations (see below). It also allows customization of the default built-in logging. The `Use` generic method returns the `LoggingFactoryDefinition`-derived object that provides the customization APIs.

partial: level


#### Changing the log path

snippet: OverrideLoggingDirectoryInCode


## Custom Logging

partial: custom

> [!NOTE]
> Moving to custom logging means the [default logging](#default-logging) approaches are replaced.

## When to configure logging

It is important to configure logging before any endpoint configuration is done since logging is configured in the static context of each NServiceBus class. It should be configured *as early as possible* at the startup of the app. For example

 * At the start of the `Main` of a console app or windows service.
 * At the start of the `Global.Application_Start` in an ASP.NET application.
 * [Using endpoint configuration API in an application hosted via NServiceBus Host](/nservicebus/hosting/nservicebus-host/logging-configuration.md)


partial: exception-data

## Writing a log entry

In legacy endpoints the NServiceBus logging abstraction is used for writing log messages from user code.

Set up a single static field to an `ILog` in the classes, and then use it in all methods:

snippet: UsingLogging

Make sure that logging is correctly initialized before resolving the `ILog` instance. Not doing so can result in a logger using an incorrect configuration.

Since `LogManager.GetLogger(..);` is an expensive call, it is important that the field is `static` so that the call happens only once per class and has the best possible performance.

To avoid unnecessary processing, especially when logging more verbose messages, such as `Debug`, make sure to first check if logging at that level is enabled.

The `*Format` APIs pass their message and format arguments to the corresponding APIs of the underlying logging framework so their behavior varies. Some frameworks, like NLog, use special syntax to create structured log entries. Refer to the documentation of the specific logging framework for details. The built-in logging uses `string.Format` to generate the message that is written.

### Logging message contents

When NServiceBus sends a message, it writes the result of the `ToString()` method of the message class to the log. By default, this writes the name of the message type only. To write the full message contents to the log, override the `ToString()` method of the relevant message class:

snippet: MessageWithToStringLogged

NServiceBus only makes these calls at a log threshold of DEBUG or lower.

## Unit testing

Unit testing of logging is supported by [the `NServiceBus.Testing` library](/nservicebus/testing/#testing-logging-behavior).