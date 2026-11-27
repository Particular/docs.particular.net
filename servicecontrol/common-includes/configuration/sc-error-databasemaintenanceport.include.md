### ServiceControl/DatabaseMaintenancePort

_Applies only to RavenDB storage_

The port to expose the RavenDB database.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_DATABASEMAINTENANCEPORT` |
| **App config key** | `ServiceControl/DatabaseMaintenancePort` |
| **SCMU field** | `DATABASE MAINTENANCE PORT NUMBER (1 - 49151)` |

| Type | Default value |
| --- | --- |
| int | `33334` |

> [!NOTE]
> This setting is not relevant when running an error instance in a container.
