## Controlling transaction scope options

The following options for transaction scopes used during message processing can be configured.


### Isolation level

NServiceBus will by default use the `ReadCommitted` [isolation level](https://docs.microsoft.com/en-us/dotnet/api/system.transactions.isolationlevel). Change the isolation level using

snippet: CustomTransactionIsolationLevel


### Transaction timeout

NServiceBus will use the [default transaction timeout](https://docs.microsoft.com/en-us/dotnet/api/system.transactions.transactionmanager.defaulttimeout) of the machine the endpoint is running on.

Change the transaction timeout using

snippet: CustomTransactionTimeout

Via a [config file](https://docs.microsoft.com/en-us/dotnet/framework/configure-apps/index) using a the [Timeout property of the DefaultSettingsSection](https://docs.microsoft.com/en-us/dotnet/api/system.transactions.configuration.defaultsettingssection.timeout).
