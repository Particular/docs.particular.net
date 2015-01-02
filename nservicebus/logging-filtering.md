---
title: Filtering logging entries
summary: How to selectively filter log entries
tags: 
- Logging
---
 
## Filtering

NServiceBus writes a significant amount of information to the log. To limit this information you can use the filtering features of your underlying logging framework. 

For example to limit log output to a specific namespace

### In Log4net 

In log4net you can achieve this with a [Filter](http://logging.apache.org/log4net/release/manual/configuration.html#filters)

Here is a code configuration example for adding a Filter 

<!-- import Log4netFiltering -->

### In NLog

In NLog you can achieve this with a [Rule](https://github.com/nlog/NLog/wiki/Configuration-file#rules)

Here is a code configuration example for adding a Rule

<!-- import NLogFiltering -->
 