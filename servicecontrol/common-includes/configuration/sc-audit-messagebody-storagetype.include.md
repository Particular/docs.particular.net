### ServiceControl.Audit/MessageBody/StorageType

_Added in version 7.0.0_

The external storage provider used for message bodies.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MESSAGEBODY_STORAGETYPE` |
| **App config key** | `ServiceControl.Audit/MessageBody/StorageType` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None (required for SQL Server and PostgreSQL storage) |

Valid values are `FileSystem`, `AzureBlob`, and `S3`. The ServiceControl Management Utility and the PowerShell module set this to `FileSystem`; to use another provider, change it in the instance's configuration file or environment.
