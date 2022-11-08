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

| Version 2 configuration option | Version 3 configuration option
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
| TokenCredential | Overloaded constructor of the transport |
| CustomRetryPolicy | RetryPolicy |

### TokenCredential

Previously when using `CustomTokenCredential` or `TokenCredential` it was required to pass a fully-qualified namespace (e.g. `<asb-namespace-name>.servicebus.windows.net`) instead of a connection string (e.g. `Endpoint=sb://<asb-namespace-name>.servicebus.windows.net>;[...]`). The dual purpose of the connection-string option has been removed. To use `TokenCredential` pass the credential plus the fully-qualified namespace to the constructor of the transport.

snippet: token-credentials

## Accessing the native incoming message

The Azure.Messaging.ServiceBus client SDK introduces a set of new classes to represent messages. Previously there was only the `Message` class to represent either an incoming message or an outgoing message. With the new client SDK the incoming message type is `ServiceBusReceivedMessage`. If access to the native incoming message is required, make sure the correct type is used. See the [native message customization documentation](/transports/azure-service-bus/native-message-access.md) for further details.

## Native message customization

`IMessageHandlerContext` and `IPipelineContext` no longer need to be passed to the `CustomizeNativeMessage` method. See the [native message customization documentation](/transports/azure-service-bus/native-message-access.md) for further details.

## Auto-lock renewal

The auto-lock renewal is now supported for an extended period of time and can be customized by specifying `MaxAutoLockRenewalDuration`.

snippet: custom-auto-lock-renewal

### Transaction timeout

The transport now allows transactions to take longer than the default [`TransactionManager.MaximumTimeout`](https://learn.microsoft.com/en-us/dotnet/api/system.transactions.transactionmanager.maximumtimeout). It is no longer required to [override the maximum timeout](/samples/azure-service-bus-netstandard/lock-renewal/?version=asbs_2#overriding-the-transactionmanager-maxtimeout-value).

### Advanced/custom lock renewal

Previous versions of the transport required custom lock renewal logic to extend the renewal beyond five minutes, as outlined by the [lock renewal sample](/samples/azure-service-bus-netstandard/lock-renewal/). While this scenario still works as expected, it is encouraged to leverage the built-in lock renewal mechanism. Existing custom lock renewal implementations are required to change from

```csharp
context.Extensions<ServiceBusReceiver>();
```

to

```csharp
context.Extensions<ProcessMessageEventArgs>();
```