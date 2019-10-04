---
title: NLog
summary: Logging to NLog
reviewed: 2019-07-29
component: NLog
tags:
- Logging
related:
- samples/logging/nlog-custom
---

Support for writing all NServiceBus log entries to [NLog](https://nlog-project.org/).


## Usage

snippet: NLogInCode


## Filtering

NServiceBus can write a significant amount of information to the log. To limit this information use the filtering features of the underlying logging framework.

For example to limit log output to a specific namespace.

Here is a code configuration example for adding a [Rule](https://github.com/nlog/NLog/wiki/Configuration-file#rules).

snippet: NLogFiltering

## Additional exception data

Starting from NServiceBus version 7.2, exceptions from failing message handlers might contain additional error information in the `Exception.Data` property. Use the `Data` format property when configuring the exception layout, e.g. `${exception:format=toString,Data}`. For more information, see the [Exception layout renderer documentation](https://github.com/NLog/NLog/wiki/Exception-layout-renderer).
