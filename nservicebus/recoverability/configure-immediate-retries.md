---
title: Configure immediate retries
summary: Shows how to configure immediate retries which happen as a first stage of the default recoverability behavior.
component: Core
reviewed: 2017-02-07
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

WARNING: Immediate Retries cannot be used when [transport transactions](/nservicebus/transports/transactions.md) are disabled.


partial: config