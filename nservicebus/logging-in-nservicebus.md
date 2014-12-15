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

## Filtering

NServiceBus writes a significant amount of information to the log. To limit this information you can use the filtering features of your underlying logging framework. 

For example to limit log output to a specific namespace

### In Log4net 

In log4net you can achieve this with a [Filter](http://logging.apache.org/log4net/release/manual/configuration.html#filters)

Here is a code configuration example for adding a Filter 

<!-- import Log4netFiltering -->

### In NLog

In Nlog you can achieve this with a [Rule](https://github.com/nlog/NLog/wiki/Configuration-file#rules)

Here is a code configuration example for adding a Rule

<!-- import NLogFiltering -->

## Logging message contents

When NServiceBus sends a message, it writes the result of the `ToString()` method of the message class to the log. By default, this writes the name of the message type only. To write the full message contents to the log, override the `ToString()` method of the relevant message class. Here's an example:

<!-- import MessageWithToStringLogged -->

## Logging Profiles

Logging can be configured via Profiles. However, unlike other profile behaviors, logging needs to be defined before you configure other components, even before the container. For that reason, logging configuration is kept separate from other profile behaviors.

NServiceBus has three built-in profiles for logging `Lite`, `Integration`, and `Production`. These profiles are only placeholders for logging customization. If no customization is done then the profiles have no impact on the logging defaults listed above.

### Customized logging via a profile

To specify logging for a given profile, write a class that implements `IConfigureLoggingForProfile<T>` where `T` is the profile type.

<!-- import LoggingConfigWithProfile -->

Here, the host passes you the instance of the class that implements `IConfigureThisEndpoint` so you don't need to implement `IWantTheEndpointConfig`.

WARNING: While you can have one class configure logging for multiple profile types, you can't have more than one class configure logging for the same profile. NServiceBus can allow only one of these classes for all profile types passed in the command-line.

See the [profiles for nservicebus host](profiles-for-nservicebus-host.md) for more information.
