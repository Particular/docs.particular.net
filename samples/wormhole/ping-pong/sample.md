---
title: Using Wormhole to send messages between distributed sites
component: Wormhole
reviewed: 2017-06-16
related:
 - nservicebus/wormhole
---

## Prerequisites

Open the solution in Visual Studio running as Administrator because the gateways need to register the listening URL, or add the registrations manually:

```
netsh http add urlacl url=http://+:7777/Gateway.SiteA/ user=DOMAIN\user
netsh http add urlacl url=http://+:7777/Gateway.SiteB/ user=DOMAIN\user
```


## Running the project

 1. Start the projects in debug mode.
 1. Press `enter` a couple of times.
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

The GatewayA project sets up the client-side gateway. This gateway is set up to be able to route messages to a remote site `B`.

snippet: ConfigureGatewayA

It does not contain any forwarding configuration because the `Pong` message is routed automatically thanks to the `reply-to` header support.


### GatewayB

The GatewayB project sets up the server-side gateway. This gateway is set up to be able to route messages to a remote site `A` and to forward the `Ping` messages to the Server endpoint:

snippet: ConfigureGatewayB


### Server

The Server project contains an NServiceBus endpoint that processes the `Ping` messages and sends the `Pong` messages as a response. It is configured to recognize GatewayB as the local gateway:

snippet: ConfigureServer

It does not need to configure routing of `Pong` messages because it is routed automatically thanks to the `reply-to` header support.


## How it works

The Client sends the `Ping` message to the gateway using local transport. The message carries a special header that denotes which site or sites the message should be delivered to.

When the gateway in the origin site (GatewayA) picks up the message it looks at the header and dispatches the message to the gateways in the destination site(s) using the tunnel transport (HTTP in this sample). Before dispatching, the gateway stamps the message with a header denoting the origin site.

When the gateway in the destination site (GatewayB) receives the message it looks at the type of the message and calculates the receiving endpoint name based on the forwarding rules configured. 

When the Server receives the `Ping` message it replies with a `Pong` message. A behavior injected into the Server's send pipeline ensures that the ultimate destination is set from the `reply-to` headers and the destination site is set to the original origin site.

NOTE: The `Wormhole` needs to be configured for all the communicating endpoints, not only the ones sending the messages.

When the Server-side gateway picks up the `Pong` messages it looks at the destination sites header and forwards the message accordingly.

When the Client-side gateway receives the message, it looks at the ultimate destination header and forwards the message accordingly to the Client endpoint.