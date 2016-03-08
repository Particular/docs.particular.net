---
title: Unit of Work
summary: Implementing your own unit of work in NServiceBus.
tags: []
redirects:
- nservicebus/unit-of-work-in-nservicebus
---

## Using a transaction scope

If your business transaction is spread across multiple handlers there is always a risk of partial updates since one handler might succeed in updating the data while other won't. To avoid this use a unit of work that wraps all handlers in a `TransactionScope` and makes sure that there are no partial updates. Use following code to enable a wrapping scope:

snippet:UnitOfWorkWrapHandlersInATransactionScope

NOTE: This requires the selected storage to support enlisting in transaction scopes.

WARNING: This might escalate to a distributed transaction if data in different databases are updated.

WARNING: This API must not be used in combination with transports running in *transaction scope* mode. Starting from version 6, wrapping handlers in a `TransactionScope` in such a situation throws an exception.


### Controlling transaction scope options

The following options for transaction scopes used to wrap all handlers can be configured.


### Isolation level

NServiceBus will by default use the `ReadCommitted` [isolation level](https://msdn.microsoft.com/en-us/library/system.transactions.isolationlevel).

Change the isolation level using

snippet:UnitOfWorkCustomTransactionIsolationLevel


### Transaction timeout

NServiceBus will use the [default transaction timeout](https://msdn.microsoft.com/en-us/library/system.transactions.transactionmanager.defaulttimeout) of the machine the endpoint is running on.

Change the transaction timeout using

snippet:UnitOfWorkCustomTransactionTimeout

Or via .config file using a [example DefaultSettingsSection](https://msdn.microsoft.com/en-us/library/system.transactions.configuration.defaultsettingssection.aspx#Anchor_5).


## Implementing custom units of work

When using a framework like NServiceBus you usually need to create your own units of work to avoid having to repeat code in the message handlers. For example, committing NHibernate transactions, or calling `SaveChanges` on the RavenDB session.

### IManageUnitsOfWork

To create your unit of work, implement the `IManageUnitsOfWork` interface.

snippet:UnitOfWorkImplementation

The semantics are that `Begin()` is called when the transport messages enters the pipeline. Remember that a transport message can consist of multiple application messages. This allows you to do any setup that is required.

The `End()` method is called when the processing is complete. If there is an exception, it is passed into the method.

This gives you a way to perform different actions depending on the outcome of the message(s).

Starting with version 6 `Begin()` and `End()` must return a `Task`, a completed `Task` or be marked `async`.

### Registering your unit of work

After implementing a `IManageUnitsOfWork`, you now need to register it with NServiceBus.
Here's an example of how to register your unit of work: 

snippet:InstancePerUnitOfWorkRegistration

NOTE: If you prefer, you can do the registration of the Unit of work by implementing the `INeedInitialization` in the same class that implements `IManageUnitsOfWork`.

