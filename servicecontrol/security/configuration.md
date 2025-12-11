---
title: Configuring authentication
summary: How to enable and configure authentication for ServiceControl and ServicePulse
reviewed: 2025-12-11
component: ServiceControl
---

This guide explains how to configure ServiceControl to enable authentication.

## Enabling authentication

Authentication is disabled by default. To enable it, add the following settings to the ServiceControl configuration file:

```xml
<add key="ServiceControl/Authentication.Enabled" value="true" />
<add key="ServiceControl/Authentication.Authority" value="https://your-identity-provider" />
<add key="ServiceControl/Authentication.Audience" value="your-api-audience" />
```

## ServiceControl settings

These settings control how ServiceControl validates incoming JWT access tokens.

| Setting | Description |
|---------|-------------|
| `ServiceControl/Authentication.Enabled` | Set to `true` to enable authentication. Default: `false` |
| `ServiceControl/Authentication.Authority` | The OpenID Connect authority URL used to validate tokens. This is typically the issuer URL of the identity provider. |
| `ServiceControl/Authentication.Audience` | The expected audience claim in the access token. This is typically the Application ID URI of the ServiceControl API registration. |

### Token validation settings

These optional settings control specific aspects of token validation. The defaults are secure and should not be changed unless there is a specific requirement.

| Setting | Default | Description |
|---------|---------|-------------|
| `ServiceControl/Authentication.ValidateIssuer` | `true` | Validates that the token was issued by the configured authority. |
| `ServiceControl/Authentication.ValidateAudience` | `true` | Validates that the token contains the expected audience claim. |
| `ServiceControl/Authentication.ValidateLifetime` | `true` | Validates that the token has not expired. |
| `ServiceControl/Authentication.ValidateIssuerSigningKey` | `true` | Validates the token signature using keys from the identity provider. |
| `ServiceControl/Authentication.RequireHttpsMetadata` | `true` | Requires HTTPS when retrieving OpenID Connect metadata. Set to `false` only for local development. |

## ServicePulse settings

These settings are served to ServicePulse through a bootstrap endpoint, allowing ServicePulse to authenticate users without its own configuration file.

| Setting | Description |
|---------|-------------|
| `ServiceControl/Authentication.ServicePulse.ClientId` | The OAuth 2.0 client ID for the ServicePulse application registration. |
| `ServiceControl/Authentication.ServicePulse.Authority` | The OpenID Connect authority URL for ServicePulse authentication. For Microsoft Entra ID, this typically includes `/v2.0` at the end. |
| `ServiceControl/Authentication.ServicePulse.Audience` | The audience for ServicePulse token requests. This is typically the same as the ServiceControl audience. |
| `ServiceControl/Authentication.ServicePulse.ApiScopes` | A JSON array of scopes to request when acquiring tokens. For example: `["api://app-id/api.access"]` |

## Configuration file example

The following example shows a complete authentication configuration for Microsoft Entra ID:

```xml
<!-- Enable authentication -->
<add key="ServiceControl/Authentication.Enabled" value="true" />

<!-- ServiceControl token validation -->
<add key="ServiceControl/Authentication.Authority" value="https://login.microsoftonline.com/your-tenant-id" />
<add key="ServiceControl/Authentication.Audience" value="api://your-app-id" />

<!-- ServicePulse client settings -->
<add key="ServiceControl/Authentication.ServicePulse.ClientId" value="your-servicepulse-client-id" />
<add key="ServiceControl/Authentication.ServicePulse.Authority" value="https://login.microsoftonline.com/your-tenant-id/v2.0" />
<add key="ServiceControl/Authentication.ServicePulse.Audience" value="api://your-app-id" />
<add key="ServiceControl/Authentication.ServicePulse.ApiScopes" value="[&quot;api://your-app-id/api.access&quot;]" />
```

## Environment variables

All settings can also be configured using environment variables, which is useful for containerized deployments and local development. Convert setting names to environment variables by replacing `/` and `.` with `_`.

| App.config key | Environment variable |
|----------------|---------------------|
| `ServiceControl/Authentication.Enabled` | `SERVICECONTROL_AUTHENTICATION_ENABLED` |
| `ServiceControl/Authentication.Authority` | `SERVICECONTROL_AUTHENTICATION_AUTHORITY` |
| `ServiceControl/Authentication.Audience` | `SERVICECONTROL_AUTHENTICATION_AUDIENCE` |
| `ServiceControl/Authentication.ServicePulse.ClientId` | `SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_CLIENTID` |
| `ServiceControl/Authentication.ServicePulse.Authority` | `SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_AUTHORITY` |
| `ServiceControl/Authentication.ServicePulse.Audience` | `SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_AUDIENCE` |
| `ServiceControl/Authentication.ServicePulse.ApiScopes` | `SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_APISCOPES` |

Environment variables take precedence over App.config settings.

### Environment variable example

```powershell
# Enable authentication
$env:SERVICECONTROL_AUTHENTICATION_ENABLED = "true"
$env:SERVICECONTROL_AUTHENTICATION_AUTHORITY = "https://login.microsoftonline.com/your-tenant-id"
$env:SERVICECONTROL_AUTHENTICATION_AUDIENCE = "api://your-app-id"

# ServicePulse settings
$env:SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_CLIENTID = "your-servicepulse-client-id"
$env:SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_AUTHORITY = "https://login.microsoftonline.com/your-tenant-id/v2.0"
$env:SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_AUDIENCE = "api://your-app-id"
$env:SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_APISCOPES = '["api://your-app-id/api.access"]'
```

