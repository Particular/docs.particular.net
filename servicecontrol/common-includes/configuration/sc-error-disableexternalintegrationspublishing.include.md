#if-version [,7)
### ServiceControl/DisableExternalIntegrationsPublishing

Controls whether ServiceControl publishes [integration events](/servicecontrol/contracts.md). Set to `false` to enable publishing or `true` to disable it.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_DISABLEEXTERNALINTEGRATIONSPUBLISHING` |
| **App config key** | `ServiceControl/DisableExternalIntegrationsPublishing` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `false` |
#end-if
