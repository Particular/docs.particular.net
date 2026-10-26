### NServiceBus/Transport

The connection string for the transport. This setting must be entered in the `connectionStrings` section of the configuration file when configured using the app config.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_CONNECTIONSTRING` OR `CONNECTIONSTRING` |
| **App config key** | `NServiceBus/Transport` in `connectionStrings` |
| **SCMU field** | `TRANSPORT CONNECTION STRING` |

| Type | Default value |
| --- | --- |
| string | None |

Valid values are documented in the [ServiceControl transport configuration documentation](/servicecontrol/transports.md).
