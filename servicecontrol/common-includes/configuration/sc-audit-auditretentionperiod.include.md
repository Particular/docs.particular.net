### ServiceControl.Audit/AuditRetentionPeriod

The grace period to keep an audit message before it is deleted.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_AUDITRETENTIONPERIOD` |
| **App config key** | `ServiceControl.Audit/AuditRetentionPeriod` |
| **SCMU field** | `AUDIT RETENTION PERIOD` |

| Type | Default value |
| --- | --- |
| timespan | None (required) |

Valid range for this setting is from 1 hour to 365 days.

#if-version [7,)
For PostgreSQL storage, the retention period must be between one and 90 days.
#end-if
