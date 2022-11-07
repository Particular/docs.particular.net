---
title: Azure Service Bus Transport Upgrade Version 2 to 3
summary: Migration instructions on how to upgrade the Azure Service Bus transport from version 2 to 3
reviewed: 2021-11-07
component: ASBS
related:
 - transports/azure-service-bus
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

## Configuring the Azure Service Bus transport

To use the Azure Service Bus transport for NServiceBus, create a new instance of `AzureServiceBusTransport` and pass it to `EndpointConfiguration.UseTransport`.

Instead of:

```csharp
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
transport.ConnectionString(connectionString);
```

Use:

```csharp
var transport = new AzureServiceBusTransport(connectionString);
endpointConfiguration.UseTransport(transport);
```

include: v7-usetransport-shim-api

## Configuration options

The Azure Service Bus transport configuration options have been moved to the `AzureServiceBusTransport` class. See the following table for further information:

| Version 1 configuration option | Version 2 configuration option | Version 3 configuration option |
| --- | --- |
| TopicName | TopicName | TopicName |
| EntityMaximumSize | EntityMaximumSize | EntityMaximumSize |
| EnablePartitioning | EnablePartitioning | EnablePartitioning |
| PrefetchMultiplier | PrefetchMultiplier | PrefetchMultiplier |
| PrefetchCount | PrefetchCount | refetchCount |
| TimeToWaitBeforeTriggeringCircuitBreaker | TimeToWaitBeforeTriggeringCircuitBreaker | TimeToWaitBeforeTriggeringCircuitBreaker |
| SubscriptionNameShortener | SubscriptionNamingConvention | SubscriptionNamingConvention |
| SubscriptionNamingConvention | SubscriptionNamingConvention | SubscriptionNamingConvention |
| RuleNameShortener | SubscriptionRuleNamingConvention | SubscriptionRuleNamingConvention |
| SubscriptionRuleNamingConvention | SubscriptionRuleNamingConvention | SubscriptionRuleNamingConvention |
| UseWebSockets | UseWebSockets | UseWebSockets |
| CustomTokenCredential | TokenCredential | Overloaded constructor of the transport |
| CustomRetryPolicy | RetryPolicy |

### TokenCredential

Previously when using `CustomTokenCredential` or `TokenCredential` it was required to pass a fully-qualified namespace (e.g. `<asb-namespace-name>.servicebus.windows.net`) instead of a connection string (e.g. `Endpoint=sb://<asb-namespace-name>.servicebus.windows.net>;[...]`). The dual purpose of the connection-string option has been removed. In order to use `TokenCredential` pass the credential plus the fully-qualified namespace to the constructor of the transport.

snippet: token-credentials

## Accessing the native incoming message

The Azure.Messaging.ServiceBus client SDK introduces a set of new classes to represent messages. Previously there was only the `Message` class to represent either an incoming message or an outgoing message. With the new client SDK the incoming message type is `ServiceBusReceivedMessage`. If access to the native incoming message is required, make sure the correct type is used. See the [native message customization documentation](/transports/azure-service-bus/native-message-access.md) for further details.

## Native message customization

`IMessageHandlerContext` and `IPipelineContext` no longer need to be passed to the `CustomizeNativeMessage` method. See the [native message customization documentation](/transports/azure-service-bus/native-message-access.md) for further details.

## Auto-lock renewal

The auto-lock renewal is now supported for an extended period of time and can be customized by specifying `MaxAutoLockRenewalDuration`.

snippet: custom-auto-lock-renewal

### Transaction timeout

The transport has been changed allowing for transactions to take longer than the default [`TransactionManager.MaximumTimeout`](https://learn.microsoft.com/en-us/dotnet/api/system.transactions.transactionmanager.maximumtimeout). It is no longer required to [override the maximum timeout](/samples/azure-service-bus-netstandard/lock-renewal/?version=asbs_2#overriding-the-value-of-transactionmanager-maxtimeout).

### Advanced/custom lock renewal

Prior to the new lock renewal support in order to have longer than five minutes lock renewal it was required to write custom lock renewal handling as outlined by the [lock renewal sample](/samples/azure-service-bus-netstandard/lock-renewal/). While it is still possible to manually renew the lock as outlined in the sample it is encouraged to leverage the built in lock renewal. Existing custom lock renewal implementations are required to change from

```csharp
context.Extensions<ServiceBusReceiver>();
```

to

```csharp
context.Extensions<ProcessMessageEventArgs>();
```