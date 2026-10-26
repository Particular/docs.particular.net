#if-version [7,)
### ServiceControl/Database/ConnectionString

The connection string for the SQL Server or PostgreSQL database. The database must already exist.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_DATABASE_CONNECTIONSTRING` |
| **App config key** | `ServiceControl/Database/ConnectionString` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None (required for SQL Server and PostgreSQL storage) |
#end-if
