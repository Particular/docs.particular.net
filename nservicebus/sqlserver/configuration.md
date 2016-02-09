---
title: SQL Server Transport configuration
summary: SQL Server Transport configuration
tags:
- SQL Server
redirects:
- nservicebus/sqlserver/concurrency
---

## Connection strings

Connection string can be configured in several ways:

### Via the configuration API

By using the `ConnectionString` extension method.

snippet:sqlserver-config-connectionstring

### Via the App.Config

By adding a connection named `NServiceBus/Transport` in the `connectionStrings` node.
  
```xml
<connectionStrings>
   <!-- SQL Server -->
   <add name="NServiceBus/Transport"
        connectionString="Data Source=.\SQLEXPRESS;
                                      Initial Catalog=nservicebus;
                                      Integrated Security=True"/>
</connectionStrings>
```

### Via a named connection string

By using the `ConnectionStringName` extension method.

snippet:sqlserver-named-connection-string

Combined with a named connection in the `connectionStrings` node of the `app.config` file.

snippet:sqlserver-named-connection-string-xml

## Sql Server Transport, the Outbox and user data: disabling the DTC

In an environment where DTC is disabled and [Outbox](/nservicebus/outbox/) is enabled, it is important to prevent a local transaction from escalating to a distributed one.

The following conditions need to be met:

* the business specific data and the `Outbox` storage must be in the same database;
* the user code accessing business related data must use the same `connection string` as the `Outbox` storage.


## Persistence

The most popular persistance used with SQL Server transport is [NHibernate persistance](/nservicebus/nhibernate/). The information regarding transactions is based on the assumption that this is the combination used. However, SQL Server Transport can be used ith other available persistance implementations.


## Transactions

The SQL Server transport can work in three modes with regards to transactions. These modes are enabled based on the bus configurations:


### Ambient transaction

The ambient transaction mode is selected by default. It relies or `Transactions.Enabled` setting being set to `true` and `Transactions.SuppressDistributedTransactions` being set to false. One needs to only select the transport:

snippet:sqlserver-config-transactionscope

When in this mode, the receive operation is wrapped in a `TransactionScope` together with the message processing in the pipeline. This means that usage of any other persistent resource manager (e.g. RavenDB client, another `SqlConnection` with different connection string) will cause escalation of the transaction to full two-phase commit protocol handled via Distributed Transaction Coordinator (MS DTC).


### Controlling transaction scope options

The following transaction scope options can be configured when the SQL Server transport is working in the ambient transaction mode.


### Isolation level

NServiceBus will by default use the `ReadCommitted` [isolation level](https://msdn.microsoft.com/en-us/library/system.transactions.isolationlevel).

Change the isolation level using

snippet:sqlserver-config-transactionscope-isolation-level


### Transaction timeout

NServiceBus will use the [default transaction timeout](https://msdn.microsoft.com/en-us/library/system.transactions.transactionmanager.defaulttimeout) of the machine the endpoint is running on.

Change the transaction timeout using

snippet:sqlserver-config-transactionscope-timeout

Or via .config file using a [example DefaultSettingsSection](https://msdn.microsoft.com/en-us/library/system.transactions.configuration.defaultsettingssection.aspx#Anchor_5).


### Native transaction

The native transaction mode requires both `Transactions.Enabled` and `Transactions.SuppressDistributedTransactions` to be set to `true`. It can be selected via

snippet:sqlserver-config-native-transactions

When in this mode, the receive operation is wrapped in a plain ADO.NET `SqlTransaction`. Both connection and the transaction instances are attached to the pipeline context under these keys `SqlConnection-{ConnectionString}` and `SqlTransaction-{ConnectionString}` and are available for user code so that the updates to user data can be done atomically with queue receive operation.


### No transaction

The no transaction mode requires `Transactions.Enabled` to be set to false which can be achieved via following API call:

snippet:sqlserver-config-no-transactions

When in this mode, the receive operation is not wrapped in any transaction so it is executed by the SQL Server in its own implicit transaction.

WARNING: This means that as soon as the `DELETE` operation used for receiving completes, the message is gone and any exception that happens during processing of this message causes it to be permanently lost.

### [Entity Framework](https://msdn.microsoft.com/en-us/data/ef.aspx) caveats

Sharing the same connection string can be problematic when dealing with entities based on the [Entity Framework ADO.Net Data Model (EDMX)](https://msdn.microsoft.com/en-us/library/vstudio/cc716685.aspx). The `DbContext` generated by Entity Framework does not expose a way to inject a simple database connection string; the underlying problem is that Entity Framework requires an `Entity Connection String` that contains more information than a simple connection string.

It is possible to generate a custom a custom `EntityConnection` and inject it into the Entity Framework `DbContext` instance:

snippet:EntityConnectionCreationAndUsage

In the snippet above the `EntityConnectionStringBuilder` class is used to create a valid `Entity Connection String`. Having that a new `EntityConnection` instance can be created.

The `DbContext` generated by default by Entity Framework does not have a constructor that accepts an `EntityConnection` as a parameter. Since it is a partial class we can add that parameter using the following snippet:

snippet:DbContextPartialWithEntityConnection

NOTE: The snippet above assumes that the created entity data model is named `MySample`. The references should match conventions used in the project.


## Callbacks

### Disable callbacks

Callbacks and consequently, callback queues (secondary queues) receivers are enabled by default. In order to disable them use the following setting:

snippet:sqlserver-config-disable-secondaries

Secondary queues use the same adaptive concurrency model as the primary queue. Secondary queues (and hence callbacks) are disabled for satellite receivers.


### Callback Receiver Max Concurrency

Changes the number of threads used for the callback receiver. The default is 1 thread.

snippet:sqlserver-CallbackReceiverMaxConcurrency


## Circuit Breaker

The Sql transport has a built in circuit breaker to handle intermittent SQL Server connectivity problems.


### Wait time

Overrides the default time to wait before triggering a circuit breaker that initiates the endpoint shutdown procedure in case of [repeated critical errors](/nservicebus/hosting/critical-errors.md).

The default is 2 minutes.

snippet:sqlserver-TimeToWaitBeforeTriggeringCircuitBreaker


### Pause Time

Overrides the default time to pause after a failure while trying to receive a message.

The default is 10 seconds.

snippet: sqlserver-PauseAfterReceiveFailure


