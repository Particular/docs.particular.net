#if-version [7,)

### ServiceControl/EnableExternalIntegrationsPublishing

Controls whether ServiceControl publishes [integration events](/servicecontrol/contracts.md). Set to `true` to enable publishing. Publishing is disabled by default.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_ENABLEEXTERNALINTEGRATIONSPUBLISHING` |
| **App config key** | `ServiceControl/EnableExternalIntegrationsPublishing` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `false` |

> [!WARNING]
> Do not configure both this setting and the legacy [`ServiceControl/DisableExternalIntegrationsPublishing`](#host-settings-servicecontroldisableexternalintegrationspublishing) setting. ServiceControl does not start when both are set.
#end-if
