### ServiceControl/DbPath

The path where the internal RavenDB is located.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_DBPATH` |
| **App config key** | `ServiceControl/DbPath` |
| **SCMU field** | `Database Path` |

| Type | Default value |
| --- | --- |
| string | `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<instance_name>\DB` |

> [!NOTE]
> This setting is not relevant when running an error instance in a container.
