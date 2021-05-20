### Connecting to additional nodes

Nodes within a cluster can be specified by providing the host and port details to the `AddClusterNode` method of the base `RabbitMQClusterTransport` class. When multiple nodes have been specified, the RabbitMQ connection factory will choose a node at random to connect to. This occurs during reconnection scenarios too.