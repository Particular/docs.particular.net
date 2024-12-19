Version 4 of `Microsoft.Data.SqlClient` includes [a breaking change](https://github.com/dotnet/SqlClient/pull/1210) which sets `Encrypt=True` by default. If the client and server are not configured with a valid certificate, this can cause an exception at startup:

```txt
System.Data.SqlClient.SqlException
  HResult=0x80131904
  Message=A connection was successfully established with the server, but then an error occurred during the login process. (provider: SSL Provider, error: 0 - The certificate chain was issued by an authority that is not trusted.)
  Source=.Net SqlClient Data Provider
```

To fix this, [the SQL Server installation must be updated with a valid certificate and the machine hosting the endpoint must be updated to trust this certificate](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/enable-encrypted-connections-to-the-database-engine).

> [!WARNING]
> It is not recommended to eliminate this warning by adding `Encrypt=False` or `TrustServerCertificate=True` to the connection string. Both of these options leave the endpoint unsecure.