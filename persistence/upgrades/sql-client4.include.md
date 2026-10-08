`Microsoft.Data.SqlClient` requires an encrypted connection by default and validates the certificate presented by SQL Server. If the server is not configured with a valid certificate, or the machine hosting the endpoint does not trust that certificate, this can cause an exception at startup:

```txt
Microsoft.Data.SqlClient.SqlException
  HResult=0x80131904
  Message=A connection was successfully established with the server, but then an error occurred during the login process. (provider: SSL Provider, error: 0 - The certificate chain was issued by an authority that is not trusted.)
```

Requiring encryption has been the default since version 4, which [changed the `Encrypt` default from `False` to `True`](https://github.com/dotnet/SqlClient/pull/1210). Endpoints that connected without encryption before upgrading to version 4 or later will encounter this exception.

To fix this, [update the SQL Server installation with a valid certificate](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/configure-sql-server-encryption) and [configure the machine hosting the endpoint to trust that certificate](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/special-cases-for-encrypting-connections-sql-server).

> [!WARNING]
> It is not recommended to eliminate this exception by adding `Encrypt=False` or `TrustServerCertificate=True` to the connection string. Both of these options leave the endpoint unsecure.
