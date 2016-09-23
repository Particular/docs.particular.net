---
title: Custom routing
summary: Customizing NServiceBus message routing
component: Core
tags:
- Routing
redirects:
---

The sample demonstrates how NServiceBus routing model can be extended to allow for configuration-free routing with MSMQ transport. It does so by making endpoint instances publish metadata information about themselves:
 * identity - logical name and physical address;
 * handled messages;
 * published events.

The advantage of no configuration approach is low development friction and simpler maintanance. The disadvantage is that the implementation is more complex.


## Prerequisites

 1. Make sure SQL Server Express is installed and accessible as `.\SQLEXPRESS`.
 1. Create database called `AutomaticRouting`.


## Running the project

 1. Start all the projects by hitting F5.
 1. The text `Press <enter> to send a message` should be displayed in the Client's console window.
 1. Wait until all the endpoints exchange their routing information. Notice each endpoint logs the routing info as it discovers other endpoints.
 1. Hit `<enter>` several times to send some messages.


### Verifying that the sample works correctly

 1. The Sales.1 and Sales.2 consoles display information about accepted orders in round-robin fashion.
 1. The Shipping endpoint displays information that orders were shipped.
 1. The Billing endpoint displays information that orders were billed.


### Detecting failure

 1. Close the Sales.2 console window.
 1. Hit `<enter>` several times to send more messages.
 1. Notice that only every second message gets processed by Sales.1. Client still does not know that Sales.2 is down.
 1. Wait until consoles show that Sales.2 heartbeat timed out.
 1. Hit `<enter>` several times to send more messages.
 1. Notice that all orders are now routed to Sales.1 queue.


### Recovery

 1. In Visual Studio right-click on `Sales2` project and select `Debug -> Start new instance`.
 1. Notice that all messages sent to Sales.2 while it was down are now processed.
 1. Wait until other endpoints detect Sales.2 again.
 1. Hit `<enter>` several times to send more messages.
 1. Notice that orders are again routed to both Sales instances in round-robin fashion.


## Code walk-through

This sample contains four projects:

 * Shared - A class library containing common routing code including the message definitions.
 * Client - A console application responsible for sending the initial `PlaceOrder` message.
 * Sales - A console application responsible for processing the `PlaceOrder` command and generating `OrderAccepted` event.
 * Shipping - A console application responsible for processing the `OrderAccepted` message.
 * Billing - Another console application responsible for processing the `OrderAccepted` message.


### Client

The Client does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end.


### Sales

The Sales project mimics the back-end system where orders are accepted. Apart from the standard NServiceBus configuration it enables the custom automatic routing:

snippet:EnableAutomaticRouting

NOTE: In order to use this custom routing all published types need to be specified.

In real-world scenarios NServiceBus is scaled out by deploying multiple physical instances of a single logical endpoint to multiple machines (e.g. `Sales` in this sample). For simplicity in this sample the scale out is simulated by having two separate endpoints `Sales` and `Sales2`.


### Shipping and Billing

Shipping and Billing mimic back-end systems subscribing to events published by Sales endpoints.


### Shared project

The shared project contains definitions for messages and the custom routing logic.


### Custom automatic routing

The automatic routing is based on the idea of endpoints exchanging information about types of messages they handle and types of messages they publish. In this sample they share the information using a table in SQL Server database. Each endpoint instance owns one row. However, other solutions can be used (e.g. using tool like [Consul](https://www.consul.io/)).

All the routing components are wired up using the following code:

snippet:Feature

It creates a publisher and a subscriber for the routing information, as well as the communication object they use to exchange information between endpoints instances. It also registers an additional behavior that ensures that all published types are properly advertised. The automatic routing discovery protocol has no master and works in a peer-to-peer manner, therefore both publisher and subscriber need to be active in each endpoint instance.

<!--
http://www.planttext.com/planttext
@startuml

package "Client" {
    component [Routng\nSubscriber] as C_S
    Component [Routing\nPublisher] as C_P
}

package "Sales.1" {
    component [Routng\nSubscriber] as S1_S
    Component [Routing\nPublisher] as S1_P
}

package "Sales.2" {
    component [Routng\nSubscriber] as S2_S
    Component [Routing\nPublisher] as S2_P
}

database "SQL Server\n" {
    [Routing information] as RI
}

C_P -down-> RI
RI -up-> C_S

S1_P -down-> RI
RI -up-> S1_S

S2_P -down-> RI
RI -up-> S2_S

@enduml
-->

![Automatic routing design](design.png)

The routing information in the core of NServiceBus consists of three parts:
 * Logical command routing (mapping of message types to logical destinations)
 * Logical event routing (mapping of even types to their respective logical publishers)
 * Physical routing (mapping of logical endpoint to physical instances)

This information is updated every time the automatic routing feature detects a change in the topology via following API calls:

snippet:AddOrReplace