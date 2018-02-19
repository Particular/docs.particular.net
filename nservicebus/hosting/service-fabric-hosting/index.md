---
title: Service Fabric Hosting
related:
 - persistence/service-fabric
 - samples/azure/azure-service-fabric-routing
reviewed: 2018-02-19
---

NServiceBus endpoints can be hosted in Service Fabric using any of these three options:

1. Stateless services
1. Stateful services
1. Guest executable

Refer to the [Service Fabric documentation](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-overview) for more information on when to use each option.

Note: The Actor model is another Service Fabric programming model. It is not currently supported by NServiceBus.

### Stateless service

Hosting with a stateless service is similar to other Azure-based hosting (using [cloud services](/nservicebus/hosting/cloud-services-host) or [self-hosting](/nservicebus/hosting/#self-hosting) with [Azure App Service](https://docs.microsoft.com/en-us/azure/app-service/)). Endpoints are stateless and use storage external to Service Fabric for managing data needed for their operation. Endpoints can be scaled out, leveraging [competing consumer](/transports/scale-out.md#broker-transports) at the transport level.

With stateless services, the number of instances of a service can range from one to the number of nodes in a cluster less one. Endpoints are self hosted and should be started using a custom Service Fabric [`ICommunicationListener`](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-communication) implementation.

snippet: StatelessEndpointCommunicationListener

NOTE: If Stateless services are used to host web application frameworks such as ASP.NET, ASP.NET Core or WebAPI it is recommended to start the endpoint inside the service collection extensions instead of the communication listener as shown in [using NServiceBus in an ASP.NET Core WebAPI Application](/samples/web/send-from-aspnetcore-webapi/). This approach is simpler than having to coordinate multiple communication listener since no coordination is needed between the listeners. If only Stateless services are used to host web application frameworks it might be useful to consider Azure AppServices. For more guidance refer to the [Azure App Service, Virtual Machines, Service Fabric, and Cloud Services comparison](https://docs.microsoft.com/en-us/azure/app-service/choose-web-site-cloud-service-vm) guidance.

### Stateful service

The process of configuring and starting an endpoint hosted in a stateful service is very similar to stateless service hosting approach. The main difference is that the endpoint cannot be started until the `OpenAsync` method has returned as the `StateManager` instance has not been fully configured yet. This is trivial to solve by calling the `Endpoint.Start` operation from the `RunAsync` operation instead.

snippet: StatefulEndpointCommunicationListener

snippet: StatefulService

Due to characteristics of stateful services, i.e. data partitioning and local data storage, additional configuration aspects must be considered:

- The service partitioning schema must be well-defined before the first deployment occurs (repartitioning requires all data to be deleted)
- Messages must be routed among the shards according to the partitioning schema
- A single instance of an NServiceBus endpoint will be running on the primary replica for each partition

See [Service Fabric Partition Aware Routing](/samples/azure/azure-service-fabric-routing) for more information on how to host NServiceBus with stateful services and to learn how to configure routing between service partitions and persist data in reliable collections.

NServiceBus provides persistence based on reliable collections for saga and outbox data. See [Service Fabric persistence](/persistence/service-fabric) for details.


### Guest executable

The [guest executable](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-deploy-existing-app) option allows packaging and deployment of an an existing endpoint into service fabric with minimal or no change at all. Service Fabric treats guest executable as stateless services.

This option can be used as an interim solution for the endpoints that eventually need to be converted to Service Fabric services, but cannot be converted right away.

WARNING: While executables can be packaged and deployed to Service Fabric without much effort, the Service Fabric hosting environment might not support all local dependencies. For example, endpoints running on the MSMQ transport should be migrated to another transport. The reason for this is that Service Fabric reallocates processes to different machines based on metrics such as CPU load. But these local dependencies will not move along with it.


## Hosting NServiceBus in a standalone cluster

Service Fabric can be deployed to run in any environment that contains a set of interconnected Windows Server machines. See the [official documentation](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-cluster-creation-for-windows-server) for details.

On Azure, Service Fabric can be hosted with [Azure Service Fabric service](https://azure.microsoft.com/en-us/services/service-fabric/).
