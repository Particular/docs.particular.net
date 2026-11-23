### ServiceControl/ExpirationProcessTimerInSeconds

The number of seconds to wait between checking for expired messages.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_EXPIRATIONPROCESSTIMERINSECONDS` |
| **App config key** | `ServiceControl/ExpirationProcessTimerInSeconds` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `600` (10 minutes) |

Valid range is `0` to `10800` (3 Hours).

Setting the value to `0` will disable the expiration process. This is not recommended and it is only provided for fault finding.
