#if-version [7,)
### ServiceControl.Audit/Database/Schema

The schema that contains the Audit instance tables. If the schema does not exist, setup creates it, which requires permission to create schemas. Changing this setting does not move existing data; an instance that already has data must keep using the schema that contains it.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_DATABASE_SCHEMA` |
| **App config key** | `ServiceControl.Audit/Database/Schema` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `sc_audit` |
#end-if
