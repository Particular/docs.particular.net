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

WARNING: Entity creation settings are applied only at creation time of the corresponding entities; they are not updated on subsequent startups.

partial: entity-topology

partial: entity-settings

partial: subscription-rule-customization

## Controlling the prefetch count

When consuming messages from the broker, throughput can be improved by having the consumer prefetch additional messages. The prefetch count is calculated by multiplying [maximum concurrency](/nservicebus/operations/tuning.md) by the prefetch multiplier. The default value of the multiplier is 10, but it can be changed by using the following:

snippet: custom-prefetch-multiplier

Alternatively, the whole calculation can be overridden by setting the prefetch count directly using the following:

snippet: custom-prefetch-count

To disable prefetching, prefetch count should be set to zero.

## Lock-renewal

For all supported transport transaction modes (expect `TransportTransactionMode.None`), the transport uses a peek-lock mechanism to ensure a single instance of an endpoint can process a message. The default lock duration is specified during the entity creation. By default, the transport uses the SDK's default maximum auto lock renewal duration of 5 minutes.

partial: lockrenewal

NOTE: Message lock renewal is initiated by client code, not the broker. If the request to renew the lock fails after all the SDK built-in retries (.e.g due to a connection-loss), the lock won't be renewed, and the message will become unlocked and available for processing by competing consumers. Lock renewal should be treated as best-effort and not as a guaranteed operation.