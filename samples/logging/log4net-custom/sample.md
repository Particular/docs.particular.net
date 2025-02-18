---
title: Custom Log4Net appender
summary: Customizing Log4Net by passing in a custom Appender.
reviewed: 2024-08-22
component: Log4Net
related:
- nservicebus/logging
---

> [!WARNING]
> This sample is obsolete and incompatible with NServiceBus 8+. Migration to a modern logging framework is recommended. If Log4Net must be used with NServiceBus, migrate to the Microsoft hosting and logging extensions and use the [Microsoft.Extensions.Logging.Log4Net.AspNetCore](https://github.com/huorswords/Microsoft.Extensions.Logging.Log4Net.AspNetCore) package.

## Introduction

Illustrates customizing [Log4Net](https://logging.apache.org/log4net/) by passing in a custom [Appender](https://logging.apache.org/log4net/release/config-examples.html).


## Configure Log4Net

snippet: ConfigureLog4Net


### Pass that configuration to NServiceBus

snippet: UseConfig



include: verifyLoggingSample
