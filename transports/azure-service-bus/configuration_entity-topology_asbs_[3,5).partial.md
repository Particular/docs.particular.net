### Topology

* `Topology`: The topology used to publish and subscribe to events between endpoints. The topology is shared by the endpoints that need to publish and subscribe to events from each other. Defaults to `Topology.DefaultBundle`, which represents a single topic called `bundle-1`.

It is possible to override the single topic topology from the default bundle to a custom bundle:

snippet: custom-single-topology

For large systems that are approaching the [topology limits](/transports/azure-service-bus/topology.md#quotas-and-limitations) it is possible to create a topology hierarchy:

snippet: custom-topology-hierarchy

> [!NOTE]
> Carefully read the [topology limitation guidelines](/transports/azure-service-bus/topology.md#quotas-and-limitations) before using topology hierarchies.

Topic names must adhere to the limits outlined in [the Microsoft documentation on topic creation](https://docs.microsoft.com/en-us/rest/api/servicebus/create-topic).