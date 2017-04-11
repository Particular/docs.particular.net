
### Oracle

Using the [Oracle.ManagedDataAccess NuGet Package](https://www.nuget.org/packages/Oracle.ManagedDataAccess).

snippet: SqlPersistenceUsageOracle

NOTE: The `Enlist=false` setting is required for the [Oracle connection string](https://docs.oracle.com/database/121/ODPNT/featConnecting.htm) to prevent auto-enlistment in a [Distributed Transaction](https://msdn.microsoft.com/en-us/library/windows/desktop/ms681205.aspx) which prevents the persistence from enlisting at the correct moment.