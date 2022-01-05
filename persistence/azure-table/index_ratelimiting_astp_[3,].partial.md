## Provisioned throughput rate-limiting with Azure Cosmos DB

When using provisioned throughput it is possible for the CosmosDB service to rate-limit usage, resulting in request rate too large `StorageException`s indicated by a 429 status code.

WARN: When using the Azure Table persistence with Outbox enabled, request rate too large errors may result in handler re-execution and/or duplicate message dispatches depending on which operation is throttled.

INFO: Microsoft provides [guidance](https://docs.microsoft.com/en-us/azure/cosmos-db/monitor-request-unit-usage) on how to monitor request rate usage.

The Cosmos DB SDK provides a mechanism to automatically retry collection operations when rate-limiting occurs. Besides changing the provisioned RUs or switching to the serverless tier, those settings can be adjusted to help prevent messages from failing during spikes in message volume.

These settings may be set when initializing the `CloudTableClient` via the `TableClientConfiguration.CosmosExecutorConfiguration` [`MaxRetryAttemptsOnThrottledRequests`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.table.cosmosexecutorconfiguration.maxretryattemptsonthrottledrequests?view=azure-dotnet) and [`MaxRetryWaitTimeOnThrottledRequests`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.table.cosmosexecutorconfiguration.maxretrywaittimeonthrottledrequests?view=azure-dotnet) properties:

snippet: CosmosDBConfigureThrottlingWithClientOptions
