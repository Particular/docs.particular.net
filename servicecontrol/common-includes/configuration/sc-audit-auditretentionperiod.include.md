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

> [!NOTE]
> Starting with version 4.26.0, new audit instances using RavenDB 5 will use the built-in RavenDB expiration process. Changing the audit retention setting will affect only newly ingested messages. Already ingested messages will expire according to the previous retention setting value.
