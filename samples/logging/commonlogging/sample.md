---
title: CommonLogging Usage
summary: Using CommonLogging logging capability of NServiceBus.
reviewed: 2016-03-21
component: CommonLogging
tags:
- Logging
related:
- nservicebus/logging
---


## Code walk-through

This sample shows a usage of [CommonLogging](http://netcommon.sourceforge.net/) within NServiceBus.


### Enabling Logging

snippet:ConfigureLogging


## Verifying that the sample works correctly

In this sample the information at the `Info` level is logged to the console window. 

There should be a few standard logging entries in the console window that are automatically created by NServiceBus when logging level is set to `Info`, for example `[INFO] NServiceBus.Features.QueuePermissions - Queue [private$\error] is running with [Everyone] and [NT AUTHORITY\ANONYMOUS LOGON] permissions. Consider setting appropriate permissions, if required by the organization. For more information, consult the documentation.`. 

There should also be a custom entry logged from inside the handler `[INFO]  MyHandler - Hello from MyHandler`.