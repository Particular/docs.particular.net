---
title: HTTPVNext Channel
summary: Alternative HTTP Channel implementation with less use of HTTP Headers
component: GatewayChannelHttpVNext
reviewed: 2018-01-26
---

This channel allows the use of [Gateways](/nservicebus/gateway/) where a gateway endpoint may be behind a reverse proxy such as NGINX or CloudFlare, or in any other environment where HTTP headers could be modified.

To use this channel install the mentioned NuGet and configure the gateway to use it when receiving messages:

snippet: HttpVNextChannel

The final step is to configure the gateway to use the new channel when transmitting messages:

snippet: HttpVNextSite
