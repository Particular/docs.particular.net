### ServiceControl/MaintenanceMode

Run a RavenDB-backed [ServiceControl Error instance in maintenance mode](/servicecontrol/storage/ravendb/accessing-database.md) to perform database maintenance.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MAINTENANCEMODE` |
| **App config key** | `ServiceControl/MaintenanceMode` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| boolean | `False` |

#if-version [7,)
SQL Server and PostgreSQL storage do not support maintenance mode. An instance using either with this setting set to `true` does not start.
#end-if
