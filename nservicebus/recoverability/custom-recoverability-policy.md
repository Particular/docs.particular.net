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
4. If ImmediateRetries and DelayedRetries are exceeded, the `MoveToError`r recoverability action is returned with the default error queue.

## Fallback

As outlined in the [Recoverability introduction](/nservicebus/recoverability/) Immediate and Delayed Retries can only be performed under certain conditions. If a custom Recoverability Policy returns a recoverability action which cannot be fulfilled by the infrastructure, the decision will be overriden with the `MoveToError` recoverability action with the default error queue. This behavior is for safety reasons and cannot be overriden.




With the new recoverability override starting from Version 6 and above it is possible to take full control over the recoverability behavior. For example the above custom SLR policy will always to first level retries even for business exceptions. That doesn't have to be like that. It is possible to disable first level retries entirely for business exceptions like shown below:

snippet:CustomExceptionPolicyHandler
