---
title: Namespace hierarchy support
summary: Configuring Azure Service Bus transport to support namespace hierarchies.
component: ASB
versions: "[7,)"
reviewed: 2021-05-11
redirects:
 - nservicebus/azure-service-bus/namespace-hierarchy
 - transports/azure-service-bus/namespace-hierarchy
---

include: legacy-asb-warning

At the core of the Azure Service Bus service, there is a Service Registry which tracks the location of each queue, topic, relay or eventhub in the service. This Service Registry provides a DNS integrated hierarchical naming system, that has a root entry point (called a namespace) at a URI with the following scheme.

```
https://{serviceNamespace}.servicebus.windows.net/{path}
```

The {path} parameter can be specified at any depth, allowing the hierarchy to be extended with application specific sub trees. This is very useful for large systems to group, manage and secure resources more efficiently, for example:

```
https://mynamespace.servicebus.Windows.net/production/tenant1/sales
https://mynamespace.servicebus.Windows.net/production/tenant1/operations
https://mynamespace.servicebus.Windows.net/acceptance/tenant1/sales
https://mynamespace.servicebus.Windows.net/acceptance/tenant1/operations
```

For more information about this feature refer to the [Addressing and Protocol](https://docs.microsoft.com/en-us/rest/api/servicebus/Addressing-and-Protocol) article on MSDN.


## Positioning an endpoint in the hierarchy

NServiceBus provides to an endpoint the capability to register its Azure Service Bus transport resources inside a namespace hierarchy by replacing part of the addressing logic - the composition strategy. The composition strategy is responsible for determining the path to an entity in the namespace. Out of the box, the transport comes with two implementations - `FlatComposition` and `HierarchyComposition` strategies. The `FlatComposition` strategy is the default and applies no composition logic, resulting in a flat namespace hierarchy. The `HierarchyComposition` strategy allows to specify a lambda expression that can calculate the path for each entity.

snippet: asb-hierarchy-composition

Note that the path generator is a lambda function, so it will be invoked each time the transport wants to determine the location of a given entity. This function must meet the following rules:

 * Returns the path to the entity with a trailing slash (`/`).
 * **Does not** append the entity name itself.


## Implementing a custom composition strategy

It is also possible to provide a custom composition strategy by implementing `ICompositionStrategy`, which might be beneficial for example with regards to performance.

snippet: asb-custom-composition-strategy

The implementation of the `ICompositionStrategy` needs to be registered:

snippet: asb-custom-composition-config
