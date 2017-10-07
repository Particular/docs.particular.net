---
title: Serilog Tracing
summary: Writing diagnostic log entries via Serilog Tracing
reviewed: 2017-10-07
component: SerilogTracing
tags:
 - Logging
related:
 - nservicebus/logging
 - samples/logging/serilog-seq
 - nservicebus/logging/serilog
---

Writing diagnostic log entries to [Serilog](https://serilog.net/). Plugs into the low level [pipeline](/nservicebus/pipeline) to give more detailed diagnostics.


## Usage

It is optional to use Serilog as the main NServiceBus logger. i.e. no need to include `LogManager.Use<SerilogFactory>();`.


#### Create an instance of a Serilog Logger

snippet: SerilogTracingLogger


### Configure the Tracing feature to use that logger

snippet: SerilogTracingPassLoggerToFeature


## Seq

To log to [Seq](https://getseq.net/):

snippet: SerilogTracingSeq
