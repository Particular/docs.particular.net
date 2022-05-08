Routing topologies are used to control how queues, exchanges, and bindings are created on the RabbitMQ broker. In version 5 and above, selecting a routing topology is mandatory. For backwards compatibility, use the `ConventionalRoutingTopology`, which was the previous default:

snippet: rabbitmq-config-useconventionalroutingtopology

See the [routing topology documentation](/transports/rabbitmq/routing-topology.md) for further details.
