---
title: Implement a custom Recoverability Policy
summary: Shows how to take full control over Recoverability by implementing a Recoverability Policy
tags:
 - Error Handling
 - Exceptions
 - Retry
 - Recoverability
redirects:
related:
 - samples/faulttolerance
---

## Default Recoverability

The default Recoverability Policy is implemented in `DefaultRecoverabilityPolicy` and publicly exposed for use cases when aggregation of the default recoverability behavior is desired when a custom recoverability policy is implemented. The default policy takes into account the settings provided for Immediate Retries, Delayed Retries and the configured error queue. The default policy works like the following:

1. When an exception of type `MessageDeserializedException` was raised, the `MoveToError` recoverability action is returned with the default error queue.
2. If the `ImmediateProcessingFailures` are less or equal to the configured `MaxNumberOfRetries` for Immediate Retries, the `ImmediateRetry` recoverability action is returned
3. When Immediate Retries are exceeded the Delayed Retries configuration are considered. If the `DelayedDeliveriesPerformed` are less or equal the configured `MaxNumberOfRetries` and the message hasn't reached the maximum time allowed for retries (24 hours), the `DelayedRetry` recoverability action is returned. The delay is calculated according to the following formula:
delay = DelayedRetry.TimeIncrease * (DelayedDeliveriesPerformed + 1)
4. If ImmediateRetries and DelayedRetries are exceeded, the `MoveToError` recoverability action is returned with the default error queue.

## Fallback

As outlined in the [Recoverability introduction](/nservicebus/recoverability/) Immediate and Delayed Retries can only be performed under certain conditions. If a custom Recoverability Policy returns a recoverability action which cannot be fulfilled by the infrastructure, the decision will be overriden with the `MoveToError` recoverability action with the default error queue. This behavior is for safety reasons and cannot be overriden.

## Recoverability Configuration

`RecoverabilityConfig` contains all required information to take into account when a recoverability policy is implemented. It provides the following information:

* Maximum number of retries for Immediate Retries
* Maximum number of retries for Delayed Retries
* Time of increase for individual Delayed Retries
* Configured error queue address

The information provided on the configuration is static and will not change between subsequent executions of the policy.

In cases when Immediate and/or Delayed Retry capabilities are not available MaxNumberOfRetries exposed to recoverability policy will be set to 0 (zero).

## Error Context

`ErrorContext` contains all required information to take into account about the currently failing message. It provides the following information:

* Exception which cause the message to fail
* Transport transaction on which the message failed
* Number of failed immediate processing attempts
* Number of delayed deliveries performed
* Message which failed

## Implement a custom policy

### Partial customization

Sometimes only a partial customization of the default Recoverability Policy is desired. In order to achieve partial customization the `DefaultRecoverabilityPolicy` needs to be called in the customized Recoverability Policy delegate.

In the following example the default Recoverability Policy is tweaked to do three Immediate Retries and three Delayed Retries with a time increase of two seconds. The configuration looks like the following:

snippet:PartiallyCustomizedPolicyRecoverabilityConfiguration

If when certain exceptions like `MyBusinessException` happen messages that triggered such an exception should be moved to error queue. And if for exceptions like `MyOtherBusinessException` the default Delayed Retries time increase should be always five seconds but for all other cases the Default Recoverability Policy should be applied then the code can look like the following:

snippet:PartiallyCustomizedPolicy

If the Default Recoverability Policy just needs to be tweaked for `MyBusinessException` then a policy might look like:

snippet:CustomExceptionPolicyHandler

If more control over Recoverability is desired the Recoverability delegate can be overriden completely.

### Full customization

snippet:FullyCustomizedPolicyRecoverabilityConfiguration

snippet:FullyCustomizedPolicy
