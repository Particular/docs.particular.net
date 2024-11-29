---
title: Configuration
summary: Explains the configuration options
component: ASBS
reviewed: 2022-11-15
---

## Configuring an endpoint

To use Azure Service Bus as the underlying transport:

snippet: azure-service-bus-for-dotnet-standard

## Connectivity

These settings control how the transport connects to the broker.

partial: connectivity

partial: custom-retry-policy

partial: custom-token-credentials

## Entity creation

These settings control how the transport creates entities in the Azure Service Bus namespace.

> [!WARNING]
> Entity creation settings are applied only at creation time of the corresponding entities; they are not updated on subsequent startups.

partial: access-rights

partial: entity-topology

partial: entity-settings

partial: subscription-rule-customization

## Controlling the prefetch count

When consuming messages from the broker, throughput can be improved by having the consumer prefetch additional messages. The prefetch count is calculated by multiplying [maximum concurrency](/nservicebus/operations/tuning.md) by the prefetch multiplier. The default value of the multiplier is 10, but it can be changed by using the following:

snippet: custom-prefetch-multiplier

Alternatively, the whole calculation can be overridden by setting the prefetch count directly using the following:

snippet: custom-prefetch-count

To disable prefetching, prefetch count should be set to zero.

> [!NOTE]
> The lock duration for all prefetched messages starts as soon as they are fetched. To avoid `LockLostException`, ensure the lock-renewal duration is longer than the total time it takes to process all prefetched messages (i.e., message handler execution time multiplied by the prefetch count).
> In addition, it's important to consider the endpoint's scaling. If the prefetch count is high, the lock may deprive other endpoint instances of messages, rendering additional endpoint instances unnecessary.

## Lock-renewal

For all supported transport transaction modes (except `TransportTransactionMode.None`), the transport utilizes a peek-lock mechanism to ensure that only one instance of an endpoint can process a message. The default lock duration is set during entity creation. By default, the transport uses the SDK's default maximum auto lock renewal duration of 5 minutes.

To ensure smooth processing, it is recommended to configuring the `MaxAutoLockRenewalDuration` property to be greater than the longest running handler for the endpoint. This helps avoid `LockLostException` and ensures the message is properly handled by [the recoverability process](/nservicebus/recoverability/).

partial: lockrenewal

> [!NOTE]
> Message lock renewal is initiated by client code, not the broker. If the request to renew the lock fails after all the SDK built-in retries (e.g., due to connection loss), the lock won't be renewed, and the message will become unlocked and available for processing by competing consumers. Lock renewal should be treated as a best effort, not as a guaranteed operation.

> [!NOTE]
> If a message lock renewal is required, it may be worth checking the duration of the handlers, and see whether these can be optimised. In addition, it may be worth checking wither the prefetch count is too high, considering that all messages are locked on peek. This may indicate that too many messages are locked for which the processing exceeds the lock duration.
