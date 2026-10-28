### ServiceControl.Audit/MaintenanceMode

Run a RavenDB-backed [ServiceControl audit instance in maintenance mode](/servicecontrol/storage/ravendb/accessing-database.md) to perform database maintenance.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MAINTENANCEMODE` |
| **App config key** | `ServiceControl.Audit/MaintenanceMode` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| boolean | `False` |

#if-version [7,)
Maintenance mode is supported only with embedded RavenDB. An instance using external RavenDB, SQL Server, or PostgreSQL with this setting set to `true` does not start. To perform database maintenance, stop the ServiceControl instance and access the external database directly.
#end-if
