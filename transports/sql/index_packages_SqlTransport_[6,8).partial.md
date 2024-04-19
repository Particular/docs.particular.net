## Packages

The SQL Server transport is available in two different packages:

* [NServiceBus.Transport.SqlServer](https://www.nuget.org/packages/NServiceBus.Transport.SqlServer), which uses the newer [Microsoft.Data.SqlClient](https://www.nuget.org/packages/Microsoft.Data.SqlClient).
* [NServiceBus.SqlServer](https://www.nuget.org/packages/NServiceBus.SqlServer), which uses the [System.Data.SqlClient](https://www.nuget.org/packages/System.Data.SqlClient) that was originally part of the .NET Framework.

A transport package should be selected based on the SqlClient used by the rest of the system.