#if-version [7,)
### ServiceControl/MessageBody/FileSystem/StoragePath

The directory where message bodies are stored when `StorageType` is `FileSystem`.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MESSAGEBODY_FILESYSTEM_STORAGEPATH` |
| **App config key** | `ServiceControl/MessageBody/FileSystem/StoragePath` |
| **SCMU field** | `MESSAGE BODY PATH` |

| Type | Default value |
| --- | --- |
| string | None (required for file system storage) |
#end-if
