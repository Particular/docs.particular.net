---
title: Logging
summary: Manage and integrate with NServiceBus logging.
reviewed: 2017-10-07
component: Core
isLearningPath: true
tags:
- Logging
redirects:
- nservicebus/logging-in-nservicebus
related:
- samples/logging
---


## Default Logging

NServiceBus has some limited, and opinionated, logging built in.

The default logging behavior is as follows:

This is applicable to both self hosting and using the NServiceBus Host


### Console

All `Info` (and above) messages will be piped to the current console.

Errors will be written with `ConsoleColor.Red`. Warnings will be written with `ConsoleColor.DarkYellow`. All other message will be written with `ConsoleColor.White`.


### Trace

All `Warn` (and above) messages will be written to `Trace.WriteLine`.


### Rolling File

All `Info` (and above) messages will be written to a rolling log file.

This file will keep 10MB per file and a maximum of 10 log files.

The default logging directory will be `HttpContext.Current.Server.MapPath("~/App_Data/")` for websites and `AppDomain.CurrentDomain.BaseDirectory` for all other processes.

The default file name will be `nsb_log_yyyy-MM-dd_N.txt`, where `N` is a sequence number for when the log file reaches the max size.


## Changing the defaults

With code both the Level and the logging directory can be configured. To do this, use the `LogManager` class.

snippet: OverrideLoggingDefaultsInCode


## Logging Levels

The Logging level, or "Threshold", indicates the log levels that will be outputted. So for example a value of `Warn` would mean all `Warn`, `Error` and `Fatal` message would be outputted.

partial: level


## Custom Logging

For more advanced logging, it is recommended to utilize one of the many mature logging libraries available for .NET.

 * [Log4Net integration](log4net.md)
 * [NLog integration](nlog.md)
 * [CommonLogging integration](common-logging.md)
 * [Serilog integration](serilog.md)
 * [EventSourceLogging integration](eventsourcelogging.md)
 * [Microsoft.Extensions.Logging integration](microsoft.md)

Note: Moving to custom logging means the [default logging](#default-logging) approaches are replaced.


## When to configure logging

It is important to configure logging before any endpoint configuration is done since logging is configured in the static context of each NServiceBus class. It should be configured *as early as possible* at the startup of the app. For example

 * At the start of the `Main` of a console app or windows service.
 * At the start of the `Global.Application_Start` in a asp.net application.
 * [Using endpoint configuration API in an application hosted via NServiceBus Host](/nservicebus/hosting/nservicebus-host/logging-configuration.md)

## Unit testing

Unit testing of logging is supported by [the `NServiceBus.Testing` library](/nservicebus/testing/#testing-logging-behavior).