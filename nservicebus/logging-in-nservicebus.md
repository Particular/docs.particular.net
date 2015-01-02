---
title: Logging in NServiceBus
summary: Logging in NServiceBus
tags: 
- Logging
---

INFO: This is relevant to versions 5 and above. For earlier versions, see [Logging in version 4 and below](logging-in-nservicebus4-and-below.md).

## Default Logging

NServiceBus has some limited, and opinionated, logging built in.

The default logging behavior is as follows:

### Console

All `Info` (and above) messages will be piped to the current console.

Errors will be written with `ConsoleColor.Red`. Warnings will be written with `ConsoleColor.DarkYellow`. All other message will be written with `ConsoleColor.White`. 

### Trace

All `Warn` (and above) messages will be written to `Trace.WriteLine`.
 
### Rolling File 

All `Info` (and above) messages will be written to a rolling log file.

This file will keep 10MB per file and a maximum of 10 log files.

The default logging directory will be `HttpContext.Current.Server.MapPath("~/App_Data/")` for websites and `AppDomain.CurrentDomain.BaseDirectory` for all other processes.

The default file name will be `nsb_log_yyyy-MM-dd_N.txt`, where `N` is a sequence number for when the log file reaches the max size.

## Logging Levels

The supported logging levels are

 * Debug
 * Info
 * Warn
 * Error
 * Fatal

## Changing the defaults

### Changing settings via configuration

The main parameter is the logging resolution of how much information is logged. Logging only errors is usually desirable in production scenarios as it gives the best performance. Yet when a system behaves erratically, having more information logged can give greater insight into what is causing the problem. This is controlled by the application configuration file by including the following entries:

```
<configSections>
<section name="Logging" type="NServiceBus.Config.Logging, NServiceBus.Core" />
</configSections>
<Logging Threshold="Debug" />
```

The `Threshold` value attribute of the `Logging` element can be any of `Debug`, `Info`, `Warn`, `Error` or `Fatal`.

The `Threshold` indicates the log levels that will be outputted. So for example a value of `Warn` would mean all `Warn`, `Error` and `Fatal` message would be outputted. 

For changes to the configuration to have an effect, the process must be restarted.

### Changing settings via code

With code you can configure both the Level and the logging directory. To do this, use the `LogManager` class.

<!-- import OverrideLoggingDefaultsInCode -->

Ensure you do this before any bus configuration is done.

## Custom Logging

For more advanced logging, it is recommended that you utilize one of the many mature logging libraries available for .Net. 

Note: Moving to custom logging means none of the approaches used in the above [Default Logging](#default-logging) apply. 

### When to configure logging

It is important to configure logging before any bus configuration is done since logging is configured in the static context of each NServiceBus class. So it should be configured at the startup of your app. For example

 * At the start of the `Main` of a console app or windows service.
 * At the start of the constructor of the class that implements `IConfigureThisEndpoint` when using [NServiceBus.Host](the-nservicebus-host.md).
 * At the start of your `Global.Application_Start` in a asp.net application.
 
### NLog

There is a [nuget](https://www.nuget.org/packages/NServiceBus.NLog/) package available that allows for simple integration of NServiceBus and [NLog](http://nlog-project.org/).

    Install-Package NServiceBus.NLog

Configure NLog using its standard API then call 

    LogManager.Use<NLogFactory>();

Example Usage 

<!-- import NLogInCode -->

### CommonLogging

There is a [nuget](https://www.nuget.org/packages/NServiceBus.CommonLogging/) package available that allows for simple integration of NServiceBus and [CommonLogging](http://netcommon.sourceforge.net/).

    Install-Package NServiceBus.CommonLogging

Configure NLog using its standard API then call 

    LogManager.Use<CommonLoggingFactory>();

Example Usage 

<!-- import CommonLoggingInCode -->

### Log4Net

There is a [nuget](https://www.nuget.org/packages/NServiceBus.Log4Net/) package available that allows for simple integration of NServiceBus and [Log4Net](http://logging.apache.org/log4net/).

    Install-Package NServiceBus.Log4Net

Configure Log4net using its standard API then call 

    LogManager.Use<Log4NetFactory>();

Example Usage 

<!-- import Log4netInCode -->
