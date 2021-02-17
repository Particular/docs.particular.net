## Supported clustering configurations

NServiceBus.RavenDB versions 6.5 and above support all clustering modes of RavenDB, including multi-master configurations.

To enable cluster-wide transactions in NServiceBus.RavenDB:

snippet: ravendb-cluster-wide-transactions

The `UseClusterWideTransactions` method must be called on the endpoint configuration regardless of whether a RavenDB session is provided to the configuration, or it is provided internally by the RavenDB persister. That is, even if a pre-configured RavenDB session is provided with cluster-wide transactions already enabled on the session, the `UseClusterWideTransactions` method must still be called. If the method is not called, the persister's operations will not enlist in the session's transaction and data could be left in an inconsistent state.

## Optimistic locking not supported with clusters

Optimistic locking is not supported when cluster-wide transactions are enabled. Starting with NServiceBus.RavenDB version 7.0, pessimistic locking is the default setting. Prior to that, it must be explicitly enabled:

snippet: ravendb-sagas-pessimistic-lock