## Concurrency control

By default the outbox uses optimistic concurrency control. That means that when two copies of the same message arrive at the endpoint, both messages are picked up (if concurrency settings of the endpoint allow for it) and processing begins on both of them. When the message handlers are completed, both processing threads attempt to insert the outbox record as part of the transaction that includes the application state change. 

At this point of of the transactions succeeds and the other fails due to unique index constraint violation. When the copy of the message that failed is picked up again, it is discarded as a duplicate.

The outcome is that the application state change is applied only once (the other attempt has been rolled back) but the message handlers have been executed twice. If the message handler contains logic that has non-transactional side effects (e.g. sending an e-mail), that logic may be executed multiple times.

### Pessimistic concurrency control



## Transaction type


By default the outbox records are persisted in the following way:

 * The table has an auto-incremented integer primary key.
 * The `MessageId` column has a unique index.
 * There are indices on `Dispatched` and `DispatchedAt` columns.

The following API can be used to provide a different mapping of outbox data to the underlying storage:

snippet: OutboxNHibernateCustomMappingConfig

snippet: OutboxNHibernateCustomMapping

If custom mapping is required, the following characteristics of the original mapping must be preserved:

 * Values stored in the `MessageId` column must be unique and an attempt to insert a duplicate entry must cause an exception.
 * Querying by `Dispatched` and `DispatchedAt` columns must be efficient because these columns are used by the cleanup process to remove outdated records.
