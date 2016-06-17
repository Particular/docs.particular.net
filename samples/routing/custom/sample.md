---
title: Custom routing
summary: Customizing NServiceBus message routing
component: Core
tags:
- Routing
redirects:
---


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

Sales endpoint is scaled out via `Sales` and `Sales2` projects. In real-world scenario there would be a single project deployed to multiple virtual machines.


### Shipping and Billing

Shipping and Billing mimic other back-end systems.


### Shared project

The shared project contains definitions for messages and the custom routing logic. 


### Custom automatic routing

The automatic routing is based on the idea of endpoints exchanging information about types of messages they handle and types of messages they publish. They do it via a table in SQL Server database (each endpoint instance own one row keyed by this endpoint's transport address). Following code is used inside the custom automatic routing feature to wire up all its components:

snippet:Feature

It creates a publisher and a subscriber for the routing information along with the communication object they use to exchange the information between endpoint instances. It also registers an additional behavior that makes sure all published types are properly advertised.

The custom routing is wired up through a set of `AddDynamic` APIs used in the routing information subscriber:

snippet:AddDynamic

The routing cache consists of three main structures and one auxiliary structure. The first maps message types to sets of endpoint which are known to have handlers for these message types. It is used in the logical routing phase.

snippet:FindEndpoint

The second structure maps endpoint names to sets of known endpoint instances. It is used in the physical routing phase.

snippet:FindInstance

The third structure maps an event type to its publisher. By definition there can be only one logical publisher for a given endpoint.

snippet:FindPublisher

The fourth auxiliary structure holds state information about the known endpoint instances. This information is used to optimize the physical routing phase by detecting inactive instances.