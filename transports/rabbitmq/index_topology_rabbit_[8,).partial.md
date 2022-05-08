Routing topologies are used to control how queues, exchanges, and bindings are created on the RabbitMQ broker. Since version 5, selecting a routing topology is mandatory. For new deployments the `ConventionalRoutingTopology` (previously the default) should be selected:

snippet: rabbitmq-config-useconventionalroutingtopology

See the [routing topology documentation](/transports/rabbitmq/routing-topology.md) for further details.
