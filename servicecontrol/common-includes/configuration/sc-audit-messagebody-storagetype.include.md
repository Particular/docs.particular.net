#if-version [7,)
### ServiceControl.Audit/MessageBody/StorageType

The external storage provider used for message bodies.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MESSAGEBODY_STORAGETYPE` |
| **App config key** | `ServiceControl.Audit/MessageBody/StorageType` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None (required for SQL Server and PostgreSQL storage) |

Valid values are `FileSystem`, `AzureBlob`, and `S3`. The ServiceControl Management Utility and the PowerShell module do not set this setting or the provider settings; add them to the instance's configuration file or environment.
#end-if
