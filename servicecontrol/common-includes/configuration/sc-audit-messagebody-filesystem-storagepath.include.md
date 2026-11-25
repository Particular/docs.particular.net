### ServiceControl.Audit/MessageBody/FileSystem/StoragePath

_Added in version 7.0.0_

The directory where message bodies are stored when `MessageBody/StorageType` is `FileSystem`. Use a separate directory for each instance. The exception is Audit instances that [share a database](/servicecontrol/audit-instances/scaling-out-ingestion.md#sql-storage-shared-database), which must all use the same directory.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MESSAGEBODY_FILESYSTEM_STORAGEPATH` |
| **App config key** | `ServiceControl.Audit/MessageBody/FileSystem/StoragePath` |
| **SCMU field** | MESSAGE BODY PATH |

| Type | Default value |
| --- | --- |
| string | None (required for file system storage) |

The ServiceControl Management Utility suggests `%ProgramData%\Particular\ServiceControl\<instance name>\MessageBodies`. The PowerShell module sets this from `-MessageBodyPath`.

Exclude this directory from antivirus scans. See [anti-virus checks](/servicecontrol/servicecontrol-in-practice.md#anti-virus-checks-message-body-directories).
