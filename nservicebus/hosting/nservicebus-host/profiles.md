---
title: NServiceBus Host Profiles
summary: 'Profiles ease the configuration process. There are three profiles out of the box: Lite, Integration, and Production.'
redirects:
 - nservicebus/profiles-for-nservicebus-host
 - nservicebus/more-on-profiles
 - nservicebus/nservicebus/hosting/nservicebus-host/more-on-profiles
component: Host
reviewed: 2020-05-26
---

include: host-deprecated-warning

There are many configuration options for endpoints using the [NServiceBus host](/nservicebus/hosting/nservicebus-host/). The endpoint configuration specifies levels of logging, subscription, saga storage, and more. 

Profiles enable tailoring endpoints configuration for different environments without recompiling code.

There are two categories of profile:

 * **Environment profiles** can be used to avoid common configuration errors when manually moving a system between different environments, e.g., from development to production.
 * **Feature profiles** allow turning NServiceBus features, such as performance counters, on and off with no code changes.


## Default profiles

By default, NServiceBus comes with a set of predefined environments and feature profiles. It's also possible to create custom profiles or customize the default profiles; to learn more about those options, refer to the [NServiceBus host profiles customization](/nservicebus/hosting/nservicebus-host/profiles-customization.md) article.


## Environment profiles

There are three built-in environment profiles that adjust the host behavior to the environment in which the endpoint is running. These profiles can be used to switch between different environments during development, testing, and deployment.


### Lite profile

Suitable for running on a development machine, for example, inside Visual Studio.

partial: lite


### Integration profile

Suitable for running the endpoint in integration and QA environments.

* [Installers](/nservicebus/operations/installers.md) are invoked to make deployment easy to automate.
partial: integration


### Production profile

The default if no explicit profile is defined. This profile configures the endpoint for production use.

* [Installers](/nservicebus/operations/installers.md) are not invoked since the endpoint is often installed as a Windows Service and does not run with elevated privileges. [Installers](/nservicebus/operations/installers.md) run only when [installing the host](/nservicebus/hosting/nservicebus-host/installation.md) or when the code runs inside Visual Studio in Debug mode.
partial: production


## Feature profiles

partial: feature


## Specifying which profiles to run

If the host is run without specifying a profile, NServiceBus defaults to the `Production` profile.

To activate a specific profile, pass the full name of the profile in the command line when starting the host. Type names are case-insensitive. Profiles can be combined by separating them with white space.

For example, to run the endpoint with the `Integration` and `PerformanceCounters` profiles:

```dos
.\NServiceBus.Host.exe nservicebus.integration nservicebus.performancecounters
```

When installing the host as a Windows Service, the profiles used during installation are saved, and they are used every time the host starts. In order to install the host with the `Production` and `PerformanceCounters` profiles:

```dos
.\NServiceBus.Host.exe /install nservicebus.production nservicebus.performancecounters
```


## Logging

The built-in profiles, by default use the console, and rolling file appends, logging information at the `Info` threshold.



The rolling file logs are written to the `logfile` in the same directory as the executable. The file grows to a maximum size of 1MB; then a new file is created. A maximum of ten files is kept, then the oldest file is erased. If not otherwise specified, the logging threshold is set to the `Warn` level. To configure the logging threshold, see [changing logging levels](/nservicebus/logging/#default-logging-changing-the-defaults-changing-the-logging-level).

Warning: For changes to the configuration to have an effect, the process must be restarted.

Refer to the [logging configuration](/nservicebus/hosting/nservicebus-host/logging-configuration.md) article to learn about customizing logging configuration.


## Persistence

Note: In NServiceBus version 5 and above, persistence must be explicitly configured.

The built-in profiles use the following default persistence settings:

| -              | Lite     | Integration  | Production   |
|:---------------|:---------|:-------------|:-------------|
|  Timeout       |In-Memory |As configured |As configured |
|  Subscription  |In-Memory |As configured |As configured |
|  Saga          |In-Memory |As configured |As configured |
|  Gateway       |In-Memory |As configured |As configured |
|  Distributor   |-         |-             |-             |

In the Lite profile, NServiceBus Host will always use the in-memory persistence. In the Integration and Production profiles, the host verifies if a specific persistence mechanism is provided, e.g., in the endpoint configuration. If not specified otherwise, then RavenDB persistence will be used by default.