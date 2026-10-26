### ServiceControl/MinimumStorageLeftRequiredForIngestion

This setting was introduced in version 4.28. The percentage threshold for the [Critical message database storage space](/servicecontrol/servicecontrol-instances/#notifications-health-monitoring-critical-message-database-storage-space) check. If the remaining hard drive space drops below this threshold (as a percentage of the total space on the drive), then the check will fail, alerting the user. The message ingestion will also be stopped to prevent data loss. Message ingestion will resume once more disk space is made available.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MINIMUMSTORAGELEFTREQUIREDFORINGESTION` |
| **App config key** | `ServiceControl/MinimumStorageLeftRequiredForIngestion` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | 5 (percent) |
