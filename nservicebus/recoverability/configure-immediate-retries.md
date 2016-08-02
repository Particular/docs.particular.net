---
title: Configure immediate retries
summary: Shows how to configure immediate retries which happen as a first stage of the default recoverability behavior.
tags:
 - Error Handling
 - Exceptions
 - Retry
 - Recoverability
 - Immediate Retries
redirects:
related:
 - samples/faulttolerance
---

NOTE: In order to get full control over Immediate Retries it is possible to override the default Recoverability Policy. For a comprehensive overview the Recoverability Policy refer to [Recoverability Policy](/nservicebus/recoverability/custom-recoverability-policy.md).

WARN: Immediate Retries cannot be used when transport transactions are disabled. For more information about transport transactions, refer to [transport transaction](/nservicebus/transports/transactions.md).


## Configuring Immediate Retries


### Using app.config

In Version 3 this configuration was available via `MsmqTransportConfig`.

In Version 4 and above the configuration for this mechanism is implemented in the `TransportConfig` section.

snippet:configureFlrViaXml


### Through code

Starting from Version 6 it is possible to provide Immediate Retries configuration via code API. 

snippet:ImmediateRetriesConfiguration


### Through IProvideConfiguration

snippet:ImmediateRetriesProvideConfiguration


### Through ConfigurationSource

snippet:ImmediateRetriesConfigurationSource

snippet:ImmediateRetriesConfigurationSourceUsage
