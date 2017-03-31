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

To decide what option to use, refer to [Service Fabric documentation](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-overview).

Note: Actor model is another Service Fabric programming model that is currently not supported by NServiceBus.

### Stateless service

Hosting with Stateless service is very similar to any other Azure-based hosting (using [Cloud services](/nservicebus/hosting/cloud-services-host) or [self-hosting](/nservicebus/hosting/#self-hosting) with [Azure App Service](https://docs.microsoft.com/en-us/azure/app-service/)). Endpoints are stateless and use storage external to SF for managing data needed for their operation. Endpoints can be scaled out, leveraging competing consumer at the transport level.

With stateless services, number of instances of a service can range from one to number of nodes in a cluster. Endpoints are self hosted and should be started using custom Service Fabric [`ICommunicationListener`](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-communication) implementation.

snippet: EndpointCommunicationListener

### Stateful service

The process of configuring and starting an endpoint hosted in stateful service is similar to stateless service hosting approach. However due to characteristics of Stateful Services i.e. data partitioning and local data storage, additional configuration aspects have to be taken into consideration: 

- Service partitioning schema must be well defined before first deployment
- Messages must be routed among the shards according to the partitioning schema
- There can only be one NServiceBus endpoint running per service partition

See [Service Fabric Partition Aware Routing](/samples/azure/azure-service-fabric-routing) on how to host NServiceBus with stateful services, configure routing between service partitions and persist data in reliable collections.

NServiceBus provides persistence based on reliable collections for Sagas and Outbox data. See [Service Fabric persistence](/nservicebus/service-fabric) for details.


### Guest Executable

[Guest executable](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-deploy-existing-app) option allows packaging and deployment of an an existing endpoint into service fabric with a minimal or no change at all. Service Fabric treats guest executable as Stateless services.

This option can be used as an interim solution for the endpoints that need to be eventually converted to Service Fabric services, but cannot be converted right away.

WARNING: While executables can be packaged and deployed to Service Fabric without much effort, SF hosting environment might not provide all external dependencies e.g. endpoints running on MSMQ transport should be migrated to other transport.