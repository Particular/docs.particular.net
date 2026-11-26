### ServiceControl.Audit/MaintenanceMode

Run [ServiceControl audit instance in maintenance mode](/servicecontrol/storage/ravendb/accessing-database.md) in order to do database maintenance.

For an instance running as a Windows service, set the app config key and restart the service. See [scripted maintenance mode](/servicecontrol/storage/ravendb/accessing-database.md#windows-deployment-maintenance-mode-scripted-maintenance-mode).

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MAINTENANCEMODE` |
| **App config key** | `ServiceControl.Audit/MaintenanceMode` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| boolean | `False` |
