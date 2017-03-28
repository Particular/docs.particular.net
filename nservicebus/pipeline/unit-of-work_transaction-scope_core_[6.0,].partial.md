
## Using a transaction scope

If a business transaction is spread across multiple handlers there is always a risk of partial updates since one handler might succeed in updating the data while other won't. To avoid this use a unit of work that wraps all handlers in a `TransactionScope` and makes sure that there are no partial updates. Use following code to enable a wrapping scope:

snippet: UnitOfWorkWrapHandlersInATransactionScope

NOTE: This requires the selected [persistence](/nservicebus/persistence/) to support enlisting in transaction scopes.

WARNING: This might escalate to a distributed transaction if data in different databases are updated.

WARNING: This API must not be used in combination with transports running in *transaction scope* mode. Wrapping handlers in a `TransactionScope` in such a situation throws an exception.


### Controlling transaction scope options

The following options for transaction scopes used to wrap all handlers can be configured.


### Isolation level

NServiceBus will by default use the `ReadCommitted` [isolation level](https://msdn.microsoft.com/en-us/library/system.transactions.isolationlevel).

Change the isolation level using

snippet: UnitOfWorkCustomTransactionIsolationLevel


### Transaction timeout

NServiceBus will use the [default transaction timeout](https://msdn.microsoft.com/en-us/library/system.transactions.transactionmanager.defaulttimeout) of the machine the endpoint is running on.

Change the transaction timeout using

snippet: UnitOfWorkCustomTransactionTimeout

Or via .config file using a [example DefaultSettingsSection](https://msdn.microsoft.com/en-us/library/system.transactions.configuration.defaultsettingssection.aspx#Anchor_5).