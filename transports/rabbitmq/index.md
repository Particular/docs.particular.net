---
title: RabbitMQ Transport
reviewed: 2016-09-13
component: Rabbit
related:
 - samples/rabbitmq/simple
 - samples/rabbitmq/native-integration
redirects:
 - nservicebus/rabbitmq/configuration-api
 - nservicebus/rabbitmq
tags:
 - Transport
---

Provides support for sending messages over [RabbitMQ](http://www.rabbitmq.com/) using the [RabbitMQ .NET Client](https://www.nuget.org/packages/RabbitMQ.Client/).

WARNING: The transport is not compatible with versions of the RabbitMQ broker earlier than 3.4.0.


## Configuring the endpoint

To use RabbitMQ as the underlying transport:

snippet: rabbitmq-config-basic

The RabbitMQ transport requires a connection string to connect to the broker. See [Connection settings](/transports/rabbitmq/connection-settings.md) for options on how to provide the connection string.


## Advantages and Disadvantages


### Advantages

 * RabbitMQ provides [native reliability](https://www.rabbitmq.com/reliability.html) and [high-availability](https://www.rabbitmq.com/ha.html) features.
 * Offers native Publish-Subscribe mechanism, therefore it doesn't require NServiceBus persistence for storing event subscriptions.
 * Wide range of [supported clients](https://www.rabbitmq.com/devtools.html) allows for integrating the system with applications written in other languages using native RabbitMQ features.
 * Supports [Competing consumers](http://www.enterpriseintegrationpatterns.com/patterns/messaging/CompetingConsumers.html) pattern out of the box. Messages are received by instances in a round-robin fashion without additional configuration.


### Disadvantages

 * Doesn't handle [network partitions](https://www.rabbitmq.com/partitions.html) well, partitioning across a WAN requires using dedicated features.
 * Requires careful consideration with regards to duplicate messages, e.g. using the [Outbox](/nservicebus/outbox/) feature or making all endpoints idempotent.
 * Many organizations don't have the same level of expertise with RabbitMQ, as for example with SQL Server, so it may require additional training.
 * Might require covering additional costs of [commercial RabbitMQ license and support](https://www.rabbitmq.com/services.html).
