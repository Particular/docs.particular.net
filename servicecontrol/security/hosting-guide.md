---
title: Hosting and Security Guide
summary: Hosting and security options for ServiceControl instances
reviewed: 2025-12-11
component: ServiceControl
---

This guide covers all hosting and security options available for ServiceControl, ServiceControl.Audit, and ServiceControl.Monitoring instances.

## Configuration Settings by Instance

Each ServiceControl instance uses a different configuration prefix:

| Instance | Configuration Prefix | Default Port |
|----------|---------------------|--------------|
| ServiceControl (Primary) | `ServiceControl/` | 33333 |
| ServiceControl.Audit | `ServiceControl.Audit/` | 44444 |
| ServiceControl.Monitoring | `Monitoring/` | 33633 |

Settings can be configured via App.config files or using environment variables.

---

## Hosting Model

ServiceControl runs as a standalone Windows service with Kestrel as the built-in web server. It does not support being hosted inside IIS (in-process hosting).

If you place IIS, nginx, or another web server in front of ServiceControl, it acts as a **reverse proxy** forwarding requests to Kestrel.

---

## Deployment Scenarios

### Scenario 1: Default Configuration

The default configuration with no additional setup required. Backwards compatible with existing deployments.

**Security Features:**

| Feature | Status |
|---------|--------|
| JWT Authentication | ❌ Disabled |
| Kestrel HTTPS | ❌ Disabled |
| HTTPS Redirection | ❌ Disabled |
| HSTS | ❌ Disabled |
| Restricted CORS Origins | ❌ Disabled (any origin) |
| Forwarded Headers | ✅ Enabled (trusts all) |
| Restricted Proxy Trust | ❌ Disabled |

```xml
<!-- No configuration needed - defaults work out of the box -->
<!-- Authentication: disabled by default -->
<!-- CORS: allows any origin by default -->
<!-- HTTPS: disabled by default (HTTP only) -->
<!-- Forwarded Headers: enabled and trusts all proxies by default -->
```

Or explicitly:

```xml
<appSettings>
  <add key="ServiceControl/Authentication.Enabled" value="false" />
  <add key="ServiceControl/Https.Enabled" value="false" />
  <add key="ServiceControl/Cors.AllowAnyOrigin" value="true" />
  <add key="ServiceControl/ForwardedHeaders.Enabled" value="true" />
  <add key="ServiceControl/ForwardedHeaders.TrustAllProxies" value="true" />
</appSettings>
```

---

### Scenario 2: Reverse Proxy with SSL Termination

ServiceControl behind a reverse proxy (nginx, IIS, cloud load balancer) that handles SSL/TLS termination.

**Security Features:**

| Feature | Status |
|---------|--------|
| JWT Authentication | ❌ Disabled |
| Kestrel HTTPS | ❌ Disabled (handled by proxy) |
| HTTPS Redirection | ❌ Disabled (handled by proxy) |
| HSTS | ❌ Disabled (handled by proxy) |
| Restricted CORS Origins | ✅ Enabled |
| Forwarded Headers | ✅ Enabled |
| Restricted Proxy Trust | ✅ Enabled |

```xml
<appSettings>
  <!-- Trust forwarded headers from your reverse proxy -->
  <add key="ServiceControl/ForwardedHeaders.Enabled" value="true" />
  <add key="ServiceControl/ForwardedHeaders.TrustAllProxies" value="false" />
  <add key="ServiceControl/ForwardedHeaders.KnownProxies" value="10.0.0.5,10.0.0.6" />

  <!-- Or use CIDR notation for networks -->
  <!-- <add key="ServiceControl/ForwardedHeaders.KnownNetworks" value="10.0.0.0/24" /> -->

  <!-- Restrict CORS to your ServicePulse domain -->
  <add key="ServiceControl/Cors.AllowedOrigins" value="https://servicepulse.yourcompany.com" />
</appSettings>
```

---

### Scenario 3: Direct HTTPS (No Reverse Proxy)

Kestrel handles TLS directly without a reverse proxy.

**Security Features:**

| Feature | Status |
|---------|--------|
| JWT Authentication | ❌ Disabled |
| Kestrel HTTPS | ✅ Enabled |
| HTTPS Redirection | ✅ Enabled |
| HSTS | ✅ Enabled |
| Restricted CORS Origins | ✅ Enabled |
| Forwarded Headers | ❌ Disabled (no proxy) |
| Restricted Proxy Trust | N/A |

```xml
<appSettings>
  <!-- Enable Kestrel HTTPS -->
  <add key="ServiceControl/Https.Enabled" value="true" />
  <add key="ServiceControl/Https.CertificatePath" value="C:\certs\servicecontrol.pfx" />
  <add key="ServiceControl/Https.CertificatePassword" value="your-certificate-password" />

  <!-- Enable HSTS for browsers -->
  <add key="ServiceControl/Https.EnableHsts" value="true" />
  <add key="ServiceControl/Https.HstsMaxAgeSeconds" value="31536000" />

  <!-- Redirect HTTP to HTTPS -->
  <add key="ServiceControl/Https.RedirectHttpToHttps" value="true" />

  <!-- Restrict CORS -->
  <add key="ServiceControl/Cors.AllowedOrigins" value="https://servicepulse.yourcompany.com" />

  <!-- Disable forwarded headers (no proxy) -->
  <add key="ServiceControl/ForwardedHeaders.Enabled" value="false" />
</appSettings>
```

---

### Scenario 4: Reverse Proxy with Authentication

Reverse proxy with SSL termination and JWT authentication via an identity provider (Azure AD, Okta, Auth0, Keycloak, etc.).

**Security Features:**

| Feature | Status |
|---------|--------|
| JWT Authentication | ✅ Enabled |
| Kestrel HTTPS | ❌ Disabled (handled by proxy) |
| HTTPS Redirection | ❌ Disabled (handled by proxy) |
| HSTS | ❌ Disabled (handled by proxy) |
| Restricted CORS Origins | ✅ Enabled |
| Forwarded Headers | ✅ Enabled |
| Restricted Proxy Trust | ✅ Enabled |

```xml
<appSettings>
  <!-- Authentication -->
  <add key="ServiceControl/Authentication.Enabled" value="true" />
  <add key="ServiceControl/Authentication.Authority" value="https://login.microsoftonline.com/{tenant-id}/v2.0" />
  <add key="ServiceControl/Authentication.Audience" value="api://servicecontrol" />

  <!-- Token validation settings -->
  <add key="ServiceControl/Authentication.ValidateIssuer" value="true" />
  <add key="ServiceControl/Authentication.ValidateAudience" value="true" />
  <add key="ServiceControl/Authentication.ValidateLifetime" value="true" />
  <add key="ServiceControl/Authentication.ValidateIssuerSigningKey" value="true" />
  <add key="ServiceControl/Authentication.RequireHttpsMetadata" value="true" />

  <!-- ServicePulse client configuration -->
  <add key="ServiceControl/Authentication.ServicePulse.ClientId" value="your-servicepulse-client-id" />
  <add key="ServiceControl/Authentication.ServicePulse.Authority" value="https://login.microsoftonline.com/{tenant-id}/v2.0" />
  <add key="ServiceControl/Authentication.ServicePulse.ApiScopes" value="api://servicecontrol/.default" />

  <!-- Forwarded headers for reverse proxy -->
  <add key="ServiceControl/ForwardedHeaders.Enabled" value="true" />
  <add key="ServiceControl/ForwardedHeaders.TrustAllProxies" value="false" />
  <add key="ServiceControl/ForwardedHeaders.KnownNetworks" value="10.0.0.0/8" />

  <!-- Restrict CORS -->
  <add key="ServiceControl/Cors.AllowedOrigins" value="https://servicepulse.yourcompany.com" />
</appSettings>
```

> [!NOTE]
> The token validation settings (`ValidateIssuer`, `ValidateAudience`, `ValidateLifetime`, `ValidateIssuerSigningKey`, `RequireHttpsMetadata`) all default to `true`.

---

### Scenario 5: Direct HTTPS with Authentication

Kestrel handles TLS directly with JWT authentication. No reverse proxy.

**Security Features:**

| Feature | Status |
|---------|--------|
| JWT Authentication | ✅ Enabled |
| Kestrel HTTPS | ✅ Enabled |
| HTTPS Redirection | ✅ Enabled |
| HSTS | ✅ Enabled |
| Restricted CORS Origins | ✅ Enabled |
| Forwarded Headers | ❌ Disabled (no proxy) |
| Restricted Proxy Trust | N/A |

```xml
<appSettings>
  <!-- Kestrel HTTPS -->
  <add key="ServiceControl/Https.Enabled" value="true" />
  <add key="ServiceControl/Https.CertificatePath" value="C:\certs\servicecontrol.pfx" />
  <add key="ServiceControl/Https.CertificatePassword" value="your-certificate-password" />
  <add key="ServiceControl/Https.EnableHsts" value="true" />
  <add key="ServiceControl/Https.HstsMaxAgeSeconds" value="31536000" />
  <add key="ServiceControl/Https.RedirectHttpToHttps" value="true" />

  <!-- Authentication -->
  <add key="ServiceControl/Authentication.Enabled" value="true" />
  <add key="ServiceControl/Authentication.Authority" value="https://login.microsoftonline.com/{tenant-id}/v2.0" />
  <add key="ServiceControl/Authentication.Audience" value="api://servicecontrol" />
  <add key="ServiceControl/Authentication.ValidateIssuer" value="true" />
  <add key="ServiceControl/Authentication.ValidateAudience" value="true" />
  <add key="ServiceControl/Authentication.ValidateLifetime" value="true" />
  <add key="ServiceControl/Authentication.ValidateIssuerSigningKey" value="true" />
  <add key="ServiceControl/Authentication.RequireHttpsMetadata" value="true" />

  <!-- ServicePulse client -->
  <add key="ServiceControl/Authentication.ServicePulse.ClientId" value="your-servicepulse-client-id" />
  <add key="ServiceControl/Authentication.ServicePulse.Authority" value="https://login.microsoftonline.com/{tenant-id}/v2.0" />
  <add key="ServiceControl/Authentication.ServicePulse.ApiScopes" value="api://servicecontrol/.default" />

  <!-- Restrict CORS -->
  <add key="ServiceControl/Cors.AllowedOrigins" value="https://servicepulse.yourcompany.com" />

  <!-- No forwarded headers (no proxy) -->
  <add key="ServiceControl/ForwardedHeaders.Enabled" value="false" />
</appSettings>
```

---

### Scenario 6: End-to-End Encryption with Reverse Proxy and Authentication

End-to-end TLS encryption where the reverse proxy terminates external TLS and re-encrypts traffic to ServiceControl over HTTPS. Includes JWT authentication.

**Security Features:**

| Feature | Status |
|---------|--------|
| JWT Authentication | ✅ Enabled |
| Kestrel HTTPS | ✅ Enabled |
| HTTPS Redirection | ❌ Disabled (handled by proxy) |
| HSTS | ❌ Disabled (handled by proxy) |
| Restricted CORS Origins | ✅ Enabled |
| Forwarded Headers | ✅ Enabled |
| Restricted Proxy Trust | ✅ Enabled |

```xml
<appSettings>
  <!-- Kestrel HTTPS for internal encryption -->
  <add key="ServiceControl/Https.Enabled" value="true" />
  <add key="ServiceControl/Https.CertificatePath" value="C:\certs\servicecontrol-internal.pfx" />
  <add key="ServiceControl/Https.CertificatePassword" value="your-certificate-password" />

  <!-- HSTS/Redirection handled by reverse proxy -->
  <add key="ServiceControl/Https.EnableHsts" value="false" />
  <add key="ServiceControl/Https.RedirectHttpToHttps" value="false" />

  <!-- Authentication -->
  <add key="ServiceControl/Authentication.Enabled" value="true" />
  <add key="ServiceControl/Authentication.Authority" value="https://login.microsoftonline.com/{tenant-id}/v2.0" />
  <add key="ServiceControl/Authentication.Audience" value="api://servicecontrol" />
  <add key="ServiceControl/Authentication.ValidateIssuer" value="true" />
  <add key="ServiceControl/Authentication.ValidateAudience" value="true" />
  <add key="ServiceControl/Authentication.ValidateLifetime" value="true" />
  <add key="ServiceControl/Authentication.ValidateIssuerSigningKey" value="true" />
  <add key="ServiceControl/Authentication.RequireHttpsMetadata" value="true" />

  <!-- ServicePulse client -->
  <add key="ServiceControl/Authentication.ServicePulse.ClientId" value="your-servicepulse-client-id" />
  <add key="ServiceControl/Authentication.ServicePulse.Authority" value="https://login.microsoftonline.com/{tenant-id}/v2.0" />
  <add key="ServiceControl/Authentication.ServicePulse.ApiScopes" value="api://servicecontrol/.default" />

  <!-- Forwarded headers - trust only your reverse proxy -->
  <add key="ServiceControl/ForwardedHeaders.Enabled" value="true" />
  <add key="ServiceControl/ForwardedHeaders.TrustAllProxies" value="false" />
  <add key="ServiceControl/ForwardedHeaders.KnownProxies" value="10.0.0.5" />

  <!-- Restrict CORS -->
  <add key="ServiceControl/Cors.AllowedOrigins" value="https://servicepulse.yourcompany.com" />
</appSettings>
```

---

## Configuration Reference

### Authentication Settings

| Setting | Type | Default | Description |
|---------|------|---------|-------------|
| `Authentication.Enabled` | bool | `false` | Enable JWT Bearer authentication |
| `Authentication.Authority` | string | - | OpenID Connect authority URL (required when enabled) |
| `Authentication.Audience` | string | - | Expected audience for tokens (required when enabled) |
| `Authentication.ValidateIssuer` | bool | `true` | Validate token issuer |
| `Authentication.ValidateAudience` | bool | `true` | Validate token audience |
| `Authentication.ValidateLifetime` | bool | `true` | Validate token expiration |
| `Authentication.ValidateIssuerSigningKey` | bool | `true` | Validate token signing key |
| `Authentication.RequireHttpsMetadata` | bool | `true` | Require HTTPS for metadata endpoint |
| `Authentication.ServicePulse.ClientId` | string | - | OAuth client ID for ServicePulse |
| `Authentication.ServicePulse.Authority` | string | - | Authority URL for ServicePulse (defaults to main Authority) |
| `Authentication.ServicePulse.ApiScopes` | string | - | API scopes for ServicePulse to request |

### HTTPS Settings

| Setting | Type | Default | Description |
|---------|------|---------|-------------|
| `Https.Enabled` | bool | `false` | Enable Kestrel HTTPS with certificate |
| `Https.CertificatePath` | string | - | Path to PFX/PEM certificate file |
| `Https.CertificatePassword` | string | - | Certificate password (if required) |
| `Https.RedirectHttpToHttps` | bool | `false` | Redirect HTTP requests to HTTPS |
| `Https.EnableHsts` | bool | `false` | Enable HTTP Strict Transport Security |
| `Https.HstsMaxAgeSeconds` | int | `31536000` | HSTS max-age in seconds (1 year) |
| `Https.HstsIncludeSubDomains` | bool | `false` | Include subdomains in HSTS |

### Forwarded Headers Settings

| Setting | Type | Default | Description |
|---------|------|---------|-------------|
| `ForwardedHeaders.Enabled` | bool | `true` | Enable forwarded headers processing |
| `ForwardedHeaders.TrustAllProxies` | bool | `true` | Trust X-Forwarded-* from any source |
| `ForwardedHeaders.KnownProxies` | string | - | Comma-separated list of trusted proxy IPs |
| `ForwardedHeaders.KnownNetworks` | string | - | Comma-separated list of trusted CIDR networks |

> **Note:** If `KnownProxies` or `KnownNetworks` are configured, `TrustAllProxies` is automatically set to `false`.

### CORS Settings

| Setting | Type | Default | Description |
|---------|------|---------|-------------|
| `Cors.AllowAnyOrigin` | bool | `true` | Allow requests from any origin |
| `Cors.AllowedOrigins` | string | - | Comma-separated list of allowed origins |

> **Note:** If `AllowedOrigins` is configured, `AllowAnyOrigin` is automatically set to `false`.

---

## Scenario Comparison Matrix

| Feature | Default | Reverse Proxy (SSL Termination) | Direct HTTPS | Reverse Proxy + Auth | Direct HTTPS + Auth | End-to-End Encryption + Auth |
|---------|:-------:|:-------------------------------:|:------------:|:--------------------:|:-------------------:|:----------------------------:|
| **JWT Authentication** | ❌ | ❌ | ❌ | ✅ | ✅ | ✅ |
| **Direct (Kestrel) HTTPS** | ❌ | ❌ | ✅ | ❌ | ✅ | ✅ |
| **HTTPS Redirection** | ❌ | ❌ | ✅ | ❌ | ✅ | ❌ (Handled by Reverse Proxy) |
| **HSTS** | ❌ | ❌ | ✅ | ❌ | ✅ | ❌ (Handled by Reverse Proxy) |
| **Restricted CORS** | ❌ | ✅ | ✅ | ✅ | ✅ | ✅ |
| **Forwarded Headers** | ✅ | ✅ | ❌ | ✅ | ❌ (Not needed. No Reverse Proxy) | ✅ |
| **Restricted Proxy Trust** | ❌ | ✅ | N/A | ✅ | N/A | ✅ |
| | | | | | | |
| **Reverse Proxy** | Optional | Yes | No | Yes | No | Yes |
| **Internal Traffic Encrypted** | ❌ | ❌ | ✅ | ❌ | ✅ | ✅ |

**Legend:**

- ✅ = Enabled
- ❌ = Disabled
- N/A = Not Applicable