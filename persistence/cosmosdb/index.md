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

By default, the persister will store records in a database named `NServiceBus` and use a container per endpoint, using the endpoint name as the name of the container.

The database name can be customized using the following configuration API:

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

The Cosmos DB persister supports using the [Cosmos DB transactional batch API](https://devblogs.microsoft.com/cosmosdb/introducing-transactionalbatch-in-the-net-sdk/). However, Cosmos DB only allows operations to be batched if all operations are performed within the same logical partition key. This is due to the distributed nature of the Cosmos DB service, which does not support distributed transactions.

The [transactions](transactions.md) documentation provides additional details on how to configure NServiceBus to resolve the incoming message to a specific partition key to take advantage of this Cosmos DB feature.

## Outbox

### Storage format

> [!WARNING]
> When the default partition key is not explicitly set, outbox rows are not separated by endpoint name. As a result, multiple logical endpoints cannot share the same database and container since [message identities are not unique across endpoints from a processing perspective](/nservicebus/outbox/#message-identity). To avoid conflicts, either separate different endpoints into different containers or [override the partition key](transactions.md).

### Outbox cleanup

When the outbox is enabled, the deduplication data is kept for seven days by default. To customize this time frame, use the following API:

snippet: CosmosDBOutboxCleanup

Outbox cleanup depends on the Cosmos DB time-to-live feature. Failure to remove the expired outbox records is caused by a misconfigured collection that has time-to-live disabled. Refer to the [Cosmos DB documentation](https://docs.microsoft.com/en-us/azure/cosmos-db/time-to-live) to configure the collection correctly.
