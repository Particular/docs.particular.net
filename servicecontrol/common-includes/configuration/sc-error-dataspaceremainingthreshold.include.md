### ServiceControl/DataSpaceRemainingThreshold

The percentage threshold for the [Message database storage space](/servicecontrol/servicecontrol-instances/#notifications-health-monitoring-message-database-storage-space) check. If the remaining hard drive space drops below this threshold (as a percentage of the total space on the drive), then the check will fail, alerting the user.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_DATASPACEREMAININGTHRESHOLD` |
| **App config key** | `ServiceControl/DataSpaceRemainingThreshold` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | 20 (percent) |
