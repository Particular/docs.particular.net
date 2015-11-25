---
title: Gateway
summary: Illustrates Gateway capability of NServiceBus.
tags:
- Gateway
related:
- nservicebus/gateway
---


## Code walk-through

This sample demonstrates how logically different sites (such as a headquarters and a remote site) communicate using the NServiceBus Gateway feature. 


## Headquarters.Shared

A shared class library for common code including message definitions.


## Headquarters

The central endpoint of the Sample.


### Gateway configuration

 * Maps the site key `RemoteSite` to `http://localhost:25899/RemoteSite`
 * Receives incoming messages on `http://localhost:25899/Headquarters/`

<!-- import HeadquatersGatewayConfig-->


## AcknowledgedHandler

Handles `PriceUpdateAcknowledged` from `RemoteSite`.

snippet:AcknowledgedHandler


## UpdatePriceHandler

Handles `UpdatePrice` from `WebClient`.

snippet:UpdatePriceHandler


## RemoteSite


### Gateway configuration

Receives incoming messages on `http://localhost:25899/RemoteSite/`

<!-- import RemoteSiteGatewayConfig-->


### PriceUpdatedHandler
     
snippet:PriceUpdatedHandler


## WebClient

This project represents an external integration point. It sends a `UpdatePrice` to the channel `http://localhost:25899/Headquarter/`.

snippet:SendUpdatePrice
