### ServiceControl/LogPath

The path for the ServiceControl logs.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_LOGPATH` |
| **App config key** | `ServiceControl/LogPath` |
| **SCMU field** | `LOG PATH` |

| Type | Default value |
| --- | --- |
| string | `%LOCALAPPDATA%\Particular\ServiceControl\logs` |

> [!NOTE]
> %LOCALAPPDATA% is a user-specific path on Windows.
>
> When hosted on containers, logs are sent to **stdout** and this setting is ignored.
