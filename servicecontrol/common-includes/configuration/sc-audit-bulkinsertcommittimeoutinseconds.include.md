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
