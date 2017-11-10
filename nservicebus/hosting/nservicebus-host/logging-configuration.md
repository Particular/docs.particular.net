---
title: NServiceBus Host Logging configuration
component: Host
reviewed: 2017-03-01
related:
 - nservicebus/logging
 - samples/logging/hostcustom
 - samples/logging/hostprofiles
---

include: host-deprecated-warning

## Logging configuration

This article explains how to customize the logging configuration when using the NServiceBus Host. For more details about logging configuration with the built-in profiles, refer to the [NServiceBus.Host Profiles - Logging](profiles.md#logging) section.

### Constructor of IConfigureThisEndpoint implementation

It is advised to customize logging in the constructor of the class that implements `IConfigureThisEndpoint`. This is recommended as this class is the earliest opportunity to initialize [any custom logging framework](/nservicebus/logging/#custom-logging).

WARNING: If logging is not initialized in the constructor and anything goes wrong during startup of the NServiceBus.Host errors could be written to the default NServiceBus logging location, and not in the expected custom log output location(s).

partial: customize

partial: profile
