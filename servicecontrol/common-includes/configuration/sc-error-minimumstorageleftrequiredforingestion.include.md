### ServiceControl/MinimumStorageLeftRequiredForIngestion

_Added in version 4.28.0_

The percentage threshold for the [Critical message database storage space](/servicecontrol/servicecontrol-instances/#notifications-health-monitoring-critical-message-database-storage-space) check. If the remaining hard drive space drops below this threshold, the check fails and message ingestion stops to prevent data loss. Message ingestion resumes when more disk space is available.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MINIMUMSTORAGELEFTREQUIREDFORINGESTION` |
| **App config key** | `ServiceControl/MinimumStorageLeftRequiredForIngestion` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `5` (percent) |
