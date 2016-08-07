---
title: Configure immediate retries
summary: Shows how to configure immediate retries which happen as a first stage of the default recoverability behavior.
component: Core
tags:
 - Error Handling
 - Exceptions
 - Retry
 - Recoverability
 - Immediate Retries
related:
 - samples/faulttolerance
---

NOTE: In order to get full control over Immediate Retries it is possible to override the default [Recoverability Policy](/nservicebus/recoverability/custom-recoverability-policy.md).

WARNING: Immediate Retries cannot be used when [transport transaction](/nservicebus/transports/transactions.md) are disabled.


## Configuring Immediate Retries

partial: config