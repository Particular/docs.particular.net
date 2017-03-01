---
title: NServiceBus Host Logging customization
component: Host
reviewed: 2016-11-04
---

Logging in NServiceBus Host is controlled by selecting an appropriate profile. See [Profiles - Logging](/nservicebus/hosting/nservicebus-host/profiles.md#Logging) section for more details.

Logging levels and sinks need to be defined before configuring other components, therefore logging profile configuration is kept separate from other profile behaviors and requires implementing a dedicated interface. To customize logging for a given profile, create a class implementing `IConfigureLoggingForProfile<T>` where `T` is the profile type:

snippet:LoggingConfigWithProfile

NOTE: It is possible have one class configure logging for multiple profile types. However, it is not possible to have logging configuration for a single profile defined in multiple classes.

See the [logging documentation](/nservicebus/logging/) for more information.

partial: customize
