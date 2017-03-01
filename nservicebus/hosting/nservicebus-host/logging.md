---
title: NServiceBus Host Logging configuration
component: Host
reviewed: 2016-11-04
---

Logging configuration for NServiceBus Host can be customized by providing endpoint configuration or using profiles. See [Profiles - Logging](/nservicebus/hosting/nservicebus-host/profiles.md#Logging) section for more details about built-in profiles. This article explains how to customize default configuration.

## Via Endpoint configuration

partial: customize

Refer to the [logging documentation](/nservicebus/logging/) for more information about available configuration options.


## Via profiles 

Logging levels and sinks need to be defined before configuring other components, therefore logging profile configuration is kept separate from other profile behaviors and requires implementing a dedicated interface. To customize logging for a given profile, create a class implementing `IConfigureLoggingForProfile<T>` where `T` is the profile type:

snippet:LoggingConfigWithProfile

NOTE: It is possible have one class configure logging for multiple profile types. However, it is not possible to have logging configuration for a single profile defined in multiple classes.

The host's [profiles](/nservicebus/hosting/nservicebus-host/profiles.md) mechanism can be used to specify different logging levels (`DEBUG`, `WARN`, etc.) or targets (`CONSOLE`, `FILE`, etc.).

Refer to the [logging documentation](/nservicebus/logging/) for more information about available configuration options.
