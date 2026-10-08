### Customizing the transaction isolation level

Use the following API to configure the transaction isolation level for outbox operations:

snippet: OutboxTransactionIsolation

> [!NOTE]
> The default isolation level is `Serializable`. The `Chaos`, `ReadUncommitted`, `Snapshot`, and `Unspecified` isolation levels are not supported. The outbox uses pessimistic locking to prevent concurrent duplicate message processing.
