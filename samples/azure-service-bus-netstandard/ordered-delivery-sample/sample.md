---
title: Azure Service Bus Ordered Delivery Sample
summary: Demonstrates ordered message delivery using NServiceBus and Azure Service Bus Transport
reviewed: 2026-10-20
component: ASBS
related:
- transports/azure-service-bus
---

This sample demonstrates the usage of the ordered delivery support in Azure Service Bus Transport. 

## Prerequisites

The appsettings.json file in each project must be updated with the connection string for the Azure Service Bus namespace.

## Code walk-through

The sample contains four executable projects:

- `Client` - An NServiceBus endpoint that sends the `AddItem` and `PlaceOrder` commands
- `Sales` - An NServiceBus endpoint that handles the `AddItem` and `PlaceOrder` commands. The endpoint publishes an `OrderPlaced` event and is an ordered endpoint.
- `Inventory` - An NServiceBus endpoint that handles the `OrderPlaced` events. The endpoint is an ordered endpoint.
- `Billing` - An NServiceBus endpoint that handles the `OrderPlaced` events. The endpoint is not ordered.

## Running the sample

1. The `Sales` endpoint must be running before the `Client` to ensure that the input queue for `Sales` has been created before the `Client` sends the commands. The `Billing` and `Inventory` endpoint can be run in any arbitrary order.
2. The `Client` endpoint sends three `AddItem` commands and a `PlaceOrder` command.

## Client 

The Client application is a send-only NServiceBus endpoint. The endpoint defines a `OrderId` and sends three `AddItem` commands and a `PlaceOrder` command using the `OrderId` as the session id.

## Sales

The `Sales` endpoint is an ordered endpoint. The endpoint handles the `AddItem` and `PlaceOrder` commands in the order in which they are delivered to the endpoint by the broker. The `Sales` endpoint publishes an `OrderPlaced` event when an order is created.

Note that the session id of the `OrderPlaced` event published by the endpoint is not `OrderId`. It uses a fixed session id - "Sales". This is intentional. An endpoint that processes `OrderPlaced` events in order should handle them in the sequence they are delivered by the broker. If the session ID were set to `OrderId`, it would create high cardinality and could cause `OrderPlaced` events for different orders to be processed in parallel, depending on the endpoint’s concurrency settings. This parallelism could break the required processing order. For example, if `Inventory` must reserve stock based on the order in which orders were placed, processing events out of order could lead to incorrect behavior.

## Inventory

The `Inventory` endpoint is an ordered endpoint. The endpoint handles the `OrderPlaced` event. It is assumed that the endpoint must reserve stock based on the order in which orders were placed.

## Billing

The `Billing` endpoint handles the `OrderPlaced` event to bill the customer. The endpoint is not ordered.