### ServiceControl.Audit/Https.RedirectHttpToHttps

_Added in version 6.11.0_

Redirects HTTP requests to HTTPS. This is intended for use with a reverse proxy that handles both HTTP and HTTPS traffic.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_HTTPS_REDIRECTHTTPTOHTTPS` |
| **App config key** | `ServiceControl.Audit/Https.RedirectHttpToHttps` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `false` |

> [!NOTE]
> When running ServiceControl directly without a reverse proxy, the application only listens on a single protocol (HTTP or HTTPS). This setting is intended for use with a reverse proxy that handles both HTTP and HTTPS traffic.
