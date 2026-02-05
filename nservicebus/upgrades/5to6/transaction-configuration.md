---
title: Transaction Configuration Changes in NServiceBus Version 6
reviewed: 2023-04-14
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

NServiceBus version 6 provides a configuration API that is more aligned with the transaction capabilities of the transport.


## Enabling transactions

Transactions are enabled by default so calls to `.Enable()` can safely be removed.

```csharp
// For NServiceBus version 6.x
// Using a transport will enable transactions automatically.
endpointConfiguration.UseTransport<MyTransport>();

// For NServiceBus version 5.x
var transactionSettings = busConfiguration.Transactions();
transactionSettings.Enable();
```


## Disabling transactions

Disabling transactions is now done by setting a transport transaction mode.

```csharp
// For NServiceBus version 6.x
var transport = endpointConfiguration.UseTransport<MyTransport>();
transport.Transactions(TransportTransactionMode.None);

// For NServiceBus version 5.x
var transactionSettings = busConfiguration.Transactions();
transactionSettings.Disable();
```


## Enabling distributed transactions

Distributed transaction mode is the default mode for transports with DTC support but can be enabled explicitly.

```csharp
// For NServiceBus version 6.x
var transport = endpointConfiguration.UseTransport<MyTransport>();
transport.Transactions(TransportTransactionMode.TransactionScope);

// For NServiceBus version 5.x
var transactionSettings = busConfiguration.Transactions();
transactionSettings.EnableDistributedTransactions();
```


## Disabling distributed transactions

Disabling distributed transactions is now done by setting a transport transaction mode.

```csharp
// For NServiceBus version 6.x
var transport = endpointConfiguration.UseTransport<MyTransport>();
transport.Transactions(TransportTransactionMode.ReceiveOnly);

// For NServiceBus version 5.x
var transactionSettings = busConfiguration.Transactions();
transactionSettings.DisableDistributedTransactions();
```

Or, if the transport supports native AtomicWithReceive:

```csharp
var transport = endpointConfiguration.UseTransport<MyTransport>();
transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
```


## Controlling transaction scope options

NServiceBus version 6 allows transaction scope options to be configured at the transport level. Setting isolation level and timeout can now be done with the following:

```csharp
// For NServiceBus version 6.x
var transport = endpointConfiguration.UseTransport<MyTransport>();
transport.Transactions(TransportTransactionMode.TransactionScope);
transport.TransactionScopeOptions(
    isolationLevel: IsolationLevel.RepeatableRead,
    timeout: TimeSpan.FromSeconds(30));

// For NServiceBus version 5.x
var transactionSettings = busConfiguration.Transactions();
transactionSettings.IsolationLevel(IsolationLevel.RepeatableRead);
transactionSettings.DefaultTimeout(TimeSpan.FromSeconds(30));
```


## Wrapping handlers execution in a transaction scope

NServiceBus version 6 comes with a unit of work that wraps execution of handlers in a transaction scope, which can now be done with the following API:

```csharp
// For NServiceBus version 6.x
var unitOfWork = endpointConfiguration.UnitOfWork();
unitOfWork.WrapHandlersInATransactionScope();

// For NServiceBus version 5.x
var transactionSettings = busConfiguration.Transactions();
transactionSettings.WrapHandlersExecutionInATransactionScope();
```


## Forwarding messages to error queue when transactions are disabled

When transactions are disabled and if any errors are encountered during the processing of the message, then the messages will be forwarded to the error queue. In NServiceBus version 5, this message would have been lost. For more details, read the [new behavior changes in version 6](/transports/transactions.md#transaction-modes-unreliable-transactions-disabled).


## Suppressing the ambient transaction

`config.Transactions().DoNotWrapHandlersExecutionInATransactionScope()` has been removed since transaction scopes are no longer used by non-DTC transports to delay the dispatch of all outgoing operations until handlers have been executed.

In NServiceBus version 6, handlers will be wrapped in a [TransactionScope](https://msdn.microsoft.com/en-us/library/system.transactions.transactionscope.aspx) only if the given transport chooses to do so. Transports that do this in their default configuration include [MSMQ](/transports/msmq/) and [SQL Server](/transports/sql/). This means that performing storage operations against data sources that also support transaction scopes will escalate to a distributed transaction. Opting out of this behavior can be done with the following:

```csharp
// For NServiceBus version 6.x
var transport = endpointConfiguration.UseTransport<MyTransport>();
transport.Transactions(TransportTransactionMode.ReceiveOnly);

// For NServiceBus version 5.x
var transactions = busConfiguration.Transactions();
transactions.DoNotWrapHandlersExecutionInATransactionScope();
```

For more information see [Transport transaction - sends atomic with receive](/transports/transactions.md#transaction-modes-transport-transaction-sends-atomic-with-receive).

NServiceBus version 6 leans on native transport transactions and the new [batched dispatch](/nservicebus/messaging/batched-dispatch.md) support to achieve the same level of consistency with better performance.

Suppressing the ambient transaction created by the MSMQ and SQL Server transports can still be achieved by creating a custom pipeline behavior with a suppressed transaction scope.


## Access to runtime settings

The following properties have been obsoleted on `TransactionSettings` class.


### SuppressDistributedTransactions

To determine if distributed transactions are suppressed.

```csharp
// For NServiceBus version 6.x
var transactionModeForReceives = readOnlySettings.GetRequiredTransactionModeForReceives();
var suppressDistributedTransactions = transactionModeForReceives != TransportTransactionMode.TransactionScope;

// For NServiceBus version 5.x
var suppressDistributedTransactions = transactionSettings.SuppressDistributedTransactions;
```


### IsTransactional

To determine if transactions are enabled.

```csharp
// For NServiceBus version 6.x
var transactionModeForReceives = readOnlySettings.GetRequiredTransactionModeForReceives();
var isTransactional = transactionModeForReceives != TransportTransactionMode.None;

// For NServiceBus version 5.x
var isTransactional = transactionSettings.IsTransactional;
```
