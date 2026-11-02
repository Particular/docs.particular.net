#if-version [7,)
### ServiceControl.Audit/MessageBody/S3/SecretAccessKey

The secret access key used to authenticate with S3. Configure this setting together with `ServiceControl.Audit/MessageBody/S3/AccessKeyId`, or leave both empty to use the AWS SDK default credential chain.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MESSAGEBODY_S3_SECRETACCESSKEY` |
| **App config key** | `ServiceControl.Audit/MessageBody/S3/SecretAccessKey` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |
#end-if
