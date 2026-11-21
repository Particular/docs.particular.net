### ServiceControl.Audit/MessageBody/FileSystem/StoragePath

_Added in version 7.0.0_

The directory where message bodies are stored when `MessageBody/StorageType` is `FileSystem`. Use a separate directory for each instance; do not share it with another Audit instance or an Error instance.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MESSAGEBODY_FILESYSTEM_STORAGEPATH` |
| **App config key** | `ServiceControl.Audit/MessageBody/FileSystem/StoragePath` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None (required for file system storage) |

The ServiceControl Management Utility and the PowerShell module set this to `%ProgramData%\Particular\ServiceControl\<instance name>\MessageBodies`.
