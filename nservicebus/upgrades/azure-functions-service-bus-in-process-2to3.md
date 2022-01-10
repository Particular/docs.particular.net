---
title: Azure Functions (in-process) for Service Bus Upgrade Version 2 to 3
summary: How to upgrade Azure Functions (in-process) with Azure Service Bus from version 2 to 3
component: ASBFunctions
reviewed: 2021-12-16
related:
 - nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

## Update to Microsoft.Azure.WebJobs.Extensions.ServiceBus SDK Version 5

The dependency to Microsoft.Azure.WebJobs.Extensions.ServiceBus have been updated which means that the new Azure.Messaging.ServiceBus SDK is being used by the function host to receive messages. Should you have code accesses native SDK types make sure to read the [Microsoft Migration Guide](https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/servicebus/Azure.Messaging.ServiceBus/MigrationGuide.md).

## Manually invoking process message

Previously `IFunctionEndpoint` exposed a two different `Process` methods where the one accepting a `IMessageReceiver` would be the one that [processed the message in atomic sends with receive mode](/nservicebus/hosting/azure-functions-service-bus/#message-consistency).

Version 3 now exposes different metods for those two different use cases.

Use:

`Task ProcessAtomic(ServiceBusReceivedMessage message, ExecutionContext executionContext, ServiceBusClient serviceBusClient, ServiceBusMessageActions messageActions, ILogger functionsLogger = null);`

to process the message with atomic sends with receive.

Use:

`Task ProcessNonAtomic(ServiceBusReceivedMessage message, ExecutionContext executionContext, ILogger functionsLogger = null);`

to process the message in receive only mode.

## Injecting FunctionEndpoint

`FunctionEndpoint` can no longer be injected, use `IFunctionEndpoint` instead.
