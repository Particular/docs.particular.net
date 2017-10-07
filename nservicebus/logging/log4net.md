---
title: Log4Net
summary: Logging to Log4Net
reviewed: 2017-10-07
component: Log4Net
related:
- samples/logging/log4net-custom
---

Support for writing all NServiceBus log entries to [Log4Net](http://logging.apache.org/log4net/).


## Usage

snippet: Log4netInCode


## Filtering

NServiceBus can write a significant amount of information to the log. To limit this information use the filtering features of the underlying logging framework.

For example to limit log output to a specific namespace.

Here is a code configuration example for adding a [Filter](http://logging.apache.org/log4net/release/manual/configuration.html#filters).


### The Filter

snippet: Log4netFilter


### Using the Filter

snippet: Log4netFilterUsage
