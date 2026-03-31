---
title: IBM MQ polymorphic event routing
summary: Demonstrating explicit subscription routing so that a handler for a base event type receives messages published as derived types.
reviewed: 2026-03-26
component: IBMMQ
related:
- transports/ibmmq/topology
- transports/ibmmq
- nservicebus/messaging/publish-subscribe
- nservicebus/messaging/messages-events-commands
---

This sample demonstrates explicit polymorphic event routing with the IBM MQ transport. An **Orders** endpoint publishes either an `OrderPlaced` or an `ExpressOrderPlaced` event. The **Shipping** endpoint has a handler for `OrderPlaced` and uses explicit subscription routes to receive both event types.

## How it works

The IBM MQ transport uses a topic-per-event model: each concrete event type is published to its own topic. Subscribing to `OrderPlaced` alone would only create a subscription on the `OrderPlaced` topic, missing messages published as `ExpressOrderPlaced`.

To receive both, the subscriber explicitly maps the `OrderPlaced` subscription to multiple topics using `SubscribeTo`:

snippet: SubscribeRoutes

NServiceBus includes all types in the .NET inheritance chain in the `NServiceBus.EnclosedMessageTypes` message header when publishing. The handler for `OrderPlaced` receives the full `ExpressOrderPlaced` instance because NServiceBus matches the enclosed type chain against registered handlers.

## Prerequisites

The sample requires a running IBM MQ broker. A Docker Compose file is included:

```bash
docker compose up -d
```

This starts IBM MQ with queue manager `QM1` on port `1414`. The management console is available at `https://localhost:9443/ibmmq/console` (credentials: `admin` / `passw0rd`).

## Running the sample

1. Start **Shipping** first. `EnableInstallers()` creates its queue, topics, and durable subscriptions on the broker.
2. Start **Orders**.
3. Press `O` - Shipping logs `Received OrderPlaced`.
4. Press `E` - Shipping logs `Received ExpressOrderPlaced`, delivered via the explicit subscription on the `ExpressOrderPlaced` topic.

## Code walk-through

### Event hierarchy

snippet: Events

`ExpressOrderPlaced` inherits from `OrderPlaced` using C# record inheritance.

### Subscription routing

The subscriber must explicitly declare which concrete types' topics to subscribe to. Without this, the transport throws an `InvalidOperationException` at startup because `OrderPlaced` has a known descendant type (`ExpressOrderPlaced`).

snippet: SubscribeRoutes

### Publishing

snippet: PublishOrders

Both events are published with the same `session.Publish` call. Each is published to its own topic (`DEV.ORDERPLACED` and `DEV.EXPRESSORDERPLACED` respectively). NServiceBus populates `NServiceBus.EnclosedMessageTypes` with the full type chain automatically.

### Subscriber handler

snippet: OrderPlacedHandler

The handler is registered for `OrderPlaced` only. Logging `message.GetType().Name` confirms the full derived type is preserved through delivery - the handler receives an `ExpressOrderPlaced` instance, not a downcast `OrderPlaced`.
