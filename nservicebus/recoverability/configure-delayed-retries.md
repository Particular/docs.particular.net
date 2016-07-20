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

### Configuring Delayed Retries using app.config

To configure SLR, enable its configuration section:

snippet:SecondLevelRetiesAppConfig

 * `Enabled`: Turns the feature on and off. Default: true.
 * `TimeIncrease`: A time span after which the time between retries increases. Default: 10 seconds (`00:00:10`).
 * `NumberOfRetries`: Number of times SLR kicks in. Default: 3.


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


#### Error Headers Helper

In Versions 5 and below access to the retry contextual information is available via the raw transport message and headers.

A Custom Policy has access to the raw message including both the [retries handling headers](/nservicebus/messaging/headers.md#retries-handling-headers) and the [error forwarding headers](/nservicebus/messaging/headers.md#error-forwarding-headers). Any of these headers can be used to control the retries for a message. In the following examples the helper class will provide access to a subset of the headers.

snippet:ErrorsHeadersHelper


#### Simple Policy

The following retry policy that will retry a message 3 times with a 5 second interval.

snippet:SecondLevelRetriesCustomPolicyHandlerConfig

snippet:SecondLevelRetriesCustomPolicyHandler


#### Exception based Policy

The following retry policy extends the previous policy with a custom handling logic for a specific exception.

snippet:SecondLevelRetriesCustomExceptionPolicyHandlerConfig

snippet:SecondLevelRetriesCustomExceptionPolicyHandler

With the new recoverability override starting from Version 6 and above it is possible to take full control over the recoverability behavior. For example the above custom SLR policy will always to first level retries even for business exceptions. That doesn't have to be like that. It is possible to disable first level retries entirely for business exceptions like shown below:

snippet:CustomExceptionPolicyHandler
