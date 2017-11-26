
### Oracle

Using the [Oracle.ManagedDataAccess NuGet Package](https://www.nuget.org/packages/Oracle.ManagedDataAccess).

snippet: SqlPersistenceUsageOracle

In Versions 2.2.0 and above it's possible to specify custom schema using the following code:

```
var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
persistence.SqlVariant(SqlVariant.Oracle);
persistence.Schema("custom_schema");
```

NOTE: The ODP.NET managed driver requires `Enlist=false` or `Enlist=dynamic` setting in the [Oracle connection string](https://docs.oracle.com/database/121/ODPNT/featConnecting.htm) to allow the persister to enlist in a [Distributed Transaction](https://msdn.microsoft.com/en-us/library/windows/desktop/ms681205.aspx) at the correct moment.