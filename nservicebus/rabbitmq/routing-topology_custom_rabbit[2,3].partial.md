## Custom Routing Topology

If the above routing topologies aren't flexible enough, it is possible to take full control over routing by implementing a custom routing topology. To do this:

 1. Define the topology by creating a class implementing `IRoutingTopology`.
 1. Register it with the transport calling `UseRoutingTopology` as shown below.

snippet:rabbitmq-config-useroutingtopology
