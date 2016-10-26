## Custom Routing Topology

If the above routing topologies aren't flexible enough, it is possible to take full control over routing by implementing a custom routing topology. To do this:

 1. Define the topology by creating a class implementing `IRoutingTopology`.
 1. Register it with the transport calling `UseRoutingTopology` as shown below.

Version 4.1 onwards:

snippet:rabbitmq-config-useroutingtopology4_1

The Boolean argument supplied to the factory delegate indicates whether the custom routing topology should create durable exchanges and queues on the broker. Read more about durable exchanges and queues in the [AMQP Concepts Guide](https://www.rabbitmq.com/tutorials/amqp-concepts.html).

Version 4.0:

snippet:rabbitmq-config-useroutingtopology4_0
