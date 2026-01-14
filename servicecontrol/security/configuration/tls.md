---
title: ServiceControl TLS Configuration
summary: How to enable and configure TLS for ServiceControl instances
reviewed: 2026-01-12
component: ServiceControl
related:
- servicecontrol/security/hosting-guide
- servicepulse/security/configuration/tls
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

include: cert-management

include: hsts-considerations

### HTTP to HTTPS Redirect

The `HTTPS_REDIRECTHTTPTOHTTPS` setting is intended for use with a reverse proxy that handles both HTTP and HTTPS traffic. When enabled:

- The redirect uses HTTP 307 (Temporary Redirect) to preserve the request method
- The reverse proxy must forward both HTTP and HTTPS requests to ServiceControl
- ServiceControl will redirect HTTP requests to HTTPS based on the `X-Forwarded-Proto` header
- **Important:** You must also set `HTTPS_PORT` to specify the HTTPS port for the redirect URL

> [!NOTE]
> When running ServiceControl directly without a reverse proxy, the application only listens on a single protocol (HTTP or HTTPS).

## Configuration examples

The following examples show common TLS configurations for different deployment scenarios.

### Direct HTTPS with certificate

When ServiceControl handles TLS directly using a PFX certificate:

```xml
<add key="ServiceControl/Https.Enabled" value="true" />
<add key="ServiceControl/Https.CertificatePath" value="C:\certs\servicecontrol.pfx" />
<add key="ServiceControl/Https.CertificatePassword" value="{certificate-password}" />
```

### Direct HTTPS with HSTS

When ServiceControl handles TLS directly and you want to enable HSTS:

```xml
<add key="ServiceControl/Https.Enabled" value="true" />
<add key="ServiceControl/Https.CertificatePath" value="C:\certs\servicecontrol.pfx" />
<add key="ServiceControl/Https.CertificatePassword" value="{certificate-password}" />
<add key="ServiceControl/Https.EnableHsts" value="true" />
<add key="ServiceControl/Https.HstsMaxAgeSeconds" value="31536000" />
```

### Reverse proxy with HTTP to HTTPS redirect

When TLS is terminated at a reverse proxy and you want ServiceControl to redirect HTTP requests:

```xml
<add key="ServiceControl/Https.RedirectHttpToHttps" value="true" />
<add key="ServiceControl/Https.Port" value="443" />
```

### Reverse proxy with HSTS

When TLS is terminated at a reverse proxy and you want ServiceControl to add HSTS headers:

```xml
<add key="ServiceControl/Https.EnableHsts" value="true" />
<add key="ServiceControl/Https.HstsMaxAgeSeconds" value="31536000" />
<add key="ServiceControl/Https.HstsIncludeSubDomains" value="true" />
```
