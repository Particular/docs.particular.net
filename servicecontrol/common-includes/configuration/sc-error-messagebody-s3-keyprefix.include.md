#if-version [7,)
### ServiceControl/MessageBody/S3/KeyPrefix

The prefix applied to message body object keys in Amazon S3.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MESSAGEBODY_S3_KEYPREFIX` |
| **App config key** | `ServiceControl/MessageBody/S3/KeyPrefix` |
| **SCMU field** | `KEY PREFIX (OPTIONAL, DEFAULTS TO error-bodies/)` |

| Type | Default value |
| --- | --- |
| string | `error-bodies/` |
#end-if
