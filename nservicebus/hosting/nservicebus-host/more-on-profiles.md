---
title: More On Profiles
summary: 'Two types of profiles in NServiceBus: environment and feature.'
tags:
- Profiles
- NServiceBus.Host
- Installers
- Features
- Distributor
- Performance Counters
redirects:
 - nservicebus/more-on-profiles
---

The NServiceBus Host profiles allow you to alter the behavior of your endpoint without recompiling your code. The profiles enable tailoring endpoints for different environments and controlling things like scaling out (running the distributor) and enabling HTTP access (gateway).

 Profiles are only available if you use the [NServiceBus host](/nservicebus/hosting/nservicebus-host/) (NServiceBus.Host.exe or the 32-bit only version of it) so this is not applicable if you self host NServiceBus in a website, WCF service, smart client, etc.

Profiles are divided into two main categories, depending on what they control:

 * Environment profiles help to avoid common configuration errors that occur when you manually move a system from development to production via integration. Environment profiles enable easy transition of the system without any code changes.
 * Feature profiles turn NServiceBus features on and off, easily and with no code changes. For example, turning on and off the Distributor and timeout manager.

Technically there is no difference between the environment- and feature-related profiles.

Let's look at each of them, starting with the environment-related profiles.


## Environment-related profiles

NServiceBus comes with three built-in profiles whose main goal is to adjust the behavior of the host, depending on the environment where the endpoint is running.

You can, of course, [create your own profiles](/nservicebus/hosting/nservicebus-host/).

The environmental-related profiles:


### Lite profile

Suitable for running on your development machine, possibly inside Visual Studio.

This profile configures all the persistence like sagas, subscriptions, timeouts, etc. to be InMemory which is easy to set up but probably not what you want for production.

Lite also turns the TimeoutManager on by default.

[Installers](http://andreasohlund.net/2012/01/26/installers-in-nservicebus-3-0/) are always invoked when running the Lite profile.

Logging is output to the console.


### Integration profile

Suitable for running your endpoint in integration and QA environments.

Installers are invoked to make deployment easy to automate.

Logging is output to the console by default.


### Production profile

The default if no explicit profile is defined.

This profile sets your endpoint up for production use. This means that all storage is durable and suitable for scale out.

Installers are not invoked since your endpoint is probably installed as a Windows Service and does not run with elevated privileges.

Installers only run when you install the host or your code runs inside Visual Studio in Debug mode.

Logging is output to a log file in the runtime directory (again, because you are probably running it as a Windows Service).

Performance counters are installed by default.


## Feature-related profiles

Feature-related profiles that come out of the box :

 * `Master` makes the endpoint a "master node endpoint". This means that it runs the gateway for multi-site interaction, the timeout manager, and the Distributor. It also starts a worker that enlists with the Distributor. It can no tbe combined with the worker or distributor profiles.
 * `Worker` makes the current endpoint enlist as a worker with its distributor running on the master node. It cannot be combined with the master or distributor profiles.
 * `Distributor` starts the endpoint as a distributor only. This means that the endpoint does no actual work and only distributes the load among its enlisted workers. It cannot be combined with the Master and Worker profiles.
 * `PerformanceCounters` turns on the NServiceBus-specific performance counters. Performance counters are installed by default when running in Production profile.


## Telling the host which profiles to run

To activate a specific profile, when you start the host, pass in the full name of the profile on the command line. Type names are case insensitive. Profiles can be combined by separating them with white space.

So to run your endpoint with the `Integration` and `Master` profiles, use this:

    .\NServiceBus.Host.exe nservicebus.integration nservicebus.master

When you install the host as a Windows Service, the profiles used when installing persist and are used every time the host starts. So to install your host with the Production and the Distributor profiles, use this:

    .\NServiceBus.Host.exe /install nservicebus.production nservicebus.distributor

