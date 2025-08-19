---
title: Configure delayed retries
summary: How to configure delayed retries as a second stage of recoverability.
component: Core
reviewed: 2025-04-28
redirects:
 - nservicebus/second-level-retries
related:
 - samples/faulttolerance
 - samples/custom-recoverability
---

> [!WARNING]
> Delayed retries cannot be used when transport transactions are disabled or delayed delivery is not available. For more information about transport transactions, refer to [transport transactions](/transports/transactions.md). For more details on delayed delivery, see the [delayed delivery](/nservicebus/messaging/delayed-delivery.md) article.

## Configuring delayed retries

 * `TimeIncrease`: Specifies the delay interval for each retry attempt. This delay increases by the same timespan with each delayed delivery. The default value is 10 seconds. For example, with the default value of 10 seconds, i.e., 00:00:10, the first delayed retry will occur at 10 seconds, the subsequent delayed retry will occur at 20 seconds, and so on.
 * `NumberOfRetries`: Number of times delayed retries are performed. Default is 3.

snippet: DelayedRetriesConfiguration


## Disabling through code

snippet: DisableDelayedRetries


## Custom retry policy

Custom retry logic can be configured via code.

snippet: DelayedRetriesCustomPolicy

More details can be found in the [recoverability policy documentation](/nservicebus/recoverability/custom-recoverability-policy.md).


### Simple policy

The following retry policy overrides the default ten second delay interval and sets it to five seconds.

snippet: DelayedRetriesCustomPolicyHandler


### Exception-based policy

Sometimes the number of retries or the delay interval might depend on the error exception thrown. The following retry policy extends the previous one by skipping delayed retries whenever `MyBusinessException` has been thrown during incoming message processing.

snippet: DelayedRetriesCustomExceptionPolicyHandler
