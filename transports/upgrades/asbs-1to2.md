---
title: Azure Service Bus Transport Upgrade Version 1 to 2
reviewed: 2021-02-10
component: ASBS
related:
 - transports/azure-service-bus
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
---

## Configuring Azure Service Bus Transport

To use Azure Service Bus Transport for NServiceBus, create a new instance `AzureServiceBusTransport` and pass it to `EndpointConfiguration.UseTransport`.

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

## Configuration Options

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

## Native Message Customization

`IMessageHandlerContext` and `IPipelineContext` do no longer need to be passed to the `CustomizeNativeMessage` method. See the [native message customization documentation](/transports/azure-service-bus/native-message-access.md) for further detail.