---
title: Configuration
summary: Explains the configuration options
component: ASBS
reviewed: 2025-03-21
related:
 - transports/azure-service-bus/operational-scripting
 - samples/azure-service-bus-netstandard/options
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
> Entity creation settings are applied only at the time the corresponding entities are created; they are not updated on subsequent startups.

include: managed-access-rights

partial: entity-topology

partial: entity-hierarchy

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
> In addition, it's important to consider how endpoints are scaled. If the prefetch count is high, the lock may deprive other endpoint instances of messages, making those instances redundant.

## Lock-renewal

For all supported transport transaction modes (except `TransportTransactionMode.None`), the transport utilizes a peek-lock mechanism to ensure that only one instance of an endpoint can process a message. The default lock duration is set during entity creation. By default, the transport uses the SDK's default maximum auto lock renewal duration of 5 minutes.

To ensure smooth processing, it is recommended to configuring the `MaxAutoLockRenewalDuration` property to be greater than the longest running handler for the endpoint. This helps avoid `LockLostException` and ensures the message is properly handled by [the recoverability process](/nservicebus/recoverability/).

partial: lockrenewal

> [!NOTE]
> Message lock renewal is initiated by client code, not the broker. If a request to renew a lock fails after all the SDK built-in retries (e.g., due to connection loss), the lock won't be renewed, and the message will become unlocked and available for processing by competing consumers. Lock renewal should be treated as a best effort, not as a guaranteed operation.

> [!NOTE]
> The following approaches may be considered to minimize or avoid the occurrence of message lock renewals:
>
> - Optimise the message handlers to reduce their execution time.
> - Reduce the prefetch count. All messages are locked on peek, so when they are prefetched, they remain locked until they are all processed.
