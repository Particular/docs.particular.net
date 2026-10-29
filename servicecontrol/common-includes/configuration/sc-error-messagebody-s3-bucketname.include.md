#if-version [7,)
### ServiceControl/MessageBody/S3/BucketName

The Amazon S3 bucket used to store message bodies when `StorageType` is `S3`.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MESSAGEBODY_S3_BUCKETNAME` |
| **App config key** | `ServiceControl/MessageBody/S3/BucketName` |
| **SCMU field** | `BUCKET NAME` |

| Type | Default value |
| --- | --- |
| string | None (required for S3 storage) |
#end-if
