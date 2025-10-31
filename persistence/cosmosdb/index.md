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

## Request unit (RU) capacity planning

A [Request Unit](https://learn.microsoft.com/en-us/azure/cosmos-db/request-units?context=%2Fazure%2Fcosmos-db%2Fnosql%2Fcontext%2Fcontext) (or RU, for short) is a performance currency abstracting the system resources such as CPU, IOPS, and memory required to perform database operations. RU capacity planning is the process of estimating the required RUs that an Azure Cosmos DB will need to handle its workload.

> [!NOTE]
> RU capacity/usage planning is recommended regardless of [Cosmos DB account type](https://learn.microsoft.com/en-us/azure/cosmos-db/throughput-serverless?context=%2Fazure%2Fcosmos-db%2Fnosql%2Fcontext%2Fcontext#detailed-comparison), however it's especially important when [Provisioned Throughput](https://learn.microsoft.com/en-us/azure/cosmos-db/set-throughput?context=%2Fazure%2Fcosmos-db%2Fnosql%2Fcontext%2Fcontext) is selected as reaching the assigned capacity will cause [throttling](https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/troubleshoot-request-rate-too-large?tabs=resource-specific), which could result in [duplication of messages](#provisioned-throughput-rate-limiting). In a [Serverless](https://learn.microsoft.com/en-us/azure/cosmos-db/serverless) account, you're charged only for the RUs that your database operations consume and for the storage that your data consumes. Planning and [monitoring](https://learn.microsoft.com/en-us/azure/cosmos-db/serverless?context=%2Fazure%2Fcosmos-db%2Fnosql%2Fcontext%2Fcontext#monitor-your-consumption) RU usage is still beneficial when using a serverless account from a cost perspective as there is a lack of predictability.

### Using the Microsoft Cosmos DB Capacity Planner

Microsoft provide a [Cosmos DB capacity calculator](https://cosmos.azure.com/capacitycalculator/) which can be used to model the throughput costs of your solution. It uses [several parameters](https://learn.microsoft.com/en-us/azure/cosmos-db/request-units?context=%2Fazure%2Fcosmos-db%2Fnosql%2Fcontext%2Fcontext#request-unit-considerations) to calculate this, but only the following are directly affected when using the NServiceBus CosmosDB Persistence.

| Capacity Calculator Parameter | NServiceBus Operation | Cosmos DB Operation |
| :---- | :---- | :---- |
| [**Point reads**](https://learn.microsoft.com/en-us/azure/cosmos-db/optimize-cost-reads-writes#point-reads) | [Logical/physical outbox read](transactions.md), [Outbox partition key fallback read](#outbox), [Saga read](saga-concurrency.md) | `ReadItemStreamAsync` |
| [**Creates**](https://learn.microsoft.com/en-us/azure/cosmos-db/optimize-cost-reads-writes#write-data) | New outbox record, New saga record | `CreateItemStream` |
| [**Updates**](https://learn.microsoft.com/en-us/azure/cosmos-db/optimize-cost-reads-writes#write-data) | [Saga update](saga-concurrency.md#default-behavior-updating-or-deleting-saga-data), [Saga acquire lease](saga-concurrency.md#sagas-concurrency-control-pessimistic-locking-internals), Saga release lock, Outbox dispatched, [Outbox delete (updates TTL)](#outbox-outbox-cleanup) | `ReplaceItemStream`, `UpsertItemStream`, `PatchItemStreamAsync`, `PatchItem` |
| [**Deletes**](https://learn.microsoft.com/en-us/azure/cosmos-db/optimize-cost-reads-writes#write-data) | Saga complete, [Outbox TTL background cleanup](#outbox-outbox-cleanup) | `DeleteItem` |
| [**Queries**](https://learn.microsoft.com/en-us/azure/cosmos-db/optimize-cost-reads-writes#queries) | [Saga migration mode](migration-from-azure-table.md) | `GetItemQueryStreamIterator` |

[Document size](https://learn.microsoft.com/en-us/azure/cosmos-db/request-units#request-unit-considerations) also affects RU usage as the size of an item increases, the number of RUs consumed to read or write the item also increases. The below table gives an **estimate** of the persistence overhead that should be considered per message when modeling throughput costs.

| Record Type | Estimated Size |
| :---- | :---- |
| **Outbox** | ~630 bytes + message body |
| **Saga** | ~300 bytes + saga data |

The below table gives an indication of what Cosmos DB operations occur in different NServiceBus endpoint configurations for every processed message. This can be used with the [Cosmos DB Capacity Planner](https://cosmos.azure.com/capacitycalculator/), along with [other factors](https://learn.microsoft.com/en-us/azure/cosmos-db/request-units?context=%2Fazure%2Fcosmos-db%2Fnosql%2Fcontext%2Fcontext#request-unit-considerations) that affect pricing (such as the selected Cosmos DB API, number of regions, etc), and the total message throughput, to produce an estimated RU capacity requirement.

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

*Persister overhead excludes message bodies and saga data. Assumes handler sends 1 outgoing message.

**Additional operations** (conditional):

- **Partition Key Fallback Read**: +1 Point Read (if enabled and using synthetic partition key)
- **Migration Mode**: +1 Query (if saga migration mode enabled and saga not found)
- **Multiple Partition Keys**: Separate operations per partition key
- **More outgoing messages**: +400 bytes overhead per additional message sent

**Example**

- Outbox: Enabled
- Sagas: Order saga (average 3 KB)
- Locking: Optimistic (default)
- Message rate: 500 messages/second peak
- Each handler sends average 2 outgoing messages (1 KB each)

**Calculator Inputs:**

| Operation Type | Calculation | Result |
|----------------|-------------|--------|
| Point Reads | 500 msg/sec × 2 reads = | **1,000/sec** |
| Creates | 500 msg/sec × 1 create (outbox) = | **500/sec** |
| Updates | 500 msg/sec × 2 updates (saga + outbox) = | **1,000/sec** |
| Deletes | 500 msg/sec avg over 24h (steady state) = | **500/sec** |
| Queries | 0 | **0/sec** |
| OutboxRecord size | 200 bytes + (2 × 1 KB) = | **2.2 KB** |
| Saga size | 3 KB + 0.3 KB metadata = | **3.3 KB** |

### Using Code

Another, more direct, approach to RU capacity planning would be to use a Cosmos DB [`RequestHandler`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.requesthandler?view=azure-dotnet) attached to a customized [`CosmosClient` provider](#usage-customizing-the-cosmosclient-provider) in your NServiceBus endpoint in a development environment. This request handler gives you the flexibility to log every Cosmos DB request and response, and its associated RU charge. In this way, you can measure exactly what operations are being performed on the Cosmos DB database for each message for that endpoint, and what the RU costs for each operation are. This can then be multiplied by the estimated throughput of that NServiceBus endpoint when in production.

> [!WARNING]
> Its not recommended to monitor the RU costs using the direct `RequestHandler` approach in production as this could have performance implications.

```csharp
//...
var endpointConfiguration = new EndpointConfiguration("Name");
var builder = new CosmosClientBuilder(cosmosConnection);
builder.AddCustomHandlers(new LoggingHandler());
CosmosClient cosmosClient = builder.Build();
//...

class LoggingHandler : RequestHandler
{
    public override async Task<ResponseMessage> SendAsync(RequestMessage request, CancellationToken cancellationToken = default)
    {
        ResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        CosmosDiagnostics diagnostics = response.Diagnostics;

        // diagnostics JSON string contains the operation name. i.e. ReadItemStreamAsync
        // use this to map the cosmos operation to the capacity planner using the table above

        string requestChargeRU = response.Headers["x-ms-request-charge"];

        if ((int)response.StatusCode == 429)
        {
            logger.LogWarning("Request throttled.");
        }

        return response;
    }
}
```

### Using Azure

Alternatively, the Azure Cosmos DB [Diagnostic Settings](https://learn.microsoft.com/en-us/azure/cosmos-db/monitor-resource-logs?tabs=azure-portal) can be configured to route the diagnostic logs to an Azure Log Analytics Workspace. Here they can be [queried](https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/diagnostic-queries?tabs=resource-specific) for the same data used for RU capacity planning. This method is not recommended for live monitoring of RU usage as diagnostic logs typically are delayed by a few minutes, and cost and retention of Log Analytics would be a limiting factor.

For [real time monitoring](https://learn.microsoft.com/en-us/azure/cosmos-db/serverless?context=%2Fazure%2Fcosmos-db%2Fnosql%2Fcontext%2Fcontext#monitor-your-consumption), the metrics pane in the Cosmos DB account can be used.

### General Recommendations

- If message throughput is extremely variable, use [Autoscaling](https://learn.microsoft.com/en-us/azure/cosmos-db/provision-throughput-autoscale?context=%2Fazure%2Fcosmos-db%2Fnosql%2Fcontext%2Fcontext)
- If some endpoints require constantly higher (or lower) RU capacity than others, use [Provisioned throughput](https://learn.microsoft.com/en-us/azure/cosmos-db/set-throughput?context=%2Fazure%2Fcosmos-db%2Fnosql%2Fcontext%2Fcontext) and [assign different containers](https://learn.microsoft.com/en-us/azure/cosmos-db/set-throughput?context=%2Fazure%2Fcosmos-db%2Fnosql%2Fcontext%2Fcontext#set-throughput-on-a-container) to that endpoint with the required RU capacity.

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
