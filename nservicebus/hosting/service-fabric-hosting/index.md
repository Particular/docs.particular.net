---
title: Service Fabric Hosting
related:
 - nservicebus/service-fabric
 - samples/azure/azure-service-fabric-routing
reviewed: 2017-03-29
---

NServiceBus endpoints can be hosted with Service Fabric using Reliable Services using, using any of the three options:

1. Stateless services
1. Stateful services
1. Guest executables

To choose the right option, see [Service Fabric documentation](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-overview) for details on each of the options.
  

### Stateless service

Hosting with Stateless service is very similar to a any other Azure-based hosting (using [Cloud services](/nservicebus/hosting/cloud-services-host) or [self-hosting](/nservicebus/hosting/#self-hosting) with Azure App Plan). Endpoints are stateless and use external persistence to store and data or state for its operation.

TODO:
- Elaborate on scale-out with # of instances
- Communication listener (general, outside of this block)
- Code snippet(s) if needed

### Stateful service

Whenever data sharding is required and data must be close to the processing, stateful service offer reliable collections to solve the problem. This does pose certain constrains on the endpoints.

- Messages must be routed among shards according to the partitioning schema
- There cannot be more than one instance (`replica`) of an endpoint per shard actively processing messages
- Partitioning schema must be well defined

TODO:
- do more to emphasize how different it is from the stateless model
- point to the sample for routing
- elaborate on the SF persitence package
- snippets if needed    


### Guest Executable

[Guest executable](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-deploy-existing-app) option allows packaging and deployment of an an existing endpoint into service fabric with a minimal or no change at all.

WARNING: MSMQ transport should not be used. Replace it with any other transport.

TODO:
- elaborate on some details of guest executables (similar to stateless)
- sample scenario 