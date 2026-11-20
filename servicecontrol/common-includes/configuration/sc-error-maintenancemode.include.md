### ServiceControl/MaintenanceMode

Run [ServiceControl error instance in maintenance mode](/servicecontrol/storage/ravendb/accessing-database.md) in order to do database maintenance.

For an instance running as a Windows service, set the app config key and restart the service. See [scripted maintenance mode](/servicecontrol/storage/ravendb/accessing-database.md#windows-deployment-maintenance-mode-scripted-maintenance-mode).

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MAINTENANCEMODE` |
| **App config key** | `ServiceControl/MaintenanceMode` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| boolean | `False` |
