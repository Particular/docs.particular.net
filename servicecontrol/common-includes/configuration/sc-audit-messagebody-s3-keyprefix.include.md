### ServiceControl.Audit/MessageBody/S3/KeyPrefix

_Added in version 7.0.0_

The prefix applied to message body object keys in Amazon S3. Every Audit instance has the same default, so set a different key prefix for each Audit instance that uses the same bucket. The exception is Audit instances that [share a database](/servicecontrol/audit-instances/scaling-out-ingestion.md#sql-storage-shared-database), which must all use the same bucket and key prefix.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MESSAGEBODY_S3_KEYPREFIX` |
| **App config key** | `ServiceControl.Audit/MessageBody/S3/KeyPrefix` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `audit-bodies/` |
