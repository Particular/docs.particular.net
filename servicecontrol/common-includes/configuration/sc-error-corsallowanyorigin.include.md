### ServiceControl/Cors.AllowAnyOrigin

_Added in version 6.11.0_

Allows requests from any origin.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_CORS_ALLOWANYORIGIN` |
| **App config key** | `ServiceControl/Cors.AllowAnyOrigin` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

> [!WARNING]
> For production environments, set this to `false` and configure `AllowedOrigins` to restrict which origins can access the API.
