---
title: Unit Testing a custom recoverability policy
summary: Writing unit tests for custom recoverability policies
reviewed: 2020-10-09
component: Core
related:
- nservicebus/recoverability
- nservicebus/recoverability/custom-recoverability-policy
- nservicebus/testing
---

This sample demonstrates how to test custom recoverability policies. The sample uses the policy as explained in the [custom recoverability policy documentation](/nservicebus/recoverability/custom-recoverability-policy.md#implement-a-custom-policy-full-customization).

## Creating the policy

Any recoverability policy with NServiceBus requires a `RecoverabilityConfig` which contains information about immediate and delayed retries and about what happens when all retries have been exhausted. This information is configured with an `EndpointConfiguration` and provided by NServiceBus to the recoverability policy. This behavior can be imitated and added to a helper method:

snippet: create-policy

Executing the policy requires an error context, which provides information about the error that occurred and how many retries have already been executed. The sample also includes a helper method that creates the error context.

snippet: create-error-context

## Creating a test

Multiple tests can then be created using the helper methods. According to the custom recoverability policy, messages that cause a certain custom exception should be discarded and not moved to the error queue.

snippet: test-example

There are additional tests in the sample that show how it is possible to verify which error queue a message was sent to.
