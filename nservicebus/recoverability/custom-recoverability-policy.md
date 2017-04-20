---
title: Custom Recoverability Policy
summary: Shows how to take full control over Recoverability by implementing a Recoverability Policy
component: Core
reviewed: 2016-08-23
versions: '[6.0,)'
tags:
 - Error Handling
 - Exceptions
 - Retry
 - Recoverability
related:
 - samples/faulttolerance
---


## Default Recoverability

The default Recoverability Policy is implemented in `DefaultRecoverabilityPolicy` and publicly exposed for use cases when the default recoverability behavior needs to be reused as part of a custom recoverability policy. The default policy takes into account the settings provided for Immediate Retries, Delayed Retries and the configured error queue. The default policy works like the following:

 1. If an unrecoverable exception is raised, then the `MoveToError` action is returned immediately with the default error queue. 
 1. If the `ImmediateProcessingFailures` value is less or equal to the configured `MaxNumberOfRetries` for Immediate Retries, then the `ImmediateRetry` action is returned.
 1. If Immediate Retries are exceeded, then the Delayed Retries configuration is considered. If the `DelayedDeliveriesPerformed` is less or equal than `MaxNumberOfRetries` and the message hasn't reached the maximum time allowed for retries (24 hours), then the `DelayedRetry` action is returned. The delay is calculated according to the following formula:

    `delay = DelayedRetry.TimeIncrease * (DelayedDeliveriesPerformed + 1)`.

 1. If `MaxNumberOfRetries` for both ImmediateRetries and DelayedRetries is exceeded, then the `MoveToError` action is returned with the default error queue.
 
NOTE: According to the default policy, only exceptions of type `MessageDeserializedException` are unrecoverable. It's possible to customize the policy and instruct NServiceBus to treat other types as unrecoverable exceptions. Refer to the [Unrecoverable exceptions](/nservicebus/recoverability/#unrecoverable-exceptions) section to learn more.


## Fallback

As outlined in the [Recoverability introduction](/nservicebus/recoverability/) Immediate and Delayed Retries can only be performed under certain conditions. If a custom Recoverability Policy returns a recoverability action which cannot be fulfilled by the infrastructure, the decision will be overridden with the `MoveToError` recoverability action with the default error queue. This behavior is for safety reasons and cannot be overridden.


## Recoverability Configuration

`RecoverabilityConfig` contains all required information to take into account when a recoverability policy is implemented. It provides the following information that has been provided via configuration:

 * Maximum number of retries for Immediate Retries
 * Maximum number of retries for Delayed Retries
 * Time of increase for individual Delayed Retries
 * Configured error queue address
 * Exceptions that need to be treated as unrecoverable (as of NServiceBus version 6.2)

The information provided on the configuration is static and will not change between subsequent executions of the policy.

NOTE: In cases when Immediate and/or Delayed Retry capabilities have been turned off and/or are not available, MaxNumberOfRetries exposed to recoverability policy will be set to 0 (zero).


## Error Context

`ErrorContext` contains all required information to take into account regarding the currently failing message. It provides the following information:

 * Exception which caused the message to fail
 * Transport transaction on which the message failed
 * Number of failed immediate processing attempts
 * Number of delayed deliveries performed
 * Message which failed


## Implement a custom policy


### Partial customization

Sometimes only a partial customization of the default Recoverability Policy is desired. In order to achieve partial customization the `DefaultRecoverabilityPolicy` needs to be called in the customized Recoverability Policy delegate. If, when certain exceptions like `MyBusinessException` happen messages that triggered, such an exception should be moved to error queue then a policy might look like:

snippet: CustomExceptionPolicyHandler

In the following example the default Recoverability Policy is tweaked to do three Immediate Retries and three Delayed Retries with a time increase of two seconds. The configuration looks like the following:

snippet: PartiallyCustomizedPolicyRecoverabilityConfiguration

And if for exceptions like `MyOtherBusinessException`, in addition to moving `MyBusinessException` directly to the error queue, the default Delayed Retries time increase should be always five seconds but for all other cases the Default Recoverability Policy should be applied then the code can look like the following:

snippet: PartiallyCustomizedPolicy

If more control over Recoverability is desired the Recoverability delegate can be overridden completely.

### Full customization

Fully customizing the Recoverability Policy basically means not calling `DefaultRecoverabilityPolicy`. It is still possible to use the recoverability high level APIs like shown below:

snippet: FullyCustomizedPolicyRecoverabilityConfiguration

The configuration will be passed into the custom policy. 

The following custom policy:

 * for unrecoverable exceptions such as `MyBusinessException` immediately moves failed messages to a custom error queue 
 * for `MyOtherBusinessException` does Delayed Retries with a constant time increase of five seconds 
 * for all other cases immediately moves failed messages to the configured standard error queue.

snippet: FullyCustomizedPolicy
