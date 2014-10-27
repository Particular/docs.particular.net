---
title: Configuration API transactions in V5
summary: Configuration API transactions in V5
tags:
- NServiceBus
- BusConfiguration
- V5
---

In order to configure the transactions settings of the endpoint it is possible to call the `Transactions()` method of the `BusConfiguration` class:

* `DefaultTimeout`: Sets the default timeout period for the transaction;
* `IsolationLevel`: Sets the isolation level of the transaction;
* `Disable` / `Enable`: Configures the current Transport to use or not use any transactions;
* `DisableDistributedTransactions` / `EnableDistributedTransactions`: Configures the crrent Transport to enlist or not in Distributed Transactions;
* `DoNotWrapHandlersExecutionInATransactionScope` / `WrapHandlersExecutionInATransactionScope`: Configures the endpoint so that `IHandleMessages<T>` are not wrapped in a `System.Transactions.TransactionScope`;