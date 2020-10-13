### TransactionScope

NOTE: This section only applies to .NET Framework as .NET Core does not support distributed transactions.

In .NET, using the [TransactionScope](https://docs.microsoft.com/en-us/dotnet/api/system.transactions.transactionscope) makes a block of code transactional, resulting in any code that stores data will automatically enlist in the transaction.

This is configured on the transport and automatically includes any message handler.

snippet: BusinessData-ConfigureTransactionScope

As long as the database resource also supports distributed transactions, the code in a handler will automatically enlist in the transaction without any additional configuration. If a handler fails processing a message the data in the datastore will automatically be rolled back.

snippet: BusinessData-InsideTransactionScope

When using a transaction mode lower than TransactionScope, NServiceBus also provides the ability to wrap handlers inside a TransactionScope to [avoid partial updates](/transports/transactions.md#avoiding-partial-updates).