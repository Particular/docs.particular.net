#if-version [5,)
### ServiceControl.Audit/BulkInsertCommitTimeoutInSeconds

Configures the maximum duration, in seconds, for processing a batch of audited messages.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_BULKINSERTCOMMITTIMEOUTINSECONDS` |
| **App config key** | `ServiceControl.Audit/BulkInsertCommitTimeoutInSeconds` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `60` (1 minute) |
#end-if
#if-version [7,)

This setting applies only to RavenDB storage. For SQL Server and PostgreSQL storage, see [`Database/CommandTimeout`](/servicecontrol/audit-instances/configuration.md#storage-servicecontrol-auditdatabasecommandtimeout).
#end-if
