---
title: NServiceBus Host Logging Configuration
summary: How to customize the logging configuration for the NServiceBus host
component: Host
reviewed: 2025-09-26
related:
 - nservicebus/logging
---

include: host-deprecated-warning

This article explains how to customize the logging configuration when using the NServiceBus host. For more details about logging configuration with the built-in profiles, refer to the [NServiceBus.Host Profiles - Logging](profiles.md#logging) section.

### Constructor of IConfigureThisEndpoint implementation

Logging should be customized in the class's constructor that implements `IConfigureThisEndpoint`. That is recommended, as the class implementing `IConfigureThisEndpoint` is the earliest opportunity to initialize [any custom logging framework](/nservicebus/logging/#custom-logging).

> [!WARNING]
> If logging is not initialized in the constructor and anything goes wrong during the startup of the NServiceBus.Host errors could be written to the default NServiceBus logging location and not in the expected custom log output location(s).

### Via endpoint configuration

To change the host's logging configuration, implement the `IConfigureThisEndoint` interface. Provide the custom configuration in the `Customize` method:

snippet: CustomHostLogging
