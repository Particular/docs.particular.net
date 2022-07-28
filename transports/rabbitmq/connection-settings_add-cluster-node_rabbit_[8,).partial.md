## Connecting to multiple cluster nodes

The connection string for an endpoint allows one host name to be specified. When connecting to a RabbitMQ cluster, it can be useful for the endpoint to know about all of the nodes in the cluster. If a node goes down, the endpoint can attempt to reconnect to a different node.

The endpoint can be told about additional nodes in the cluster using the `AddClusterNode` API:

snippet: rabbitmq-add-cluster-node

There is another overload to specify a non-default port:

snippet: rabbitmq-add-cluster-node-with-port