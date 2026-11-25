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
| string | None |

Valid values are `FileSystem`, `AzureBlob`, and `S3`. The ServiceControl Management Utility sets this to `FileSystem`. The PowerShell module sets it from `-MessageBodyStorageType`, which defaults to `FileSystem`.

When this setting is not set, an Audit instance using SQL Server or PostgreSQL storage has no external body store. It still starts, but it does not store bodies that are not text or that are larger than [`ServiceControl.Audit/MaxBodySizeToStore`](/servicecontrol/audit-instances/configuration.md#performance-tuning-servicecontrol-auditmaxbodysizetostore), and those bodies cannot be viewed in ServicePulse. Set it for every SQL-backed Audit instance.
