---
title: Configuration
summary: Explains the configuration options
component: ASBS
tags:
 - Azure
reviewed: 2018-06-27
---

## Entity Creation

Settings to control how the transport creates entities in the Azure Service Bus namespace.

WARNING: Entity creation settings are only applied at creation time of the corresponding entities, they are not updated on subsequent startups.

 * `TopicName(string)`: The name of the topic used to publish events between endpoints. This topic is shared by all endpoints, so ensure all endpoints configure the same topic name. Defaults to `bundle-1`. Topic names must adhere to the limits outlined in [the Microsoft documentation on topic creation](https://docs.microsoft.com/en-us/rest/api/servicebus/create-topic).
 * `EntityMaximumSize(int)`: The maximum entity size in GB. The value must correspond to a valid value for the namespace type. Defaults to 5. See [the Microsoft documentation on quota's and limits](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quotas) for valid values.
 * `EnablePartitioning()`: Partitioned entities offer higher availability, reliability and throughput over conventional non-partitioned queues and topics. For more information about partitioned entities [see the Microsoft documentation on Partitioned Messaging Entities](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-partitioning).
 * `SubscriptionNameShortener(Func<string, string>)`: By default subscription names are derived from the endpoint name, which may exceed the maximum length of subscription names. This callback allows to provide a replacement name for the subscription. Subscription names must adhere to the limits outlined in [the Microsoft documentation on subscription creation](https://docs.microsoft.com/en-us/rest/api/servicebus/create-subscription).
 * `RuleNameShortener(Func<string, string>)`: By default rule names are derived from the message type's full name, which may exceed the maximum length of rule names. This callback allows to provide a replacement name for the rule. Rule names must adhere to the limits outlined in [Service Bus quotas](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quotas).
 
## Receiving

Settings to control the speed at which the transport receives messages. 

 * `PrefetchMultiplier(int)`: Specifies the multiplier to apply to the maximum concurrency value to calculate the prefetch count. Defaults to 10.
 * `PrefetchCount(int)`: Overrides the default prefetch count calculation with the specified value.
 
## Connectivity

Settings to control how the transport connects to the broker

 * `UseWebSockets()`: Configures the transport to use AMQP over websockets.
 * `TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan)`: The time to wait before triggering the circuit breaker after a critical error occurred. Defaults to 2 minutes.
 * `CustomTokenProvider(ITokenProvider)`: Allows to replace the default token provider, which uses the shared secret in the connectionstring for authentication. This opens up additional authentication mechanisms such as [shared access signatures](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-sas), [SAML, Oauth, SWT, windows authentication](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.tokenprovider?view=azure-dotnet) or even custom implementations.
