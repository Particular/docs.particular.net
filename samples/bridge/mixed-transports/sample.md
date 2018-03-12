---
title: Using bridge to connect endpoints that use different transports
component: Bridge
reviewed: 2018-01-10
related:
 - nservicebus/bridge
---

The sample demonstrates how NServiceBus.Bridge can be used to connect endpoints that use different transports, in this case MSMQ and RabbitMQ.

## Prerequisistes

 1. Ensure an instance of RabbitMQ is running and accessible.
 1. Ensure that MSMQ has been installed.


## Running the project

 1. Start the projects in debug mode.
 1. Press `enter` in the Client console a couple of times.
 1. Observe the `RequestHandler` logging processed message IDs in the Server window.
 1. Observe the `ReplyHandler` logging processed message IDs in the Client window.
 1. Observe the `EventHandler` logging processed message IDs in the Client window.


## Code walk-through

The solution consists of four projects.


### Shared

The Shared project contains the message contracts.


### Client

The Client project contains an NServiceBus endpoint that runs the MSMQ transport. It is configured to route `MyMessage` request through the bridge to the Server endpoint. It is also configured to route subscribe messages for `MyEvent` messages through the bridge. 

snippet: ClientBridgeConfig


### Server

The Server project contains an NServiceBus endpoint that runs the RabbitMQ transport. It processes `MyMessage` messages. As a result it publishes `MyEvent` events as well as replies directly to the Client with `MyResponse`.

snippet: RequestHandler

Both of these messages travel through the bridge but because they are not direct sends, the Server does not require any bridge configuration.


### Bridge

The Bridge project sets up a bi-directional bridge between MSMQ and RabbitMQ.

snippet: BridgeConfig

The bridge routes the `MyMessage` messages based on the destination endpoint specified by the Client in the message headers. When forwarding, it replaces the `NServiceBus.ReplyToAddress` header with its own header so that `MyReply` messages sent by the Server are automatically routed to the bridge and then forwarded to the ultimate recipient, the Client.

The bridge translates the incoming *subscribe* messages from the Client to the native RabbitMQ subscription action (creating the exchange hierarchies) and uses its own subscription store to keep track of subscribers on the MSMQ side. When an event is published by the Server it is routed to the Bridge using RabbitMQ transport native mechanisms and then forwarded to the Client based on the data in the subscription store.
