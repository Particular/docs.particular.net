---
title: Using NServiceBus.Router to send messages between distributed sites
component: Router
reviewed: 2021-01-12
related:
 - nservicebus/router
---


## Running the project

 1. Start the projects in debug mode.
 1. Press <kbr>Enter</kbr> a couple of times.
 1. Observe the `PingHandler` logging processed message IDs in the Server window.
 1. Observe the `PongHandler` logging processed message IDs in the Client window.


## Code walk-through

The solution consists of five projects.


### Shared

The Shared project contains the message contracts.


### Client

The Client project contains an NServiceBus endpoint that sends the `Ping` messages and expects `Pong` responses. It is configured to send the `Ping` messages through a local router:

snippet: ConfigureClient


### RouterA

The RouterA project sets up the client-side router. This router is set up to forward messages sent to `SiteB` to the `SiteB` router via the `Tunnel` interface.

snippet: ConfigureRouterA

It does not contain any forwarding configuration because the `Pong` reply message is routed automatically.


### RouterB

The RouterB project sets up the server-side router. This router is set up to forward messages coming out of the `Tunnel` interface to the local transport.

snippet: ConfigureRouterB


### Server

The Server project contains an NServiceBus endpoint that processes the `Ping` messages and sends the `Pong` messages as a response. 


## How it works

The Client sends the `Ping` message to the router using local transport. The message carries a special header that denotes which site or sites the message should be delivered to.

When the router in the origin site (RouterA) picks up the message it looks at the header and dispatches the message to the router in the destination site(s) using the tunnel transport (MSMQ in this sample). Before dispatching, the router stamps the message with a header denoting the origin site.

When the router in the destination site (RouterB) receives the message it looks at the type of the message and calculates the receiving endpoint name based on the forwarding rules configured. 

When the Server receives the `Ping` message it replies with a `Pong` message. The reply does not require any routing because the routers along the path of the the original `Ping` message leave breadcrumbs that can be used to route the reply along the same path in the opposite direction. 