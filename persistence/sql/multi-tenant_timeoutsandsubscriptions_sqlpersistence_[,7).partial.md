## Connections for timeouts and subscriptions

When using multi-tenant mode, [timeouts](timeouts.md) and [subscriptions](subscriptions.md) are stored in a single database if the message transport does not provide those features (delayed delivery and publish/subscribe) natively.

If these persistence features are used, but a connection builder is not specified, the following exception will be thrown:

> Couldn't find connection string for `{storageType.Name}`. The connection to the database must be specified using the `ConnectionBuilder` method. When in multi-tenant mode with `MultiTenantConnectionBuilder`, you must still use `ConnectionBuilder` to provide a database connection for subscriptions/timeouts on message transports that don't support those features natively.

To specify the connection builder for timeouts or subscriptions, refer to the usage documentation for [Microsoft SQL](dialect-mssql.md#usage), [MySQL](dialect-mysql.md#usage), [PostgreSQL](dialect-postgresql.md#usage), or [Oracle](dialect-oracle.md#usage).

When using a transport with both native delayed delivery and native publish/subscribe, this is not required and no exception will be thrown.