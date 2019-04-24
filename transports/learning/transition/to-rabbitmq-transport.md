---
title: Transitioning to the RabbitMQ Transport
summary: Demonstrates the required changes to switch your POC from the Learning Transport to the RabbitMQ Transport
reviewed: 2019-04-24
component: Rabbit
tags:
 - RabbitMQ
 - Transport
related:
 - transports/rabbitmq
 - samples/rabbitmq/simple
---

The Learning transport is not a production-ready transport, but is intended for learning the NServiceBus API and creating demos/proof-of-concepts. A number of changes to endpoint configuration are required to transition an endpoint from the Learning transport to the RabbitMQ transport.


## Install the NuGet Package

The [NServiceBus.RabbitMQ](https://www.nuget.org/packages/NServiceBus.RabbitMQ/) must be installed. This can be accomplished using the [NuGet Package Manager UI](https://docs.microsoft.com/en-us/nuget/tools/package-manager-ui) or run this command from the [NuGet Package Manager Console](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console):

```
   Install-Package NServiceBus.RabbitMQ
```


## Change the Endpoint Configuration

To use the Azure Service Bus Transport first update the `UseTransport` call:

```c#
-   endpointConfiguration.UseTransport<LearningTransport>();
+   endpointConfiguration.UseTransport<RabbitMQTransport>();
```


### Set a Connection String

The RabbitMQ Transport requires a connection string be specified:

snippet: rabbitmq-config-connectionstring-in-code


### Specify the Conventional Routing Topology

The RabbitMQ transport has the concept of a routing topology, which controls how it creates exchanges, queues, and the bindings between them in the RabbitMQ broker. The recommended routing topology is the conventional routing topology which should be used by all new NServiceBus customers using RabbitMQ:

snippet: rabbitmq-config-useconventionalroutingtopology


### Enable Installers

The endpoint must enable installers to allow the RabbitMQ Transport to create the necessary [queues, bindings, and exchanges](https://www.rabbitmq.com/tutorials/amqp-concepts.html) in the RabbitMQ broker for your endpoint.

snippet: Installers
