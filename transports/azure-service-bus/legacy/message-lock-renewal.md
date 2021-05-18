---
title: Message lock renewal
summary: Extending message lock for long-running operations
component: ASB
versions: '[7,)'
reviewed: 2021-05-18
related:
 - samples/azure-service-bus-netstandard/azure-service-bus-long-running
 - samples/azure-service-bus-netstandard/lock-renewal
redirects:
 - nservicebus/azure-service-bus/message-lock-renewal
 - transports/azure-service-bus/message-lock-renewal
---

include: legacy-asb-warning

When messages are received using `PeekLock` mode, the receive operation becomes a two-stage operation. In the first stage, the message is locked by the broker for a specific consumer for a fixed period known as `LockDuration`. When the consumer is done with the message, the message is marked as completed by the consumer, indicating to the broker that second stage is finished. Default lock duration for the Azure Service Bus transport is set to 30 seconds. Maximum lock duration allowed by the broker service is 5 minutes. `LockDuration` is a global setting that is applied to all entities.

Occasionally, processing can take longer than the maximum allowed time for `LockDuration`. As a result, messages will re-appear on the queue and will be available to other consumers. Azure Service Bus transport version 7 and above supports automatic message lock renewal.


## How message lock renewal works

Message lock renewal is applied only on messages actively processed by endpoints and is not applicable to prefetched messages.

Message lock renewal should be greater than `LockDuration`. When the `LockDuration` period is about to expire, the Azure Service Bus transport will issue a lock renewal request to the broker, keeping the message locked and invisible to other consumers. Lock renewal will automatically take place while the total time of the message processing stays less than the auto renewal time set by Azure Service Bus transport. The default lock renewal time is 5 minutes. Auto lock renewal will **not** increase `DeliveryCount` of the processed message.

Message lock renewal applies to the currently processed message. Prefetched messages that are not handled within the `LockDuration` time will lose their lock, indicated with a `LockLostException` in the log when they are completed by the transport. To prevent the exception with lock renewal prefetching can be disabled by setting `PrefetchCount` to zero. 

include: autorenewtimeout-warning


## Configuring message lock renewal

The configuration of message lock renewal is done on the `MessageReceivers` extension of the transport, specifying the maximum period that lock renewal should use.

snippet: asb-auto-lock-renewal

For example, setting lock renewal time to 10 minutes and `LockDuration` set to 1 minute will ensure that message will remain locked for up to 10 minutes, but no longer. If message processing exceeds 10 minutes, the message will become available to other consumers.
