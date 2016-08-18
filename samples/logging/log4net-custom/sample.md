---
title: Custom Log4Net appender
summary: Customizing Log4Net by passing in a custom Appender.
reviewed: 2016-03-21
component: Log4Net
tags:
- Logging
related:
- nservicebus/logging
---

## Code walk-through

Illustrates customizing Log4Net by passing in a custom Appender.

### Configure Log4Net

snippet:ConfigureLog4Net

### Pass that configuration to NServiceBus

snippet:UseConfig


## Verifying that the sample works correctly

In this sample the information at the Info level is logged to the console window.

There should be a few standard logging entries in the console window that are automatically created by NServiceBus when logging level is set to Info, for example `Queue [private$\error] is running with [Everyone] and [NT AUTHORITY\ANONYMOUS LOGON] permissions. Consider setting appropriate permissions, if required by the organization. For more information, consult the documentation.`

There should also be a custom entry logged from inside the handler `Hello from MyHandler.`.