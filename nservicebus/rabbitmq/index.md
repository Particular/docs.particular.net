---
title: RabbitMQ Transport
reviewed: 2016-09-13
component: Rabbit
related:
 - samples/rabbitmq/simple
 - samples/rabbitmq/native-integration
redirects:
 - nservicebus/rabbitmq/configuration-api
tags:
 - RabbitMQ
 - Transports
---

Provides support for sending messages over [RabbitMQ](http://www.rabbitmq.com/) using the [RabbitMQ .NET Client](https://www.nuget.org/packages/RabbitMQ.Client/).


## Configuring the endpoint

To use RabbitMQ as the underlying transport:

snippet:rabbitmq-config-basic


## Advantages and Disadvantages


### Advantages

 * RabbitMQ provides native reliability and high-availability features.
 * Offers native Publish-Subcribe mechanism, therefore it doesn't require NServiceBus persistence for storing event subscriptions.
 * Wide range of supported clients allows for integrating the system with applications written in other languages using native RabbitMQ features.
 * Supports [Competing consumers](http://www.enterpriseintegrationpatterns.com/patterns/messaging/CompetingConsumers.html) pattern out of the box. Messages are received by instances in a round-robin fashion without additional configuration.


### Disadvantages

 * Doesn't handle [network partitions](https://www.rabbitmq.com/partitions.html) well, partitioning across a WAN requires using dedicated features.
 * Requires careful consideration with regards to duplicate messages, e.g. using the [Outbox](/nservicebus/outbox/) feature or making all endpoints idempotent.
 * Many organizations don't have the same level of expertise with RabbitMQ as for example with SQL Server, so it might require additional training.
 * Might require covering additional costs of commercial RabbitMQ license and support.
