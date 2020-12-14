#### Support for SQL Always Encrypted

The SQL Server transport has support for [SQL Server Always Encrypted](https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/always-encrypted-database-engine).

Note: Since Always Encrypted support only works with `Microsoft.Data.SqlClient`, this feature is only supported by the `NServiceBus.Transports.SqlTransport` package and not by the `NServiceBus.SqlTransport` package.

The steps to make use of SQL Always Encrypted are:

1. Make sure SQL Always Encrypted is configured correctly with the correct certificate or key stores on the database engine and the client machines.
1. Encrypt the `Body` and `Header` columns for the `endpoint` table that encryption is being enabled for. For more information on how to encrypt columns in SQL Server, refer to the [Microsoft documentation](https://docs.microsoft.com/en-us/sql/connect/ado-net/sql/sqlclient-support-always-encrypted?view=sql-server-ver15#retrieving-and-modifying-data-in-encrypted-columns).
1. Encrypt the `Body` and `Header` columns for the `endpoint.Delayed` table.
1. Ensure the connection string for the endpoint includes the `Column Encryption Setting = Enabled;` connection string parameter.
1. Ensure the connection string for ServiceControl also includes the `Column Encryption Setting = Enabled;` connection string parameter.

The `Body` and `Header` columns will now be readable only by clients that have the correct certificate or key stores configured.

Note: Encrypting columns requires a few parameters including the type of encryption, the algorithm and the key. Installers currently do not support encryption, therefore, [installers](/nservicebus/operations/installers.md) cannot be enabled in combination with SQL Server Always Encrypted.
