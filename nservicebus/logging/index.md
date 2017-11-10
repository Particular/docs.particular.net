---
title: Logging
summary: Manage and integrate with NServiceBus logging.
reviewed: 2016-08-10
component: Core
tags:
- Logging
redirects:
- nservicebus/logging-in-nservicebus
related:
- samples/logging
---


## Default Logging

NServiceBus has some limited, and opinionated, logging built in.

The default logging behavior is as follows:

partial: defaults


## Logging Levels

The Logging level, or "Threshold", indicates the log levels that will be outputted. So for example a value of `Warn` would mean all `Warn`, `Error` and `Fatal` message would be outputted.

partial: level


## Custom Logging

For more advanced logging, it is recommended to utilize one of the many mature logging libraries available for .NET.

 * [Log4Net integration](log4net.md)
 * [NLog integration](nlog.md)
 * [CommonLogging integration](common-logging.md)
 * [Serilog integration](serilog.md)
 * [EventSourceLogging integration](eventsourcelogging.md)

Note: Moving to custom logging means none of the approaches used in the above [Default Logging](#default-logging) apply.


## When to configure logging

It is important to configure logging before any endpoint configuration is done since logging is configured in the static context of each NServiceBus class. It should be configured as early as possible at the startup of the app. For example

 * At the start of the `Main` of a console app or windows service.
 * At the start of the `Global.Application_Start` in a asp.net application.
 * [Using endpoint configuration API in an application hosted via NServiceBus Host](/nservicebus/hosting/nservicebus-host/logging-configuration.md)
