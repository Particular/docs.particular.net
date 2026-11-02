#if-version [7,)
### ServiceControl/StorageType

The storage implementation used by the instance. This setting is required; an instance without it does not start.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_STORAGETYPE` |
| **App config key** | `ServiceControl/StorageType` |
| **SCMU field** | `STORAGE TYPE` |

| Type | Default value |
| --- | --- |
| string | None (required) |

Valid values are `RavenDB`, `SQLServer`, and `PostgreSQL`. To choose where message bodies are stored with SQL Server or PostgreSQL storage, use [`ServiceControl/MessageBody/StorageType`](/servicecontrol/servicecontrol-instances/configuration.md#storage-servicecontrolmessagebodystoragetype).
#end-if
