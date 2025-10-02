---
title: NLog
summary: Logging to NLog
reviewed: 2024-08-20
component: NLog
---

partial: obsolete-warning

partial: usage


## Filtering

NServiceBus can write a significant amount of information to the log. To limit this information use the filtering features of the underlying logging framework.

For example to limit log output to a specific namespace.

Here is a code configuration example for adding a [Rule](https://github.com/nlog/NLog/wiki/Configuration-file#rules).

snippet: NLogFiltering

partial: nlog-exception-data
