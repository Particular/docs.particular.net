### ServiceControl.Audit/ExpirationProcessBatchSize

This controls the batch size used when deleting audit messages that have exceeded the audit retention period.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_EXPIRATIONPROCESSBATCHSIZE` |
| **App config key** | `ServiceControl.Audit/ExpirationProcessBatchSize` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `65512` |

The minimum allowed value for this setting is `10240`; there is no hard-coded maximum as this is dependent on system performance.
