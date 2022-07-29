## Connecting to multiple cluster nodes

When connecting to a RabbitMQ cluster, it is beneficial if endpoints are able to connect to any of the nodes in the cluster. For example, if a node goes down, the endpoint can attempt to reconnect to a different node and continue operation.

Since endpoint connection strings are limited to specifying a single hostname, the `AddClusterNode` API can be used to tell the endpoint about additional cluster nodes:

snippet: rabbitmq-add-cluster-node

There is another overload to specify a non-default port:

snippet: rabbitmq-add-cluster-node-with-port