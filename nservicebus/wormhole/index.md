---
title: Wormhole Gateway
summary: How to send messages between geographically separate sites using Wormhole Gateway 
component: Wormhole
reviewed: 2019-03-22
---

`NServiceBus.Wormhole` is an alternative to the [gateway](/nservicebus/gateway/). Both components support connecting NServiceBus endpoints across geographical boundaries. The following table compares important aspects of both technologies.

|                         | Gateway                                 | Wormhole                                                                                |
|-------------------------|-----------------------------------------|-----------------------------------------------------------------------------------------|
| Encryption              | Transport-level                         | No                                                                                      |
| Transports              | HTTP / Custom                           | All NServiceBus Transports / HTTP                                                       |
| Topology                | gateway-in-endpoint                     | gateway-per-site                                                                        |
| Specify destination     | When sending                            | Via configuration or message content                                                           |
| Failure handling        | <ul style="padding-left: 20px;"><li>Immediate retries</li><li>Delayed retries</li></ul> | <ul style="padding-left: 20px;"><li>Immediate retries only</li></ul>                                 |
| Automatic deduplication | Built-in via persistence                | No                                                                                      |


## Topology

A wormhole-enabled system consists of *sites*. Sites can be geographically distributed. It is assumed that the connectivity within the site is better than between the sites. Each site forms an independent NServiceBus system with its own infrastructure, for example, local instances of [ServiceControl](/servicecontrol/) and [ServicePulse](/servicepulse/).

Each site hosts a wormhole *gateway* which is a special node that allows to send messages to remote sites. Any given message type can be configured to be routed to the gateway (instead of to a regular endpoint). 

Wormhole *tunnels* connect the gateways. Tunnels are always single-hop so a message always travels through exactly two gateways: it enters the tunnel through a gateway in the origin site and leaves it through a gateway in the destination site.

The transport used to implement the tunnels must be the same (and be configured with the same parameters) for all gateways that take part in a given system.


## Transports

The key advantage of the wormhole is the ability to run any NServiceBus transport, in addition to the built-in HTTP transport, for exchanging messages between the sites. For interoperability reasons the most useful transports for the wormhole are these based on HTTP protocol.


### HTTP

The HTTP transport runs an HTTP listener in each wormhole gateway, similar to the standard NServiceBus gateway. The advantage of a listener-based approach is immunity to connection problems in the tunnel. When the connection is down, the messages queue up on the sender, waiting to be forwarded.

The disadvantage is the need to expose an HTTP endpoint in the public network.


### Azure transports

As an alternative to the HTTP transport, Azure transports (both Service Bus and Storage Queues) can be used as a tunnel transport. The advantage of Azure transports is the ability to run the wormhole nodes behind a firewall without the need to open ports since the transport client does not run the HTTP listener.

A disadvantage of the Azure transports is the need for the connection to be open constantly because the receive is implemented via long polling.


## Routing


The routing in a wormhole is broken down into three areas:

 * The origin endpoint must be configured to route messages of a given type to the local gateway
 * The origin site gateway must have a tunnel configured for the destination site
 * The destination site gateway must know which endpoint should receive the message


### Origin endpoint

The origin endpoint specifies the destination sites for a given message, either statically or based on the message content.

snippet: DestinationSites


### Origin gateway

In order to pass messages between the sites, the wormhole gateways need to know their counterparts. The following code configures the two-way routing between `SiteA` and `SiteB`.

snippet: RemoteSiteA

snippet: RemoteSiteB


### Destination gateway

The gateway in the destination site must be configured to route the message to the ultimate destination endpoint. The following code shows how to configure the routing at type, namespace and assembly level.

snippet: DestinationEndpoints


#### Sender-side distribution

[Federated transports](/transports/types.md#federated-transports), such as MSMQ, allow scaling out via [sender-side distribution](/transports/msmq/sender-side-distribution.md). The wormhole allows configuring the sender-side distribution in the destination site.

snippet: SSD

NOTE: The snippet above contains a hard-coded list of instances. In a real system it is better to load the list of instances from a configuration file or database. The [file-based routing](/nservicebus/messaging/file-based-routing.md) extension is a good starting point to see how this can be implemented.


## De-duplication

The messages traveling through a wormhole can get duplicated along the way, between the endpoints and the wormhole nodes or in the wormhole tunnel. Even when using a DTC-capable transport, like MSMQ or SQL Server, on both ends, the message can get duplicated when entering or exiting the tunnel. The wormhole does not come with an integrated message deduplication mechanism.

Wormhole does, however, preserve the message ID between the source and the ultimate destination. The message ID can be used to deduplicate at the destination. If the destination endpoint uses the [outbox](/nservicebus/outbox/) the deduplication will be done automatically.