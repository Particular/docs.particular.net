---
title: Configure delayed retries
summary: Shows how to configure delayed retries which happen as a second stage of the default recoverability behavior.
tags:
 - Error Handling
 - Exceptions
 - Retry
 - Recoverability
 - Second Level Retries
redirects:
 - nservicebus/second-level-retries
related:
 - samples/faulttolerance
---

NOTE: Starting from NServiceBus Version 6 Delayed Retries Policy (formerly known as Second Level Retries Policy) has been deprecated in favour of the new custom recoverability policy which allows much more control over the Recoverability behavior. This documentation shows how previous Delayed Retries Policies can be implemented with the new Recoverability Policy. For a comprehensive overview the Recoverability Policy refer to [Recoverability Policy](/nservicebus/recoverability/custom-recoverability-policy.md).

WARN: Delayed Retries cannot be used when transport transactions are disabled or Delayed Delivery is not available. For more information about transport transactions, refer to [transport transaction](/nservicebus/transports/transactions.md). For more information about delayed delivery, refer to [delayed-delivery](/nservicebus/messaging/delayed-delivery.md#caveats).

### Configuring Delayed Retries using app.config

To configure Delayed Retries, enable its configuration section:

snippet:SecondLevelRetiesAppConfig

 * `Enabled`: Turns the feature on and off. Default: true.
 * `TimeIncrease`: A time span after which the time between retries increases. Default: 10 seconds (`00:00:10`).
 * `NumberOfRetries`: Number of times Delayed Retries are performed. Default: 3.


### Configuring Delayed Retries through code

snippet:SlrCodeFirstConfiguration


### Configuration Delayed Retries through IProvideConfiguration

snippet:SlrProvideConfiguration

### Configuring Delayed Retries through ConfigurationSource

snippet:SlrConfigurationSource

snippet:SLRConfigurationSourceUsage

### Disabling Delayed Retries through code

snippet:DisableSlrWithCode

### Custom Retry Policy

Custom retry logic can be configured based on headers or timing in code.

#### Applying a custom policy

snippet:SecondLevelRetriesCustomPolicy

#### Simple Policy

The following retry policy that will retry a message 3 times with a 5 second interval.

snippet:SecondLevelRetriesCustomPolicyHandlerConfig

snippet:SecondLevelRetriesCustomPolicyHandler

#### Exception based Policy

The following retry policy extends the previous policy with a custom handling logic for a specific exception.

snippet:SecondLevelRetriesCustomExceptionPolicyHandlerConfig

snippet:SecondLevelRetriesCustomExceptionPolicyHandler
