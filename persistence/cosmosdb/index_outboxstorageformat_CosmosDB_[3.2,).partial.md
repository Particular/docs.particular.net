A default [synthetic partition key](https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/synthetic-partition-keys) will be used for all incoming messages, in the format `{endpointName}-{messageId}`, if not explicitly [overwritten](/persistence/cosmosdb/transactions.md#specifying-the-partitionkey-to-use-for-the-transaction) at runtime.

> [!NOTE]
> The [default partition key should be overwritten](/persistence/cosmosdb/transactions.md#specifying-the-partitionkey-to-use-for-the-transaction) whenever the message handler creates or updates business records in CosmosDB. To guarantee atomicity, explicitly set the Outbox partition key to match the partition key of your business record. This is a requirement for including both the business record and the Outbox record in the same [Cosmos DB transactional batch](https://learn.microsoft.com/en-us/azure/cosmos-db/partitioning-overview). Conversely, for simplicity, you can use the default partition key when a handler's logic does not involve persisting business data.

To support backward compatibility of control messages during migration, the persistence includes a fallback mechanism. When enabled (default), and if a record is not found using the synthetic key format, the system falls back to the legacy `{messageId}` format. Since the fallback mechanism involves an additional read operation on the Outbox container, it is recommended to turn it off once all legacy records have expired.

```csharp
endpointConfiguration
    .EnableOutbox()
    .DisableReadFallback();
```

> [!WARNING]
> Since [message identities are not unique across endpoints from a processing perspective](/nservicebus/outbox/#message-identity), when overwriting the default synthetic key, either separate different endpoints into different containers or [override the default synthetic partition key](transactions.md) in a way that ensures message identities are unique to each processing endpoint.