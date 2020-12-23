---
title: Configuration
summary: Explains the configuration options
component: ASBS
reviewed: 2020-04-15
---

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
 
## Connectivity

These settings control how the transport connects to the broker.

 * `UseWebSockets()`: Configures the transport to use AMQP over websockets.
 * `TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan)`: The time to wait before triggering the circuit breaker after a critical error occurred. Defaults to 2 minutes.
 * `CustomTokenProvider(ITokenProvider)`: Allows replacement of the default token provider, which uses the shared secret in the connection string for authentication. This opens up additional authentication mechanisms such as [shared access signatures](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-sas), [SAML, Oauth, SWT, windows authentication](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.tokenprovider?view=azure-dotnet), [managed identities for Azure resources](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-managed-service-identity), or even custom implementations.
partial: custom-retry-policy