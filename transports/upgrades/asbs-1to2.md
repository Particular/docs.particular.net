---
title: Azure Service Bus Transport Upgrade Version 1 to 2
summary: Migration instructions on how to upgrade the Azure Service Bus transport from version 1 to 2.
reviewed: 2022-03-25
component: ASBS
related:
 - transports/azure-service-bus
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

## Support for Azure.Messaging.ServiceBus client SDK

This version of Azure Service Bus Transport uses the [Azure.Messaging.ServiceBus client SDK](https://www.nuget.org/packages/Azure.Messaging.ServiceBus). As a result the following changes to the configuration API have been made:

- `CustomRetryPolicy(...)` - accepts the new [`ServiceBusRetryOptions`](https://docs.microsoft.com/en-us/dotnet/api/azure.messaging.servicebus.servicebusretryoptions) SDK type
- `CustomTokenProvider(...)` - renamed to `CustomTokenCredential` and accepts a [`TokenCredential`](https://docs.microsoft.com/en-us/dotnet/api/azure.core.tokencredential) object

## Support for Azure.Identity

Passing a [Azure.Identity](https://www.nuget.org/packages/Azure.Identity/) `TokenCredential` to `CustomTokenCredential(TOKEN)` enables authentication against [Microsoft Entra ID](https://azure.microsoft.com/en-us/services/active-directory).

## Accessing the native incoming message

The new SDK uses specific types for incoming and outgoing messages while the old SDK had a single `Message` to represent both. The incoming message type is [`ServiceBusReceivedMessage`](https://docs.microsoft.com/en-us/dotnet/api/azure.messaging.servicebus.servicebusreceivedmessage) and the outgoing type is [`ServiceBusMessage`](https://docs.microsoft.com/en-us/dotnet/api/azure.messaging.servicebus.servicebusmessage).

See the [native message customization documentation](/transports/azure-service-bus/native-message-access.md) for further details.

## Native message customization

`IMessageHandlerContext` and `IPipelineContext` are no longer needed to be passed to the `CustomizeNativeMessage` method. See the [native message customization documentation](/transports/azure-service-bus/native-message-access.md) for further details.
