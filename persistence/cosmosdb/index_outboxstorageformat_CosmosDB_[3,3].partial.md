#### Version 3.2 and up

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

#### Version 3.1 and under

> [!NOTE]
> **Versions `v3.1.3` and up:** For control messages, a default partition key in the format `{endpointName}-{messageId}` will be used. As a result, multiple logical endpoints _can_ share the same database and container.

> [!WARNING]
> **Versions `v3.1.2` and under:** For control messages, a default partition key in the format `{messageId}` will be used, however these Outbox records are not separated by endpoint name. As a result, multiple logical endpoints _cannot_ share the same database and container since [message identities are not unique across endpoints from a processing perspective](/nservicebus/outbox/#message-identity). To avoid conflicts, either separate different endpoints into different containers, [override the partition key](transactions.md), or update to `NServiceBus.Persistence.CosmosDB 3.1.3`.