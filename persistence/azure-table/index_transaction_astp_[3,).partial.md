### Transactions

The Azure Table persister supports using the Azure Table [TableBatchOperation API](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.table.tablebatchoperation?view=azure-dotnet). However, Azure Table only allows operations to be batched if all operations are performed within the same partition key. This is due to the distributed nature of the Azure Table service, which does not support distributed transactions.

The [transactions documentation](transactions.md) provides additional details on how to configure NServiceBus to resolve the incoming message to a specific partition key to take advantage of this Azure Table feature.