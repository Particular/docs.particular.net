---
title: Configure immediate retries
summary: Shows how to configure immediate retries which happen as a first stage of the default recoverability behavior.
component: Core
reviewed: 2025-08-26
related:
 - samples/faulttolerance
 - samples/custom-recoverability
---

> [!NOTE]
> In order to get full control over Immediate Retries it is possible to override the default [Recoverability Policy](/nservicebus/recoverability/custom-recoverability-policy.md).

> [!WARNING]
> Immediate Retries cannot be used when [transport transactions](/transports/transactions.md) are disabled.


## Configuring

 * `NumberOfRetries`: Number of times Immediate Retries are performed. Default: 5.

snippet: ImmediateRetriesConfiguration


## Disabling

snippet: DisablingImmediateRetriesConfiguration


> [!NOTE]
> Configuration through app.config, `IProvideConfiguration` or `ConfigurationSource` is not available in Versions 6 and above.
