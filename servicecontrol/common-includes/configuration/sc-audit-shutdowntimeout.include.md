### ServiceControl.Audit/ShutdownTimeout

_Added in version 6.5.0_

The maximum allowed time for the process to gracefully complete the shutdown after which the process will try to terminate.

> [!NOTE]
> An ungraceful shutdown could result in the next start to require a lengthy database recovery operation.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_SHUTDOWNTIMEOUT` |
| **App config key** | `ServiceControl.Audit/ShutdownTimeout` |
| **SCMU field** | N/A |

| Environment/Installation type            | Type     | Default value |
| --- | --- | --- |
| Containers | TimeSpan | `00:00:05` (5 seconds) |
| Installation via PowerShell (on Windows) | TimeSpan | `00:02:00` (2 minutes) |
| Installation via ServiceControl Management Utility (SCMU) (on Windows) | TimeSpan | `00:02:00` (2 minutes) |
