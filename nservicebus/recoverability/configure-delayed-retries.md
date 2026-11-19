---
title: Configure delayed retries
summary: How to configure delayed retries as a second stage of recoverability.
component: Core
reviewed: 2026-11-02
redirects:
 - nservicebus/second-level-retries
related:
 - samples/faulttolerance
 - samples/custom-recoverability
---

> [!WARNING]
> Delayed retries cannot be used when transport transactions are disabled or delayed delivery is not available. In that case the endpoint fails to start unless delayed retries are [disabled](#disabling-through-code). For more information about transport transactions, refer to [transport transactions](/transports/transactions.md). For more details on delayed delivery, see the [delayed delivery](/nservicebus/messaging/delayed-delivery.md) article.

## Configuring delayed retries

 * `TimeIncrease`: Specifies the delay interval for each retry attempt. This delay increases by the same timespan with each delayed delivery. The default value is 10 seconds. For example, with the default value of 10 seconds, i.e., 00:00:10, the first delayed retry will occur at 10 seconds, the subsequent delayed retry will occur at 20 seconds, and so on.
 * `NumberOfRetries`: Number of times delayed retries are performed. Default is 3.

snippet: DelayedRetriesConfiguration

The default policy stops scheduling delayed retries once 24 hours have passed since the previous delayed retry and moves the message to the error queue. See [delayed retries](/nservicebus/recoverability/#delayed-retries) for the reasoning.


## Disabling through code

snippet: DisableDelayedRetries


## Custom retry policy

Custom retry logic can be configured via code.

snippet: DelayedRetriesCustomPolicy

More details can be found in the [recoverability policy documentation](/nservicebus/recoverability/custom-recoverability-policy.md).


### Simple policy

The following retry policy replaces the default delays, which grow by 10 seconds with each delayed retry, with a fixed delay of five seconds for every delayed retry.

snippet: DelayedRetriesCustomPolicyHandler


### Exception-based policy

Sometimes the number of retries or the delay interval might depend on the error exception thrown. The following retry policy extends the previous one by skipping delayed retries whenever `MyBusinessException` has been thrown during incoming message processing. Immediate retries still happen first.

snippet: DelayedRetriesCustomExceptionPolicyHandler

To skip both immediate and delayed retries for an exception type without a custom policy, declare it as an [unrecoverable exception](/nservicebus/recoverability/#unrecoverable-exceptions).

To control what happens after all retries have failed, see [configure error handling](/nservicebus/recoverability/configure-error-handling.md).
