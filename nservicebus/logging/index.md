---
title: Logging in NServiceBus
summary: Manage and integrate with NServiceBus logging.
reviewed: 2016-03-17
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


### Version 3 and 4 Defaults

In these versions default logging was only enabled in the NServiceBus Host. For self hosting [enable the logging using code](log4net.md).

See [NServiceBus Host Profiles](/nservicebus/hosting/nservicebus-host/profiles.md) for the default logging in the NServiceBus Host.


### Version 5 and above Defaults

This is applicable to both self hosting and using the NServiceBus Host


#### Console

All `Info` (and above) messages will be piped to the current console.

Errors will be written with `ConsoleColor.Red`. Warnings will be written with `ConsoleColor.DarkYellow`. All other message will be written with `ConsoleColor.White`.


#### Trace

All `Warn` (and above) messages will be written to `Trace.WriteLine`.


#### Rolling File

All `Info` (and above) messages will be written to a rolling log file.

This file will keep 10MB per file and a maximum of 10 log files.

The default logging directory will be `HttpContext.Current.Server.MapPath("~/App_Data/")` for websites and `AppDomain.CurrentDomain.BaseDirectory` for all other processes.

The default file name will be `nsb_log_yyyy-MM-dd_N.txt`, where `N` is a sequence number for when the log file reaches the max size.


## Logging Levels

The Logging level, or "Threshold", indicates the log levels that will be outputted. So for example a value of `Warn` would mean all `Warn`, `Error` and `Fatal` message would be outputted.

In NServiceBus Version 5 and above the supported logging levels are

 * Debug
 * Info
 * Warn
 * Error
 * Fatal

In NServiceBus Versions 3 and 4 the supported Levels are defined by the [Log4Net.Level](https://logging.apache.org/log4net/release/sdk/index.html) class.


### Changing the Logging Level

{{Note: When logging level is defined in both app.config and code:

 * In Version 4 and below the app.config wins.
 * In Version 5 and above the code wins.

}}


#### Via app.config

snippet:OverrideLoggingDefaultsInAppConfig


#### Via IProvideConfiguration

snippet:LoggingThresholdFromIProvideConfiguration


#### Via config API

snippet:OverrideLoggingLevelInCode

The above API is only available in Versions 5 and above. In earlier versions the same can be achieved by taking full control over the [Log4Net integration](log4net.md).


## Changing the defaults


### Changing settings via code

With code both the Level and the logging directory can be configured. To do this, use the `LogManager` class.

snippet:OverrideLoggingDefaultsInCode

Do this before any bus configuration is done.

In earlier versions the same can be achieved by taking full control over the [Log4Net integration](log4net.md).


## Custom Logging

For more advanced logging, it is recommended to utilize one of the many mature logging libraries available for .Net.

 * [Log4Net integration](log4net.md)
 * [NLog integration](nlog.md)
 * [CommonLogging integration](common-logging.md)

Note: Moving to custom logging means none of the approaches used in the above [Default Logging](#default-logging) apply.


### When to configure logging

It is important to configure logging before any bus configuration is done since logging is configured in the static context of each NServiceBus class. So it should be configured at the startup of the app. For example

 * At the start of the `Main` of a console app or windows service.
 * At the start of the constructor of the class that implements `IConfigureThisEndpoint` when using [NServiceBus.Host](/nservicebus/hosting/nservicebus-host/).
 * At the start of the `Global.Application_Start` in a asp.net application.
