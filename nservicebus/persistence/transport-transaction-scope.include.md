## TransactionScope mode

When the transport is set to `TransactionScope` transaction mode NServiceBus expects the persistence to hook into the ambient transaction in order to ensure *exactly-once* message processing. The persistence does it by ensuring that the database connection created for synchronized storage session is enlisted in the transports transaction by calling `DbConnection.EnlistTransaction` method.

If a database driver configured for the persistence does not support this method (e.g. MySQL), an exception will be thrown to prevent incorrect business behavior (executing handler in *at-least-once* mode when user expects *exactly-once*). To mitigate this problem
 * use database that supports distributed transactions (such as SQL Server or Oracle) or
 * enable [Outbox](/nservicebus/outbox/) or
 * rewrite message handler logic to ensure it is idempotent.