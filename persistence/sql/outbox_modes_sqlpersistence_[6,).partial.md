## Concurrency control

By default the outbox uses optimistic concurrency control. That means that when two copies of the same message arrive at the endpoint, both messages are picked up (if concurrency settings of the endpoint allow for it) and processing begins on both of them. When the message handlers are completed, both processing threads attempt to insert the outbox record as part of the transaction that includes the application state change.

At this point, one of the transactions succeeds and the other fails due to a unique index constraint violation. When the copy of the message that failed is picked up again, it is discarded as a duplicate.

The outcome is that the application state change is applied only once (the other attempt has been rolled back) but the message handlers have been executed twice. If the message handler contains logic that has non-transactional side effects (e.g. sending an e-mail), that logic may be executed multiple times.

### Pessimistic concurrency control

The pessimistic concurrency control mode can be activated using the following API:

snippet: SqlPersistenceOutboxPessimisticMode

In the pessimistic mode the outbox record is inserted before the handlers are executed. As a result, when using a database that creates locks on insert, only one thread is allowed to execute the message handlers. The other thread, even though it picked up the second copy of a message, is blocked on a database lock. Once the first thread commits the transaction, the second thread is interrupted with an exception as it is not allowed to insert the outbox. As a result, the message handlers are executed only once.

The trade-off is that each message processing attempt requires additional round trip to the database.

> [!NOTE]
> The pessimistic mode depends on the locking behavior of the database when inserting rows. Consult the documentation of the database to check in which isolation modes the outbox pessimistic mode is appropriate.

> [!WARNING]
> Even the pessimistic mode does not ensure that the message handling logic is always executed exactly once. Non-transactional side effects, such as sending e-mail, can still be duplicated in case of errors that cause handling logic to be retried.

## Transaction type

The outbox supports transactions via [ADO.NET local transactions](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/local-transactions) or [TransactionScopes](https://learn.microsoft.com/en-us/dotnet/api/system.transactions.transactionscope).

### ADO.NET

By default, the outbox uses the ADO.NET local transactions abstracted via `DbTransaction`. This is appropriate for most situations.

#### Isolation Level

If required, the transaction [isolation level](https://learn.microsoft.com/en-us/dotnet/api/system.data.isolationlevel) may be adjusted:

snippet: SqlPersistenceOutboxIsolationLevel

A change in the isolation level affects all data access included in transactions. This should be used only if the business logic executed by message handlers within outbox transactions requires a different isolation level than the default (`ReadCommited`) to guarantee correctness.

Supported isolation levels:

- `Serializable` (Not supported by Oracle)
- `Read Commited` 
- `Repeatable Read` (Not supported by Oracle)
#if-version [8.2,)
- `Snapshot` (Only supported by SqlServer and Postgres)
#end-if

### Transaction Scope

In cases where the outbox transaction spans multiple databases, the `TransactionScope` support has to be enabled:

snippet: SqlPersistenceOutboxTransactionScopeMode

In this mode the SQL persistence creates a `TransactionScope` that wraps the whole message processing attempt and within that scope it opens a connection, that is used for:

- storing the outbox record
- persisting the application state changes applied via `SynchronizedStorageSession`

In addition to the connection managed by NServiceBus, users can open their own database connections in message handlers. If the underlying database technology supports distributed transactions managed by the Microsoft Distributed Transaction Coordinator (MSDTC) (e.g. SQL Server or Oracle), the transaction is escalated to a distributed transaction.

The `TransactionScope` mode is most useful in legacy scenarios such as when migrating from the MSMQ transport to a messaging infrastructure that does not support MSDTC. In this scenario, it is no longer possible to use a distributed transaction which includes the transport and the database. To maintain consistency, the outbox must be used instead. If the outbox table cannot be added to the legacy database, it may be placed in a separate database, but access to both databases must be included in distributed transactions.

#### Isolation Level

If required, the transaction [isolation level](https://learn.microsoft.com/en-us/dotnet/api/system.transactions.isolationlevel?) may be adjusted:

snippet: SqlPersistenceOutboxTransactionScopeMode

A change in the isolation level affects all data access included in transactions. This should be done only if the business logic executed by message handlers within outbox transactions requires a lower isolation level than the default (`Serializable`) to guarantee correctness.

Supported isolation levels:

- `Serializable`
- `Read Commited` 
- `Repeatable Read` (Not supported by Oracle)
#if-version [8.2,)
- `Snapshot` (Only supported by SqlServer and Postgres)
#end-if
