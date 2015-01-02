---
title: Logging in NServiceBus 4 and below
summary: NServiceBus extends Log4Net APIs with a simple model to prevent admins from changing behavior you set at design time.
tags: 
- Logging
---

INFO: This is relevant to versions 4 and below. For newer versions, see [Logging in NServiceBus](logging-in-nservicebus.md).

NServiceBus extends the Log4Net APIs with a simplified model that prevents administrators from accidentally changing behavior you set at design time.

## Logging basics

Start to log with NServiceBus:

    NServiceBus.Configure.With().Log4Net();

This makes use of `ConsoleAppender`, which sets the logging threshold to Debug. All logging statements performed by NServiceBus or the application at a level at or above Debug (i.e., Warn, Error, and Fatal) are sent to the console for output.

To make use of the standard Log4Net configuration found in the application configuration file, make the following call before the call to `NServiceBus.Configure.With()`:

    NServiceBus.SetLoggingLibrary.Log4Net(log4net.Config.XmlConfigurator.Configure);

Include a Log4Net configuration section in the application configuration file that results in the Debug threshold with the `ConsoleAppender`, as shown:

```
<log4net debug="false">
	<appender name="console" type="log4net.Appender.ConsoleAppender">
		<layout type="log4net.Layout.PatternLayout">
		<param name="ConversionPattern" value="%d [%t] %-5p %c [%x] <%X{auth}> - %m%n"/>
		</layout>
	</appender>
	<root>
		<level value="DEBUG"/>
		<appender-ref ref="console"/>
	</root>
</log4net>
```

For more information about standard Log4Net functionality, see the
[Log4Net home page](http://logging.apache.org/log4net/index.html).

## Customized logging

You can tell NServiceBus to use any of the built-in Log4Net appenders, specifying additional properties of the chosen appender using the following API:

    NServiceBus.Configure.With().Log4Net(a => a.From = "no-reply@YourApp.YourCompany.com");

This example shows all logging calls sent using SMTP from the email address `no-reply@YourApp.YourCompany.com`.

If there isn't a built-in appender for the technology you want to use for logging, write a class that inherits from `AppenderSkeleton`, as follows:

    public class YourAppender : log4net.Appender.AppenderSkeleton
    {
	    public string YourProperty { get; set; }
	    protected override void Append(LoggingEvent loggingEvent)
	    {
	  	  //call your logging technology here
	    }
    }

Then plug your appender into NServiceBus like this:

    NServiceBus.Configure.With().Log4Net(a => a.YourProperty = "value");

## Administrative configuration

As you saw before, most of the logging configuration done with NServiceBus is in code. This prevents administrators from accidentally changing values set by developers. It also provides developers with compile-time checking, intellisense, and the other productivity-enhancing capabilities of Visual Studio.

Yet certain parameters need to be under administrative control. The main parameter is the logging resolution of how much information is logged. Logging only errors is usually desirable in production scenarios as it gives the best performance. Yet, when a system behaves erratically, having more information logged can give greater insight into what is causing the problems. This is controlled by the application configuration file by including the following entries:

```
<configSections>
	<section name="Logging" type="NServiceBus.Config.Logging, NServiceBus.Core" />
</configSections>
<Logging Threshold="WARN" />
```

The 'Threshold' value attribute of the 'Logging' element can be any of the standard Log4Net entries: ALERT, ALL, CRITICAL, DEBUG, EMERGENCY, ERROR, FATAL, FINE, FINER, FINEST, INFO, NOTICE, OFF, SEVERE, TRACE, VERBOSE, and WARN. Make sure you use all caps for these entries.

NOTE: If you set this value in code, the configuration value is ignored.

The production profile only logs to a file, unless you are running within Visual Studio. See
[Profiles](profiles-for-nservicebus-host.md) for more detail.