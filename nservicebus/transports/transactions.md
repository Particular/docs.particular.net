---
title: Transport Transactions
summary: Supported transaction modes and their consistency guarantees
component: Core
versions: "[4,)"
tags:
- Transactions
- Transport
redirects:
- nservicebus/messaging/transactions
reviewed: 2016-08-31
---

This article covers various levels of consistency guarantees NServiceBus provides with regards to:

 * receiving messages
 * updating user data
 * sending messages

It does not discuss the transaction isolation aspect. Transaction isolation applies only to the process of updating the user data, it does not affect the overall coordination and failure handling.


## Transactions

NServiceBus offers four levels of guarantees with regards to message processing. Levels availability depends on the selected transport. See also [Transaction handling in Azure](/nservicebus/azure/understanding-transactionality-in-azure.md).


### Transaction levels supported by NServiceBus transports

The implementation details for each transport are discussed in the dedicated documentation sections. They can be accessed by clicking the links with the transport name in the following table:

partial:matrix


### Transaction scope (Distributed transaction)

In this mode the transport receive operation is wrapped in a [`TransactionScope`](https://msdn.microsoft.com/en-us/library/system.transactions.transactionscope). Other operations inside this scope, both sending messages and manipulating data, are guaranteed to be executed (eventually) as a whole or rolled back as a whole.

If required the transaction is escalated to a distributed one (following two-phase commit protocol). That depends on the transport and persistence choice, for example it is not needed when using [SQL Server transport](/nservicebus/sqlserver/) with [NHibernate persistence](/nservicebus/nhibernate/) both using the same database. In this case the ADO.NET driver guarantees that everything happens inside a single database transaction, ACID guarantees are held for the whole processing.

NOTE: MSMQ will escalate to a distributed transaction right away since it doesn't support promotable transaction enlistments.

*Transaction scope* mode is enabled by default for the transports that support it (i.e. MSMQ and SQL Server transport). It can be enabled explicitly via

snippet:TransportTransactionScope


#### Consistency guarantees

In this mode handlers will execute inside the `TransactionScope` created by the transport. This means that all the data updates are executed as a whole or rolled back as a whole.

Distributed transactions do not guarantee atomicity for an outside observer. For example outgoing messages might be dispatched and processed by some other endpoint before changes to the database get committed. However it is guaranteed that eventually all resources will sync up to reflect the outcome of the transaction.

NOTE: This mode requires the selected storage to support participating in distributed transactions.

partial:native

### Unreliable (Transactions Disabled)

Disabling transactions is generally not recommended, because it might lead to the message loss. It might be considered if losing some messages is not problematic and if the messages get outdated quickly, e.g. when sending readings from sensors at regular intervals.

DANGER: In this mode, when encountering a critical failure such as system or endpoint crash, the message is **permanently lost**.

snippet:TransactionsDisable

partial:unreliable

partial:outbox


## Avoiding partial updates

In this mode there is a risk of partial updates since one handler might succeed in updating business data while another handler fails. To avoid this configure NServiceBus to wrap all handlers in a `TransactionScope` that will act as a unit of work and make sure that there is no partial updates. Use following code to enable a wrapping scope:

snippet:TransactionsWrapHandlersExecutionInATransactionScope

NOTE: This requires that all the data stores used by the handler support enlisting in a distributed transaction (e.g. SQL Server), including the saga store when using sagas.

WARNING: This might escalate to a distributed transaction if data in different databases are updated.

partial:partial-updates


partial:scope-options
