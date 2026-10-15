#if-version [7,)
### ServiceControl.Audit/Database/Schema

The schema that contains the Audit instance tables. The schema must already exist.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_DATABASE_SCHEMA` |
| **App config key** | `ServiceControl.Audit/Database/Schema` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | The database provider's default schema (`dbo` for SQL Server or `public` for PostgreSQL) |
#end-if
