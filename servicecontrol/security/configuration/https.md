---
title: HTTPS
summary: How to enable and configure HTTPS for ServiceControl instances
reviewed: 2026-01-12
component: ServiceControl
---

ServiceControl instances can be configured to use HTTPS directly, enabling encrypted connections without relying on a reverse proxy for SSL termination.

## Configuration

ServiceControl instances can be configured via environment variables or App.config. Each instance type uses a different prefix. See the [Hosting Guide](../hosting-guide.md) for example usage of these configuration settings in conjustion with [Authentication](authentication.md) and [Forward Headers](forward-headers.md) configuration settings in a scenario based format.

include: servicecontrol-instance-prefix

### Settings

| Environment Variable                   | App.config                                   | Default    | Description                                                    |
|----------------------------------------|----------------------------------------------|------------|----------------------------------------------------------------|
| `{PREFIX}_HTTPS_ENABLED`               | `{PREFIX}/Https.Enabled`                     | `false`    | Enable HTTPS with Kestrel                                      |
| `{PREFIX}_HTTPS_CERTIFICATEPATH`       | `{PREFIX}/Https.CertificatePath`             | (none)     | Path to the certificate file (.pfx)                            |
| `{PREFIX}_HTTPS_CERTIFICATEPASSWORD`   | `{PREFIX}/Https.CertificatePassword`         | (none)     | Password for the certificate file                              |
| `{PREFIX}_HTTPS_REDIRECTHTTPTOHTTPS`   | `ServiceControl/Https.RedirectHttpToHttps`   | `false`    | Redirect HTTP requests to HTTPS                                |
| `{PREFIX}_HTTPS_PORT`                  | `ServiceControl/Https.Port`                  | (none)     | HTTPS port for redirect (required for reverse proxy scenarios) |
| `{PREFIX}_HTTPS_ENABLEHSTS`            | `{PREFIX}/Https.EnableHsts`                  | `false`    | Enable HTTP Strict Transport Security                          |
| `{PREFIX}_HTTPS_HSTSMAXAGESECONDS`     | `ServiceControl/Https.HstsMaxAgeSeconds`     | `31536000` | HSTS max-age in seconds (default: 1 year)                      |
| `{PREFIX}_HTTPS_HSTSINCLUDESUBDOMAINS` | `ServiceControl/Https.HstsIncludeSubDomains` | `false`    | Include subdomains in HSTS policy                              |

## Security Considerations

### Certificate Management

- ServiceControl supports PFX (PKCS#12) certificate files
- Store certificate files securely with appropriate file permissions
- Rotate certificates before expiration
- Use certificates from a trusted Certificate Authority for production
- Never commit certificate files to source control
- Restrict read access to the certificate file to only the ServiceControl service account
- Use environment variables rather than App.config (environment variables are not persisted to disk)

### HSTS Considerations

- HSTS should not be tested on localhost because browsers cache the policy, which could break other local development
- HSTS is disabled in Development environment (ASP.NET Core excludes localhost by default)
- HSTS can be configured at either the reverse proxy level or in ServiceControl (but not both)
- HSTS is cached by browsers, so test carefully before enabling in production
- Start with a short max-age during initial deployment
- Consider the impact on subdomains before enabling `includeSubDomains`

### HTTP to HTTPS Redirect

The `HTTPS_REDIRECTHTTPTOHTTPS` setting is intended for use with a reverse proxy that handles both HTTP and HTTPS traffic. When enabled:

- The redirect uses HTTP 307 (Temporary Redirect) to preserve the request method
- The reverse proxy must forward both HTTP and HTTPS requests to ServiceControl
- ServiceControl will redirect HTTP requests to HTTPS based on the `X-Forwarded-Proto` header
- **Important:** You must also set `HTTPS_PORT` to specify the HTTPS port for the redirect URL

> [!NOTE]
> When running ServiceControl directly without a reverse proxy, the application only listens on a single protocol (HTTP or HTTPS).
