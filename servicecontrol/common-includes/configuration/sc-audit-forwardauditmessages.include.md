### ServiceControl.Audit/ForwardAuditMessages

Use this setting to configure whether processed audit messages are forwarded to another queue or not. This entry should be set to `false` if there is no external process reading messages from the [`ServiceBus/AuditLogQueue`](#transport-servicebusauditlogqueue)

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_FORWARDAUDITMESSAGES` |
| **App config key** | `ServiceControl.Audit/ForwardAuditMessages` |
| **SCMU field** | `Forward audit messages?` |

| Type | Default value |
| --- | --- |
| bool | `false` (Off) |
