---
title: More On Profiles
summary: 'Two types of profiles in NServiceBus: environment and feature.'
component: Host
tags:
- Profiles
- NServiceBus.Host
- Installers
- Feature
- Distributor
- Performance Counters
redirects:
 - nservicebus/more-on-profiles
reviewed: 2016-11-03
---

The NServiceBus Host profiles enable altering the behavior of an endpoint without recompiling the code. The profiles enable tailoring endpoints configuration for different environments.

Profiles are only available if using the [NServiceBus host](/nservicebus/hosting/nservicebus-host/) (NServiceBus.Host.exe or the 32-bit only version of it) so this is not applicable if self hosting NServiceBus in a website, WCF service, smart client, etc.

Profiles are divided into two main categories, depending on what they control:

 * Environment profiles help to avoid common configuration errors that occur when manually moving a system from development to production via integration. Environment profiles enable easy transition of the system without any code changes.
 * Feature profiles turn NServiceBus features on and off, easily and with no code changes. For example, turning on and off the performance counters.

Technically there is no difference between the environment- and feature-related profiles.

## Environment-related profiles

NServiceBus comes with three built-in profiles whose main goal is to adjust the behavior of the host, depending on the environment where the endpoint is running.

It is possible to [create custom profiles](/nservicebus/hosting/nservicebus-host/).

The environmental-related profiles:


### Lite profile

Suitable for running on the development machine, possibly inside Visual Studio.

partial: lite

### Integration profile

Suitable for running the endpoint in integration and QA environments.

[Installers](/nservicebus/operations/installers.md) are invoked to make deployment easy to automate.

partial: integration


### Production profile

The default if no explicit profile is defined.

This profile sets the endpoint up for production use.

[Installers](/nservicebus/operations/installers.md) are not invoked since the endpoint is probably installed as a Windows Service and does not run with elevated privileges.

[Installers](/nservicebus/operations/installers.md) only run when [installing the host](/nservicebus/hosting/nservicebus-host/installation.md) or the code runs inside Visual Studio in Debug mode.

partial: production


## Feature-related profiles

Feature-related profiles that come out of the box :

partial: feature


## Telling the host which profiles to run

To activate a specific profile, when starting the host, pass the full name of the profile in the command line. Type names are case insensitive. Profiles can be combined by separating them with white space.

For example, to run the endpoint with the `Integration` and `PerformanceCounters` profiles:

```dos
.\NServiceBus.Host.exe nservicebus.integration nservicebus.performancecounters
```

When installing the host as a Windows Service, the profiles used during installation are saved and they are used every time the host starts. In order to install the host with the `Production` and `PerformanceCounters` profiles:

```dos
.\NServiceBus.Host.exe /install nservicebus.production nservicebus.performancecounters
```
