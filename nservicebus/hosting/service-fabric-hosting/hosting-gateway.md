---
title: Hosting Gateway with Service Fabric
related:
 - persistence/service-fabric
reviewed: 2020-11-26
---

WARNING: [Gateway](/nservicebus/gateway) hosted in Service Fabric does not support forwarding messages containing [Databus](/nservicebus/messaging/databus/) properties.

When adopting Service Fabric, it's not uncommon that the Service Fabric hosted endpoints need to interact with endpoints outside of the cluster. This can get tricky especially when the endpoints inside Service Fabric are stateful. When integrating using sender side distribution, or when using the Service Fabric built-in [reverse proxy](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reverseproxy) to expose the endpoints as web services, the partition information needs to be provided by the consumer.

Alternatively the [NServiceBus gateway](/nservicebus/gateway/) can be leveraged as an intermediary to solve this problem. It provides reliable request-reply semantics, with deduplication between sites.

To host the NServiceBus Gateway in an endpoint deployed to Service Fabric, the following has to be taken into account:

1. Host the gateway as a [stateless service](/nservicebus/hosting/service-fabric-hosting/#stateless-service), and use [partition aware routing](/samples/azure/azure-service-fabric-routing/) to forward messages to other parts of the cluster.
2. Because the service instances are hosted behind a load balancer, it is required to configure the gateway channel address to use a URL with a wildcard. The reason is that the public IP address is the one from the cluster load balancer and will therefore differ from the one that the gateway communication listener will effectively be listening on.

Snippet: configureWildcardGatewayChannel

3. Open the correct port for the gateway on the cluster load balancer by specifying an `Endpoint` entry in the service manifest.

Snippet: serviceManifestEndpoint

4. Add a second communication listener that returns the gateway address using the local FQDN instead of the + sign, so that Service Fabric instance can set up the correct ACL on the host machine.

Snippet: GatewayCommunicationListener

5. Make sure the host service is present on all Service Fabric cluster instances, by specifying -1 for the `InstanceCount` value.
