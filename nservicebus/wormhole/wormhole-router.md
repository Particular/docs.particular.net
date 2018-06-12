---
title: Upgrade Wormhole 2 to Router 2
summary: Instructions on how to upgrade NServiceBus.Wormhole Version 2 to NServiceBus.Router Version 2.
component: Wormhole
reviewed: 2018-06-08
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

The NServiceBus.Wormhole package has been deprecated and replaced by the more powerful NServiceBus.Router. Each Wormhole gateway is replaced by a two-interface router that forwards messages between the transport that the endpoints use in the local site and the transport that is used to move messages between sites (e.g. Amazon SQS, Azure Storage Queues, Azure Service Bus). 


## Endpoint-side

The fixed or callback-based destination site configuration in the routing settings is no longer available. In order to instruct the router connector to forward a given message to a remote site, use the `SendToSites` extension method.

snippet: wormhole-to-router-connector


## Bridge/router side

The transport between the routers forms a tunnel through which messages are sent when they need to be delivered to a remote site. Each router is configured with two interfaces, one for the local endpoints and the other for the tunnel.

snippet: wormhole-to-router-router

NOTE: The routing rules forward all the messages coming from the local site to the remote site through the remote router. The configuration that describes if a given destination endpoint is in the local or remote site is done on the endpoint side.


## HTTP transports

The HTTP transport (NServiceBus.Transports.Http) is also deprecated as it was intended to be used exclusively with the Wormhole. The Router should be used with one of the cloud-native transports instead e.g. Amazon SQS, Azure Storage Queues or Azure Service Bus.