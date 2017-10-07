---
title: Serilog
summary: Logging to Serilog
reviewed: 2017-10-07
component: Serilog
tags:
 - Logging
related:
 - nservicebus/logging
 - samples/logging/serilog-custom
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