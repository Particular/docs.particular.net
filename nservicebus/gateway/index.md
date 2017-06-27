---
title: Gateway
summary: Durable fire-and-forget messaging across physically separated IT infrastructure
component: Gateway
reviewed: 2016-07-23
redirects:
 - nservicebus/introduction-to-the-gateway
related:
 - samples/gateway
---

The purpose of the gateway is to provide durable fire-and-forget messaging with NServiceBus across physically separated sites. Note that "sites" in this context mean distinct physical locations run using IT infrastructure, not web sites. The gateway should be only used for [logically different sites](#logically-different-sites) and use explicit messages for cross-site communication. It provides automatic de-duplication and retries, however publish-subscribe pattern is not supported.

It is useful for communicating when using regular queued transports for communication is not possible, for example when setting up a VPN connection is prevented because of security concerns, bandwidth limitations, latency problems, high availability constraints, etc.

It should not be used as a disaster recovery mechanism between sites. In such scenario all sites are expected to be the exact replicas. From the logical perspective they're a single application, replicated in multiple locations. In such situation it is recommended to utilize existing support infrastructure to keep all sites synchronized.

The gateway supports the following features:

 * Recoverability.
 * De-duplication of messages.
 * Transport level encryption with SSL.
 * Data bus for large payloads.
 * HTTP/HTTPS channel implementation.
 * It is possible to create additional channel types.
 * Can listen to multiple channels of different types.


## Logically different sites

Sites are _logically different_ when each site differs in behavior from others, they might also serve a completely different business purposes.

A good example is a chain of retail stores. The prices of products are specified by people working in headquarters. All the stores in the chain need to know the prices in order to function. Also all stores send sales reports to headquarters in order to provide them with information for adjusting prices. The prices have to remain effective for a minimum of one day, so it is sufficient for the headquarters to push the price updates to the sites only once per day.

![Gateway Store and Headquarters example](store-to-headquarters-pricing-and-sales.png "Logical view")

The headquarters site and stores sites are _logically different_, because they have different responsibilities and different logical behaviors:

 * Headquarters - Maintains the prices and pushes price updates to the different stores on a daily basis.
 * Store - Stores the prices locally for read-only purposes, sends sales reports to the headquarters.

The price updates for stores can be modeled as a `DailyPriceUpdatesMessage` message type containing the list of price updates for the coming business day. In this scenario each site needs to receive only one update message per day.

Sending messages across sites has very different transport characteristics than sending them within a given site, e.g. latency will be typically higher, bandwidth and reliability will be lower. Therefore only the dedicated message types should be used for gateway communication in order to explicitly inform developers when they're about to make a cross-site call.


## Using the gateway

A gateway runs inside each host process. It gets its input from the current [transport](/transports/) queue and forwards the message over the desired channel to the receiving gateway. On the receiving side there's another gateway, listening on the input channel. It de-duplicates incoming messages and forwards them to the main input queue of its local endpoint:

![](gateway-headquarter-to-site-a.png "Physical view")

In order to send message to other sites call `SendToSites` method:

snippet: SendToSites

`SendToSite` accepts a list of sites to which it should send messages. Note that each site can be configured with a different transport mechanism.


### Configuring the gateway

In Versions 5 and above the gateway is provided by the `NServiceBus.Gateway` NuGet package. In Version 4 the gateway is part of the `NServiceBus` NuGet package.

The gateway feature needs to be explicitly enabled using configuration API:

snippet: GatewayConfiguration


### Recoverability

partial: recoverability


### Version compatibility

The Gateway component ensures only forward compatibility for one major version. That means a message sent by the NServiceBus 3.x Gateway can be understood by the NServiceBus 4.x Gateway, and a message sent by the NServiceBus 4.x Gateway can be understood by NServiceBus.Gateway 1.x (the Gateway package targeting NServiceBus 5.x).

However, a message sent by the NServiceBus 3.x Gateway will not be understood by NServiceBus.Gateway 1.x (NServiceBus 5.x) as this skips a major version. Likewise, a message sent by NServiceBus.Gateway 2.x will not be understood by NServiceBus.Gateway 1.x, as backwards communication is not supported.
