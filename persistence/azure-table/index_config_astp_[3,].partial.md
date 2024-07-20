First add a reference to the assembly that contains the Azure Table persistence, which is done by adding a NuGet package reference to `NServiceBus.Persistence.AzureTable`.

snippet: PersistenceWithAzure

### Token-credentials

Enables usage of Microsoft Entra ID authentication such as [managed identities for Azure resources](https://learn.microsoft.com/en-us/azure/storage/tables/authorize-access-azure-active-directory) instead of the shared secret in the connection string.

Use the corresponding [`TableServiceClient`](https://learn.microsoft.com/en-us/dotnet/api/azure.data.tables.tableserviceclient.-ctor?view=azure-dotnet#azure-data-tables-tableserviceclient-ctor(system-uri-azure-core-tokencredential-azure-data-tables-tableclientoptions)) constructor overload when [creating the client passed to the persistence](configuration.md).
