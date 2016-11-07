---
title: Event Store Transport
summary: High-level description of NServiceBus EventStore Transport.
component: EventStoreTransport
tags:
 - EventStore
 - GetEventStore
---

The Event Store transport implements a message queuing mechanism on top of [Event Store](https://geteventstore.com/) stream database.


## How it works

The Event Store transport **does not** rely on the projections functionality for routing messages. Instead it maintains a routing topology data structure inside the database in a dedicated event stream `nsb-exchanges`. The structure is a simplified version of [AMQP 0.9.1 routing topology](https://www.rabbitmq.com/tutorials/amqp-concepts.html).

partial:topology

When a message is addressed to a given exchange `e` it is copied to all exchanges (similar to [AMQP `fanout` exchange](https://www.rabbitmq.com/tutorials/amqp-concepts.html#exchange-fanout)) and to all queues bound to that exchange. In the example above a message sent to the `ExchangeB` would go to the `Queue1`, and a message sent to the `ExchangeA` would go to both the `Queue1` and `Queue2`.


## Transactions

Event Store transport supports Receive only transaction mode. Refer to [Transport Transactions](/nservicebus/transports/transactions.md) for detailed explanation of the supported transaction handling modes and available configuration options.