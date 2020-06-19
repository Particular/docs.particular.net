---
title: Using NServiceBus in Azure Functions with Storage Queue triggers
reviewed: 2020-06-15
component: ASQFunctions
related:
 - samples/previews/azure-functions/service-bus
---

This sample shows how to host NServiceBus within an Azure Function, in this case, a function triggered by an incoming Storage Queues message. This enables hosting message handlers in Azure Functions, gaining the abstraction of message handlers implemented using `IHandleMessages<T>` and also taking advantage of NServiceBus's extensible message processing pipeline.

When hosting NServiceBus within Azure Functions, each Function (as identified by the `[FunctionName]` attribute) hosts an NServiceBus endpoint that is capable of processing multiple different message types.

The Azure Functions SDK enforces certain constraints that are also applied to NServiceBus endpoints. Review these [constraints](/nservicebus/hosting/azure-functions/) before running the sample.

downloadbutton

## Prerequisites

Unlike a traditional NServiceBus endpoint, an endpoint hosted in Azure Functions cannot create its own input queue. In this sample, that queue name is `ASQTriggerQueue`.

To create the queue with the Azure CLI, execute the following [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) command:

```
 az storage queue create --name ASQTriggerQueue --connection-string "<storage-account-connection-string>"
```

To use the sample, a valid storage account connection string must be configured in  2 locations:

* `AzureFunctions.Sender/local.settings.json`
* `AzureFunctions.ASQTrigger/local.settings.json`

## Running the sample

Running the sample should launch two console windows:

* **AzureFunctions.Sender** is a console application that will send a `TriggerMessage` to the `ASQTriggerQueue` queue, which is monitored by the Azure Function.
* The **Azure Functions runtime** window will receive messages from the `ASQTriggerQueue` queue and process them using the Azure Functions runtime.

To try the Azure Function:

1. From the **AzureFunctions.Sender** window, press <kbd>Enter</kbd> to send a `TriggerMessage` to the trigger queue.
1. The Azure Function will receive the `TriggerMessage` and process it with NServiceBus.
1. The NServiceBus message handler for `TriggerMessage` sends a `FollowUpMessage`.
1. The Azure Function will receive the `FollowUpMessage` and process it with NServiceBus.

## Code walk-through

The static NServiceBus endpoint must be configured using details that come from the Azure Functions `ExecutionContext`. Since that is not available until a message is handled by the function, the NServiceBus endpoint instance is deferred until the first message is processed, using a lambda expression like this:

snippet: EndpointSetup

The same class defines the Azure Function which makes up the hosting for the NServiceBus endpoint. The Function hands off processing of the message to NServiceBus:

snippet: Function

Meanwhile, the message handlers for `TriggerMessage` and `FollowUpMessage`, also hosted within the Azure Functions project, are normal NServiceBus message handlers, which are also capable of sending messages themselves.

snippet: TriggerMessageHandler

snippet: FollowupMessageHandler

