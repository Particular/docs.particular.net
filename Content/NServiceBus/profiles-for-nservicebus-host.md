<!--
title: "Profiles For NServiceBus Host"
tags: ""
summary: ""
-->

Moving a system
<span style="font-size: 14px; line-height: 24px;">reliably </span>
<span style="font-size: 14px; line-height: 24px;">from development through test to production is a core capability for any development organization.</span>

Manual configuration and code changes make this process error-prone and make versioning the system a nightmare.

The NServiceBus Host provides facilities of profiles designed specifically to ease this process and provide structure when versioning the configuration of a system.
<span style="font-size: 14px; line-height: 24px;">Read about the </span>
[host](the-nservicebus-host.md)
<span style="font-size: 14px; line-height: 24px;">.</span>

Configuration difficulties
--------------------------

Starting out with NServiceBus development isn't always easy. There are many configuration options for levels of logging, technologies for storing subscribers, and types of saga storage (to name a few). Often, you want an appropriate combination of all these options as long as you can change it later. Profiles give you that flexibility.

NServiceBus comes with three profiles out of the box: Lite, Integration, and Production. Each profile configures a cohesive combination of technologies:

-   Lite keeps everything in memory with the most detailed logging.
-   Integration uses technologies closer to production but without
    scale-out and less logging.
-   Production uses scale-out friendly technologies and minimal
    file-based logging.

Specifying which profiles to run
--------------------------------

To tell the host to run using a specific profile, you need to pass the namespace-qualified type of the profile class to the NServiceBus host as a command-line parameter. Specify the Lite profile, as follows:


```C#
NServiceBus.Host.exe NServiceBus.Lite
```

 You may be concerned about the use of command-line parameters. Be aware that when installing the NServiceBus host as a Windows Service, all provide profiles are baked into the installation. Second, having the ability to sit down at a tester workstation and turn on and off various behaviors without touching configuration files or code makes isolating bugs much easier. It may take some time to get used to, but the benefits are worth it.

If you just run the host without specifying a profile, NServiceBus defaults to the Production profile. You can pass in as many profiles as you want and NServiceBus runs them all.

Writing your own profile
------------------------

Writing a profile is as simple as defining a class that implements the NServiceBus.IProfile marker interface. Here's an example:


```C#
namespace YourNamespace
{
    public class YourProfile : NServiceBus.IProfile { }
}
```

 To tell the host to run your profile and the NServiceBus Lite profile together:


```C#
NServiceBus.Host.exe YourNamespace.YourProfile NServiceBus.Lite
```

 As you can see, the profile itself does not contain any behavior itself. It is just a placeholder around which different kinds of behavior can be hooked. See how those behaviors are connected to their profiles.

Profile behaviors
-----------------

To provide behavior around a profile, implement the NServiceBus.IHandleProfile<t> interface where T is the given profile.
<span style="font-size: 14px; line-height: 24px;">For example, an email component</span>

-   <span style="font-size: 14px; line-height: 24px;">Does nothing with
    the Lite profile</span>
-   <span style="font-size: 14px; line-height: 24px;">Writes emails to
    disk with the Integration profile</span>
-   <span style="font-size: 14px; line-height: 24px;">Uses an SMTP
    server with the Production profile</span>

<span style="font-size: 14px; line-height: 24px;">Set it up as follows:</span>


```C#
    internal class LiteEmailBehavior : IHandleProfile<NServiceBus.Lite>
    {
        public void ProfileActivated()
        {
            // set the NullEmailSender in the container
        }
    }

    internal class IntegrationEmailBehavior : IHandleProfile<NServiceBus.Integration>
    {
        public void ProfileActivated()
        {
            // set the FileEmailSender in the container
        }
    }

    internal class ProductionEmailBehavior : IHandleProfile<NServiceBus.Production>
    {
        public void ProfileActivated()
        {
            // set the SmtpEmailSender in the container
        }
    }
```

 With these classes, switching profiles doesn't only change NServiceBus behaviors but also your own applicative behaviors as a consistent set. There is no worry about keeping different parts of a configuration file in sync or changing the configuration file your application uses.
<span style="font-size: 14px; line-height: 24px;">You can also have multiple classes provide behaviors for the same profile, or you can have a single class handle multiple profiles (by implementing IHandleProfile<t> for each profile type) if you want identical behavior across profiles.</span>

Dependent profile behaviors
---------------------------

You may want slight variations of behavior based on the properties of the class that implements IConfigureThisEndpoint. Also, you don't necessarily want all profile handlers to be dependent on the type that implements IConfigureThisEndpoint, just for it to check whether it also implements some other interface. The host itself does this when it handles publishers. Endpoints that don't publish don't need to have a subscription storage. Those that are publishers do need different storage technologies configured, based on profile. Just as the host defines the AsAPublisher interface and customizes behavior around it, you can do the same with your own interfaces.

For a profile handler to access the type that implements IConfigureThisEndpoint, it has to implement IWantTheEndpointConfig, like this:


```C#
    class MyProfileHandler : IHandleProfile<MyProfile>, IWantTheEndpointConfig
    {
        public void ProfileActivated()
        {
            if (Config is AnInterfaceICareAbout)
            {
                // set something in the container
            }
            else
            {
                // set something else in the container
            }
        }

        public IConfigureThisEndpoint Config { get; set; }
    }
```

 This lets you extend the host and write additional profiles and behaviors to customize various aspects of your system, all while maintaining loose-coupling and composability between the various parts of your system.

Logging behaviors
-----------------

Logging is another kind of behavior that you can change from one profile to another. However, unlike other profile behaviors, logging levels and sinks need to be defined before you configure other components, even before the container. For that reason, logging configuration is kept separate from other profile behaviors.

The logging behavior configured for the three built-in profiles is shown:


  ------------- -------------- -------------------------------
  Profile       Appender       Threshold
  Lite          Console        Debug
  Integration   Console        Info
  Production    Rolling File   Configurable, Warn by default
  ------------- -------------- -------------------------------

<span style="font-size: 14px; line-height: 24px;">When running under the production profile, the logs are written to 'logfile' in the same directory as the exe. The file grows to a maximum size of 1MB and then a new file is created. A maximum of 10 files is held and then the oldest file is erased. If no configuration exists, the logging threshold is Warn. Configure the logging threshold by including the following code in the application config file:</span>


```XML
  <configSections>
    <section name="Logging" type="NServiceBus.Config.Logging, NServiceBus.Core" />
  </configSections>
  <Logging Threshold="ERROR" />
```

 For changes to the configuration to have an effect, the process must be restarted.

If you want different logging behaviors than these, see the next section.

Customized logging
==================

To specify logging for a given profile, write a class that implements IConfigureLoggingForProfile<t> where T is the profile type. The implementation of this interface is similar to that described for IWantCustomLogging in the [host page](the-nservicebus-host.md) .


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

 Here, the host passes you the instance of the class that implements IConfigureThisEndpoint so you don't need to implement IWantTheEndpointConfig.

**IMPORTANT** : While you can have one class configure logging for multiple profile types, you can't have more than one class configure logging for the same profile. NServiceBus can allow only one of these classes for all profile types passed in the command-line.

See the [logging documentation](logging-in-nservicebus.md) for more information.

Behavior requiring initialization to be complete
------------------------------------------------

In your profile handlers, you may occasionally want to make use of the container to build an object for you. The only issue is that the necessary type may not yet have been registered, so you want to wait until initialization is complete. You can tell NServiceBus to call you at that stage of the configuration process by i
<span style="font-size: 14px; line-height: 24px;">mplementing the IWantToRunWhenConfigurationIsComplete interface:</span>


```C#
class MyProfileHandler : IHandleProfile<MyProfile>, IWantToRunWhenConfigurationIsComplete
    {
        public void ProfileActivated()
        {
            _wasActivated = true;
            // configure the object
        }

        public void Run()
        {
            if (!_wasActivated)
                return;
            var obj = Builder.Build<SomeType>();
            // do something with the object
        }
        
        static bool _wasActivated;
        
        public IBuilder Builder { get; set; }
    }
```

 This approach is particularly useful when you want your profile to hook into an event on the object you used the container to build, allowing your profile to perform activities at specific points in the lifecycle of the application not just at startup.




