---
title: Azure Cosmos DB Persistence
summary: How to use NServiceBus with Azure Cosmos DB
component: CosmosDB
reviewed: 2025-09-25
related:
- samples/cosmosdb/transactions
- samples/cosmosdb/container
- samples/cosmosdb/simple
redirects:
- previews/cosmosdb
---

The Azure Cosmos DB persister uses the [Azure Cosmos DB](https://azure.microsoft.com/en-us/services/cosmos-db/) NoSQL database service for storage.

> [!WARNING]
> It is important to [read and understand](https://docs.microsoft.com/en-us/azure/cosmos-db/partitioning-overview) partitioning in Azure Cosmos DB before using `NServiceBus.Persistence.CosmosDB`.

## Persistence at a glance

For a description of each feature, see the [persistence at a glance legend](/persistence/#persistence-at-a-glance).

|Feature                    |   |
|:---                       |---
|Supported storage types    |Sagas, Outbox
|Transactions               |Using TransactionalBatch, [with caveats](transactions.md)
|Concurrency control        |Optimistic concurrency, optional pessimistic concurrency
|Scripted deployment        |Not supported
|Installers                 |Container is created by installers.

> [!NOTE]
> The Outbox feature requires partition planning.

## Usage

Add a NuGet package reference to `NServiceBus.Persistence.CosmosDB`. Configure the endpoint to use the persister through the following configuration API:

snippet: CosmosDBUsage

### Token credentials

Using a `TokenCredential` enables the usage of Microsoft Entra ID authentication such as [managed identities for Azure resources](https://learn.microsoft.com/en-us/azure/cosmos-db/role-based-access-control) instead of the requiring a shared secret in the connection string.

A `TokenCredential` can be provided by using the corresponding [`CosmosClient`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.cosmosclient.-ctor?view=azure-dotnet#microsoft-azure-cosmos-cosmosclient-ctor(system-string-azure-core-tokencredential-microsoft-azure-cosmos-cosmosclientoptions)) constructor overload when creating the client passed to the persister.

### Customizing the database used

By default, the persister will store records in a database named `NServiceBus`. This can be overwritten by using the following configuration API:

snippet: CosmosDBDatabaseName

### Customizing the container used

include: defaultcontainer

> [!NOTE]
> The [transactions](transactions.md) documentation details additional options on how to configure NServiceBus to specify the container using the incoming message headers or contents.

### Customizing the `CosmosClient` provider

When the `CosmosClient` is configured and used via dependency injection, a custom provider can be implemented:

snippet: CosmosDBCustomClientProvider

and registered on the container:

snippet: CosmosDBCustomClientProviderRegistration

## Request unit capacity planning

A [Request Unit](https://learn.microsoft.com/en-us/azure/cosmos-db/request-units?context=%2Fazure%2Fcosmos-db%2Fnosql%2Fcontext%2Fcontext) (or RU, for short) is a performance currency abstracting the system resources such as CPU, IOPS, and memory required to perform database operations. RU capacity planning is the process of estimating the required RUs that an Azure Cosmos DB will need to handle its workload.

> [!NOTE]
> RU capacity planning is recommended regardless of [Cosmos DB account type](https://learn.microsoft.com/en-us/azure/cosmos-db/throughput-serverless?context=%2Fazure%2Fcosmos-db%2Fnosql%2Fcontext%2Fcontext#detailed-comparison), however its especially important when [Provisioned Throughput](https://learn.microsoft.com/en-us/azure/cosmos-db/set-throughput?context=%2Fazure%2Fcosmos-db%2Fnosql%2Fcontext%2Fcontext) is selected as reaching the assigned capacity will cause [throttling](https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/troubleshoot-request-rate-too-large?tabs=resource-specific) which can result in duplication of messages. In a [Serverless](https://learn.microsoft.com/en-us/azure/cosmos-db/serverless) acount, you're charged only for the RUs that your database operations consume and for the storage that your data consumes. Planning for the estimated RU usage is still benefitial while using a serverless account from a cost planning perspective.

Microsoft provide a [capacity calculator](https://cosmos.azure.com/capacitycalculator/) which can be used to model the throughput costs of your solution. It uses [several parameters](https://learn.microsoft.com/en-us/azure/cosmos-db/request-units?context=%2Fazure%2Fcosmos-db%2Fnosql%2Fcontext%2Fcontext#request-unit-considerations) to calculate this, but only the following are directly affected by using the NServiceBus CosmosDB Persistence (assuming the outbox and sagas are used).

| Capacity Calculator Parameter | NServiceBus Operations | Cosmos DB API |
| :---- | :---- | :---- |
| Point reads | Outbox deduplication, Saga load, Partition key fallback | `ReadItemStreamAsync` |
| Creates | New outbox record, New saga record | `CreateItemStream` |
| Updates | Saga update, Saga aquire lock, Saga release lock, Outbox dispatched | `ReplaceItemStream`, `UpsertItemStream`, `PatchItemStreamAsync`, `PatchItem` |
| Deletes | Saga complete, Outbox TTL background cleanup | `DeleteItem` |
| Queries | Saga migration mode | `GetItemQueryStreamIterator` |

Each item size also affects RU planning. The below gives an esitmate of the size overhead that should be considered per message.

| Record Type | Typical Size |
| :---- | :---- |
| Outbox | 630 bytes + message body |
| Saga | 300 bytes + saga data |

The below table gives an indication of what Cosmos DB operations occur in different scenarios, for every processed message, per NServiceBus endpoint. This can be used with the Microsoft capacity planner, and other factors that affect pricing (such as the selected Cosmos DB API selected, number of regions, etc), and the message throughput of each NServiceBus endpoint, to produce a RU capacity requirement estimate.

| Scenario | Point Reads | Creates | Updates | Deletes | Queries | Persister Overhead* |
|----------|-------------|---------|---------|---------|---------|---------------------|
| **Message WITH Outbox (No Saga)** | 1 | 1 | 1 | 1 (delayed) | 0 | 630 bytes (1 msg sent) |
| **Message WITH Outbox + Saga (new)** | 2 | 2 | 1 | 1 (delayed) | 0 | 630 + 300 = 930 bytes |
| **Message WITH Outbox + Saga (update)** | 2 | 1 | 2 | 1 (delayed) | 0 | 630 + 300 = 930 bytes |
| **Message WITH Outbox + Saga (complete)** | 2 | 1 | 1 | 2 (delayed) | 0 | 630 + 300 = 930 bytes |
| **Message WITH Outbox + Saga + Locking (no contention)** | 1 | 1-2 | 3-4 | 1-2 (delayed) | 0 | 630 + 360 = 990 bytes |
| **Message WITH Outbox + Saga + Locking (3 retries)** | 1 | 1-2 | 9-10 | 1-2 (delayed) | 0 | 630 + 360 = 990 bytes |
| **Message WITHOUT Outbox (No Saga)** | 0 | 0 | 0 | 0 | 0 | 0 bytes |
| **Message WITHOUT Outbox + Saga (new)** | 1 | 1 | 0 | 0 | 0 | 300 bytes |
| **Message WITHOUT Outbox + Saga (update)** | 1 | 0 | 1 | 0 | 0 | 300 bytes |
| **Message WITHOUT Outbox + Saga (complete)** | 1 | 0 | 0 | 1 | 0 | 300 bytes |

*Persister overhead excludes your message bodies and saga data. Assumes handler sends 1 outgoing message.

**Additional operations** (conditional):

- **Fallback Read**: +1 Point Read (if enabled and using synthetic partition key)
- **Migration Mode**: +1 Query (if saga migration mode enabled and saga not found)
- **Multiple Partition Keys**: Separate operations per partition key
- **More outgoing messages**: +400 bytes overhead per additional message sent

Another, more direct, approach to RU capacity planning would be to use a custom request handler attached to a [customized `CosmosClient` provider](#customizing-the-cosmosclient-provider) in your NServiceBus endpoint. This request handler could log every request/response and its assosiated RU charge. In this way, you can measure exactly what operations are being performed on the Cosmos DB for each message for that endpoint, and what the RU costs for each operations is. This can then be multipled by the estimated throughput of that NServiceBus endpoint when in production.

> [!WARNING]
> Its not recommended to monitor the RU costs using the direct approach in production as this could have performance immplications.

```csharp
// todo
```

## Provisioned throughput rate-limiting

When using provisioned throughput, it is possible for the CosmosDB service to rate-limit usage, resulting in "request rate too large" exceptions indicated by a 429 status code.

> [!WARNING]
> When using the Cosmos DB persister with the outbox enabled, "request rate too large" errors may result in handler re-execution and/or duplicate message dispatches depending on which operation is throttled.

> [!NOTE]
> Microsoft provides [guidance](https://docs.microsoft.com/en-us/azure/cosmos-db/sql/troubleshoot-request-rate-too-large) on how to diagnose and troubleshoot request rate too large exceptions.

The Cosmos DB SDK provides a mechanism to automatically retry collection operations when rate-limiting occurs. Besides changing the provisioned RUs or switching to the serverless tier, those settings can be adjusted to help prevent messages from failing during spikes in message volume.

These settings may be set when initializing the `CosmosClient` via the `CosmosClientOptions` [`MaxRetryAttemptsOnRateLimitedRequests`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.cosmosclientoptions.maxretryattemptsonratelimitedrequests?view=azure-dotnet) and [`MaxRetryWaitTimeOnRateLimitedRequests`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.cosmosclientoptions.maxretrywaittimeonratelimitedrequests?view=azure-dotnet) properties:

snippet: CosmosDBConfigureThrottlingWithClientOptions

They may also be set when using a `CosmosClientBuilder` via the [`WithThrottlingRetryOptions`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.fluent.cosmosclientbuilder.withthrottlingretryoptions?view=azure-dotnet) method:

snippet: CosmosDBConfigureThrottlingWithBuilder

## Transactions

The Cosmos DB persister supports using the [Cosmos DB transactional batch API](https://devblogs.microsoft.com/cosmosdb/introducing-transactionalbatch-in-the-net-sdk/). However, Cosmos DB only allows operations to be batched if all operations are performed within the [same logical partition key](https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/transactional-batch?tabs=dotnet). This is due to the distributed nature of the Cosmos DB service, which does not support distributed transactions.

The [transactions](transactions.md) documentation provides additional details on how to configure NServiceBus to resolve the incoming message to a specific partition key to take advantage of this Cosmos DB feature.

## Outbox

### Storage format

partial: outboxstorageformat

### Outbox cleanup

When the outbox is enabled, the deduplication data is kept for seven days by default. To customize this time frame, use the following API:

snippet: CosmosDBOutboxCleanup

Outbox cleanup depends on the Cosmos DB time-to-live feature. Failure to remove the expired outbox records is caused by a misconfigured collection that has time-to-live disabled. Refer to the [Cosmos DB documentation](https://docs.microsoft.com/en-us/azure/cosmos-db/time-to-live) to configure the collection correctly.
