#if-version [7,)
### ServiceControl/PersistenceType

The ServiceControl storage implementation used by the instance.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_PERSISTENCETYPE` |
| **App config key** | `ServiceControl/PersistenceType` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `RavenDB` |

Valid values are `RavenDB`, `SQLServer`, and `PostgreSQL`.
#end-if
