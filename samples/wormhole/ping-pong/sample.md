---
title: Using Wormhole to Send Messages Between Distributed Sites
summary: A sample showing how to use a wormhole between distributed sites
component: Wormhole
reviewed: 2019-03-22
related:
 - nservicebus/wormhole
---

## Prerequisites

Open the solution in Visual Studio running with Administrator privilieges, since the gateways must register the listening URL. Alternatively, add the registrations manually:

```
netsh http add urlacl url=http://+:7777/Gateway.SiteA/ user=DOMAIN\user
netsh http add urlacl url=http://+:7777/Gateway.SiteB/ user=DOMAIN\user
```


## Running the project

 1. Start the projects in debug mode.
 1. Press <kbd>enter</kbd> a couple of times.
 1. Observe the `PingHandler` logging processed message IDs in the Server window.
 1. Observe the `PongHandler` logging processed message IDs in the Client window.


## Code walk-through

The solution consists of five projects.


### Shared

The Shared project contains the message contracts.


### Client

The Client project contains an NServiceBus endpoint that sends the `Ping` messages and expects `Pong` responses. It is configured to send the `Ping` messages through a local gateway:

snippet: ConfigureClient


### GatewayA

The GatewayA project sets up the client-side gateway. This gateway is configured to route messages to a remote site `B`.

snippet: ConfigureGatewayA

It does not contain any forwarding configuration because the `Pong` message is routed automatically thanks to the `reply-to` header.


### GatewayB

The GatewayB project sets up a server-side gateway. This gateway is configured to route messages to a remote site `A` and to forward the `Ping` messages to the Server endpoint:

snippet: ConfigureGatewayB


### Server

The Server project contains an NServiceBus endpoint that processes the `Ping` messages and sends the `Pong` messages as a response. It is configured to recognize GatewayB as the local gateway:

snippet: ConfigureServer

It does not need to configure routing of `Pong` messages because it is routed automatically thanks to the `reply-to` header.


## How it works

The Client sends the `Ping` message to the gateway using a local transport. The message carries a special header that denotes which site or sites the message should be delivered to.

When the gateway in the origin site (GatewayA) picks up the message, it examines theheader and dispatches the message to the gateways in the destination site(s) using the tunnel transport (HTTP in this sample). Before dispatching, the gateway stamps the message with a header denoting the origin site.

When the gateway in the destination site (GatewayB) receives the message, it examines the message type and calculates the receiving endpoint name based on the forwarding rules configured. 

When the Server receives the `Ping` message it replies with a `Pong` message. A behavior injected into the Server's send pipeline ensures that the ultimate destination is set from the `reply-to` headers and the destination site is set to the original site.

NOTE: The `Wormhole` must be configured for all communicating endpoints, not just the ones sending the messages.

When the server-side gateway picks up the `Pong` messages, it examines the destination site header and forwards the message accordingly.

When the client-side gateway receives the message, it examines the ultimate destination header and forwards the message to the Client endpoint.