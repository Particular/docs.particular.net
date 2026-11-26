---
title: Azure Service Bus Ordered Delivery Sample
summary: Demonstrates ordered message delivery using NServiceBus and Azure Service Bus Transport
reviewed: 2026-11-24
component: ASBS
related:
- transports/azure-service-bus
- transports/azure-service-bus/ordered-delivery-with-sessions
---

This sample demonstrates [ordered delivery with sessions](/transports/azure-service-bus/ordered-delivery-with-sessions.md) in the Azure Service Bus transport. Ordered delivery with sessions requires version 6.6 or later of the transport.

## Prerequisites

include: asb-connectionstring-xplat

## Code walk-through

The sample contains five executable projects:

- `Client` - An NServiceBus endpoint that sends the `AddItem` and `PlaceOrder` commands
- `Sales` - An NServiceBus endpoint that handles the `AddItem` and `PlaceOrder` commands. The endpoint publishes an `OrderPlaced` event and is an ordered endpoint.
- `Inventory` - An NServiceBus endpoint that handles the `OrderPlaced` events. The endpoint is an ordered endpoint.
- `InventoryMainframe` - An NServiceBus endpoint that pretends to be a legacy mainframe application that expects a series of messages arriving in specific order. The endpoint is an ordered endpoint.
- `Billing` - An NServiceBus endpoint that handles the `OrderPlaced` events. The endpoint is not ordered.

The ordered endpoints enable sessions and disable delayed retries, which session-enabled endpoints don't support.

## Running the sample

1. The `Sales` endpoint must be running before the `Client` to ensure that the input queue for `Sales` has been created before the `Client` sends the commands. The `Billing`, `Inventory`, and `InventoryMainframe` endpoints can be run in any order.
2. The `Client` endpoint sends three `AddItem` commands and a `PlaceOrder` command.

### Client

The `Client` application is a send-only NServiceBus endpoint. The endpoint defines an `OrderId` and sends three `AddItem` commands and a `PlaceOrder` command using the `OrderId` as the session ID.

### Sales

The `Sales` endpoint is an ordered endpoint. The endpoint handles the `AddItem` and `PlaceOrder` commands in the order in which they are delivered to the endpoint by the broker. The `Sales` endpoint publishes an `OrderPlaced` event when an order is placed.

The `OrderPlaced` event also uses the `OrderId` as the session ID, so that all messages for one order are processed in order. Events for different orders can be processed in parallel. To process all `OrderPlaced` events in the order in which they were published, use the same session ID for all of them, at the cost of processing them one at a time.

### Inventory

The `Inventory` endpoint is an ordered endpoint. The endpoint handles the `OrderPlaced` event and sends a series of commands to `InventoryMainframe`: one `BeginTransaction` message followed by a number of `Reserve` messages and ending with a `CompleteTransaction` message. All commands of a transaction use the same new session ID.

### InventoryMainframe

The legacy application expects the commands of a transaction to arrive in order. Because they share a session ID, the `InventoryMainframe` endpoint processes them in the order in which `Inventory` sent them.

### Billing

The `Billing` endpoint handles the `OrderPlaced` event to bill the customer. The endpoint is not ordered.

## Session state

The `Sales` endpoint also demonstrates how to use [session state](/transports/azure-service-bus/ordered-delivery-with-sessions.md#ordered-delivery-session-state) to implement ordered processing across a series of related messages.

Each time the `AddItemHandler` handles an `AddItem` message, it creates or updates an `OrderState` object in session state, incrementing its `ItemCount` property by one.

When the `PlaceOrderHandler` handles a `PlaceOrder` message, it reads the `OrderState` from session state and includes the item count in the `OrderPlaced` event. Because the order is complete, the handler then clears the session state.
