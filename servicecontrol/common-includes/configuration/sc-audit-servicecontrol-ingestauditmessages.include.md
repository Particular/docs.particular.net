### ServiceControl/IngestAuditMessages

> [!WARNING]
> This is the same setting as `ServiceControl.Audit/IngestAuditMessages` but kept for backward compatibility

Set to `false` to disable ingesting new audit messages. Useful in some upgrade scenarios.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_INGESTAUDITMESSAGES` |
| **App config key** | `ServiceControl/IngestAuditMessages` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |
