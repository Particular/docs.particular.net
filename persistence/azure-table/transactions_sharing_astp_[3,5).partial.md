## Sharing the transaction

Once a behavior is introduced to identify the partition key for a given message, it is possible to share a Azure Table [TableBatchOperation](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.table.tablebatchoperation?view=azure-dotnet) between both the Saga persistence and business data. The shared `TableBatchOperation` can then be used to persist document updates for both concerns atomically.

### Within a handler method using `IMessageHandlerContext`

To use the shared `TableBatchOperation` in a message handler:

snippet: HandlerSharedTransaction

#### Testing

The `TestableAzureTableStorageSession` class in the `NServiceBus.Testing` namespace has been provided to facilitate [testing a handler](/nservicebus/testing/) that utilizes the shared transaction feature.

### With dependency injection

For custom types that require access to the shared `TableBatchOperation`:

snippet: TransactionalBatchRegisteredWithDependencyInjectionResolvedInCustomType

And alternatively to using the the extension method `IMessageHandlerContext.SynchronizedStorageSession.AzureTablePersistenceSession()`:

snippet: TransactionalBatchRegisteredWithDependencyInjectionResolvedInHandler