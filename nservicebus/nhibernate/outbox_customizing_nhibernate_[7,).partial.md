## Customizing outbox record peristence

By default the outbox records are persisted in the following way:
 * The table has an auto-incremented integer primary key
 * The `MessageId` column has a unique index
 * There are indices on `Dispatched` and `DispatchedAt` columns.

Following API can be used to provide a different mapping of outbox data to the underlying storage:

snippet:OutboxNHibernateCustomMapping

The following properties of the original mapping need to be preserved when providing a custom mapping:
 * `MessageId` column values are guaranteed to be unique and an attempt to insert same values to different rows must cause an exception
 * Querying by `Dispatched` and `DispatchedAt` columns must be efficient because these columns are used by the cleaner process to remove outdated outbox records.
 
