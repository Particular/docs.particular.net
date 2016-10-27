## Customizing Outbox record peristence

By default the Outbox records are persisted in the following way:
 * The table has an auto-incremented integer primary key.
 * The `MessageId` column has a unique index.
 * There are indices on `Dispatched` and `DispatchedAt` columns.

Following API can be used to provide a different mapping of Outbox data to the underlying storage:

snippet:OutboxNHibernateCustomMapping

Should custom mapping be needed, the following characteristics of the original mapping needs to be preserved:
 * Values stored in `MessageId` column must be unique and an attempt to insert a duplicate entry must cause an exception.
 * Querying by `Dispatched` and `DispatchedAt` columns must be efficient because these columns are used by the cleaner process to remove outdated records.
 
