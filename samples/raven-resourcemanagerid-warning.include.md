{{WARNING: When running RavenDB Persister Samples the following warning will be logged.

> NServiceBus has detected that a RavenDB DocumentStore is being used with Distributed Transaction Coordinator transactions, but without the recommended production-safe settings for ResourceManagerId or TransactionStorageRecovery.

The reason for this is that, for simplicity, the [Raven Persister DTC settings](/nservicebus/ravendb/manual-dtc-settings.md#configuring-safe-settings) have not been configured for production scenarios.
}}