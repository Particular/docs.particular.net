#if-version [7,)
### ServiceControl.Audit/StorageType

The storage implementation used by the Audit instance. This setting is required; an instance without it does not start.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_STORAGETYPE` |
| **App config key** | `ServiceControl.Audit/StorageType` |
| **SCMU field** | `STORAGE TYPE` |

| Type | Default value |
| --- | --- |
| string | None (required) |

Valid values are `RavenDB`, `SQLServer`, and `PostgreSQL`. To choose where message bodies are stored with SQL Server or PostgreSQL storage, use [`ServiceControl.Audit/MessageBody/StorageType`](/servicecontrol/audit-instances/configuration.md#storage-servicecontrol-auditmessagebodystoragetype).
#end-if
