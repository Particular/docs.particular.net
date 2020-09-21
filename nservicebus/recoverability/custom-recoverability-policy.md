---
title: Custom Recoverability Policy
summary: Shows how to take full control over Recoverability by implementing a Recoverability Policy
component: Core
reviewed: 2020-01-29
versions: '[6.0,)'
related:
 - samples/faulttolerance
---


## Default Recoverability

The default Recoverability Policy is implemented in `DefaultRecoverabilityPolicy` class. It is publicly exposed in case the default recoverability behavior needs to be reused as part of a custom recoverability policy. The default policy takes into account the settings provided for Immediate Retries, Delayed Retries and the configured error queue. 

The default policy works in the following way:

 1. If an unrecoverable exception is raised, then the `MoveToError` action is returned immediately with the default error queue. 
 1. If the `ImmediateProcessingFailures` value is less or equal to the configured `MaxNumberOfRetries` for Immediate Retries, then the `ImmediateRetry` action is returned.
 1. If Immediate Retries are exceeded, then the Delayed Retries configuration is considered. If the `DelayedDeliveriesPerformed` is less than `MaxNumberOfRetries` and the message hasn't reached the maximum time allowed for retries (24 hours), then the `DelayedRetry` action is returned. The delay is calculated according to the following formula:

    `delay = DelayedRetry.TimeIncrease * (DelayedDeliveriesPerformed + 1)`.

 1. If `MaxNumberOfRetries` for both ImmediateRetries and DelayedRetries is exceeded, then the `MoveToError` action is returned with the default error queue.
 
NOTE: According to the default policy, only exceptions of type `MessageDeserializedException` are unrecoverable. It's possible to customize the policy and instruct NServiceBus to treat other types as unrecoverable exceptions. Refer to the [Unrecoverable exceptions](/nservicebus/recoverability/#unrecoverable-exceptions) section to learn more.


## Fallback

As outlined in the [Recoverability introduction](/nservicebus/recoverability/), Immediate and Delayed Retries can only be performed under certain conditions. If a custom Recoverability Policy returns a recoverability action which cannot be fulfilled by the infrastructure, the decision will be overridden with the `MoveToError` action with the default error queue. 

This behavior guarantees safety in edge cases and cannot be overridden.


## Recoverability Configuration

`RecoverabilityConfig` contains all the information required when a recoverability policy is implemented. This includes:

 * Maximum number of retries for Immediate Retries
 * Maximum number of retries for Delayed Retries
 * Time increase interval for delays in subsequent Delayed Retries executions
 * Configured error queue address
 * Exceptions that need to be treated as unrecoverable (NServiceBus Version 6.2 and above)

The information provided in the configuration is static and will not change between subsequent executions of the policy.

NOTE: In cases when Immediate and/or Delayed Retry capabilities have been turned off and/or are not available, the `MaxNumberOfRetries` exposed to recoverability policy will be set to 0 (zero).


## Error Context

`ErrorContext` provides all the information about the currently failing message. It contains the following information:

 * Exception which caused the message to fail
 * Transport transaction on which the message failed
 * Number of failed immediate processing attempts
 * Number of delayed deliveries performed
 * Message which failed


## Implement a custom policy

NOTE: New APIs were made available starting in version 6.2. The examples below show how to implement recovery customizations both prior to and after version 6.2. It is not necessary to implement both snippets for a given example.


### Partial customization

Sometimes only part of the default Recoverability Policy needs to be customized. In these situations, the `DefaultRecoverabilityPolicy` needs to be called in the customized Recoverability Policy delegate. 

For example, the following custom policy will move the message directly to an error queue without retries when a `MyBusinessException` triggers the policy:

snippet: CustomExceptionPolicyHandler

In the following example the default Recoverability Policy is tweaked to do three Immediate Retries and three Delayed Retries with a time increase of two seconds:

snippet: PartiallyCustomizedPolicyRecoverabilityConfiguration

In the following example, for exceptions of type `MyOtherBusinessException` the default Delayed Retries time increase will be always five seconds, in all other cases the Default Recoverability Policy will be applied:

snippet: PartiallyCustomizedPolicy

If more control over Recoverability is needed, the Recoverability delegate can be overridden completely.


### Full customization

If the Recoverability Policy is fully customized, then the `DefaultRecoverabilityPolicy` won't be called. In this case it is still possible to use the recoverability high level APIs, for example:

partial: fullconfig

Note that the `RecoverabilityConfig` will be passed into the custom policy so the code can be fine-tuned based on the configured values. 

NOTE: The custom error queue specified by `MoveToError` will not be created by NServiceBus and must be manually created.