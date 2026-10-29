#if-version [7,)
### ServiceControl.Audit/Database/ConnectionString

The connection string for the SQL Server or PostgreSQL database. The database must already exist.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_DATABASE_CONNECTIONSTRING` |
| **App config key** | `ServiceControl.Audit/Database/ConnectionString` |
| **SCMU field** | `DATABASE CONNECTION STRING` |

| Type | Default value |
| --- | --- |
| string | None (required for SQL Server and PostgreSQL storage) |
#end-if
