## Customizing outbox record persistence

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
