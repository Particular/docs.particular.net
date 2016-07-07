---
title: Gateway
summary: NServiceBus enables durable fire-and-forget messaging across physically separated IT infrastructure.
redirects:
 - nservicebus/introduction-to-the-gateway
related:
 - samples/gateway
---

The purpose of the gateway is to provide durable fire-and-forget messaging with NServiceBus across physically separated sites. Note that "sites" in this context mean distinct locations run using IT infrastructure, not web sites. The gateway should be only used for [logically different sites](#logically-different-sites) and use explicit messages for cross-site communication. It provides automatic de-duplication and retries, however publish-subscribe pattern is not supported.

It is useful for communicating when using regular queued transports for communication is not possible, for example when setting up a VPN connection is prevented because of security concerns, bandwidth limitations, latency problems, high availability constraints, etc.

It should not be used as a disaster recovery mechanism between sites. In case of disaster recovery all sites are expected to be the exact replicas. From the logical perspective they're a single application, replicated in multiple locations. In such situation it is recommended to utilize existing support infrastructure to keep all sites synchronized.

The gateway has the following features:

 * Automatic retries
 * De-duplication of messages
 * Transport level encryption with SSL
 * Support for data bus properties with large payloads
 * Can listen on multiple channels of different types
 * Included in every endpoint
 * Easily extensible with other channels


## Logically different sites

Sites are _logically different_ when each site differs in behavior from other sites, it might also serve a completely different business purpose.

A good example is a chain of retail stores. The prices of products are specified by people working in headquarters. All the stores in the chain need to know the prices in order to function. All stores send sales reports to headquarters in order to provide them with information for adjusting prices. The prices can remain effective for a minimum of one day, so it is sufficient for the headquarters to push the price updates to the sites only once per day.

![Gateway Store and Headquarters example](store-to-headquarters-pricing-and-sales.png "Logical view")

The headquarters site and stores sites are _logically different sites_, because they have different responsibilities and different logical behaviors:

 * Headquarters - Maintains the prices and pushes price updates to the different stores on a daily basis
 * Store - Stores the prices locally for read-only purposes, sends sales statistics to headquarters

The information about prices need to be highly available to all the stores. If the link to the headquarters is down then the stores can't sell anything.

The price updates for stores can be modeled as a `DailyPriceUpdatesMessage` message type containing the list of price updates for the coming business day. In this scenario each site needs to receive only one update message per day. 

Sending messages across sites has very different transport characteristics than sending them within a given site, e.g. latency will be typically higher, bandwidth and reliability will be lower. Therefore only the dedicated message types should be used for gateway communication in order to inform developers when they're about to make cross-site calls.


## Using the gateway

In order to send message to other sites call `SendToSites` method:

snippet:SendToSites

`SendToSite` accepts a list of sites to which it should send messages. Note that each site can be configured with a different transport mechanism.

On the receiving side there is another gateway listening on the input channel and forwarding the incoming message to the target endpoint:

![](gateway-headquarter-to-site-a.png "Physical view")

A gateway runs inside each host process. The gateway gets its input from a regular MSMQ queue and forwards the message over the desired channel to the receiving gateway. The receiving side de-duplicates the message and forwards it to the main input queue of its local endpoint. 


### Configuring the gateway

In Versions 5 and above the gateway is provided by the `NServiceBus.Gateway` NuGet. In Version 3 and Version 4 the gateway is included in the core assembly.

To turn on the gateway, add the following to the configuration:

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

The number of retries and the time to increase between retries can be configured as follows:

snippet:GatewayDefaultRetryPolicyConfiguration

The default retry policy can be replaced by implementing a `Func<IncomingMessage,Exception,int,TimeSpan>` to calculate the delay for each retry:

snippet:GatewayCustomRetryPolicyConfiguration

In the provided example the custom retry policy will produce the same results as the default retry policy.

Custom retry policies should eventually give up or a message could get stuck in a loop being retried forever. To discontinue retries return `TimeSpan.MinValue` from the custom retry policy and the message will be treated as a fault. [Faulted messages are routed to the configured error queue](/nservicebus/errors/).

WARNING: The recoverability mechanisms built into the Gateway do not roll back the [receive transaction](/nservicebus/messaging/) or any ambient transaction when sending a message to another site fails. Any custom recoverability policy cannot rely on an ambient transaction being rolled back.

To disable retries in the gateway use the `DisableRetries` setting:

snippet: GatewayDisableRetriesConfiguration

When retries are disabled, any messages that fail to be sent to another site will be immediately treated as faulted and routed to the configured error queue.


## Custom Channel Types

The Gateway includes an HTTP/HTTPS channel implementation, but it is possible create additional channel types by implementing the `IChannelSender` and `IChannelReceiver` interfaces.

NServiceBus will pass headers / message data to the configured `IChannelSender` which is responsible for transmitting this information over the desired channel to a receiving Gateway.

`IChannelReceiver` must accept transmissions from the incoming channel and provide the incoming headers / message data to NServiceBus through a `DataReceivedOnChannelArgs` instance that is passed to a provided `Func<DataReceivedOnChannelArgs, Task>`.

`IChannelSender` and `IChannelReceiver` implementations are not automatically registered through assembly scanning. The custom channel types must be registered using a channel factory `Func` through the `ChannelFactories` setting:

snippet: GatewayChannelFactoriesConfiguration
