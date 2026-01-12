---
title: Authentication Configuration
summary: How to enable and configure authentication for ServiceControl and ServicePulse
reviewed: 2026-01-12
component: ServiceControl
---

ServiceControl instances can be configured to require JWT authentication using OpenID Connect (OIDC). This enables integration with identity providers like Microsoft Entra ID (Azure AD), Okta, Auth0, and other OIDC-compliant providers. This guide explains how to configure ServiceControl to enable authentication for both ServiceControl and ServicePulse.

> [!NOTE]
> Authentication is disabled by default. To enable it, see below.

## Configuration

ServiceControl instances can be configured via environment variables or App.config. Each instance type uses a different prefix. See the [Hosting Guide](../hosting-guide.md) for example usage of these configuration settings in conjustion with [Forward Header](forward-headers.md) and [HTTPS](https.md) configuration settings in a scenario based format.

include: servicecontrol-instance-prefix

### Core Settings

| Environment Variable                | App.config                          | Default | Description                                                                               |
|-------------------------------------|-------------------------------------|---------|-------------------------------------------------------------------------------------------|
| `{PREFIX}_AUTHENTICATION_ENABLED`   | `{PREFIX}/Authentication.Enabled`   | `false` | Enable JWT authentication                                                                 |
| `{PREFIX}_AUTHENTICATION_AUTHORITY` | `{PREFIX}/Authentication.Authority` | (none)  | OpenID Connect authority URL (e.g., `https://login.microsoftonline.com/{tenant-id}/v2.0`) |
| `{PREFIX}_AUTHENTICATION_AUDIENCE`  | `{PREFIX}/Authentication.Audience`  | (none)  | The audience identifier (typically your API identifier or client ID, e.g., `api://servicecontrol`)                      |

### Token Validation Settings

These settings control specific aspects of token validation. The defaults below provide a secure validation process and should not be changed unless there is a specific requirement.

| Environment Variable                               | App.config                                         | Default | Description                                                                                        |
|----------------------------------------------------|----------------------------------------------------|---------|----------------------------------------------------------------------------------------------------|
| `{PREFIX}_AUTHENTICATION_VALIDATEISSUER`           | `{PREFIX}/Authentication.ValidateIssuer`           | `true`  | Validates that the token was issued by the configured authority.                                   |
| `{PREFIX}_AUTHENTICATION_VALIDATEAUDIENCE`         | `{PREFIX}/Authentication.ValidateAudience`         | `true`  | Validates that the token contains the expected audience claim.                                     |
| `{PREFIX}_AUTHENTICATION_VALIDATELIFETIME`         | `{PREFIX}/Authentication.ValidateLifetime`         | `true`  | Validates that the token has not expired.                                                          |
| `{PREFIX}_AUTHENTICATION_VALIDATEISSUERSIGNINGKEY` | `{PREFIX}/Authentication.ValidateIssuerSigningKey` | `true`  | Validates the token signature using keys from the identity provider.                               |
| `{PREFIX}_AUTHENTICATION_REQUIREHTTPSMETADATA`     | `{PREFIX}/Authentication.RequireHttpsMetadata`     | `true`  | Requires HTTPS when retrieving OpenID Connect metadata. Set to `false` only for local development. |

### ServicePulse settings

These settings are served to ServicePulse through a bootstrap endpoint hosting in ServiceControl, allowing ServicePulse to authenticate users without its own configuration file.

> [!NOTE]
> These settings are only valid on the primary ServiceControl instance.

| Environment Variable                                   | App.config                                             | Description                                                                                                                           |
|--------------------------------------------------------|--------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------|
| `SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_CLIENTID`  | `ServiceControl/Authentication.ServicePulse.ClientId`  | The OAuth 2.0 client ID for the ServicePulse application registration.                                                                |
| `SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_AUTHORITY` | `ServiceControl/Authentication.ServicePulse.Authority` | The OpenID Connect authority URL for ServicePulse authentication. For Microsoft Entra ID, this typically includes `/v2.0` at the end. |
| `SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_APISCOPES` | `ServiceControl/Authentication.ServicePulse.ApiScopes` | A JSON array of scopes to request when acquiring tokens. For example: `["api://app-id/api.access"]`                                   |

> [!NOTE]
> The ServicePulse audience Open ID Connect configuration is automatically filled in using the [ServiceControl audience setting](#configuration-core-settings).

## Identity provider guides

For step-by-step instructions on configuring specific identity providers, see:

- [Microsoft Entra ID](../entra-id-authentication.md)

## Configuration examples

The following examples show complete authentication configurations for common identity providers.

### Microsoft Entra ID

```xml
<!-- Enable authentication -->
<add key="ServiceControl/Authentication.Enabled" value="true" />
<add key="ServiceControl/Authentication.Authority" value="https://login.microsoftonline.com/{tenant-id}" />
<add key="ServiceControl/Authentication.Audience" value="api://{app-id}" />

<!-- ServicePulse settings -->
<add key="ServiceControl/Authentication.ServicePulse.ClientId" value="{client-id}" />
<add key="ServiceControl/Authentication.ServicePulse.Authority" value="https://login.microsoftonline.com/{tenant-id}/v2.0" />
<add key="ServiceControl/Authentication.ServicePulse.ApiScopes" value="[&quot;api://{app-id}/api.access&quot;]" />
```

### Auth0

```xml
<!-- Enable authentication -->
<add key="ServiceControl/Authentication.Enabled" value="true" />
<add key="ServiceControl/Authentication.Authority" value="https://{auth0-domain}" />
<add key="ServiceControl/Authentication.Audience" value="https://{api-identifier}" />

<!-- ServicePulse settings -->
<add key="ServiceControl/Authentication.ServicePulse.ClientId" value="{client-id}" />
<add key="ServiceControl/Authentication.ServicePulse.Authority" value="https://{auth0-domain}" />
<add key="ServiceControl/Authentication.ServicePulse.ApiScopes" value="[&quot;{scope1}&quot;,&quot;{scope2}&quot;]" />
```

### Keycloak

```xml
<!-- Enable authentication -->
<add key="ServiceControl/Authentication.Enabled" value="true" />
<add key="ServiceControl/Authentication.Authority" value="https://{keycloak-host}/realms/{realm}" />
<add key="ServiceControl/Authentication.Audience" value="{api-client-id}" />

<!-- ServicePulse settings -->
<add key="ServiceControl/Authentication.ServicePulse.ClientId" value="{spa-client-id}" />
<add key="ServiceControl/Authentication.ServicePulse.Authority" value="https://{keycloak-host}/realms/{realm}" />
<add key="ServiceControl/Authentication.ServicePulse.ApiScopes" value="[&quot;{scope}&quot;]" />
```
