---
title: RabbitMQ Transport
related:
- samples/rabbitmq/simple
- samples/rabbitmq/native-integration
---

Provides support for sending messages over [RabbitMQ](http://www.rabbitmq.com/) using the [RabbitMQ .NET Client](https://www.nuget.org/packages/RabbitMQ.Client/).


## NuGet

The NuGet package is available here https://www.nuget.org/packages/nservicebus.rabbitmq.

    PM> Install-Package NServiceBus.RabbitMQ


## Advantages and Disadvantages of choosing RabbitMQ Transport


### Advantages
- RabbitMQ provides native reliability and high-availability features.
- Offers native Publish-Subcribe mechanism, therefore it doesn't require NServiceBus persistence for storing event subscriptions.
- Wide range of supported clients allows for integrating the system with applications written in other languages using native RabbitMQ features.
- Supports [Competing consumers](http://www.enterpriseintegrationpatterns.com/patterns/messaging/CompetingConsumers.html) pattern. Messages are received by instances in a round-robin fashion.

### Disadvantages
- It doesn't handle [network partitions](https://www.rabbitmq.com/partitions.html) well, partitioning across a WAN requires using dedicated features.
- It requires careful consideration with regards to duplicate messages, e.g. using the [Outbox](/nservicebus/outbox/) feature or making all endpoints idempotent.
- Many organizations don't have the same level of expertise with RabbitMQ as for example with SQL Server, so it might require additional training.
- Might require covering additional costs of commercial RabbitMQ license and support.
