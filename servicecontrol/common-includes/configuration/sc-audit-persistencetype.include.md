#if-version [7,)
### ServiceControl.Audit/PersistenceType

The storage implementation used by the Audit instance.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_PERSISTENCETYPE` |
| **App config key** | `ServiceControl.Audit/PersistenceType` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `RavenDB` |

Valid values are `RavenDB`, `SQLServer`, and `PostgreSQL`.
#end-if
