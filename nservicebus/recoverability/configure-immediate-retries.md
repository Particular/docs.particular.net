---
title: Configure immediate retries
summary: Shows how to configure immediate retries which happen as a first stage of the default recoverability behavior.
tags:
 - Error Handling
 - Exceptions
 - Retry
 - Recoverability
 - First Level Retries
redirects:
related:
 - samples/faulttolerance
---

### Configuring FLR using app.config

In Version 3 this configuration was available via `MsmqTransportConfig`.

In Version 4 and above the configuration for this mechanism is implemented in the `TransportConfig` section.

snippet:configureFlrViaXml


### Configuring FLR through code

snippet:FlrCodeFirstConfiguration


### Configuring FLR through IProvideConfiguration

snippet:FlrProvideConfiguration


### Configuring FLR through ConfigurationSource

snippet:FLRConfigurationSource

snippet:FLRConfigurationSourceUsage
