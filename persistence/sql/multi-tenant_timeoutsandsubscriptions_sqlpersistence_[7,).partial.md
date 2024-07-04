## Connections for subscriptions

When using multi-tenant mode, [subscriptions](subscriptions.md) are stored in a single database if the message transport does not provide native publish/subscribe.

If this persistence feature is used, but a connection builder is not specified, the following exception will be thrown:

> Couldn't find connection string for `{storageType.Name}`. The connection to the database must be specified using the `ConnectionBuilder` method. When in multi-tenant mode with `MultiTenantConnectionBuilder`, you must still use `ConnectionBuilder` to provide a database connection for subscriptions on message transports that don't support this feature natively.

To specify the connection builder for subscriptions, refer to the usage documentation for [Microsoft SQL](dialect-mssql.md#usage), [MySQL](dialect-mysql.md#usage), [PostgreSQL](dialect-postgresql.md#usage), or [Oracle](dialect-oracle.md#usage).

When using a transport with native publish/subscribe, this is not required and no exception will be thrown.