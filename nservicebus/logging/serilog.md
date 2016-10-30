---
title: Serilog
summary: Logging to Serilog
reviewed: 2016-10-31
component: Serilog
tags:
 - Logging
related:
 - samples/logging/serilog-custom
---

Support for writing all NServiceBus log entries to [Serilog](https://serilog.net/).


## Usage

snippet:SerilogInCode


## Filtering

NServiceBus can write a significant amount of information to the log. To limit this information use the filtering features of the underlying logging framework.

For example to limit log output to a specific namespace.

Here is a code configuration example for adding a [Filter](https://github.com/serilog/serilog/wiki/Configuration-Basics#filters).

snippet:SerilogFiltering