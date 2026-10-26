#if-version [7,)
### ServiceControl/Database/Schema

The schema that contains the ServiceControl tables. The schema must already exist. Schema names can contain letters, digits, and underscores, must start with a letter or underscore, and are limited to 63 characters.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_DATABASE_SCHEMA` |
| **App config key** | `ServiceControl/Database/Schema` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | The database provider's default schema (`dbo` for SQL Server or `public` for PostgreSQL) |
#end-if
