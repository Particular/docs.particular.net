---
title: Logging
summary: NServiceBus logging
reviewed: 2026-04-27
component: Core
isLearningPath: true
redirects:
- nservicebus/logging-in-nservicebus
- nservicebus/logging/usage
- nservicebus/logging/message-contents
related:
- samples/logging
---

## Default logging

NServiceBus has a built-in logging mechanism that does not depend on any external libraries. While limited in terms of available log targets, this built-in mechanism is production-ready and offers defaults that are reasonable for most deployments. The built-in framework is available and used as default in all NServiceBus hosting modes. Regardless of whether the built-in logging or a custom logging library is used under the hood, the NServiceBus logging abstractions can be used for writing log messages in user code. By default NServiceBus has three log targets configured:

### Console

All `Info` (and above) messages are written to the current console if one is available in the hosting environment.

Errors will be written with `ConsoleColor.Red`. Warnings will be written with `ConsoleColor.DarkYellow`. All other messages will be written with `ConsoleColor.White`.

### Trace

All `Warn` (and above) messages are written to `Trace.WriteLine` and therefore can be forwarded to any [trace listener](https://learn.microsoft.com/en-us/dotnet/framework/debug-trace-profile/trace-listeners).

### Rolling file

All `Info` (and above) messages are written to file. NServiceBus maintains up to 10 log files, each up to 10 MB in size. When the current file becomes full, NServiceBus automatically switches to the next one. If all ten files are full, the oldest file is overwritten.

The default logging directory is `HttpContext.Current.Server.MapPath("~/App_Data/")` for websites and `AppDomain.CurrentDomain.BaseDirectory` for all other processes.

The default file name is `nsb_log_yyyy-MM-dd_N.txt`, where `N` is a sequence number for when the log file reaches the max size.

### Changing the defaults

The built-in logging mechanism allows customizing the logging directory and applying a global filter/threshold for log entries.

#### Changing the logging level

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

## Custom logging

partial: custom

## When to configure logging

Logging must be configured *as early as possible* at application startup, before any NServiceBus endpoint configuration is performed. This is required because the NServiceBus logging infrastructure is initialized in a static context.

For example:

 * At the start of the `Main` method in console applications or Windows services
 * During application startup configuration in ASP.NET Core applications

## Writing a log entry

In legacy endpoints, the NServiceBus logging abstraction is used for writing log messages in application code.

Set up a single static field to an `ILog` in the classes, and then use it in all methods:

snippet: UsingLogging

Make sure that logging is correctly initialized before resolving the `ILog` instance. Not doing so can result in a logger using an incorrect configuration.

Since `LogManager.GetLogger(..);` is a relatively expensive call, it is important that the field is `static` so that the call happens only once per class and has the best possible performance. In addition, wrapping logging messages in conditional statements prevents unnecessary processing when a given level of logging is not needed. For example, when writing debug messsages, ensure `log.IsDebugEnabled` before proceeding.

The `*Format` APIs pass the message template and its arguments to the underlying logging framework, so their behavior can vary depending on the framework being used. Some frameworks, such as NLog, support special formatting syntax that allows structured log entries to be created. Refer to the documentation for the specific logging framework for details. When using the built-in logger, the message is formatted using `string.Format` before it is written.

partial: exception-data

## Logging message contents

When NServiceBus sends a message, it writes the result of the `ToString()` method of the message class to the log. By default, this writes the name of the message type only. To write the full message contents to the log, override the `ToString()` method of the relevant message class:

snippet: MessageWithToStringLogged

NServiceBus only makes these calls at a log threshold of DEBUG or lower.

## Unit testing

Unit testing of logging is supported by [the `NServiceBus.Testing` library](/nservicebus/testing/#testing-logging-behavior).
