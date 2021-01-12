---
title: Upgrade Wormhole 2 to Router 3
summary: Instructions on how to upgrade NServiceBus.Wormhole Version 2 to NServiceBus.Router Version 3.
component: Wormhole
reviewed: 2021-01-12
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

The NServiceBus.Wormhole package has been deprecated and replaced by the more powerful NServiceBus.Router. Each Wormhole gateway is replaced by a two-interface router that forwards messages between the transport that the endpoints use in the local site and the transport that is used to move messages between sites (e.g. Amazon SQS, Azure Storage Queues, Azure Service Bus).


## Endpoint-side

The following code snippets were used to set up the Wormhole connector and send messages

snippet: upgrade-wormhole-connector

snippet: upgrade-wormhole-connector-send

The fixed or callback-based destination site configuration in the routing settings is no longer available. In order to instruct the router connector to forward a given message to a remote site, use the `SendToSites` extension method.

snippet: upgrade-router-connector-send

Another change is that with the Router it is the sending endpoint who is, by default, responsible for providing the name of the destination endpoint via `RouteToEndpoint` API.

snippet: upgrade-router-connector

NOTE: For NServiceBus Version 6 the `NServiceBus.Bridge.Connector` package can be used instead of `NServiceBus.Router.Connector` as the Router is backwards-compatible with the bridge.


## Router side

The Wormhole configuration required remote sites to be declared in the following way:

snippet: upgrade-gateway-config-a

In addition to that, the destination site routed had to specify forwarding for messages received from other sites:

snippet: upgrade-gateway-config-b

With the Router the transport between the routers forms a tunnel through which messages are sent when they need to be delivered to a remote site. Each router is configured with two interfaces, one for the local endpoints and the other for the tunnel.

snippet: upgrade-router-config

NOTE: The is no forwarding configuration. The destination endpoint has been specified by the sending endpoint.


## HTTP transports

The HTTP transport (NServiceBus.Transports.Http) is also deprecated as it was intended to be used exclusively with the Wormhole. The Router should be used with one of the cloud-native transports instead e.g. Amazon SQS, Azure Storage Queues or Azure Service Bus.