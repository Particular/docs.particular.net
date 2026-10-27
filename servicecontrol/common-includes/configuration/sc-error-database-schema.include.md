#if-version [7,)
### ServiceControl/Database/Schema

The schema that contains the Error instance tables. If the schema does not exist, setup creates it, which requires permission to create schemas. Changing this setting does not move existing data; an instance that already has data must keep using the schema that contains it.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_DATABASE_SCHEMA` |
| **App config key** | `ServiceControl/Database/Schema` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None. Tables are created in the connection's default schema: the login's default schema on SQL Server (usually `dbo`), or the current schema from `search_path` on PostgreSQL (usually `public`). |
#end-if
