### ServiceControl.Audit/MinimumStorageLeftRequiredForIngestion

The percentage threshold for the [Critical message database storage space](/servicecontrol/servicecontrol-instances/#notifications-health-monitoring-message-database-storage-space) check. If the remaining hard drive space drops below this threshold (as a percentage of the total space on the drive), then the check will fail, alerting the user. The message ingestion will also be stopped to prevent data loss. Message ingestion will resume once more disk space is made available.

| Type | Default value |
| --- | --- |
| int | `5` |

#if-version [7,)
This setting applies only to RavenDB storage. SQL Server and PostgreSQL storage do not stop ingestion based on free disk space.
#end-if
