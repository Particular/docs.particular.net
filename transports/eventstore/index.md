---
title: Event Store Transport
summary: High-level description of the EventStore Transport.
reviewed: 2017-06-30
component: EventStoreTransport
redirects:
 - nservicebus/eventstoretransport
---

The Event Store transport implements a message queuing mechanism on top of the [Event Store](https://geteventstore.com/) stream database.


## How it works

The Event Store transport **does not** rely on the projections functionality for routing messages. Instead, it maintains a routing topology data structure inside the database in a dedicated event stream `nsb-exchanges`. The structure is a simplified version of [AMQP 0.9.1 routing topology](https://www.rabbitmq.com/tutorials/amqp-concepts.html).

partial: topology

When a message is addressed to a given exchange `E` it is copied to all exchanges (similar to [AMQP `fanout` exchange](https://www.rabbitmq.com/tutorials/amqp-concepts.html#exchange-fanout)) and to all queues bound to that exchange. In the example above a message sent to the `ExchangeB` would go to the `Queue1`, and a message sent to the `ExchangeA` would go to both the `Queue1` and `Queue2`.


## Transactions

Event Store transport supports Receive Only transaction mode. Refer to [Transport Transactions](/transports/transactions.md) for a detailed explanation of the supported transaction handling modes and available configuration options.
