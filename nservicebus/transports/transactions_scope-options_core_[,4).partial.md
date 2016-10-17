## Controlling transaction scope options

The following options for transaction scopes used during message processing can be configured.


### Isolation level

NServiceBus will by default use the `Serializable` [isolation level](https://msdn.microsoft.com/en-us/library/system.transactions.isolationlevel). Change the isolation level using

snippet:CustomTransactionIsolationLevel


### Transaction timeout

NServiceBus will use the [default transaction timeout](https://msdn.microsoft.com/en-us/library/system.transactions.transactionmanager.defaulttimeout) of the machine the endpoint is running on.

Change the transaction timeout using

snippet:CustomTransactionTimeout

Via a [config file](https://msdn.microsoft.com/en-us/library/1xtk877y.aspx) using a the [Timeout property of the DefaultSettingsSection](https://msdn.microsoft.com/en-us/library/system.transactions.configuration.defaultsettingssection.timeout.aspx).
