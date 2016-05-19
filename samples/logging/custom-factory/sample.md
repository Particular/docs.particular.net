---
title: Custom Logger Factory
summary: Illustrates a custom implementation of a logging factory.
reviewed: 2016-05-20
component: Core
tags:
 - Logging
related:
 - nservicebus/logging
---


Illustrates a custom implementation of a logging factory. For simplicity this sample writes all log message to the console.


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

snippet:ConfigureLogging