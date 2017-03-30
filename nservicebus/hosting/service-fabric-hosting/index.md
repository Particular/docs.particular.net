---
title: Service Fabric Hosting
related:
 - nservicebus/service-fabric
 - samples/azure/azure-service-fabric-routing
reviewed: 2017-03-30
---

NServiceBus endpoints can be hosted with Service Fabric using Reliable Services, using any of the three options:

1. Stateless services
1. Stateful services
1. Guest executable 

To decide what option to use, refer to [Service Fabric documentation](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-overview) for details, followed by this document.

Note: Actor model is another Service Fabric programming model that is not suitable for hosting NServiceBus endpoints.
  

### Stateless service

Hosting with Stateless service is very similar to any other Azure-based hosting (using [Cloud services](/nservicebus/hosting/cloud-services-host) or [self-hosting](/nservicebus/hosting/#self-hosting) with [Azure App Service](https://docs.microsoft.com/en-us/azure/app-service/)). Endpoints are stateless and use external persistence to store and data or state for its operation. Endpoints can be scaled out, leveraging competing consumer on the transport level.

With stateless services, number of instance of a service can be between one and number of nodes in a cluster. Endpoints are self hosted and require Service Fabric [`ICommunicationListener`](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-communication) to be implemented.

snippet: EndpointCommunicationListener


### Stateful service

Unlike with stateless service, whenever data sharding is required and data must be close to the processing, stateful service option offers reliable collections to solve the problem. This poses certain constrains that need to be taken into consideration:

- Service partitioning schema must be well defined upfront
- Messages must be routed among the shards according to the partitioning schema
- There cannot be more than one actively processing instance (`replica`) of an endpoint per shard

See [Service Fabric Partition Aware Routing](/samples/azure/azure-service-fabric-routing) on how to host NServiceBus with stateful services.

NServiceBus provides native implementation for Sagas and the Outbox feature to support endpoints hosted as stateful services. See [Service Fabric persistence](/nservicebus/service-fabric) for details.


### Guest Executable

[Guest executable](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-deploy-existing-app) option allows packaging and deployment of an an existing endpoint into service fabric with a minimal or no change at all. Service Fabric treats guest executable as Stateless services.

This option can be used as an interim solution for the endpoints that need to be eventually converted to Service Fabric services, but cannot be converted right away.

WARNING: MSMQ transport should not be used. Replace it with any other transport.