---
title: Router
summary: How to connect parts of the system that use different transports 
component: Router
related:
- samples/azure/azure-service-bus-msmq-bridge
- samples/msmq/sql-bridge
reviewed: 2018-05-04
---

`NServiceBus.Router` is a universal component that connects parts of an NServiceBus-based solution that otherwise cannot talk to each other (e.g. because they use different transport or transport settings or because of physical barriers). 

Unlike the [Gateway](/nservicebus/gateway/) or the [Wormhole](/nservicebus/wormhole/), the Router handles both sending and publishing. Unlike the [Bridge](/nservicebus/bridge/) the Router can use site-based addressing to route message between logically significant sites (just like the Gateway does).

The Router is transparent to the publishing and replying endpoint. That is:

 * The endpoint that *replies* to a message does not have to know if the initiating message came through a router. The reply will be routed automatically to the correct router and then forwarded to the initiating endpoint.
 * The endpoint that *publishes* events does not have to know if the subscribers are behind the router.


## Connecting to the router

Regular endpoints connect to the router using *connectors* that allow them to configure the routing

snippet: connector

The snippet above tells the endpoint that a designated router listens on queue `MyRouter` and that messages of type `MyMessage` should be sent to the endpoint `Receiver` via the router. It also tell the subscription infrastructure that the event `MyEvent` is published by the endpoint `Publisher` that is hosted behind the router.


## Router configuration

The following snippet shows a simple MSMQ-to-RabbitMQ router configuration

snippet: two-way-router

The router has a simple life cycle:

snippet: lifecycle


## Topologies

A router consists of multiple [NServiceBus.Raw](/nservicebus/rawmessaging.md) endpoints, called *interfaces* and a *routing protocol* that controls how messages should be forwarded between them.


### Bridge

![Bridge](bridge.svg)

The arrows show the path of messages sent from `Endpoint A` to `Endpoint C` and from `Endpoint D` to `Endpoint B`. The messages cross the router in the opposite direction. Each message is initially sent to the router queue (based on the routing configuration of the connector) and then forwarded to the destination queue. There is one additional *hop* compared to a direct communication of endpoints. The following snippet configures the built-in *static routing protocol* to forward messages between the router's interfaces.

snippet: simple-routing


### Multi-way routing

![Multi-way](multi-way.svg)

The router is not limited to only two interfaces but in case there is more than two interfaces, the routing protocol rules need to be more complex and specific. The following snippet configures the built-in *static routing protocol* to forward messages to interfaces based on the prefix of the destination endpoint's name.

snippet: three-way-router

NOTE: All three interfaces use the same transport type (SQL Server Transport) but may use different settings e.g. a different database instances. This way each part of the system (Sales, Shipping and Billing) can be autonomous and own its database server yet they still can exchange messages in the same way as if they were connected to a single shared instance.


### Backplane

![Backplane](backplane.svg)

Two or more routers can be connected together to form a _backplane_ topology. This setup usually makes most sens for the geo-distributed systems. The following snippet configures the router hosted in the Europen part of the globally distributed system to route messages coming from outsize via the Azure Storage Queues interface directly to the local endpoints and to route messages sent by local endpoints to either East or West US through *designated gateway* routers.

NOTE: The *designated gateway* concept is not related to the NServiceBus.Gateway package. When the *designated gateway* is specified in the route, the message is forawrded to it instead of the actual destination.

snippet: three-way-router

NOTE: As an example the routing rules here use the `Site` property that can be set through the `SendOptions` object when sending messages. The backplane topology does not require site-based routing and can be configured e.g. using endpoint-based convention like in the multi-way routing example.