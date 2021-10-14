---
title: Azure Service Bus Transport Upgrade Version 2 to 3
summary: Migration instructions on how to upgrade the Azure Service Bus transport from version 2 to 3.
reviewed: 2021-10-01
component: ASBS
related:
 - transports/azure-service-bus
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
---

## Configuring the Azure Service Bus transport

To use the Azure Service Bus transport for NServiceBus, create a new instance of `AzureServiceBusTransport` and pass it to `EndpointConfiguration.UseTransport`.

Instead of

```csharp
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
transport.ConnectionString(connectionString);
```

Use:

```csharp
var transport = new AzureServiceBusTransport(connectionString);
endpointConfiguration.UseTransport(transport);
```

## Configuration options

The Azure Service Bus Transport configuration options have been moved to the `AzureServiceBusTransport` class. See the following table for further information:

| Version 1 configuration option | Version 2 configuration option |
| --- | --- |
| TopicName | TopicName |
| EntityMaximumSize | EntityMaximumSize |
| EnablePartitioning | EnablePartitioning |
| PrefetchMultiplier | PrefetchMultiplier |
| PrefetchCount | PrefetchCount |
| TimeToWaitBeforeTriggeringCircuitBreaker | TimeToWaitBeforeTriggeringCircuitBreaker |
| SubscriptionNameShortener | SubscriptionNamingConvention |
| SubscriptionNamingConvention | SubscriptionNamingConvention |
| RuleNameShortener | SubscriptionRuleNamingConvention |
| SubscriptionRuleNamingConvention | SubscriptionRuleNamingConvention |
| UseWebSockets | UseWebSockets |
| CustomTokenProvider | TokenProvider |
| CustomRetryPolicy | RetryPolicy |

## Accessing the native incoming message

The new Azure.Messaging.ServiceBus client SDK introduces a set of new classes to represent messages. Previously there was only the `Message` class to represent either an incoming message or an outgoing message. With the new client SDK the incoming message type is `ServiceBusReceivedMessage`. In case access to the native incoming message is required, make sure the correct type is used. See the [native message customization documentation](/transports/azure-service-bus/native-message-access.md) for further details.

## Native message customization

`IMessageHandlerContext` and `IPipelineContext` no longer need to be passed to the `CustomizeNativeMessage` method. See the [native message customization documentation](/transports/azure-service-bus/native-message-access.md) for further details.
