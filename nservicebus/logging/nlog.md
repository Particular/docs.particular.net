---
title: NLog
summary: Logging using NLog.
reviewed: 2016-03-17
tags:
- Logging
- nlog
related:
- samples/logging/nlog-custom
---

Support for [NLog](http://nlog-project.org/) is available starting with NServiceBus Versions 5 and above.

The [NServiceBus.NLog NuGet package](https://www.nuget.org/packages/NServiceBus.NLog/) facilitates logging of all NServiceBus entries in the endpoint using NLog.


## Usage

snippet:NLogInCode


## Filtering

NServiceBus can write a significant amount of information to the log. To limit this information use the filtering features of the underlying logging framework.

For example to limit log output to a specific namespace.

Here is a code configuration example for adding a [Rule](https://github.com/nlog/NLog/wiki/Configuration-file#rules).

snippet:NLogFiltering