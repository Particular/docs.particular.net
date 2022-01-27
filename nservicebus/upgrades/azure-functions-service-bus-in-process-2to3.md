---
title: Azure Functions (in-process) for Service Bus Upgrade Version 2 to 3
summary: How to upgrade Azure Functions (in-process) with Azure Service Bus from version 2 to version 3
component: ASBFunctions
reviewed: 2021-12-16
related:
 - nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

## Update to Microsoft.Azure.WebJobs.Extensions.ServiceBus SDK version 5

The dependency on Microsoft.Azure.WebJobs.Extensions.ServiceBus has been updated which means that the new Azure.Messaging.ServiceBus SDK is being used by the function host to receive messages. If the solution contains code that directly accesses native SDK types, read the [Microsoft Migration Guide](https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/servicebus/Azure.Messaging.ServiceBus/MigrationGuide.md).

include: servicebus_options_enable_cross_entity_transactions

## Manually invoking process message

In version 2 of the Azure Functions package, `IFunctionEndpoint` exposes two different `Process` methods where the one accepting a `IMessageReceiver` is the one that [processed the message in "atomic sends with receive" mode](/nservicebus/hosting/azure-functions-service-bus/#message-consistency).

Version 3 exposes different methods for the two different use cases.

Use:

`Task ProcessAtomic(ServiceBusReceivedMessage message, ExecutionContext executionContext, ServiceBusClient serviceBusClient, ServiceBusMessageActions messageActions, ILogger functionsLogger = null);`

to process the message with "atomic sends with receive" transaction mode.

Use:

`Task ProcessNonAtomic(ServiceBusReceivedMessage message, ExecutionContext executionContext, ILogger functionsLogger = null);`

to process the message in "receive only" transaction mode.

## Injecting FunctionEndpoint

`FunctionEndpoint` can no longer be injected, use `IFunctionEndpoint` instead.
