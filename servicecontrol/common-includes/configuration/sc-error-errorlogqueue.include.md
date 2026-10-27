### ServiceBus/ErrorLogQueue

The error queue name to use for forwarding error messages. This setting is ignored unless `ServiceControl/ForwardErrorMessages` is enabled.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICEBUS_ERRORLOGQUEUE` |
| **App config key** | `ServiceBus/ErrorLogQueue` |
| **SCMU field** | `ERROR FORWARDING QUEUE NAME` |

| Type | Default value |
| --- | --- |
| string | `<ErrorQueue>.log` |

> [!NOTE]
> Changing the configuration file or environment value directly will not result in the queue being created. If you are using the ServiceControl Management utility to manage your ServiceControl error instance changing the value will create the forwarding queue if it has not been created.
