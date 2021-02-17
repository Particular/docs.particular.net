## Supported clustering configurations

As of NServiceBus.RavenDB version 6.5, all clustering modes are supported. For clusters of three or more nodes, it is recommended to use transactions to ensure consistency throughout the cluster.

Optimistic locking is not supported when cluster-wide transactions are enabled.
