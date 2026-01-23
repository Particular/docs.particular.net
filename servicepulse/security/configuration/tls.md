---
title: ServicePulse TLS Configuration
summary: How to enable and configure TLS for ServicePulse
reviewed: 2026-01-14
component: ServicePulse
related:
- servicepulse/security/hosting-guide
- servicecontrol/security/configuration/tls
---

> [!NOTE]
> This page is **not** relevant if the [ServicePulse static files have been extracted](/servicepulse/install-servicepulse-in-iis.md), and is being hosted in anything other than the [Container](/servicepulse/containerization/) or [Windows Service](/servicepulse/installation.md) hosting options provided. If using [authentication](/servicepulse/security/configuration/authentication.md), it is recommended to use TLS encryption.

ServicePulse can be configured to use HTTPS directly, enabling encrypted connections without relying on a reverse proxy for SSL termination.

## Configuration

There are two hosting options for ServiceControl, [Container](/servicepulse/containerization/) and [Windows Service](/servicepulse/installation.md). The container is configured via environment variables, while the Windows Service is configured using command-line arguments. See the [Hosting Guide](../hosting-guide.md) for example usage of these configuration settings, along with [Authentication](authentication.md) and [Forward Headers](forward-headers.md), in a scenario-based format.

### Container

- [Container TLS settings](/servicepulse/containerization/#settings-tls)

### Window Service

| Command-Line Argument           | Default    | Description                                                    |
|---------------------------------|------------|----------------------------------------------------------------|
| `--httpsenabled=`               | `false`    | Enable HTTPS with Kestrel                                      |
| `--httpsredirecthttptohttps=`   | `false`    | Redirect HTTP requests to HTTPS                                |
| `--httpsport=`                  | (none)     | HTTPS port for redirect (required for reverse proxy scenarios) |
| `--httpsenablehsts=`            | `false`    | Enable HTTP Strict Transport Security                          |
| `--httpshstsmaxageseconds=`     | `31536000` | HSTS max-age in seconds (default: 1 year)                      |
| `--httpshstsincludesubdomains=` | `false`    | Include subdomains in HSTS policy                              |

> [!NOTE]
> The windows service uses Windows HttpListener which requires [SSL certificate binding at the OS level using `netsh`](https://learn.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-configure-a-port-with-an-ssl-certificate). The certificate is not configured in the application itself.

Example:

```cmd
"C:\Program Files (x86)\Particular Software\ServicePulse\ServicePulse.Host.exe" --httpsenabled=true --httpsredirecthttptohttps=false
```

## Security Considerations

include: cert-management

include: hsts-considerations

### HTTP to HTTPS Redirect

The `SERVICEPULSE_HTTPS_REDIRECTHTTPTOHTTPS` setting is intended for use with a reverse proxy that handles both HTTP and HTTPS traffic. When enabled:

- The redirect uses HTTP 307 (Temporary Redirect) to preserve the request method
- The reverse proxy must forward both HTTP and HTTPS requests to ServicePulse
- ServicePulse will redirect HTTP requests to HTTPS based on the `X-Forwarded-Proto` header
- **Important:** You must also set `SERVICEPULSE_HTTPS_PORT` (or `--httpsport=` for .NET Framework) to specify the HTTPS port for the redirect URL

> [!NOTE]
> When running ServicePulse directly without a reverse proxy, the application only listens on a single protocol (HTTP or HTTPS).

## Configuration Examples

The following examples show common TLS configurations for different deployment scenarios.

### Direct HTTPS with certificate

When ServicePulse handles TLS directly using a PFX certificate:

**Container:**

```bash
docker run -e SERVICEPULSE_HTTPS_ENABLED=true \
           -e SERVICEPULSE_HTTPS_CERTIFICATEPATH=/certs/servicepulse.pfx \
           -e SERVICEPULSE_HTTPS_CERTIFICATEPASSWORD={certificate-password} \
           -v /path/to/certs:/certs \
           ...
           particular/servicepulse:latest
```

**Windows Service:**

The Windows service uses Windows HttpListener which requires SSL certificate binding at the OS level:

```cmd
netsh http add sslcert ipport=0.0.0.0:443 certhash={certificate-thumbprint} appid={application-guid}
ServicePulse.Host.exe --httpsenabled=true
```

### Direct HTTPS with HSTS

When ServicePulse handles TLS directly and you want to enable HSTS:

**Container:**

```bash
docker run -e SERVICEPULSE_HTTPS_ENABLED=true \
           -e SERVICEPULSE_HTTPS_CERTIFICATEPATH=/certs/servicepulse.pfx \
           -e SERVICEPULSE_HTTPS_CERTIFICATEPASSWORD={certificate-password} \
           -e SERVICEPULSE_HTTPS_ENABLEHSTS=true \
           -e SERVICEPULSE_HTTPS_HSTSMAXAGESECONDS=31536000 \
           -v /path/to/certs:/certs \
           ...
           particular/servicepulse:latest
```

**Windows Service:**

```cmd
ServicePulse.Host.exe --httpsenabled=true --httpsenablehsts=true --httpshstsmaxageseconds=31536000
```

### Reverse proxy with HTTP to HTTPS redirect

When TLS is terminated at a reverse proxy and you want ServicePulse to redirect HTTP requests:

**Container:**

```bash
docker run -e SERVICEPULSE_HTTPS_REDIRECTHTTPTOHTTPS=true \
           -e SERVICEPULSE_HTTPS_PORT=443 \
           ...
           particular/servicepulse:latest
```

**Windows Service:**

```cmd
ServicePulse.Host.exe --httpsredirecthttptohttps=true --httpsport=443
```

### Reverse proxy with HSTS

When TLS is terminated at a reverse proxy and you want ServicePulse to add HSTS headers:

**Container:**

```bash
docker run -e SERVICEPULSE_HTTPS_ENABLEHSTS=true \
           -e SERVICEPULSE_HTTPS_HSTSMAXAGESECONDS=31536000 \
           -e SERVICEPULSE_HTTPS_HSTSINCLUDESUBDOMAINS=true \
           ...
           particular/servicepulse:latest
```

**Windows Service:**

```cmd
ServicePulse.Host.exe --httpsenablehsts=true --httpshstsmaxageseconds=31536000 --httpshstsincludesubdomains=true
```

## Troubleshooting

include: tls-troubleshooting

### SSL certificate binding fails (Windows Service)

**Symptom**: ServicePulse fails to start with HTTPS enabled, or `netsh http add sslcert` returns an error.

**Cause**: The certificate is not properly bound to the port, the certificate is missing from the certificate store, or the thumbprint/appid is incorrect.

**Solutions**:

- Verify the certificate is installed in the Windows certificate store (Local Computer > Personal)
- Check the certificate thumbprint is correct (no spaces, lowercase)
- Ensure the port is not already bound to another certificate: `netsh http show sslcert`
- Remove existing binding before adding a new one: `netsh http delete sslcert ipport=0.0.0.0:443`
- Run the command prompt as Administrator
