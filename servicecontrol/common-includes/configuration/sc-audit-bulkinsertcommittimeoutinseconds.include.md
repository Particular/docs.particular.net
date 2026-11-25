### ServiceControl.Audit/BulkInsertCommitTimeoutInSeconds

_Applies only to RavenDB storage_

Configures the maximum duration, in seconds, for processing a batch of audited messages.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_BULKINSERTCOMMITTIMEOUTINSECONDS` |
| **App config key** | `ServiceControl.Audit/BulkInsertCommitTimeoutInSeconds` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `60` (1 minute) |

For SQL Server and PostgreSQL storage, see [`Database/CommandTimeout`](/servicecontrol/audit-instances/configuration.md#sql-server-and-postgresql-storage-servicecontrol-auditdatabasecommandtimeout).
