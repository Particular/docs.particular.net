---
title: Routing to Log4Net
summary: Route all NServiceBus log entries to Log4Net
tags:
- log4net
related:
- samples/logging/log4net-custom
---

Support for routing log entries to [Log4Net](http://logging.apache.org/log4net/) is compatible with NServiceBus Version 3 and above.


### NServiceBus Version 4 and below

NServiceBus Log4Net support was built in.


### NServiceBus Version 5 and above

Log4Net was externalized to its own [nuget](https://www.nuget.org/packages/NServiceBus.Log4Net/) package available that allows for simple integration of NServiceBus.


## Usage

snippet:Log4netInCode


## Filtering

If NServiceBus writes a significant amount of information to the log. To limit this information you can use the filtering features of your underlying logging framework.

For example to limit log output to a specific namespace.

Here is a code configuration example for adding a [Filter](http://logging.apache.org/log4net/release/manual/configuration.html#filters).


### The Filter

snippet:Log4netFilter


### Using the Filter

snippet:Log4netFilterUsage
