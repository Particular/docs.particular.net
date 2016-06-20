---
title: Custom routing
summary: Customizing NServiceBus message routing
component: Core
tags:
- Routing
redirects:
---

The sample demonstrates how NServiceBus routing model can be extended to allow for configuration-free routing with MSMQ transport. It does so by making endpoint instances publish metadata information about themselves
 * what is my identify (logical endpoint name and physical address)
 * what messages can I handle
 * what event types do I publish

The advantage of such approach is low development friction (no configuration to keep up to date for environment). The disadvantage is the added complexity and failure modes due to problems with the discovery mechanism. 

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

In real-world scenarios NServiceBus is scaled out by deploying multiple physical instances of a single logical endpoint to multiple machines (e.g. `Sales` in this sample). For simplicity in this sample the scale-out is simulated by having two separate endpoints `Sales` and `Sales2`.


### Shipping and Billing

Shipping and Billing mimic other back-end systems.


### Shared project

The shared project contains definitions for messages and the custom routing logic. 


### Custom automatic routing

The automatic routing is based on the idea of endpoints exchanging information about types of messages they handle and types of messages they publish. In this sample they do it via a table in SQL Server database (each endpoint instance owns one row) but other solutions can be used (e.g. Consul). Following code is used inside the custom automatic routing feature to wire up all its components:

snippet:Feature

It creates a publisher and a subscriber for the routing information along with the communication object they use to exchange the information between endpoint instances. It also registers an additional behavior that makes sure all published types are properly advertised. The automatic routing discovery protocol has no master and works in a peer-to-peer manner so both publisher and subscriber need to be active in each endpoint instance.

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

In order to define custom routing it's necessary to add logical addresses of endpoints to the routing table, specify all physical instances of the endpoints, and specify all event publishers, as show in the following snippet

snippet:AddDynamic

The routing cache consists of four parts. The first one is used in the [endpoint mapping layer](http://docs.particular.net/nservicebus/messaging/routing#unicast-routing-endpoint-mapping-layer) and maps message types to sets of endpoint which are known (because they advertised this fact) to have handlers for these message types. 

snippet:FindEndpoint

The second one is used in the [instance mapping layer](http://docs.particular.net/nservicebus/messaging/routing#unicast-routing-endpoint-instance-mapping-layer) and maps endpoint names to sets of known endpoint instances.

snippet:FindInstance

The third one maps an event type to its publisher. By definition there can be only one logical publisher for a given endpoint.

snippet:FindPublisher

The fourth, auxiliary, structure holds state information about the known endpoint instances. This information is used to optimize the instance mapping by excluding inactive known to be inactive from the round-robin load distribution process.
