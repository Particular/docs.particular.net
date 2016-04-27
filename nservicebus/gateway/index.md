---
title: Gateway
summary: NServiceBus enables durable fire-and-forget messaging across physically separated IT infrastructure.
tags: []
redirects:
 - nservicebus/introduction-to-the-gateway
related:
 - samples/gateway
---

The purpose of the gateway is to provide durable fire-and-forget messaging with NServiceBus across physically separated sites, where "sites" are locations run using IT infrastructure and not web sites.

The gateway comes into play where the use of regular queued transports for communication are not possible i.e. when setting up a VPN connection is not an option. The reason for not using a VPN could be security concerns, bandwidth limitations, latency problems, high availability constraints, etc.


## When not avoid using the gateway

The gateway should not be used for disaster recovery between sites. Under those circumstances all sites are exact replicas and are not logically different. It is recommended to utilize existing support infrastructure to keep all sites in sync.

So if sites are logically similar, use one of the approaches above; if they are logically different, the gateway may come in handy.


## Logically different sites

Logically different sites serve different business purposes where each site differs in behavior from all other sites. For example: A chain of retail stores where the headquarters is responsible for the prices of the goods being sold. Those prices need to be highly available to all the stores. If the link to the headquarters is down, business is interrupted.

Looking at this scenario from a logical point of view, all the pricing communication goes on within the same business service (BS). The different physical sites have different logical behavior. This is a sure sign that the gateway might come in handy. Dig deeper and look at the actual responsibilities of each site:

 * Headquarters - Maintains the prices and pushes any price change out to the different stores on a daily basis
 * Store - Stores the prices locally for read-only purposes

Prices are usually set to remain effective for a minimum of one day so it is sufficient for the headquarters to push the price updates to the sites only once per day. Model this as `DailyPriceUpdatesMessage` containing the list of price updates for the coming business day. Given this design, only one message is required for each site per day, which lowers the infrastructure requirements.

Internally in the headquarters other business services may require more frequent updates, so model this with another logically different message, `PriceUpdatedForProduct`, which allows the use of the (pub/sub pattern)[/nservicebus/messaging/publish-subscribe] while communicating with other BS.

The gateway doesn't support pub/sub but this isn't a problem since request/response is adequate within a BS, remembering that those sites are physically different but the communication is within the same logical BS. So when using the gateway, the guideline is to model the messages going explicitly across sites. The following picture illustrates the sample and includes a sales service responsible for reporting the sales statistics so that the pricing service can set appropriate prices.

![Gateway Store and Headquarters example](store-to-headquarters-pricing-and-sales.png "Logical view")

The prices are pushed daily to the stores and sales reports are pushed daily to the HQ. Any pub/sub goes on within the same physical site. This is the reason that the NServiceBus gateway doesn't support pub/sub across sites since it shouldn't be needed in a well designed system.

Going across sites usually means radically different transport characteristics like latency, bandwidth, reliability, and explicit messages for the gateway communication, helping to make it obvious for developers that they are about to make cross-site calls. This is where Remote Procedure Call (RPC) really starts to break down as it will meet all [the fallacies of distributed computing](https://en.wikipedia.org/wiki/Fallacies_of_distributed_computing) head on.


## Using the gateway

In order to send message to other sites call `SendToSites` method:

snippet:SendToSites

`SendToSite` accepts a list of sites to send the messages to. Each site can be configured with a different transport mechanism.

On the receiving side is another gateway listening on the input channel and forwarding the incoming message to the target endpoint. The image below shows the physical parts involved:

![](gateway-headquarter-to-site-a.png "Physical view")

A gateway runs inside each host process. The gateway gets its input from a regular MSMQ queue and forwards the message over the desired channel to the receiving gateway. The receiving side de-duplicates the message (ensures it is not a duplicated message, i.e., a message that was already sent) and forwards it to the main input queue of its local endpoint. The gateway has the following features:

 * Automatic retries
 * De-duplication of messages
 * Transport level encryption with SSL
 * Support for data bus properties with large payloads
 * Can listen on multiple channels of different types
 * Included in every endpoint
 * Easily extensible with other channels


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

This example custom retry policy will produce the same results as the default retry policy.

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


## Key messages

 * Only use the gateway for logically significant sites.
 * Use explicit messages for cross-site communication.
 * The gateway doesn't support pub/sub.
 * Automatic de-duplication and retries come out of the box.