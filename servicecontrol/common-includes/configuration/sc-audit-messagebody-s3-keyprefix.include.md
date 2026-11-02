#if-version [7,)
### ServiceControl.Audit/MessageBody/S3/KeyPrefix

The prefix applied to message body object keys in Amazon S3. Every Audit instance has the same default, so set a different key prefix for each Audit instance that uses the same bucket.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MESSAGEBODY_S3_KEYPREFIX` |
| **App config key** | `ServiceControl.Audit/MessageBody/S3/KeyPrefix` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `audit-bodies/` |
#end-if
