---
title: Logging in NServiceBus 4 and below
summary: NServiceBus extends Log4Net APIs with a simple model to prevent admins from changing behavior you set at design time.
tags: 
- Logging
---


INFO: This is relevant to versions 4 and below. For newer versions, see [Logging in NServiceBus](logging-in-nservicebus.md).

Like many other open-source frameworks on the .NET platform, NServiceBus uses Log4Net for its logging capabilities. Familiar to developers and administrators alike, Log4Net has been proven in years of production use.

NServiceBus extends the Log4Net APIs with a simplified model that prevents administrators from accidentally changing behavior you set at design time.

## Logging basics

Start to log with NServiceBus:

    NServiceBus.Configure.With().Log4Net();

This makes use of `ConsoleAppender`, which sets the logging threshold to Debug. All logging statements performed by NServiceBus or the application at a level at or above Debug (i.e., Warn, Error, and Fatal) are sent to the console for output.

Calling Log4Net from your code is very straightforward. Often you'll set up a single static read-only reference to a logger in your classes, and then use it in all your methods, like this:

```
using log4net;
  
public class YourClass
{
    public void SomeMethod()
    {
        //your code
        Logger.Debug("Something interesting happened.");
    }
    static ILog Logger = LogManager.GetLogger("Name");
}
```

To make use of the standard Log4Net configuration found in the application configuration file, make the following call before the call to `NServiceBus.Configure.With()`:

    NServiceBus.SetLoggingLibrary.Log4Net(log4net.Config.XmlConfigurator.Configure);

This isn't supported in the Fluent initialization API because NServiceBus frowns on the Log4Net model of mixing developer settings
(such as the type of appender console, file, etc.) and administrator settings (such as the logging level) in the same place. In its place, NServiceBus suggests more operation-friendly approaches, as described lower down.

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


NOTE : NServiceBus only makes these calls at a log threshold of DEBUG or lower.

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

As you can see, there isn't much effort involved in plugging in your own logging technology. That being said, with the number of appenders available out of the box with Log4Net, you should be able to find something to suit your needs. Here's a taste:

-   ADO.NET
-   ASP.NET Trace
-   System.Diagnostics.Debug
-   System.Diagnostics.Trace
-   System Event Log
-   Rolling File
-   SMTP
-   UDP

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


## Logging Profiles

Logging is another kind of behavior that you can change from one profile to another. However, unlike other profile behaviors, logging levels and sinks need to be defined before you configure other components, even before the container. For that reason, logging configuration is kept separate from other profile behaviors.

The logging behavior configured for the three built-in profiles is shown:

| Profile     | Appender     | Threshold  
|-------------|--------------|-----
| Lite        | Console      | Info                         
| Integration | Console      | Info 
| Production  | Rolling File | Info 

When running under the production profile, the logs are written to 'logfile' in the same directory as the exe. The file grows to a maximum size of 1MB and then a new file is created. A maximum of 10 files is held and then the oldest file is erased. If no configuration exists, the logging threshold is Warn. Configure the logging threshold by including the following code in the application config file:

```XML
<configSections>
	<section name="Logging" type="NServiceBus.Config.Logging, NServiceBus.Core" />
</configSections>
<Logging Threshold="ERROR" />
```

 For changes to the configuration to have an effect, the process must be restarted.

If you want different logging behaviors than these, see the next section.

## Customized logging

To specify logging for a given profile, write a class that implements `IConfigureLoggingForProfile<T>` where `T` is the profile type. The implementation of this interface is similar to that described for `IWantCustomLogging` in the [host page](the-nservicebus-host.md).


```C#
class YourProfileLoggingHandler : IConfigureLoggingForProfile<YourProfile>
{
    public void Configure(IConfigureThisEndpoint specifier)
    {
        // setup your logging infrastructure then call
        NServiceBus.SetLoggingLibrary.Log4Net(null, yourLogger);
    }
}
```

 Here, the host passes you the instance of the class that implements `IConfigureThisEndpoint` so you don't need to implement `IWantTheEndpointConfig`.

**IMPORTANT** : While you can have one class configure logging for multiple profile types, you can't have more than one class configure logging for the same profile. NServiceBus can allow only one of these classes for all profile types passed in the command-line.

See the [logging documentation](profiles-for-nservicebus-host.md) for more information.
