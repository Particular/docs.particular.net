---
title: Using NServiceBus in Azure Functions with Service Bus triggers
component: ASBFunctions
related:
 - nservicebus/hosting/azure-functions/service-bus
redirects:
 - samples/previews/azure-functions/service-bus
reviewed: 2021-04-19
---

This sample shows how to host NServiceBus within an Azure Function, in this case, a function triggered by an incoming Service Bus message. This enables hosting message handlers in Azure Functions, gaining the abstraction of message handlers implemented using `IHandleMessages<T>` and also taking advantage of NServiceBus's extensible message processing pipeline.

The sample demonstrates two configuration approaches that achieve the same outcome:
1. Integrating with the `IFunctionHostBuilder`, including the host managed DI container.
2. Configuring the endpoint inside the trigger class as a static field.

When hosting NServiceBus within Azure Functions, each Function (as identified by the `[FunctionName]` attribute) hosts an NServiceBus endpoint that is capable of processing different message types.

The Azure Functions SDK enforces certain constraints that are also applied to NServiceBus endpoints. Review these [constraints](/nservicebus/hosting/azure-functions/service-bus.md) before running the sample.

downloadbutton

## Prerequisites

Unlike a traditional NServiceBus endpoint, an endpoint hosted in Azure Functions cannot create its own input queue. In this sample, that queue name is `ASBTriggerQueue`.

To create the queue with the Azure CLI, execute the following [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) command:

```
az servicebus queue create --name ASBTriggerQueue --namespace-name <asb-namespace-to-use> --resource-group <resource-group-containing-namespace>
```

To use the sample, a valid Service Bus connection string must be provided in the `local.settings.json` file.

## Sample structure

The sample contains the following project:
- `AzureFunctions.ASBTrigger.FunctionsHostBuilder` - NServiceBus endpoint
- `AzureFunctions.Messages` - message definitions

NOTE: `AzureFunctions.ASBTrigger.FunctionsHostBuilder` and `AzureFunctions.ASBTrigger.Static`are both using the same trigger queue and should not be executed simultaneously. 

## Running the sample

Each Functions project contains two functions:
1. Service Bus-triggered function.
1. HTTP-triggered function.

Running the sample will launch the **Azure Functions runtime** window.

To try the Azure Function:

1. Open a browser and navigate to `http://localhost:7071/api/HttpSender`. The port number might be different and will be indicated when the function project is started.
1. The queue-triggered function will receive the `TriggerMessage` and process it with NServiceBus.
1. The NServiceBus message handler for `TriggerMessage` sends a `FollowUpMessage`.
1. The queue-triggered function will receive the `FollowUpMessage` and process it with NServiceBus.

## Code walk-through

The NServiceBus endpoint configured using `IFunctionHostBuilder` is using the convention and is wired using `Startup` class like this:

snippet: configuration-with-function-host-builder

`IFunctionEndpoint` is then injected into the function class:

snippet: endpoint-injection

And is invoked in the following manner:

snippet: injected-function

### Handlers

Both approaches use the same message handlers, with a `CustomDependency` passed in.

snippet: TriggerMessageHandler

snippet: FollowupMessageHandler
