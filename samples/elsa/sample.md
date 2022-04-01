---
title: Integration with Elsa
summary: How to use NServiceBus with Elsa workflows
reviewed: 2022-04-01
component: Core
---

This sample demonstrates how to use NServiceBus message handlers in Elsa workflows.

## Overview

The demo is similar to the [saga tutorial](/tutorials/nservicebus-sagas/1-saga-basics/) using dynamic Elsa workflows instead of a saga. Elsa activities are used to send and receive messages, and to publish events. The workflows can be built by the designer at runtime.

## Running the project

To run the sample, start the ClientUI, Sales, and Billing projects and navigate to https://localhost:5001/place-order to send an initial `PlaceOrder` message. The Sales endpoint handles the message and publishes an `OrderPlaced` event which will be handled by the Billing endpoint.

## `Send a message` and `Publish an event` activities

The code for the `Send a message` activity is as follows:

snippet: SendNServiceBusMessage

The activity uses the `IMessageSession` from NServiceBus to send the data that is passed into the activity as input from the workflow context (see the Elsa documentation on [workflow variables](https://elsa-workflows.github.io/elsa-core/docs/concepts/concepts-workflow-variables) and [workflow context](https://elsa-workflows.github.io/elsa-core/docs/concepts/concepts-workflow-context)).

The code for the `Publish an event` activity follows a similar pattern:

snippet: PublishNServiceBusEvent

Both the ClientUI and Sales projects implement their own Elsa activities for instantiating message and event instances: `CreatePlaceOrderMessage` for the ClientUI, and `CreateOrderPlacedEvent` for the Sales endpoint.

## Receiving a message in an activity

When a message is received through NServiceBus, the Elsa workflow is triggered from a [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md) in `CustomElsaHandlerTrigger.cs`:

snippet: ElsaWorkflow

 An [Elsa bookmark](https://elsa-workflows.github.io/elsa-core/docs/next/guides/guides-blocking-activities#bookmarks) is created when the workflow is initialized. The bookmark allows Elsa to distinguish between received messages of different types. After the bookmark is created, the behavior searches for workflows defined for a specific message type. If a workflow is found, it is executed immediately. Otherwise, the NServiceBus pipeline continues as normal.

## Using the designer

The ClientUI and Sales workflows were created using the designer; all NServiceBus messages are sent or received through Elsa. The workflows can be inspected visually by starting the `ElsaDesigner` project and navigating to the "Workflow Definitions" page. The code in this project is pulled directly from the [Elsa designer tutorial](https://elsa-workflows.github.io/elsa-core/docs/quickstarts/quickstarts-aspnetcore-server-dashboard).

The ClientUI and the Sales endpoints are both configured as Elsa API endpoints. The designer is able to publish workflows at runtime without the need to recycle the endpoint.

Note that the default designer can point to only one endpoint at a time. The `_Host.cshtml` file controls which endpoint the designer is pointing at. In the sample, the ClientUI project runs on port 5001, and the Sales project runs on port 6001.

The workflows are saved in a Sqlite database (elsa.sqlite.db) in each of the Client and Sales project directories.
