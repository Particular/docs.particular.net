### ServiceControl.Audit/Authentication.RequireHttpsMetadata

_Added in version 6.11.0_

Controls whether HTTPS is required when retrieving metadata from the authority.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_AUTHENTICATION_REQUIREHTTPSMETADATA` |
| **App config key** | `ServiceControl.Audit/Authentication.RequireHttpsMetadata` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

> [!WARNING]
> Setting this to `false` is not recommended for production environments.
