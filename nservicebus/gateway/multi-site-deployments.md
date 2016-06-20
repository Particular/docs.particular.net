---
title: Gateway And Multi-Site Deployments
summary: Explanation of how NServiceBus handles multi-site communication.
redirects:
 - nservicebus/the-gateway-and-multi-site-distribution
related:
 - samples/gateway
---

The number of multi-site deployments of enterprise .NET systems are increasing due to the challenges of high availability and the requirement for faster response times for users, as the servers and data they access is closer.

RPC technologies quickly run into trouble in these environments as they make machines in the same site and those in remote sites look the same.

In these cases, messaging is better than RPC, but many developers mistakenly represent physical site boundaries as logical boundaries, resulting in problems. NServiceBus prevents developers from going down the wrong path but may leave them wondering how NServiceBus handles multi-site communication.


## Disaster recovery and physical sites

In some cases, physical sites are replicas of one other. This is a common configuration for the purposes of disaster recovery and is largely influenced by technology, cost, and performance.

![Disaster Recovery](disaster-recovery.png)

NServiceBus provides no special facilities for disaster recovery other than to enable developers to plug in their own specific technologies. This can take the form of database replication of subscription information, configuring MSMQ to store its message data on a SAN, etc. The difference in price and performance of the various options is quite large and is not covered here.

The next section describes the use of NServiceBus in logically significant, physical sites.


## Logically significant physical sites

While each branch of a bank or retail store has significance in each domain, when looking at the behavior of each site there is a great deal of similarity even to the point of identical functionality. This may not be true across all sites, especially when examining sites that serve as regional centers or headquarters.

![Logically significant physical sites](distributed-sites.png)

The logical services that make up the business solution can have components installed at multiple physical sites. Some of the components may be the same; others may be different. Multiple logical services in the same site often collaborate closely with each other, and possibly less closely than with their own components at other sites.

For example, expect the Sales service in a store to talk to the pricing service in the same store for every transaction. On the other hand, the pricing service at the headquarters most likely pushes updated prices daily at most to the stores. Similarly, expect an end-of-day push of transactions from the sales service at each store to the headquarters.

![Store to headquarters pricing and sales interaction](store-to-headquarters-pricing-and-sales.png)

This approach is not only common but is recommended for use in situations where physical sites have logical significance, keeping all inter-site communication within logical service boundaries.


## Intra-service cross-site messaging

When sites have logical significance, the messages passed between them are different from the messages sent within the site.

For example, the act of publishing prices from the headquarters has logical significance. The manager of a store explicitly performs an end-of-day operation after collecting and counting all cash in the tills. Therefore, design separate classes for the messages passed between sites.


## Cross-site data transfer

Depending on the network technology, it is possible to set up a virtual private network (VPN) between sites. This provides Windows networking visibility of queues in the target site from the sending site. Use standard NServiceBus APIs to direct messages to their relevant targets, in the form of `Bus.Send(toDestination, msg);`.

This model is recommended as it provides all the benefits of durable messaging between unreliably connecting machines; at several sites, the same as within a single site. It is possible to read a great deal of information on [setting up and managing a Windows VPN](https://technet.microsoft.com/en-US/network/dd420463).

In cases where only have access to HTTP for connection between sites, it is possible to enable the NServiceBus Gateway on each site so it transmits messages from a queue in one site to a queue in another site, including the hash of the messages to ensure that the message is transmitted correctly. The following diagram shows how it works:

![Gateway Headquarter to Site A](gateway-headquarter-to-site-a.png) 

The sending process in site A sends a message to the gateway's input queue. The gateway then initiates an HTTP connection to its configured target site. The gateway in site B accepts HTTP connections, takes the message transmitted, hashes it, and returns the hash to site A. If the hashes match, the gateway in site B transmits the message it receives to a configured queue. If the hashes don't match, the gateway in site A re-transmits.


## Configuration and code

When configuring the client endpoint, ensure the [Message Owner](/nservicebus/messaging/message-owner.md) has been configured so that the relevant message types go to the gateway's input queue.

To send a message to a remote site, use the `SendToSites` API call, as shown:

snippet:SendToSites

This values (`SiteA` and `SiteB`) is the list of remote sites where the message(s) are sent.


### Configuring Destination

While these URLs can be placed directly in the call, it recommend to put these settings in `app.config` so  administrators can change them should the need arise. To do this, add this config section:


#### Using App.Config

snippet:GatewaySitesAppConfig


Or specify this physical routing in code:


#### Using a IConfigurationProvider

snippet:GatewaySitesConfigurationProvider


#### Using a ConfigurationSource

snippet:GatewaySitesConfigurationSource

Then at configuration time:

snippet:UseCustomConfigurationSourceForGatewaySitesConfig


NServiceBus automatically sets the required headers to enable sending messages back over the gateway using the familiar `Bus.Reply`.

NOTE: All cross-site interactions are performed internally to a service, so publish and subscribe are not supported across gateways.


## Securing the gateway with SSL

To provide data encryption for messages transmitted between sites, configure SSL on the machines in each site where the gateway is running.

Follow the steps for [configuring SSL](https://msdn.microsoft.com/en-us/library/ms733768.aspx) and make sure to configure the gateway to listen on the appropriate port, as well as to contact the remote gateway on the same port.


## Automatic de-duplication

Going across alternate channels like HTTP means that the MSMQ safety guarantees of exactly one message are lost. This means that communication errors resulting in retries can lead to receiving messages more than once. To avoid being burdened with de-duplication, the NServiceBus gateway supports this out of the box. Message IDs are stored in the configured [Persistence](/nservicebus/persistence/) so potential duplicates can be detected.


### Versions 5 and above

The gateway will use the storage type configured. At this stage [InMemory](/nservicebus/persistence/in-memory.md), [NHibernate](/nservicebus/nhibernate/) and [RavenDB](/nservicebus/ravendb/) is supported.


### Version 4

By default, NServiceBus uses [RavenDB](/nservicebus/ravendb/) to store the IDs but [InMemory](/nservicebus/persistence/in-memory.md) and [NHibernate](/nservicebus/nhibernate/) persistences are supported as well.


## Incoming channels

When the gateway is enabled it automatically sets up an HTTP channel to listen to `http://localhost/{name of the endpoint}`. To change this URL or add more than one incoming channel, configure `app.config`, as shown:


#### Using App.Config

snippet:GatewayChannelsAppConfig

Or specify the physical routing in code:


#### Using a IConfigurationProvider

snippet:GatewayChannelsConfigurationProvider


#### Using a ConfigurationSource

snippet:GatewayChannelsConfigurationSource

Then at configuration time:

snippet:UseCustomConfigurationSourceForGatewayChannelsConfig


The `Default` on the first channel tells the gateway which address to attach on outgoing messages if the sender does not specify it explicitly. Any number of channels can be added.

Follow the steps for [configuring SSL](https://msdn.microsoft.com/en-us/library/ms733768.aspx) and make sure to configure the gateway to listen on the appropriate port, as well as to contact the remote gateway on the same port.