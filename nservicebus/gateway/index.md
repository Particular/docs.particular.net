---
title: Gateway
summary: NServiceBus enables durable fire-and-forget messaging across physically separated IT infrastructure
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

 * Automatic retries.
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

A gateway runs inside each host process. It gets its input from the current [transport](/nservicebus/transports/) queue and forwards the message over the desired channel to the receiving gateway. On the receiving side there's another gateway, listening on the input channel. It de-duplicates incoming messages and forwards them to the main input queue of its local endpoint:

![](gateway-headquarter-to-site-a.png "Physical view")

In order to send message to other sites call `SendToSites` method:

snippet:SendToSites

`SendToSite` accepts a list of sites to which it should send messages. Note that each site can be configured with a different transport mechanism.


### Configuring the gateway

In Versions 5 and above the gateway is provided by the `NServiceBus.Gateway` NuGet package. In Version 3 and Version 4 the gateway is part of the `NServiceBus` NuGet package.

The gateway feature needs to be explicitly enabled using configuration API:

snippet:GatewayConfiguration


### Retries

In Gateway Version 1 and NServiceBus Versions 4 and below the Gateway shares the core [message retry](/nservicebus/errors/automatic-retries.md) behavior.

In Gateway Versions 2 and above the Gateway has its own retry mechanism. It will retry failed messages 4 times by default, increasing the delay by 60 seconds each time as follows:

Retry | Delay
---- | ----
1 | 60 seconds
2 | 120 seconds
3 | 180 seconds
4 | 240 seconds

The number of retries and the time to increase between retries can be configured using configuration API:

snippet:GatewayDefaultRetryPolicyConfiguration

The default retry policy can be replaced by implementing a `Func<IncomingMessage, Exception, int, TimeSpan>` to calculate the delay for each retry:

snippet:GatewayCustomRetryPolicyConfiguration

The provided example shows the built-in default retry policy.

Custom retry policies should eventually give up retrying, otherwise the message could get stuck in a loop being retried forever. To discontinue retries return `TimeSpan.MinValue` from the custom retry policy. The message will be then [routed to the configured error queue](/nservicebus/errors/).

WARNING: The recoverability mechanisms built into the Gateway do not roll back the [receive transaction](/nservicebus/messaging/) or any ambient transaction when sending a message to another site fails. Any custom recoverability policy cannot rely on an ambient transaction being rolled back.

To disable retries in the gateway use the `DisableRetries` setting:

snippet: GatewayDisableRetriesConfiguration

When retries are disabled, any messages that fail to be sent to another site will be immediately routed to the configured error queue.
