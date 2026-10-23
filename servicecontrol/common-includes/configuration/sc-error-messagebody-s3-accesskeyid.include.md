#if-version [7,)
### ServiceControl/MessageBody/S3/AccessKeyId

The access key ID used to authenticate with S3. Configure this setting together with `ServiceControl/MessageBody/S3/SecretAccessKey`, or leave both empty to use the AWS SDK default credential chain.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MESSAGEBODY_S3_ACCESSKEYID` |
| **App config key** | `ServiceControl/MessageBody/S3/AccessKeyId` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |
#end-if
