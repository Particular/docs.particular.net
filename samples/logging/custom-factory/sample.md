---
title: Custom Logger Factory
summary: Illustrates a custom implementation of a logging factory.
reviewed: 2016-06-18
component: Core
tags:
 - Logging
related:
 - nservicebus/logging
---


Illustrates a custom implementation of a logging factory. For simplicity this sample writes all log message to the console.

Note: The approach of creating a custom logging factory should not be required in the development of most business applications. This API is designed for routing NServiceBus log messages to a [third party logging libraries](/components#loggers). To gain more control over logging targets is is recommended to leverage one of these logging libraries.

It is also possible to see full implementations of logging factories by looking at the code for the other logging libraries.

 * https://github.com/Particular/NServiceBus.NLog
 * https://github.com/Particular/NServiceBus.Log4Net
 * https://github.com/Particular/NServiceBus.CommonLogging
 * https://github.com/SimonCropp/NServiceBus.Serilog


## Logging Definition

To configure a custom logging factory a custom `LoggingFactoryDefinition` must be implemented.

snippet: definition


## Logger Factory

The `LoggingFactoryDefinition` then returns an instance of `ILoggerFactory`

snippet: factory

Note that the `ConsoleLoggerFactory` can optionally expose extra configuration, as is illustrated in this case by a custom `LogLevel()` method.


## Log

The logger factory then returns an instance of a `ILog`.

snippet: log

The implementation of `ILog` handles writing the entries and the log filtering.


### Enabling Logging

snippet: ConfigureLogging


include: verifyLoggingSample