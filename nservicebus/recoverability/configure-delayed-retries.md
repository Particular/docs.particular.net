---
title: Configure delayed retries
summary: Shows how to configure delayed retries which happens as a second stage of recoverability.
component: Core
reviewed: 2016-10-21
tags:
 - Error Handling
 - Exceptions
 - Retry
 - Recoverability
 - Delayed Retries
redirects:
 - nservicebus/second-level-retries
related:
 - samples/faulttolerance
---

NOTE: Starting from NServiceBus Version 6 Delayed Retries Policy (formerly known as Second Level Retries Policy) has been deprecated in favor of the new custom Recoverability policy which allows much more control over the Recoverability behavior. This documentation shows how previous Delayed Retries Policies can be implemented with the new [Recoverability Policy](/nservicebus/recoverability/custom-recoverability-policy.md).

WARNING: Delayed Retries cannot be used when transport transactions are disabled or Delayed Delivery is not available. For more information about transport transactions, refer to [transport transaction](/nservicebus/transports/transactions.md). For more details on the caveats, see: [Delayed Delivery and its Caveats](/nservicebus/messaging/delayed-delivery.md#caveats) article.

partial: config


## Disabling Delayed Retries through code

snippet:DisableDelayedRetries


## Custom Retry Policy

Custom retry logic can be configured via code.

snippet:DelayedRetriesCustomPolicy


### Simple Policy

The following retry policy overrides default delay interval and sets it to 5 seconds.

snippet:DelayedRetriesCustomPolicyHandler


### Exception based Policy

Sometimes the number of retries or the delay interval might depend on the error exception thrown. The following retry policy extends the previous one by skipping delayed retries whenever `MyBusinessException` has been thrown during incoming message processing.

snippet:DelayedRetriesCustomExceptionPolicyHandler


partial: legacy
