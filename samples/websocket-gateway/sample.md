---
title: WebSocket Gateway
reviewed: 2017-08-22
component: Gateway
related:
- nservicebus/gateway
---


## Code walk-through

This sample demonstrates replacing the channel that is used for the NServiceBus Gateway with a [WebSockets](https://tools.ietf.org/html/rfc6455) channel based on the [websocket-sharp](http://sta.github.io/websocket-sharp/) library.


## Messages

A shared class library for the sample message transmitted through the gateway.


## SiteA

An endpoint that sends a message to SiteB via the WebSocket gateway.


partial: config


## WebScoketGateway

A shared library that contains the implementations for the WebSocket gateway channels


### WebSocketGatewayChannelSender

* Connects to a remote WebSocket server
* Serializes outgoing messages

snippet: WebScoketGateway-ChannelSender


### WebSocketGatewayChannelReceiver

* Listens for incoming WebSocket connections
* Deserializes incoming messages

snippet: WebSockectGateway-ChannelReceiver