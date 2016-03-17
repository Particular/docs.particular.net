---
title: Routing to NLog
summary: Route all NServiceBus log entries to NLog.
reviewed: 2016-03-17
tags:
- Logging
- nlog
related:
- samples/logging/nlog-custom
---

Support for routing log entries to [NLog](http://nlog-project.org/) is compatible with NServiceBus 5 and above.

There is a [nuget](https://www.nuget.org/packages/NServiceBus.NLog/) package available that allows for simple integration of NServiceBus and NLog.


## Usage

snippet:NLogInCode


## Filtering

If NServiceBus writes a significant amount of information to the log. To limit this information use the filtering features of the underlying logging framework.

For example to limit log output to a specific namespace.

Here is a code configuration example for adding a [Rule](https://github.com/nlog/NLog/wiki/Configuration-file#rules).

snippet:NLogFiltering