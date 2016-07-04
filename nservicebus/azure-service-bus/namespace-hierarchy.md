---
title: Namespace hierarchy support
summary: Configuring Azure Service Bus transport to support namespace hierarchies. 
component: ASB
tags:
- Cloud
- Azure
- Transports 
---

At the core of the Azure Service Bus service, there is a Service Registry which tracks the location of each queue, topic, relay or eventhub in the service. This Service Registry provides a DNS integrated hierarchical naming system, that has a root entry point (called a namespace) at a uri with the following scheme.

`https://{serviceNamespace}.servicebus.Windows.net/{path}`

The {path} parameter can be specified at any depth, allowing the hierarchy to be extended with application specific sub trees. This is very usefull for large systems to group, manage and secure resources more efficiently.

for example:

```
https://mynamespace.servicebus.Windows.net/production/tenant1/sales
https://mynamespace.servicebus.Windows.net/production/tenant1/operations
https://mynamespace.servicebus.Windows.net/acceptance/tenant1/sales
https://mynamespace.servicebus.Windows.net/acceptance/tenant1/operations
```

For more information about this feature refer to the [Azure Service Bus MSDN documentation on 'Addressing and Protocol'](https://msdn.microsoft.com/en-us/library/azure/hh780781.aspx)

## Positioning an endpoint in the hierarchy

NServiceBus provides the capability to an endpoint to register it's Azure Service Bus transport resources inside a namespace hierarchy by replacing part of the addressing logic, the composition strategy. This strategy is responsible for composing the path to an entity in the namespace. Out of the box, the transport comes with a `HierarchyComposition` strategy that allows to specify a lambda expression that can calculate the path for each entity.

Snippet: asb-hierarchy-composition

Note that the path generator is a lambda function, so it will be invoked each time the transport wants to determine the location of a given entity. This function should return the path to the entity, without appending the entity name itself and without a trailing slash.

## Implementing a custom composition strategy

If for performance or other reasons a lambda function invocation would not be ideal, it is also possible to create a custom composition strategy, by implementing `ICompositionStrategy`.

Snippet: asb-custom-composition-strategy

and register it in the API

Snippet: asb-custom-composition-config