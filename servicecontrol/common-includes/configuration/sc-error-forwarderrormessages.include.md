### ServiceControl/ForwardErrorMessages

Use this setting to configure whether processed error messages are forwarded to another queue or not. This entry should be set to `false` if there is no external process reading messages from the [`ServiceBus/ErrorLogQueue`](#transport-servicecontrolforwarderrormessages).

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_FORWARDERRORMESSAGES` |
| **App config key** | `ServiceControl/ForwardErrorMessages` |
| **SCMU field** | `ERROR FORWARDING` |

| Type | Default value |
| --- | --- |
| bool | `false` (Off) |

This entry should be set to `false` if there is no external process reading messages from the `Error Forwarding Queue`.
