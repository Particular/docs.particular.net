---
title: SQL Server Transport transaction support
summary: The design and implementation details of SQL Server Transport transaction support
reviewed: 2016-08-03
component: SqlServer
tags:
- SQL Server
- Transactions
- Transport
---

SQL Server transport supports the following [Transport Transaction Modes](/nservicebus/transports/transactions.md):

 * Transaction scope (Distributed transaction)
 * Transport transaction - Sends atomic with Receive
 * Transport transaction - Receive Only
 * Unreliable (Transactions Disabled)


### Transaction scope (Distributed transaction)

In this mode the ambient transaction is started before receiving the message. The transaction encompasses all stages of processing including user data access and saga data access. 

If either the configured NServiceBus persistence mechanism or the user data access also support transactions via `TransactionScope`, the ambient transaction is escalated to a distributed one via the Distributed Transaction Coordinator (DTC).

NOTE: If the peristence mechanisms use SQL Server 2008 or later as an underlying data store and the connection string configured for the SQL Server transport and the persistence is the same, there will be no DTC escalation as SQL Server is able to handle multiple non-overlapping connections via a local transaction.

See also [Sample covering this mode of operation](/samples/sqltransport-nhpersistence/).


### Native transactions

In this mode the message is received inside a native ADO.NET transaction

#### Versions 3 and above

There are two available options within native transaction level:

 * **ReceiveOnly** - An input message is received using native transaction. The transaction is committed only when message processing succeeds.

NOTE: This transaction is not shared outside of the message receiver. That means there is a possibility of persistent side-effects when processing fails, i.e. *ghost messages* might occur.

 * ** SendsAtomicWithReceive** - This mode is similar to the `ReceiveOnly`, but transaction is shared with sending operations. That means the message receive operation and any send or publish operations are committed atomically.


#### Versions 2 and below

There was no distinction between `ReceiveOnly` and `SendsAtomicWithReceive`. Using native transaction was equivalent to `SendsAtomicWithReceive` mode.


### Unreliable (Transactions Disabled)

In this mode when a message is received it is immediately removed from the input queue. If processing fails the message is lost because the operation cannot be rolled back. Any other operation that is performed when processing the message is executed without a transaction and cannot be rolled back. This can lead to undesired side effects when message processing fails part way through.


## Concurrency

The SQL Server transport adapts the number of receiving threads (up to `MaximumConcurrencyLevel` [set via `TransportConfig` section](/nservicebus/msmq/transportconfig.md)) to the amount of messages waiting for processing. When idle it maintains only one thread that continuously polls the table queue.


### Version 3

In Versions 3 and above SQL Server transport maintains a dedicated monitoring thread for each input queue. It is responsible for detecting the number of messages waiting for delivery and creating receive [Task](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.aspx)s - one for each pending message.

The maximum number of concurrent tasks will never exceed `MaximumConcurrencyLevel`. The number of tasks does not translate to the number of running threads which is controlled by the TPL scheduling mechanisms.


### Version 2.1

Version 2.1 of SqlTransport uses an adaptive concurrency model. The transport adapts the number of polling threads based on the rate of messages coming in. The key concept in this new model is the *ramp up controller* which controls the ramping up of new threads and decommissioning of unnecessary threads. It uses the following algorithm:

 * if last receive operation yielded a message, it increments the *consecutive successes* counter and resets the *consecutive failures* counter
 * if last receive operation yielded no message, it increments the *consecutive failures* counter and resets the *consecutive successes* counter
 * if *consecutive successes* counter goes over a certain threshold and there is less polling threads than `MaximumConcurrencyLevel`, it starts a new polling thread and resets the *consecutive successes* counter
 * if *consecutive failures* counter goes over a certain threshold and there is more than one polling thread it kills one of the polling threads


### Version 2.0

In 2.0 release support for callbacks has been added. Callbacks are implemented by each endpoint instance having a unique [secondary queue](./#secondary-queues). The receive for the secondary queue does not use the `MaximumConcurrencyLevel` and defaults to 1 thread. This value can be adjusted via the configuration API.


### Prior to version 2.0

Prior to 2.0 each endpoint running SQLServer transport spins up a fixed number of threads (controlled by `MaximumConcurrencyLevel` property of `TransportConfig` section) both for input and satellite queues. Each thread runs in loop, polling the database for messages awaiting processing.

The disadvantage of this simple model is the fact that satellites (e.g. [Delayed Retries](/nservicebus/recoverability/#delayed-retries), Timeout Manager) share the same concurrency settings but usually have much lower throughput requirements. If both Delayed Retries and Timeout Manager are enabled, setting `MaximumConcurrencyLevel` to 10 results in 40 threads in total, each polling the database even if there are no messages to be processed.