---
title: SQL Transport Troubleshooting
summary: Tips on what to do when the SQL Transport is not behaving as expected
component: SQLTransport
reviewed: 2022-03-24
related:
 - transport/sql
---

## SqlException: certificate chain not trusted

After upgrading to version 4 of the `Microsoft.Data.SqlClient` package the endpoint may throw the following error at startup:

```
System.Data.SqlClient.SqlException
  HResult=0x80131904
  Message=A connection was successfully established with the server, but then an error occurred during the login process. (provider: SSL Provider, error: 0 - The certificate chain was issued by an authority that is not trusted.)
  Source=.Net SqlClient Data Provider
```

SQL Server uses a certificate to encrypt communication between itself and endpoints. Version 4 of the `Microsoft.Data.SqlClient` package includes a [breaking change](https://github.com/dotnet/SqlClient/pull/1210) to set `Encrypt=true` by default (the previous default was `false`) which causes this exception.

To fix it, [the SQL Server installation must be updated with a valid certificate and the client machine must be updated to trust this certificate](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/enable-encrypted-connections-to-the-database-engine).

WARNING: It is not recommended to eliminate this warning by adding `Encrypt=False` or `TrustServerCertificate=True` to the connection string. Both of these options leave the endpoint unsecure.

NOTE: If the endpoint connection string already contains `Encrypt=true` it may be removed.