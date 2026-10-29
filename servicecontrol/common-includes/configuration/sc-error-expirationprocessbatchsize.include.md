#if-version [,5)
### ServiceControl/ExpirationProcessBatchSize

This controls the batch size used when deleting error messages that have exceeded the error retention period after they have been successfully retried or manually archived.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_EXPIRATIONPROCESSBATCHSIZE` |
| **App config key** | `ServiceControl/ExpirationProcessBatchSize` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `65512` |

The minimum allowed value for this setting is `10240`; there is no hard-coded maximum as this is dependent on system performance.
#end-if
