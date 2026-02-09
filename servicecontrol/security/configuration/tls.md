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

ServiceControl instances can be configured via environment variables or App.config. Each instance type uses a different prefix. See the [Hosting Guide](../hosting-guide.md) for example usage of these configuration settings, along with [Authentication](authentication.md) and [Forward Headers](forward-headers.md), in a scenario-based format.

- [Primary Instance](/servicecontrol/servicecontrol-instances/configuration.md#tls)
- [Audit Instance](/servicecontrol/audit-instances/configuration.md#tls)
- [Monitoring Instance](/servicecontrol/monitoring-instances/configuration.md#tls)

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

The following examples show common TLS configurations for different deployment scenarios using the primary ServiceControl instance.

### Direct HTTPS with certificate

When ServiceControl handles TLS directly using a PFX certificate:

```xml
<add key="ServiceControl/Https.Enabled" value="true" />
<add key="ServiceControl/Https.CertificatePath" value="C:\certs\servicecontrol.pfx" />
<add key="ServiceControl/Https.CertificatePassword" value="{certificate-password}" />
```

**Container**:

> [!NOTE]
> The following is a docker compose snippets. For full examples, see the [Platform Container Examples repository](https://github.com/Particular/PlatformContainerExamples).

```bash
servicecontrol:
    image: particular/servicecontrol:latest
    env_file: .env
    ports:
      - "33333:33333"
    environment:
      RAVENDB_CONNECTIONSTRING: http://servicecontrol-db:8080
      REMOTEINSTANCES: '[{"api_uri":"https://servicecontrol-audit:44444/api"}]'
      SERVICECONTROL_HTTPS_ENABLED: "true"
      SERVICECONTROL_HTTPS_CERTIFICATEPATH: "/usr/share/ParticularSoftware/certificate.pfx"
      SERVICECONTROL_HTTPS_CERTIFICATEPASSWORD: "..."
      SSL_CERT_FILE: "/etc/ssl/certs/ca-bundle.crt"
      SERVICECONTROL_AUTHENTICATION_ENABLED: "true"
      SERVICECONTROL_AUTHENTICATION_AUTHORITY: "https://login.microsoftonline.com/{tenantId}"
      SERVICECONTROL_AUTHENTICATION_AUDIENCE: "api://{api-id}"
      SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_CLIENTID: "{servicepulse-clientid}"
      SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_AUTHORITY: "https://login.microsoftonline.com/{tenantId}/v2.0"
      SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_APISCOPES: '["api://{api-id}/access_as_user"]'
    command: --setup-and-run
    restart: unless-stopped
    volumes:
      - C:\path\to\servicecontrol\cert.pfx:/usr/share/ParticularSoftware/certificate.pfx
      - C:\path\to\servicecontrol\ca-bundle.crt:/etc/ssl/certs/ca-bundle.crt:ro
    healthcheck:
      test: ["CMD", "/healthcheck/healthcheck", "https://localhost:33333/api"]
      interval: 30s
      timeout: 10s
      start_period: 60s
      retries: 3
    depends_on:
      servicecontrol-db:
        condition: service_healthy
      rabbitmq:
        condition: service_healthy
```

**SSL_CERT_FILE (CRT)**: This tells SSL/TLS libraries (including .NET's HttpClient, OpenSSL, and libcurl) the path to a CA (Certificate Authority) certificate bundle file. This bundle contains the public certificates of trusted CAs used to verify the authenticity of SSL/TLS certificates presented by remote servers. This certificate is used to verify HTTPS (trust others).

**SERVICEPULSE_HTTPS_CERTIFICATEPATH (PFX)**: This is a binary archive format that bundles together the private key, public certificate, and certificate chain. This certificate is used to serve HTTPS (prove identity).

> [!NOTE]
> When containers communicate with each other over HTTPS, they use the Docker service names (like `servicecontrol`, `servicecontrol-audit`) as hostnames, and TLS validation will fail if the certificate doesn't include these names in its Subject Alternative Names (SANs).

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

## Troubleshooting

include: tls-troubleshooting
