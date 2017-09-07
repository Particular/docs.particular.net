Topologies are used to control how native Rabbit constructs like queues and exchanges are created. In Versions 5 and above, selecting a topology is mandatory. For backwards compatiblility use the `ConventionalRoutingTopology`, which was the previous default:

snippet: rabbitmq-config-topology

See the [routing topology documentation](/transports/rabbitmq/routing-topology.md) for further details.
