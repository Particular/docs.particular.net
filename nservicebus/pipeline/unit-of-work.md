---
title: Unit of Work
summary: Implementing a unit of work in NServiceBus
component: Core
reviewed: 2025-12-02
redirects:
 - nservicebus/unit-of-work-in-nservicebus
related:
 - samples/unit-of-work
 - samples/pipeline/unit-of-work
---

## Using a transaction scope

If a business transaction spans multiple handlers, there is always a risk of partial updates, since one handler might succeed in updating the data while others don't. To avoid this, it is possible to use a unit of work that wraps all handlers in a `TransactionScope` and ensures no partial updates. Use the following code to enable a wrapping scope:

snippet: UnitOfWorkWrapHandlersInATransactionScope

> [!NOTE]
> This requires the selected [persistence](/persistence/) to support enlisting in transaction scopes.

> [!WARNING]
> This might escalate to a distributed transaction if data across different databases is updated.

> [!WARNING]
> This API must not be used in combination with transports running in *transaction scope* mode. Wrapping handlers in a `TransactionScope` in such a situation throws an exception.

### Controlling transaction scope options

The following options for transaction scopes used to wrap all handlers can be configured.

### Isolation level

NServiceBus will by default use the `ReadCommitted` [isolation level](https://msdn.microsoft.com/en-us/library/system.transactions.isolationlevel).

Change the isolation level using

snippet: UnitOfWorkCustomTransactionIsolationLevel

### Transaction timeout

NServiceBus will use the [default transaction timeout](https://msdn.microsoft.com/en-us/library/system.transactions.transactionmanager.defaulttimeout) of the machine on which the endpoint runs.

Change the transaction timeout using

snippet: UnitOfWorkCustomTransactionTimeout

Or via .config file using a [example DefaultSettingsSection](https://msdn.microsoft.com/en-us/library/system.transactions.configuration.defaultsettingssection.aspx#Anchor_5).

partial: custom-unit-of-work
