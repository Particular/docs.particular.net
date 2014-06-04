---
title: Logging in NServiceBus
summary: Logging in NServiceBus
tags: 
- Logging
---

**NOTE: this is relevant to versions 5 and above. For earlier versions see [Logging in version 4 and below](logging-in-nservicebus4_and_below.md)  **

## Default Logging

NServiceBus has some limited, and opinionated, logging built in.

The default logging behavior is as follows.

### Console

All `Info` (and above) messages will be piped to the current console.

### Trace

All `Warn` (and above) messages to a written to `Trace.WriteLine`.
 
### Rolling File 

All `Info` (and above) messages will be written to a rolling log file.

This file will keep 10MB per file and a maximum of 10 log files.

The default logging directory will be `HttpContext.Current.Server.MapPath("~/App_Data/")` for websites and `AppDomain.CurrentDomain.BaseDirectory` for all other processes.

The default file name will be `nsb_log_yyyy-MM-dd_N.txt` where `N` is a sequence number for when the log file reaches the max size.

## Changing the defaults

### Changing settings via configuration

The main parameter is the logging resolution of how much information is logged. Logging only errors is usually desirable in production scenarios as it gives the best performance. Yet, when a system behaves erratically, having more information logged can give greater insight into what is causing the problems. This is controlled by the application configuration file by including the following entries:

```
<configSections>
	<section name="Logging" type="NServiceBus.Config.Logging, NServiceBus.Core" />
</configSections>
<Logging Threshold="Debug" />
```

The `Threshold` value attribute of the `Logging` element can be any of `Debug`, `Info`, `Warn`, `Error` or `Fatal`.

For changes to the configuration to have an effect, the process must be restarted.

### Changing settings via code

With code you can configure both the Level and the logging directory. To do this use the `LogManager` class.

```
LogManager.ConfigureDefaults(LogLevel.Debug, pathToLoggingDirectory);
```

Ensure you do this before `Configure.With` is called.

## Custom Logging

For more advanced logging it is recommended that you utilize one of the many mature logging libraries available for .net. 

### NLog

There is a [nuget](https://www.nuget.org/packages/NServiceBus.NLog/) available that allows for simple integration of NServiceBus and [NLog](http://nlog-project.org/).

    Install-Package NServiceBus.NLog

Then call 

    NLogConfigurator.Configure();

### Log4Net

There is a [nuget](https://www.nuget.org/packages/NServiceBus.Log4Net/) available that allows for simple integration of NServiceBus and [Log4Net](http://logging.apache.org/log4net/).

    Install-Package NServiceBus.Log4Net

Then call 

    Log4NetConfigurator.Configure();

## Logging message contents

When NServiceBus sends a message, it writes the result of the `ToString()` method of the message class to the log. By default, this writes the name of the message type only. To write the full message contents to the log, override the `ToString()` method of the relevant message class. Here's an example:

    public class MyMessage : IMessage
    {
	    public Guid EventId { get; set; }
	    public DateTime? Time { get; set; }
	    public TimeSpan Duration { get; set; }

	    public override string ToString()
	    {
		    return string.Format(
		    "MyMessage: EventId={0}, Time={1}, Duration={2}",
		    EventId, Time, Duration
		    );
	    }
    }

## Logging Profiles

Logging can be configured via Profiles. However, unlike other profile behaviors, logging needs to be defined before you configure other components, even before the container. For that reason, logging configuration is kept separate from other profile behaviors.

NServiceBus has three built-in profiles for logging `Lite`, `Integration` and `Production`. These profiles a only placeholders for logging customisation. If no customisation is done then the profiles have no impact on the logging defaults listed above.

### Customized logging via a profile

To specify logging for a given profile, write a class that implements `IConfigureLoggingForProfile<T>` where `T` is the profile type. The implementation of this interface is similar to that described for `IWantCustomLogging` in the [host page](the-nservicebus-host.md).

```C#
class YourProfileLoggingHandler : IConfigureLoggingForProfile<YourProfile>
{
    public void Configure(IConfigureThisEndpoint specifier)
    {
        // setup your logging infrastructure then call
		Log4NetConfigurator.Configure();
    }
}
```

Here, the host passes you the instance of the class that implements `IConfigureThisEndpoint` so you don't need to implement `IWantTheEndpointConfig`.

**IMPORTANT** : While you can have one class configure logging for multiple profile types, you can't have more than one class configure logging for the same profile. NServiceBus can allow only one of these classes for all profile types passed in the command-line.

See the [profiles for nservicebus host](profiles-for-nservicebus-host.md) for more information.
