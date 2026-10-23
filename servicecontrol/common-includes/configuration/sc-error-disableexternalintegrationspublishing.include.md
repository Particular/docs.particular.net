### ServiceControl/DisableExternalIntegrationsPublishing

Controls whether ServiceControl publishes [integration events](/servicecontrol/contracts.md). Set to `false` to enable publishing or `true` to disable it.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_DISABLEEXTERNALINTEGRATIONSPUBLISHING` |
| **App config key** | `ServiceControl/DisableExternalIntegrationsPublishing` |
| **SCMU field** | N/A |

#if-version [,7)
| Type | Default value |
| --- | --- |
| bool | `false` |
#end-if

#if-version [7,)
This Boolean setting is supported for compatibility with existing configurations. When it is not configured, [`ServiceControl/EnableExternalIntegrationsPublishing`](#host-settings-servicecontrolenableexternalintegrationspublishing) controls publishing, which is disabled by default. Use that setting for new configurations. Do not configure both settings.
#end-if
