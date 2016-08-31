---
title: Configure delayed retries
summary: Shows how to configure delayed retries which happens as a second stage of recoverability.
component: Core
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

WARNING: Delayed Retries cannot be used when transport transactions are disabled or Delayed Delivery is not available. For more information about transport transactions, refer to [transport transaction](/nservicebus/transports/transactions.md). For more information about delayed delivery, refer to [delayed-delivery](/nservicebus/messaging/delayed-delivery.md#caveats).

partial: config


## Disabling Delayed Retries through code

snippet:DisableDelayedRetries


## Custom Retry Policy

Custom retry logic can be configured based on headers or timing in code.


### Applying a custom policy

snippet:DelayedRetriesCustomPolicy


### Simple Policy

The following retry policy that will retry a message 3 times with a 5 second interval.

snippet:DelayedRetriesCustomPolicyHandler


### Exception based Policy

The following retry policy extends the previous policy with a custom handling logic for a specific exception.

snippet:DelayedRetriesCustomExceptionPolicyHandler

### Legacy .Retries message receiver

In NServiceBus Versions 5 and below, the [Delayed Retries](/nservicebus/recoverability/#delayed-retries) of NServiceBus used the `[endpoint_name].Retries` queue for persistent storage of messages to be retried. To prevent message loss when upgrading in Version 6, a dedicated .retries queue receiver is started if not explicity disabled. It serves a purpose of forwarding messages from the `.retries` queue to the endpoint's main queue to be retried appropriately. 

NOTE: The receiver is needed only during the upgrade from Version 5 and below and is not needed for new endpoints using Version 6. For details on upgrade process and how to safely disable the receiver please refer to: [Version 5 Upgrade Guide](/nservicebus/upgrades/5to6-recoverability#legacy-.retries-queue).  

Letting the receiver run might have negative performance implications depending on the transport. For endpoints using [SqlServer](/nservicebus/sqlserver) or [Msmq](/nservicebus/msmq) it will result in periodic polling to check for messages in the .retries queue.  

The `.Retries` can be disabled via code using:

snippet: 5to6-DisableLegacyRetriesSatellite 

INFO: This configuration API will be obsoleted and removed in Version 7.

