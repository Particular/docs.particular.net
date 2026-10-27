### ServiceControl.Audit/DatabaseMaintenancePort

The port to expose the RavenDB database.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_DATABASEMAINTENANCEPORT` |
| **App config key** | `ServiceControl.Audit/DatabaseMaintenancePort` |
| **SCMU field** | `DATABASE MAINTENANCE PORT NUMBER (1 - 49151)` |

| Type | Default value |
| --- | --- |
| int | `44445` |

> [!NOTE]
> This setting is not relevant when running an audit instance in a container.

#if-version [7,)
This setting applies only to RavenDB storage.
#end-if
