---
title: Azure Service Bus Transport Upgrade Version 1 to 2
summary: Migration instructions on how to upgrade the Azure Service Bus transport from version 1 to 2.
reviewed: 2021-02-10
component: ASBS
related:
 - transports/azure-service-bus
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

## Support for Azure.Messaging.ServiceBus client SDK

This version of Azure Service Bus Transport uses the [Azure.Messaging.ServiceBus client SDK](https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/servicebus/Azure.Messaging.ServiceBus). As a result of that the following changes has been made.

### Configuration Options

The following calls has been removed from out Public API due to them using classes that were present in the previously used client SDK.

CustomRetryPolicy - this method was replaced with property `RetryPolicyOptions` in `AzureServiceBusTransport` class.  
CustomTokenProvider - this method was removed. In place of that a property `TokenCredential` in `AzureServiceBusTransport` class was introduced. 

## Usage of Azure.Identity 

Passing of `TokenCredential` allows use of [Azure.Identity](https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/identity/Azure.Identity/README.md) as a authentication mechanisms using Azure Active Directory.  

## Native message customization

`IMessageHandlerContext` and `IPipelineContext` no longer need to be passed to the `CustomizeNativeMessage` method. See the [native message customization documentation](/transports/azure-service-bus/native-message-access.md) for further details.