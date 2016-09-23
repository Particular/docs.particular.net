---
title: Profiles For NServiceBus Host
summary: 'Profiles ease the configuration process. Three profiles straight from the box: Lite, Integration, and Production.'
tags:
- Profiles
- Hosting
- NServiceBus.Host
- Logging
- Persistence
redirects:
 - nservicebus/profiles-for-nservicebus-host
---

Moving a system reliably from development through test to production is a core capability for any development organization.

Manual configuration and code changes make this process error-prone and make versioning the system a nightmare.

The NServiceBus Host provides facilities of profiles designed specifically to ease this process and provide structure when versioning the configuration of a system. Read about the [host](/nservicebus/hosting/nservicebus-host/).


## Configuration difficulties

Starting out with NServiceBus development isn't always easy. There are many configuration options for levels of logging, technologies for storing subscribers, and types of saga storage (to name a few). Often, an appropriate combination of all these options is required, as long as they can be changed later. Profiles give that flexibility.

NServiceBus comes with three profiles out of the box: Lite, Integration, and Production. Each profile configures a cohesive combination of technologies:

 * Lite keeps everything in memory with the most detailed logging.
 * Integration uses technologies closer to production but without scale out and less logging.
 * Production uses scale out friendly technologies and minimal file-based logging.


## Specifying which profiles to run

To tell the host to run using a specific profile, pass the namespace-qualified type of the profile class to the NServiceBus host as a command-line parameter. Specify the Lite profile, as follows:

```dos
NServiceBus.Host.exe NServiceBus.Lite
```

Be aware that when installing the NServiceBus host as a Windows Service, all provide profiles are baked into the installation. Second, having the ability to sit down at a tester workstation and turn on and off various behaviors without touching configuration files or code makes isolating bugs much easier. It may take some time to get used to, but the benefits are worth it.

If the host is run without specifying a profile, NServiceBus defaults to the Production profile. Pass in as many other profiles as required and NServiceBus runs them all.


## Writing a custom profile

Writing a profile is as simple as defining a class that implements the `NServiceBus.IProfile` marker interface. Here's an example:

snippet:defining_profile

To tell the host to run the profile and the NServiceBus Lite profile together:

```dos
NServiceBus.Host.exe YourNamespace.YourProfile NServiceBus.Lite
```

Note the profile itself does not contain any behavior itself. It is just a place-holder around which different kinds of behavior can be hooked. See how those behaviors are connected to their profiles.


## Profile behaviors

To provide behavior around a profile, implement the `NServiceBus.Hosting.Profiles.IHandleProfile<T>` interface where `T` is the given profile.
For example, an email component

 * Does nothing with the Lite profile
 * Writes emails to disk with the Integration profile
 * Uses an SMTP server with the Production profile

Set it up as follows:

snippet:profile_behavior

With these classes, switching profiles doesn't only change NServiceBus behaviors but also custom behaviors as a consistent set. There is no worry about keeping different parts of a configuration file in sync or changing the configuration file the application uses. It is possible have multiple classes provide behaviors for the same profile, or have a single class handle multiple profiles (by implementing `IHandleProfile<T>` for each profile type) if identical behavior across profiles is required.


## Dependent profile behaviors

It is possible to have slight variations of behavior based on the properties of the class that implements `IConfigureThisEndpoint`. Also, it is not necessarily desired to have all profile handlers to be dependent on the type that implements `IConfigureThisEndpoint`, just for it to check whether it also implements some other interface. The host itself does this when it handles publishers. Endpoints that don't publish don't need to have a subscription storage. Those that are publishers do need different storage technologies configured, based on profile. Just as the host defines the `AsAPublisher` interface and customizes behavior around it, it is possible to do the same with the own interfaces.

For a profile handler to access the type that implements `IConfigureThisEndpoint`, it has to implement `IWantTheEndpointConfig`, like this:

snippet:dependent_profile

This allows extending the host and write additional profiles and behaviors to customize various aspects of the system, all while maintaining loose-coupling and composability between the various parts of the system.


## Logging behaviors

Logging is another kind of behavior that can be changed from one profile to another. However, unlike other profile behaviors, logging levels and sinks need to be defined before configuring other components, even before the container. For that reason, logging configuration is kept separate from other profile behaviors.

The logging behavior configured for the three built-in profiles is shown:

| Profile     | Appender     | Threshold
|-------------|--------------|-----
| Lite        | Console      | Debug
| Integration | Console      | Info
| Production  | Rolling File | Configurable, Warn by default

When running the production profile, the logs are written to 'logfile' in the same directory as the exe. The file grows to a maximum size of 1MB and then a new file is created. A maximum of 10 files is held and then the oldest file is erased. If no configuration exists, the logging threshold is Warn. To configure the logging threshold see [changing logging level via config file](/nservicebus/logging/#logging-levels-changing-the-logging-level-via-app-config).

For changes to the configuration to have an effect, the process must be restarted.

For different logging behaviors than these, see the next section.


## Customized logging

To specify logging for a given profile, write a class that implements `IConfigureLoggingForProfile<T>` where `T` is the profile type. The implementation of this interface is similar to that described for `IWantCustomLogging` in the [host page](/nservicebus/hosting/nservicebus-host/).

snippet:configure_logging

Here, the host passes the instance of the class that implements `IConfigureThisEndpoint` so it is not necessary to implement `IWantTheEndpointConfig`.

NOTE: While it is possible have one class configure logging for multiple profile types, it is not possible to have more than one class configure logging for the same profile. NServiceBus can allow only one of these classes for all profile types passed in the command-line.

See the [logging documentation](/nservicebus/logging/) for more information.


## Persistence

When using the NServiceBus.Host.exe out of the box, it is possible to utilize one of the available profiles. The following table shows which persistence technology each pre-built profile configures by default. In addition, it is possible override the configured defaults.

The following table summarizes the different persistence technologies being used by the built-in profiles.

NOTE: Before configuring persistence technology, to avoid overriding custom configurations, the profiles check if other types of storage are used.

|-                                |In-Memory|RavenDB			   |NHibernate   |MSMQ                         |
|:--------------------------------|:--------|:---------------------|:------------|:----------------------------|                                       
|  Timeout                        |Lite     |Integration/Production|-            |Keeps a queue for management |
|  Subscription                   |Lite     |Integration/Production|-            |-                            |
|  Saga                           |Lite     |Integration/Production|-            |-    				           |
|  Gateway                        |Lite     |MultiSite             |-            |-     					   |
|  Distributor                    |- 	    |-                     |-            |Distributor				   |


## Default persisting technology

The `AsAServer` role activates the timeout manager. This role does not explicitly determine which persisting technology to use. Hence, the default persisting technology for timeout manager (RavenDB) is used.

Similarly to the `AsAServer` role, the various profiles activate the different NServiceBus features, without explicitly configuring the persisting technology.
