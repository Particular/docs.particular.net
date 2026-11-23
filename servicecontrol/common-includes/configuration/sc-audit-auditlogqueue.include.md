### ServiceBus/AuditLogQueue

The audit queue name to use for forwarding audit messages. This setting is ignored unless `ServiceControl.Audit/ForwardAuditMessages` is enabled.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICEBUS_AUDITLOGQUEUE` |
| **App config key** | `ServiceBus/AuditLogQueue` |
| **SCMU field** | `AUDIT FORWARDING QUEUE NAME` |

| Type | Default value |
| --- | --- |
| string | `<AuditQueue>.log` |

> [!NOTE]
> Changing the configuration file or environment value directly will not result in the queue being created. If you are using the ServiceControl Management utility to manage your ServiceControl audit instance changing the value will create the forwarding queue if it has not been created.
