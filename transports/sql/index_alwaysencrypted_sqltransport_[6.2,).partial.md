#### Support for SQL Always Encrypted

SQL Server Transport has support for [SQL Server Always Encrypted](https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/always-encrypted-database-engine).

The steps to make use of SQL Always Encrypted are:

1. Make sure SQL Always Encrypted is configured correctly with the correct certificate or key stores on the database engine and the client machines.
1. Encrypt the `Body` and `Header` columns for the `endpoint` table that you are enabling encryption for.
1. Encrypt the `Body` and `Header` columns for the `endpoint.Delayed` table.
1. Ensure the connection string for the endpoint includes the `Column Encryption Setting = Enabled;` connection string parameter.
1. Ensure the connection string for ServiceControl also includes the `Column Encryption Setting = Enabled;` connection string parameter.

The `Body` and `Header` columns will now only be readable by clients that have the correct certificate or key stores configured.

Note: Since Always Encrypted support only works with `Microsoft.Data.SqlClient`, this feature is only supported by the `NServiceBus.Transports.SqlTransport` package and not `NServiceBus.SqlTransport` package.