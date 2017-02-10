---
title: Logging Profiles
summary: Configure logging using profiles
component: Host
reviewed: 2016-12-09
tags:
- Logging
- Profiles
redirects:
- nservicebus/logging-profiles
related:
- nservicebus/hosting/nservicebus-host/profiles
---

Logging can be configured via Profiles. However, unlike other profile behaviors, logging needs to be defined before configuring other components, even before the container. For that reason, logging configuration is kept separate from other profile behaviors.

The [NServiceBus Host](/nservicebus/hosting/nservicebus-host/) has three built-in profiles for logging `Lite`, `Integration`, and `Production`.


## Default profile behavior


partial:behavior


## Customized logging via a profile

To specify logging for a given profile, write a class that implements `IConfigureLoggingForProfile<T>` where `T` is the profile type.

snippet:LoggingConfigWithProfile

Here, the host passes the instance of the class that implements `IConfigureThisEndpoint` so implementing `IWantTheEndpointConfig` is not required.

WARNING: While it is possible to have one class configure logging for multiple profile types, it is not supported to have more than one class configure logging for the same profile. NServiceBus can allow only one of these classes for all profile types passed in the command-line.