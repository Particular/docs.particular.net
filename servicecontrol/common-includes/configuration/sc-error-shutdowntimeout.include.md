### ServiceControl/ShutdownTimeout

_Added in version 6.5.0_

The maximum allowed time for the process to complete the shutdown.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_SHUTDOWNTIMEOUT` |
| **App config key** | `ServiceControl/ShutdownTimeout` |
| **SCMU field** | N/A |

| Environment/Installation type            | Type     | Default value |
|---|---|---|
| Containers | TimeSpan | `00:00:05` (5 seconds) |
| Installation via PowerShell (on Windows) | TimeSpan | `00:02:00` (2 minutes) |
| Installation via ServiceControl Management Utility (SCMU) (on Windows) | TimeSpan | `00:02:00` (2 minutes) |
