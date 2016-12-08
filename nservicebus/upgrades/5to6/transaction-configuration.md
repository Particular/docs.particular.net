---
title: Transaction configuration changes in Version 6
reviewed: 2016-10-26
tags:
 - upgrade
 - migration
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

Version 6 provide a configuration API that is more aligned with the transaction capabilities of the transport.


## Enabling transactions

Transactions are enabled by default so calls to `.Enable()` can safely be removed.

snippet:5to6EnableTransactions


## Disabling transactions

Disabling transactions is now done by setting a transport transaction mode.

snippet:5to6DisableTransactions


## Enabling distributed transactions

Distributed transactions is the default mode for transports with DTC support but can be enabled explicitly.

snippet:5to6EnableDistributedTransactions


## Disabling distributed transactions

Disabling distributed transactions is now done by setting a transport transaction mode.

snippet:5to6DisableDistributedTransactions

Or, if the transport supports native AtomicWithReceive:

snippet:5to6DisableDistributedTransactionsNative


## Controlling transaction scope options

Version 6 allows transaction scope options to be configured at the transport level. Setting isolation level and timeout can now be done with the following:

snippet:5to6TransportTransactionScopeOptions


## Wrapping handlers execution in a transaction scope

Version 6 comes with a unit of work that wraps execution of handlers in a transaction scope, which can now be done with this API:

snippet:5to6WrapHandlersExecutionInATransactionScope


## Forwarding messages to error queue when transactions are disabled

When transactions are disabled and if any errors are encountered during the processing of the message then the messages will be forwarded to the error queue. In Version 5, this message would have been lost. For more details, read the [new behavior changes in Version 6](/nservicebus/transports/transactions.md#transactions-unreliable-transactions-disabled).


## Suppressing the ambient transaction

`config.Transactions().DoNotWrapHandlersExecutionInATransactionScope()` has been removed since transaction scopes are no longer used by non DTC transports to delay the dispatch of all outgoing operations until handlers have been executed.

In Version 6, handlers will only be wrapped in a [TransactionScope](https://msdn.microsoft.com/en-us/library/system.transactions.transactionscope.aspx) if the given transport chooses to do so. Transports that do this in their default configuration include [MSMQ](/nservicebus/msmq/) and [SQL Server](/nservicebus/sqlserver/). This means that performing storage operations against data sources that also support transaction scopes will escalate to a distributed transaction. Opting out of this behavior can be done with the following:

snippet: 5to6DoNotWrapHandlersInTransaction

For more information see [Transport transaction - Sends atomic with Receive](/nservicebus/transports/transactions.md#transactions-transport-transaction-sends-atomic-with-receive).

Version 6 leans on native transport transaction and the new [batched dispatch](/nservicebus/messaging/batched-dispatch.md) support to achieve the same level of consistency with better performance.

Suppressing the ambient transaction created by the MSMQ and SQL Server transports can still be achieved by creating a custom pipeline behavior with a suppressed transaction scope.


## Access to runtime settings

The following properties have been obsoleted on `TransactionSettings` class.


### SuppressDistributedTransactions

To determine if distributed transactions are suppressed.

snippet: 5to6SuppressDistributedTransactions


### IsTransactional

To determine if transactions are enabled.

snippet: 5to6IsTransactional