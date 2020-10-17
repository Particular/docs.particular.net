---
title: NServiceBus Host Logging Configuration
summary: How to customize the logging configuration for the NServiceBus host
component: Host
reviewed: 2020-10-17
related:
 - nservicebus/logging
 - samples/logging/hostcustom
 - samples/logging/hostprofiles
---

include: host-deprecated-warning

This article explains how to customize the logging configuration when using the NServiceBus host. For more details about logging configuration with the built-in profiles, refer to the [NServiceBus.Host Profiles - Logging](profiles.md#logging) section.

### Constructor of IConfigureThisEndpoint implementation

Logging should be customized in the constructor of the class that implements `IConfigureThisEndpoint`. This is recommended, as this class is the earliest opportunity to initialize [any custom logging framework](/nservicebus/logging/#custom-logging).

WARNING: If logging is not initialized in the constructor and anything goes wrong during startup of the NServiceBus.Host, errors could be written to the default NServiceBus logging location, and not in the expected custom log output location(s).

### Via endpoint configuration

To change the host's logging configuration, implement the `IConfigureThisEndoint` interface. Provide the custom configuration in the `Customize` method:

snippet: CustomHostLogging

partial: profile
