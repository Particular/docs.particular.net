### ServiceControl/Authentication.RequireHttpsMetadata

_Added in version 6.11.0_

Controls whether HTTPS is required when retrieving metadata from the authority.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUTHENTICATION_REQUIREHTTPSMETADATA` |
| **App config key** | `ServiceControl/Authentication.RequireHttpsMetadata` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

> [!WARNING]
> Setting this to `false` is not recommended for production environments. Disabling this setting allows metadata to be retrieved over unencrypted HTTP connections, which could expose sensitive configuration information to attackers.
