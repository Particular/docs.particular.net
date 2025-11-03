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

## Capacity planning using request units (RU)

Understanding [Request Units (RUs)](https://learn.microsoft.com/en-us/azure/cosmos-db/request-units) is essential for effective capacity planning in Azure Cosmos DB. RUs represent the cost of database operations in terms of system resources. Knowing how your workload consumes them helps you avoid throttling, control costs, and size your setup appropriately, especially when using [Provisioned Throughput](https://learn.microsoft.com/en-us/azure/cosmos-db/set-throughput) or [Serverless](https://learn.microsoft.com/en-us/azure/cosmos-db/serverless) accounts.

### Using the Microsoft Cosmos DB Capacity Planner

Microsoft provide a [Cosmos DB capacity calculator](https://cosmos.azure.com/capacitycalculator/) which can be used to model the throughput costs of your solution. It uses [several parameters](https://learn.microsoft.com/en-us/azure/cosmos-db/request-units?context=%2Fazure%2Fcosmos-db%2Fnosql%2Fcontext%2Fcontext#request-unit-considerations) to calculate this, but only the following are directly affected when using the Azure Cosmos DB Persistence.

| Capacity Calculator Parameter | Persistence Operation | Cosmos DB Operation |
| :---- | :---- | :---- |
| [**Point reads**](https://learn.microsoft.com/en-us/azure/cosmos-db/optimize-cost-reads-writes#point-reads) | [Logical/physical outbox read](transactions.md), [Outbox partition key fallback read](#outbox), [Saga read](saga-concurrency.md) | `ReadItemStreamAsync` |
| [**Creates**](https://learn.microsoft.com/en-us/azure/cosmos-db/optimize-cost-reads-writes#write-data) | New outbox record, New saga record | `CreateItemStream` |
| [**Updates**](https://learn.microsoft.com/en-us/azure/cosmos-db/optimize-cost-reads-writes#write-data) | [Saga update](saga-concurrency.md#default-behavior-updating-or-deleting-saga-data), [Saga acquire lease](saga-concurrency.md#sagas-concurrency-control-pessimistic-locking-internals), Saga release lock, Outbox dispatched, [Outbox delete (updates TTL)](#outbox-outbox-cleanup) | `ReplaceItemStream`, `UpsertItemStream`, `PatchItemStreamAsync`, `PatchItem` |
| [**Deletes**](https://learn.microsoft.com/en-us/azure/cosmos-db/optimize-cost-reads-writes#write-data) | Saga complete, [Outbox TTL background cleanup](#outbox-outbox-cleanup) | `DeleteItem` |
| [**Queries**](https://learn.microsoft.com/en-us/azure/cosmos-db/optimize-cost-reads-writes#queries) | [Saga migration mode](migration-from-azure-table.md) | `GetItemQueryStreamIterator` |

[Document size](https://learn.microsoft.com/en-us/azure/cosmos-db/request-units#request-unit-considerations) also affects RU usage as the size of an item increases, the number of RUs consumed to read or write the item also increases. The table below provides an **estimate** of the persistence cost that should be considered per message when modeling throughput requirements.

| Record Type | Estimated Size |
| :---- | :---- |
| **Outbox** | ~630 bytes + message body |
| **Saga** | ~300 bytes + saga data |

The below tables gives an indication of what Cosmos DB operations occur in different NServiceBus endpoint configurations for every processed message. This can be used with the [Cosmos DB Capacity Planner](https://cosmos.azure.com/capacitycalculator/), along with [other factors](https://learn.microsoft.com/en-us/azure/cosmos-db/request-units?context=%2Fazure%2Fcosmos-db%2Fnosql%2Fcontext%2Fcontext#request-unit-considerations) that affect pricing (such as the selected Cosmos DB API, number of regions, etc), and the total message throughput to produce an estimated RU capacity requirement.

#### No Outbox

| Incoming message Scenario | Point Reads | Creates | Updates | Deletes | Queries | Persistence Requirements* |
|---------------------------|-------------|---------|---------|---------|---------|---------------------------|
| **No Saga**               | 0           | 0       | 0       | 0       | 0       | 0 bytes             |  
| **Saga (new)**            | 1           | 1       | 0       | 0       | 0       | 300 bytes           |
| **Saga (new) + [Migration Mode](/persistence/cosmosdb/migration-from-azure-table.md)**            | 1           | 1       | 0       | 0       | 1       | 300 bytes           |
| **Saga (update)**         | 1           | 0       | 1       | 0       | 0       | 300 bytes           |  
| **Saga (complete)**       | 1           | 0       | 0       | 1       | 0       | 300 bytes           |

#### With Outbox

| Incoming message scenario          | Point Reads | Creates | Updates | Deletes       | Queries | Persister Requirements*    |  
|------------------------------------|-------------|---------|---------|---------------|---------|------------------------|  
| **No Saga**                        | 1           | 1       | 1       | 1 (delayed)   | 0       | 630 bytes (1 msg sent) |
| **No Saga + [Partition Key Fallback Read](#outbox-storage-format)**                        | 2           | 1       | 1       | 1 (delayed)   | 0       | 630 bytes (1 msg sent) |  
| **Saga (new)**                     | 2           | 2       | 1       | 1 (delayed)   | 0       | 630 + 300 = 930 bytes  |  
| **Saga (update)**                  | 2           | 1       | 2       | 1 (delayed)   | 0       | 630 + 300 = 930 bytes  |  
| **Saga (complete)**                | 2           | 1       | 1       | 2 (delayed)   | 0       | 630 + 300 = 930 bytes  |  
| **Saga + [Pessimistic Locking](/persistence/cosmosdb/saga-concurrency.md#sagas-concurrency-control-pessimistic-locking-internals) (no contention)** | 1           | 1-2     | 3-4     | 1-2 (delayed) | 0       | 630 + 360 = 990 bytes  |  
| **Saga + Pessimistic Locking (3 retries)**     | 1           | 1-2     | 9-10    | 1-2 (delayed) | 0       | 630 + 360 = 990 bytes  |

*Persister requirements exclude message bodies and saga data and assume one handler sends one outgoing message.

**Additional operations** (conditional):

- **Multiple Partition Keys**: Separate operations per partition key
- **More outgoing messages**: +400 bytes overhead per additional message sent

#### Example

- Outbox: Enabled
- Sagas: Order saga (average 3 KB)
- Locking: Optimistic (default)
- Message rate: 500 messages/second peak
- Each handler sends average 2 outgoing messages (1 KB each)

##### Calculator Inputs

| Operation Type | Calculation | Result |
|----------------|-------------|--------|
| Point Reads | 500 msg/sec × 2 reads = | **1,000/sec** |
| Creates | 500 msg/sec × 1 create (outbox) = | **500/sec** |
| Updates | 500 msg/sec × 2 updates (saga + outbox) = | **1,000/sec** |
| Deletes | 500 msg/sec avg over 24h (steady state) = | **500/sec** |
| Queries | 0 | **0/sec** |
| OutboxRecord size | 200 bytes + (2 × 1000 bytes) = | **2.2 KB** |  
| Saga size | 3000 bytes + 300 bytes metadata = | **3.3 KB** |

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
            logger.LogWarning("Request throttled");
        }

        return response;
    }
}
```

### Using Azure

Alternatively, the Azure Cosmos DB [Diagnostic Settings](https://learn.microsoft.com/en-us/azure/cosmos-db/monitor-resource-logs?tabs=azure-portal) can be configured to route the diagnostic logs to an Azure Log Analytics Workspace. Here they can be [queried](https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/diagnostic-queries?tabs=resource-specific) for the same data used for RU capacity planning. This method is not recommended for live monitoring of RU usage as diagnostic logs typically are delayed by a few minutes, and cost and retention of Log Analytics would be a limiting factor.

For [real time monitoring](https://learn.microsoft.com/en-us/azure/cosmos-db/serverless?context=%2Fazure%2Fcosmos-db%2Fnosql%2Fcontext%2Fcontext#monitor-your-consumption), the metrics pane in the Cosmos DB account can be used.

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
