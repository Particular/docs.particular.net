### ServiceControl.Audit/DbPath

The path where the internal RavenDB is located.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_DBPATH` |
| **App config key** | `ServiceControl.Audit/DbPath` |
| **SCMU field** | `DATABASE PATH` |

| Type | Default value |
| --- | --- | --- | --- |
| string | `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<instance_name>\DB` |

> [!NOTE]
> This setting is not relevant when the audit instance is [deployed using a container](/servicecontrol/audit-instances/deployment/containers.md).
