### ServiceControl/MessageBody/StorageType

_Added in version 7.0.0_

The external storage provider used for message bodies.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MESSAGEBODY_STORAGETYPE` |
| **App config key** | `ServiceControl/MessageBody/StorageType` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None (required for SQL Server and PostgreSQL storage) |

Valid values are `FileSystem`, `AzureBlob`, and `S3`. The ServiceControl Management Utility sets this to `FileSystem`. The PowerShell module sets it from `-MessageBodyStorageType`, which defaults to `FileSystem`.
