---
title: Log4Net
summary: Logging using Log4Net.
reviewed: 2016-03-17
related:
- samples/logging/log4net-custom
---

Support for [Log4Net](http://logging.apache.org/log4net/) is available starting with NServiceBus Versions 3 and above.


### NServiceBus Versions 4 and below

NServiceBus Log4Net support was built in.


### NServiceBus Versions 5 and above

Log4Net was externalized to its own NuGet package, [NServiceBus.Log4Net](https://www.nuget.org/packages/NServiceBus.Log4Net/) to facilitate logging all NServiceBus entries in the endpoint using Log4Net.


## Usage

snippet:Log4netInCode


## Filtering

NServiceBus can write a significant amount of information to the log. To limit this information use the filtering features of the underlying logging framework.

For example to limit log output to a specific namespace.

Here is a code configuration example for adding a [Filter](http://logging.apache.org/log4net/release/manual/configuration.html#filters).


### The Filter

snippet:Log4netFilter


### Using the Filter

snippet:Log4netFilterUsage
