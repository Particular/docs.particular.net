## Concurrency control

By default the outbox uses optimistic concurrency control. That means that when two copies of the same message arrive at the endpoint, both messages are picked up (if concurrency settings of the endpoint allow for it) and processing begins on both of them. When the message handlers are completed, both processing threads attempt to insert the outbox record as part of the transaction that includes the application state change. 

At this point of of the transactions succeeds and the other fails due to unique index constraint violation. When the copy of the message that failed is picked up again, it is discarded as a duplicate.

The outcome is that the application state change is applied only once (the other attempt has been rolled back) but the message handlers have been executed twice. If the message handler contains logic that has non-transactional side effects (e.g. sending an e-mail), that logic may be executed multiple times.

### Pessimistic concurrency control

The pessimistic concurrency control mode can be activated using the following API:

snippet: OutboxPessimisticMode

In the pessimistic mode the outbox record is inserted before the handlers are executed. As a result, when using a database that creates locks on insert, only one thread is allowed to execute the message handlers. The other thread, even though it picked up the second copy of a message, is blocked on a database lock. Once the first thread commits the transaction, the second thread is interrupted with an exception as it is not allowed to insert the outbox. As a result, the message handlers are executed only once.

The trade-off is that each message processing attempt requires additional round trip to the database.

NOTE: The pessimistic mode depends on the locking behavior of the database when inserting rows. Consult the documentation of the database to check in which isolation modes the outbox pessimistic mode is appropriate. 

WARN: Even the pessimistic mode does not ensure that the message handling logic is always executed exactly once. Non-transactional side effects, such as sending e-mail, can still be duplicated in case of errors that cause handling logic to be retried.

## Transaction type

By default the outbox uses the ADO.NET transactions abstracted via `ITransaction`. This is appropriate for most situations.

### Transaction Scope

In cases where the outbox transaction spans multiple databases, the `TransactionScope` support has to be enabled:

snippet: OutboxTransactionScopeMode

In this mode the NHibernate persistence creates a `TransactionScope` that wraps the whole message processing attempt and within that scope it opens a session, that is used for:
 - storing the outbox record
 - persisting the application state change applied via `SynchronizedStorageSession`

In addition to that session managed by NServiceBus, users can open their own NHibernate sessions or plain database connections in the message handlers. If the underlying database technology supports distributed transactions managed by Microsoft Distributed Transaction Coordinator -- MS DTC (e.g. SQL Server, Oracle or PostgreSQL), the transaction gets escalated to a distributed transaction.

The `TransactionScope` mode is most useful in legacy scenarios e.g. when migrating from MSMQ transport to a messaging infrastructure that does not support MS DTC. In order to maintain consistency the outbox has to be used in place of distributed transport-database transactions. If the legacy database cannot be modified to add the outbox table, the only option is to place the outbox table in a separate database and use distributed transactions between the databases.