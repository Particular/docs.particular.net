### ServiceControl/DisableExternalIntegrationsPublishing

Controls whether ServiceControl publishes [integration events](/servicecontrol/contracts.md). Set to `false` to enable publishing or `true` to disable it.

Publishing is disabled by default in ServiceControl version 7 and later. If subscribers use these events, explicitly set this setting to `false`.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_DISABLEEXTERNALINTEGRATIONSPUBLISHING` |
| **App config key** | `ServiceControl/DisableExternalIntegrationsPublishing` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `True` in version 7 and later; `False` in earlier versions |
