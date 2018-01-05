---
title: HTTPVNext Channel
summary: Alternative HTTP Channel implementation with less use of HTTP Headers
component: GatewayChannelHttpVNext
---

This channel allows the use of [Gateways] (/nservicebus/gateway/index.md) where a gateway endpoint may be behind a reverse proxy such as NGINX or CloudFlare, or in any other environment where HTTP headers could be modified.

## Registering a receive channel

snippet: HttpVNextChannel

## Registering a site

snippet: HttpVNextSite
