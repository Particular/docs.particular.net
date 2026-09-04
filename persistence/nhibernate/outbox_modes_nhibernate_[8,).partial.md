## Concurrency control

By default, the outbox uses optimistic concurrency control; when two copies of the same message arrive, the endpoint may process both concurrently. After the message handlers finish, both processing attempts try to insert an outbox record in the transaction that contains the application state change. One transaction will succeed, and the other will fail with a unique index constraint violation. When the failed message is processed again, the endpoint discards it as a duplicate.

The application state change is applied only once since the other attempt is rolled back, but the message handlers still run twice. Non-transactional side effects, e.g. sending an email, may therefore occur more than once.

### Pessimistic concurrency control

Enable pessimistic concurrency control using the following API:

snippet: OutboxPessimisticMode

In pessimistic mode, the outbox record is inserted before the handlers run. With a database that locks inserted rows, only one processing attempt can run the message handlers. The attempt processing the duplicate waits for the database lock. After the first attempt commits, the duplicate insert fails, and the handlers do not run for the duplicate.

The trade-off is that each message processing attempt requires an additional round trip to the database.

> [!NOTE]
> Pessimistic mode depends on how the database locks inserted rows. Consult the database documentation to determine which transaction isolation levels support this mode.

> [!WARNING]
> Pessimistic mode does not guarantee that message handling logic runs exactly once. Errors that cause retries can still duplicate non-transactional side effects, such as sending an email.

## Transactions

By default, the outbox uses an ADO.NET transaction through NHibernate's `ITransaction` abstraction. This mode is appropriate for most scenarios.

### Transaction Scope

When an outbox transaction must span multiple databases, enable `TransactionScope` support:

snippet: OutboxTransactionScopeMode

In this mode, NHibernate Persistence creates a `TransactionScope` around the entire message processing attempt. Within that scope, it opens a session that is used for:

- Storing the outbox record.
- Persisting application state changes made through `SynchronizedStorageSession`.

Message handlers can also open NHibernate sessions or database connections. When the database supports transactions managed by Microsoft Distributed Transaction Coordinator (MS DTC), enlisting multiple connections escalates the transaction to a distributed transaction. Examples of supported databases include SQL Server, Oracle, and PostgreSQL.

`TransactionScope` mode is primarily useful in legacy scenarios, e.g. when migrating from MSMQ to a transport that does not support distributed transactions. The outbox provides consistency in place of distributed transactions between the transport and the database. If the existing database cannot be modified to add the outbox table, place the table in a separate database and use a distributed transaction between the two databases.
