---
title: Hosting Gateway with Service Fabric
related:
 - nservicebus/service-fabric
reviewed: 2017-03-30
---

When adopting Service Fabric, it's not uncommon that the Service Fabric hosted endpoints need to interact with endpoints outside of the cluster. This can get tricky especially when the endpoints inside Service Fabric are stateful. When integrating using client side distribution, or when using the Service Fabric built in [reverse proxy](https://docs.microsoft.com/nl-nl/azure/service-fabric/service-fabric-reverseproxy) to expose the endpoints as web services, then the partition information needs to be provided by the consumer, which is often not desired.

Alternatively the [nservicebus gateway](/nservicebus/gateway/) can be leveraged as an intermediary to solve this problem. It provides reliable request reply semantics, with deduplication, between sites.

To host the NServiceBus Gateway in an endpoint deployed to Service Fabric, the following has to be taken into account:

1. Host the gateway as a [stateless service](/nservicebus/hosting/service-fabric/hosting/#stateful service), and use [partition aware routing](/samples/azure/azure-service-fabric-routing/) within to forward messages to other parts of the cluster.
2. Make sure the endpoint is present on all service fabric cluster instances, by specifying -1 for the `InstanceCount` value.
3. Add a second communication listener that opens up communication on the gateway address using the local FQDN

(Similar to https://github.com/adam3039/SampleNsbAsfProject/blob/master/WebApi/OwinCommunicationListener.cs#L95, just return the right address but don't open a port as the gateway is already doing that)

4. Configure the gateway channel address to use a URL with wildcard as the public IP address will differ from the one that the HTTP communication listener is effectively listening on. 

Snippet: configureWildcardGatewayChannel
