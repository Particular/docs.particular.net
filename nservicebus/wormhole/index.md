---
title: Sending messages between geographically separate sites using Worm Hole Gateway
summary: How to send messages between geographically separate sites using Worm Hole Gateway 
component: WormHole
reviewed: 2017-05-20
---

`NServiceBus.WormHole` is an alternative to the [Gateway](/nservicebus/gateway/) component of NServiceBus. Following table compares the most important aspects of both technologies.

|                         | Gateway                                 | WormHole                                                                                |
|-------------------------|-----------------------------------------|-----------------------------------------------------------------------------------------|
| Encryption              | transport-level                         | no                                                                                      |
| Transports              | <ul><li>HTTP</li><li>custom</li></ul>   | <ul><li>all NServiceBus transports</li><li>HTTP</li></ul>                               |
| Topology                | gateway-in-endpoint                     | gateway-per-site                                                                        |
| Routing                 | set destination site when sending       | <ul><li>set destination site via routing config</li><li>set destination site from message content</li></ul> |
| Failure handling        | <ul><li>immediate retries</li><li>delayed retries</li></ul> | <ul><li>immediate retries</li></ul>                                 |
| Automatic deduplication | built-in via persistence                | no                                                                                      |
|                         |                                         |                                                                                         |
|                         |                                         |                                                                                         |
|                         |                                         |                                                                                         |

## Transports

The key advantage of Worm Hole is the ability to run any NServiceBus transport, in addition to built-in HTTP transport, for exchanging messages between the sites. For interoperability reasons the most useful transports for the Worm Hole are these based on HTTP protocol.


### HTTP

The HTTP transport runs a HTTP listener in each Worm Hole node in a similar way to the standard NServiceBus Gateway. This allows the Worm Hole to be run in a limited connectivity environments because the listener is not affected by network problems between the sender and itself. When the connection is down the messages queue up on the sender side, waiting to be forwarded.


### ASB and ASQ

As an alternative to the HTTP transport, Azure transports (ASB or ASQ) can be used for the Worm Hole backplane. The advantage of Azure transports is ability to run the Worm Hole nodes behind firewall without the need to open any ports as the ASB/ASQ client does not run the HTTP listener.

The downside of ASB/ASQ is the need for the connection to up on all the time.

## Routing

The routing in Worm Hole is broken down into three areas:
 * Which sites should a message be sent to
 * What is the address for the Worm Hole node for a given site
 * In the destination site, which endpoint should receive the message


### Sender

The sending endpoint specifies the destination sites for a given message, either statically or based on the message content.

snippet: DestinationSites


### Gateway

In order to pass messages between the sites the Worm Hole gateways need to know their counterparts. Following code configures the two-way routing between gateway in `SiteA` and in `SiteB`.

snippet: RemoteSiteA
snippet: RemoteSiteB  


### Receiver

Last, but not least the gateway in the destination site needs to be configured to route the message to the ultimate destination endpoint. Following code show how to configure the routing on type-, namespace- and assembly-level.

snippet: DestinationEndpoints

#### Sender-side distribution

[Bus transports](/nservicebus/transports/#types-of-transports-bus-transports), such as MSMQ allow scaling out via [sender-side distribution](/nservicebus/msmq/sender-side-distribution.md). The Worm Hole allows to configure the sender-side distribution in the destination site.

snippet: SSD

NOTE: The snippet above contains a hard-coded list of instances. In real system it might be better to load the list of instances from a configuration file or database. The [file-based routing](/nservicebus/messaging/file-based-routing) extensions is a good starting point to see how this can be implemented.

## De-duplication

The messages travelling through a Worm Hole can get duplicated along the way, between the endpoints and the Worm Hole nodes or in the Worm Hole tunnel. The Worm Hole does not come with an integrated message de-duplication mechanism.

Worm Hole does, however, preserve the message ID between the source and the ultimate destination. The message ID can be used to de-duplicate at the destination. If the destination endpoint uses the [Outbox](/nservicebus/outbox/) the de-duplication will be done automatically by means of the Outbox mechanism.