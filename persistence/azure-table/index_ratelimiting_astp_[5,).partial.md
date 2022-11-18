## Provisioned throughput rate-limiting with Azure Cosmos DB

When using the provisioned throughput feature, it is possible for the CosmosDB service to rate-limit usage, resulting in "request rate too large" `RequestFailedException`s indicated by a [429 status code](https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/troubleshoot-request-rate-too-large).

WARN: When using the Azure Table persistence with the outbox enabled, "request rate too large" errors may result in handler re-execution and/or duplicate message dispatches depending on which operation is throttled.

INFO: Microsoft provides [guidance](https://docs.microsoft.com/en-us/azure/cosmos-db/monitor-request-unit-usage) on how to monitor request rate usage.

The Azure.Data.Tables SDK handles these exceptions by [automatically retrying the failed request](https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/conceptual-resilient-sdk-applications#http-429) based on headers included in the response.
The retry policy can be adjusted through [`TableClientOptions`](https://learn.microsoft.com/en-us/dotnet/api/azure.data.tables.tableclientoptions?view=azure-dotnet) when initializing the `TableServiceClient`.