---
title: Gateway
summary: Durable fire-and-forget messaging across physically separated IT infrastructure
component: Gateway
isLearningPath: true
reviewed: 2019-12-17
redirects:
 - nservicebus/introduction-to-the-gateway
related:
 - samples/gateway
---

The purpose of the gateway is to provide durable fire-and-forget messaging with NServiceBus across physically separated sites. Note that "sites" in this context mean distinct physical locations run using IT infrastructure, not web sites. The gateway should be used only for [logically different sites](#logically-different-sites) and use explicit messages for cross-site communication. It provides automatic de-duplication and retries; however, the publish-subscribe pattern is not supported.

It is useful for communicating when using regular queued transports for communication is not possible, for example when setting up a VPN connection is prevented because of security concerns, bandwidth limitations, latency problems, high availability constraints, etc.

The gateway should not be used as a disaster recovery mechanism between sites. In this scenario, all sites are expected to be exact replicas. From a logical perspective they're a single application, replicated in multiple locations. In this situation it is recommended to use the existing support infrastructure to keep all sites synchronized.

The gateway supports the following features:

 * Recoverability.
 * De-duplication of messages.
 * Transport level encryption with SSL.
 * Databus for large payloads.
 * HTTP/HTTPS channel implementation.
 * Creation of additional channel types.
 * Listening to multiple channels of different types.


## Logically different sites

Sites are _logically different_ when each site differs in behavior from others. In fact, they might also serve a completely different business purposes.

One example is a chain of retail stores. The prices of products are specified by people working in headquarters. All the stores in the chain need to know the prices in order to function. Also all stores send sales reports to headquarters in order to provide them with information for adjusting prices. The prices have to remain effective for a minimum of one day, so it is sufficient for the headquarters to push the price updates to the sites only once per day.

![Gateway Store and Headquarters example](store-to-headquarters-pricing-and-sales.png "Logical view")

The headquarters site and stores sites are _logically different_, because they have different responsibilities and different logical behaviors:

 * Headquarters - Maintains the prices and pushes price updates to the different stores on a daily basis.
 * Store - Stores the prices locally for read-only purposes, sends sales reports to the headquarters.

The price updates for stores can be modeled as a `DailyPriceUpdatesMessage` message type containing the list of price updates for the coming business day. In this scenario each site needs to receive only one update message per day.

Sending messages across sites has very different transport characteristics than sending them within a given site. For example, latency will typically be higher, and bandwidth and reliability will be lower. Therefore only dedicated message types should be used for gateway communication in order to explicitly inform developers when they're about to make a cross-site call.


## Using the gateway

A gateway runs inside each host process. It gets its input from the current [transport](/transports/) queue and forwards the message over the desired channel to the receiving gateway. On the receiving side, there's another gateway listening on the input channel. It de-duplicates incoming messages and forwards them to the main input queue of its local endpoint:

![](gateway-headquarter-to-site-a.png "Physical view")

In order to send message to other sites, call the `SendToSites` method:

snippet: SendToSites

`SendToSites` accepts a list of sites to which it should send messages. Note that each site can be configured with a different transport mechanism.


### Configuring the gateway

In NServiceBus version 5 and above, the gateway is provided by the `NServiceBus.Gateway` NuGet package. In version 4 and below the gateway is part of the `NServiceBus` NuGet package.

NOTE: The gateway requires NServiceBus persistence to operate though not all persisters support it. Currently, the gateway is supported only by InMemory, RavenDB and NHibernate persisters. If the configured persister doesn't support gateway, an exception will be thrown at endpoint startup.

The gateway feature must be explicitly enabled using the configuration API:

snippet: GatewayConfiguration


### Recoverability

partial: recoverability


### Version compatibility

The gateway component ensures only forward compatibility for one major version. That means a message sent by the NServiceBus 3.x Gateway can be understood by the NServiceBus 4.x Gateway, and a message sent by the NServiceBus 4.x Gateway can be understood by NServiceBus.Gateway 1.x (the Gateway package targeting NServiceBus 5.x).

However, a message sent by the NServiceBus 3.x Gateway will not be understood by NServiceBus.Gateway 1.x (NServiceBus 5.x) as this skips a major version. Likewise, a message sent by NServiceBus.Gateway 2.x will not be understood by NServiceBus.Gateway 1.x, as backwards communication is not supported.

### Alternate channels

[NServiceBus.Gateway.Channels.HttpVNext](https://github.com/welshdave/NServiceBus.Gateway.Channels.HttpVNext) is a [community package](/nservicebus/community/) that provides an HTTP channel implementation for the Gateway that doesn't use HTTP headers for message content or metadata. This makes it easier to use this channel in situations where HTTP headers may be modified, such as when a gateway is behind a reverse proxy such as NGINX.