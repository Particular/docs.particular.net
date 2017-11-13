## Effect on RavenDB DocumentStore

When the Outbox is enabled, the default [transport transaction level](/transports/transactions.md) will be set so that the endpoint does not utilize distributed transactions. As a result of this, the RavenDB persistence will alter the RavenDB DocumentStore so that it also does not enlist in distributed transactions.

Although this will automatically occur when the endpoint initializes, it's still recommended to set disable RavenDB enlistment in DTC transactions in order to be explicit:

snippet: OutboxDisableDocStoreDtc

When the RavenDB Outbox is enabled none of the following properties, on the RavenDB DocumentStore, should be set. They are only used by RavenDB's DTC implementation:

* `ResourceManagerId`
* `TransactionRecoveryStorage`