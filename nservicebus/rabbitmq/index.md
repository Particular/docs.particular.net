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
- RabbitMQ provides dedicated reliability and high-availability features.
- Offers native Publish-Subcribe mechanism, therefore it doesn't require NServiceBus persistence for storing event subscriptions.
- Wide range of supported clients allows for integrating the system with applications written in other languages using native RabbitMQ features.

### Disadvantages
- The additional cost of commercial RabbitMQ license and support.
- Many organizations don't have the same level of expertise with RabbitMQ as for example with SQL Server, so it might require additional training.
