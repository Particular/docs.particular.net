---
title: Serilog
summary: Logging to Serilog
reviewed: 2018-10-29
component: Serilog
tags:
 - Logging
related:
 - nservicebus/logging
 - samples/logging/serilog-custom
 - samples/logging/serilog-seq
---

Support for writing all NServiceBus log entries to [Serilog](https://serilog.net/).


## Usage

snippet: SerilogInCode


## Seq

To log to [Seq](https://getseq.net/):

snippet: SerilogSeq


## Filtering

NServiceBus can write a significant amount of information to the log. To limit this information use the filtering features of the underlying logging framework.

For example to limit log output to a specific namespace.

Here is a code configuration example for adding a [Filter](https://github.com/serilog/serilog/wiki/Configuration-Basics#filters).

snippet: SerilogFiltering


## Tracing

Writing diagnostic log entries to [Serilog](https://serilog.net/). Plugs into the low level [pipeline](/nservicebus/pipeline) to give more detailed diagnostics.

When using Serilog for tracing it is optional to use Serilog as the main NServiceBus logger. i.e. no need to include `LogManager.Use<SerilogFactory>();`.


### Create an instance of a Serilog Logger

snippet: SerilogTracingLogger


### Configure the Tracing feature to use that logger

snippet: SerilogTracingPassLoggerToFeature


## Seq

To log to [Seq](https://getseq.net/):

snippet: SerilogTracingSeq

