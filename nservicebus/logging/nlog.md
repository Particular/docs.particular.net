---
title: NLog
summary: Logging to NLog
reviewed: 2019-07-29
component: NLog
related:
- samples/logging/nlog-custom
---

Support for writing all NServiceBus log entries to [NLog](https://nlog-project.org/).


partial: usage


## Filtering

NServiceBus can write a significant amount of information to the log. To limit this information use the filtering features of the underlying logging framework.

For example to limit log output to a specific namespace.

Here is a code configuration example for adding a [Rule](https://github.com/nlog/NLog/wiki/Configuration-file#rules).

snippet: NLogFiltering

partial: nlog-exception-data