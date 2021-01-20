---
title: Using NServiceBus in Azure Functions with Storage Queue triggers
reviewed: 2020-11-10
component: ASQFunctions
related:
 - previews/azure-functions-storage-queues
---

This sample shows how to host NServiceBus within an Azure Function, in this case, a function triggered by an incoming Storage Queues message. This enables hosting message handlers in Azure Functions, gaining the abstraction of message handlers implemented using `IHandleMessages<T>` and also taking advantage of NServiceBus's extensible message processing pipeline.

The sample demonstrates two configuration approaches that achieve the same outcome:
1. Integrating with the `IFunctionHostBuilder`, including the host managed DI container.
2. Configuring the endpoint inside the trigger class as a static field.

When hosting NServiceBus within Azure Functions, each Function (as identified by the `[FunctionName]` attribute) hosts an NServiceBus endpoint that is capable of processing multiple different message types.

The Azure Functions SDK enforces certain constraints that are also applied to NServiceBus endpoints. Review these [constraints](/previews/azure-functions-storage-queues.md) before running the sample.

downloadbutton

## Prerequisites

Unlike a traditional NServiceBus endpoint, an endpoint hosted in Azure Functions cannot create its own input queue. In this sample, that queue name is `ASQTriggerQueue`.

To create the queue with the Azure CLI, execute the following [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) command:

```
 az storage queue create --name ASQTriggerQueue --connection-string "<storage-account-connection-string>"
```

To use the sample, a valid Queue Storage connection string must be provided in the `local.settings.json` file.

## Sample structure

The sample contains the following projects:
- `AzureFunctions.ASQTrigger.FunctionsHostBuilder` - Using the `IFunctionHostBuilder` approach to host the NServiceBus endpoint
- `AzureFunctions.ASQTrigger.Static` - Using a static approach to host the NServiceBus endpoint

NOTE: `AzureFunctions.ASQTrigger.FunctionsHostBuilder` and `AzureFunctions.ASQTrigger.Static` are both using the same trigger queue and should not be executed simultaneously. 

## Running the sample

Each Functions project contains two functions:
1. Storage Queue-triggered function.
1. HTTP-triggered function.

Running the sample will launch the **Azure Functions runtime** window.

To try the Azure Function:

1. Open a browser and navigate to `http://localhost:7071/api/HttpSender`. The port number might be different and will be indicated when the function project is started.
1. The queue-triggered function will receive the `TriggerMessage` and process it with NServiceBus.
1. The NServiceBus message handler for `TriggerMessage` sends a `FollowUpMessage`.
1. The queue-triggered function will receive the `FollowUpMessage` and process it with NServiceBus.

## Code walk-through

### `IFunctionHostBuilder` approach

The NServiceBus endpoint configured using `IFunctionHostBuilder` is using the convention and is wired using `Startup` class like this:

snippet: configuration-with-function-host-builder

`IFunctionEndpoint` is then injected into the function class:

snippet: endpoint-injection

It is invoked in the following manner:

snippet: injected-function

### Static approach

The static NServiceBus endpoint must be configured using details that come from the Azure Functions `ExecutionContext`. Since that is not available until a message is handled by the function, the NServiceBus endpoint instance is deferred until the first message is processed, using a lambda expression like this:

snippet: EndpointSetup

The same class defines the Azure Function which makes up the hosting for the NServiceBus endpoint. The Function hands off processing of the message to NServiceBus:

snippet: Function

Meanwhile, the message handlers for `TriggerMessage` and `FollowUpMessage`, also hosted within the Azure Functions project, are normal NServiceBus message handlers, which are also capable of sending messages themselves.

### Handlers

Both approaches use the same message handlers, with a `CustomDependency` passed in.

snippet: TriggerMessageHandler

snippet: FollowupMessageHandler
