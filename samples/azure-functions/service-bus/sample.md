---
title: Using NServiceBus in Azure Functions
summary: Using NServiceBus with Azure Service Bus triggers and Azure Functions hosting
component: AzureFunctions
related:
  - nservicebus/hosting/azure/functions
reviewed: 2026-05-25
---

This sample shows how to host NServiceBus in an Azure Functions app using the `NServiceBus.AzureFunctions.AzureServiceBus` package. A Service Bus-triggered function receives messages and dispatches them through the NServiceBus pipeline, while an HTTP-triggered function uses a send-only endpoint to start the conversation.

downloadbutton

## Prerequisites

### Configure connection string

To use the sample, provide a valid Azure Service Bus connection string in the `local.settings.json` file.

## Sample structure

The sample contains the following projects:

- `AzureFunctions.ServiceBus` - The Azure Functions host, the NServiceBus endpoint, and the HTTP-triggered sender
- `AzureFunctions.Messages` - Message definitions

## Running the sample

The Functions project contains two user-authored functions and one generated trigger method:

1. `Orders` is the Service Bus-triggered function endpoint.
1. `HttpSender` is an HTTP-triggered function that sends a `TriggerMessage` through a send-only endpoint.
1. The source generator emits the body for the partial `Orders` method and forwards incoming messages into NServiceBus.

Running the sample launches the Azure Functions runtime.

To try the sample:

1. Open a browser and navigate to `http://localhost:7071/api/HttpSender`. The port number might be different and will be indicated when the function project starts.
1. The HTTP-triggered function sends a `TriggerMessage` to the `Orders` endpoint.
1. The `TriggerMessageHandler` processes the message and sends a `FollowupMessage`.
1. The `FollowupMessageHandler` processes the follow-up message.

## Code walk-through

The Functions host is configured as follows:

snippet: service-bus-program-builder

The endpoint is defined with `[NServiceBusFunction]` and configured in `ConfigureOrders`:

snippet: service-bus-endpoint

The HTTP-triggered sender uses a keyed `IMessageSession` from the send-only endpoint registration:

snippet: service-bus-http-sender

These are the message handlers:

snippet: service-bus-trigger-handler

snippet: service-bus-followup-handler
