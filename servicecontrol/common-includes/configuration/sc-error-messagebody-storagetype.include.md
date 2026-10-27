#if-version [7,)
### ServiceControl/MessageBody/StorageType

The external storage provider used for message bodies.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MESSAGEBODY_STORAGETYPE` |
| **App config key** | `ServiceControl/MessageBody/StorageType` |
| **SCMU field** | `MESSAGE BODY STORAGE` |

| Type | Default value |
| --- | --- |
| string | None (required for SQL Server and PostgreSQL storage) |

Valid values are `FileSystem`, `AzureBlob`, and `S3`.
#end-if
