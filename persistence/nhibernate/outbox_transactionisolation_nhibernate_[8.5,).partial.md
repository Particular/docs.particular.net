### Customizing the transaction isolation level

The transaction isolation level for the outbox operation can be specified:

snippet: OutboxTransactionIsolation

> [!NOTE]
> The default isolation level is `Serializable`. The isolation level values of `Chaos`, `ReadUncommitted`, `Snapshot` and `Unspecified` are not allowed. Outbox relies on pessimistic locking to prevent concurrent more-than-once invocation.
