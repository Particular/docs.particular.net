## Controlling transaction scope options

The following options for transaction scopes used during message processing can be configured.

NOTE: The isolation level and timeout for transaction scopes are also configured at the transport level.


### Isolation level

NServiceBus will by default use the `ReadCommitted` [isolation level](https://docs.microsoft.com/en-us/dotnet/api/system.transactions.isolationlevel). Change the isolation level using

snippet: CustomTransactionIsolationLevel


The only recommended isolation levels used with TransactionScope guarantee are `ReadCommited` and `RepeatableRead`. Using lower isolation levels may lead to subtle errors in certain configurations that are hard to troubleshoot.


### Transaction timeout

NServiceBus will use the [default transaction timeout](https://docs.microsoft.com/en-us/dotnet/api/system.transactions.transactionmanager.defaulttimeout) of the machine the endpoint is running on.

Change the transaction timeout using

snippet: CustomTransactionTimeout

Via a [config file](https://docs.microsoft.com/en-us/dotnet/framework/configure-apps/index) using a the [Timeout property of the DefaultSettingsSection](https://docs.microsoft.com/en-us/dotnet/api/system.transactions.configuration.defaultsettingssection.timeout).