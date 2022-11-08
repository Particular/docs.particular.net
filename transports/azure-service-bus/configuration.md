---
title: Configuration
summary: Explains the configuration options
component: ASBS
reviewed: 2020-04-15
---

## Configuring an endpoint

To use Azure Service Bus as the underlying transport:

snippet: azure-service-bus-for-dotnet-standard

## Connectivity

These settings control how the transport connects to the broker.

* `UseWebSockets()`: Configures the transport to use AMQP over websockets.
* `TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan)`: The time to wait before triggering the circuit breaker after a critical error occurred. Defaults to 2 minutes.

partial: custom-retry-policy

partial: custom-token-credentials

## Entity creation

These settings control how the transport creates entities in the Azure Service Bus namespace.

WARNING: Entity creation settings are applied only at creation time of the corresponding entities; they are not updated on subsequent startups.

* `TopicName(string)`: The name of the topic used to publish events between endpoints. This topic is shared by all endpoints, so ensure all endpoints configure the same topic name. Defaults to `bundle-1`. Topic names must adhere to the limits outlined in [the Microsoft documentation on topic creation](https://docs.microsoft.com/en-us/rest/api/servicebus/create-topic).
* `EntityMaximumSize(int)`: The maximum entity size in GB. The value must correspond to a valid value for the namespace type. Defaults to 5. See [the Microsoft documentation on quotas and limits](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quotas) for valid values.
* `EnablePartitioning()`: Partitioned entities offer higher availability, reliability, and throughput over conventional non-partitioned queues and topics. For more information about partitioned entities [see the Microsoft documentation on partitioned messaging entities](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-partitioning).
partial: subscription-rule-customization

## Controlling the prefetch count

When consuming messages from the broker, throughput can be improved by having the consumer prefetch additional messages. The prefetch count is calculated by multiplying [maximum concurrency](/nservicebus/operations/tuning.md#tuning-concurrency) by the prefetch multiplier. The default value of the multiplier is 10, but it can be changed by using the following:

snippet: custom-prefetch-multiplier

Alternatively, the whole calculation can be overridden by setting the prefetch count directly using the following:

snippet: custom-prefetch-count

To disable prefetching, prefetch count should be set to zero.

## Lock-renewal

For all supported transport transaction modes (expect `TransportTransactionMode.None`), the transport uses a peek-lock mechanism to ensure a single instance of an endpoint can process a message. The default lock duration is specified during the entity creation. By default, the transport uses the SDK's default maximum auto lock renewal duration of 5 minutes.

partial: lockrenewal

NOTE: Message lock renewal is initiated by client code, not the broker. If the request to renew the lock fails after all the SDK built-in retries (.e.g due to a connection-loss), the lock won't be renewed, and the message will become unlocked and available for processing by competing consumers. Lock renewal should be treated as best-effort and not as a guaranteed operation.