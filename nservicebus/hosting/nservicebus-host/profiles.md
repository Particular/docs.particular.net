---
title: Profiles For NServiceBus Host
summary: 'Profiles ease the configuration process. Three profiles straight from the box: Lite, Integration, and Production.'
tags:
- Profiles
- Hosting
- Logging
- Persistence
redirects:
 - nservicebus/profiles-for-nservicebus-host
 - nservicebus/more-on-profiles
 - nservicebus/nservicebus/hosting/nservicebus-host/more-on-profiles
component: Host
reviewed: 2016-10-27
---

There are many configuration options for endpoints using [NServiceBus Host](/nservicebus/hosting/nservicebus-host/). The endpoint configuration specifies levels of logging, subscription and saga storage, etc. 

Profiles enable tailoring endpoints configuration for different environments without recompiling code.

There are two categories of profiles:

 * **Environment profiles** can be used to avoid common configuration errors that occur when manually moving a system between different environments, e.g. from development to production.
 * **Feature profiles** allow to turn on and off NServiceBus features such as performance counters with no code changes.


## Default profiles

Out of the box there are a set of predefined environment and feature profiles. It's also possible to create custom profiles or customize the default profiles, to learn more about those options refer to the [NServiceBus Host Profiles customization](/nservicebus/hosting/nservicebus-host/profiles-customization.md) article.


## Environment-related profiles

There are three built-in environment profiles that adjust the behavior of the host to the environment in which the endpoint is running. These profiles can be used to easily switch between different environments during development, testing and deployment.


### Lite profile

Suitable for running on the development machine, possibly inside Visual Studio.

partial: lite


### Integration profile

Suitable for running the endpoint in integration and QA environments.

* [Installers](/nservicebus/operations/installers.md) are invoked to make deployment easy to automate.
partial: integration


### Production profile

The default if no explicit profile is defined. This profile sets the endpoint up for production use.

* [Installers](/nservicebus/operations/installers.md) are not invoked since the endpoint is probably installed as a Windows Service and does not run with elevated privileges. [Installers](/nservicebus/operations/installers.md) only run when [installing the host](/nservicebus/hosting/nservicebus-host/installation.md) or the code runs inside Visual Studio in Debug mode.
partial: production


## Feature-related profiles

partial: feature


## Specifying which profiles to run

If the host is run without specifying a profile, NServiceBus defaults to the `Production` profile.

To activate a specific profile, when starting the host pass the full name of the profile in the command line. Type names are case insensitive. Profiles can be combined by separating them with white space.

For example, to run the endpoint with the `Integration` and `PerformanceCounters` profiles:

```dos
.\NServiceBus.Host.exe nservicebus.integration nservicebus.performancecounters
```

When installing the host as a Windows Service, the profiles used during installation are saved and they are used every time the host starts. In order to install the host with the `Production` and `PerformanceCounters` profiles:

```dos
.\NServiceBus.Host.exe /install nservicebus.production nservicebus.performancecounters
```


## Logging behaviors

Logging is another kind of behavior that can be changed from one profile to another. However, unlike other profile behaviors, logging levels and sinks need to be defined before configuring other components, even before the container. For that reason, logging configuration is kept separate from other profile behaviors.

The logging behavior configured for the three built-in profiles is shown:

| Profile     | Appender     | Threshold
|-------------|--------------|-----
| Lite        | Console      | Debug
| Integration | Console      | Info
| Production  | Rolling File | Configurable, Warn by default

When running the production profile, the logs are written to `logfile` in the same directory as the exe. The file grows to a maximum size of 1MB and then a new file is created. A maximum of 10 files is held and then the oldest file is erased. If no configuration exists, the logging threshold is Warn. To configure the logging threshold see [changing logging level via config file](/nservicebus/logging/#logging-levels-changing-the-logging-level-via-app-config).

For changes to the configuration to have an effect, the process must be restarted.

For different logging behaviors than these, see the next section.


## Customized logging

To specify logging for a given profile, write a class that implements `IConfigureLoggingForProfile<T>` where `T` is the profile type. The implementation of this interface is similar to that described for `IWantCustomLogging` in the [host page](/nservicebus/hosting/nservicebus-host/).

snippet:configure_logging

Here, the host passes the instance of the class that implements `IConfigureThisEndpoint` so it is not necessary to implement `IWantTheEndpointConfig`.

NOTE: While it is possible have one class configure logging for multiple profile types, it is not possible to have more than one class configure logging for the same profile. Only one of these classes for all profile types passed in the command-line.

See the [logging documentation](/nservicebus/logging/) for more information.


## Persistence

When using the Host out of the box, it is possible to utilize one of the available profiles. The following table shows which persistence technology each pre-built profile configures by default. In addition, it is possible override the configured defaults.

The following table summarizes the different persistence technologies being used by the built-in profiles.

NOTE: Before configuring persistence technology, to avoid overriding custom configurations, the profiles check if other types of storage are used.

|-                                |In-Memory|RavenDB               |NHibernate   |MSMQ                         |
|:--------------------------------|:--------|:---------------------|:------------|:----------------------------|
|  Timeout                        |Lite     |Integration/Production|-            |Keeps a queue for management |
|  Subscription                   |Lite     |Integration/Production|-            |-                            |
|  Saga                           |Lite     |Integration/Production|-            |-                            |
|  Gateway                        |Lite     |MultiSite             |-            |-                            |
|  Distributor                    |-        |-                     |-            |Distributor                  |


## Default persisting technology

The `AsA_Server` role activates the timeout manager. This role does not explicitly determine which persisting technology to use. Hence, the default persisting technology for timeout manager (RavenDB) is used.

Similarly to the `AsA_Server` role, the various profiles activate the different NServiceBus features, without explicitly configuring the persisting technology.