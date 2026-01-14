---
title: ServiceControl Authentication Configuration
summary: How to enable and configure authentication for ServiceControl
reviewed: 2026-01-12
component: ServiceControl
related:
- servicecontrol/security/hosting-guide
- servicepulse/security/configuration/authentication
---

ServiceControl instances can be configured to require [JWT](https://en.wikipedia.org/wiki/JSON_Web_Token) authentication using [OpenID Connect (OIDC)](https://openid.net/developers/how-connect-works/). This enables integration with identity providers like Microsoft Entra ID (Azure AD), Okta, Auth0, and other OIDC-compliant providers. This guide explains how to configure ServiceControl to enable authentication for both ServiceControl and ServicePulse.

> [!NOTE]
> Authentication is disabled by default. To enable it, see below.

## Configuration

ServiceControl instances can be configured via environment variables or App.config. Each instance type uses a different prefix. See the [Hosting Guide](../hosting-guide.md) for example usage of these configuration settings in conjunction with [Forward Header](forward-headers.md) and [TLS](tls.md) configuration settings in a scenario based format.

include: servicecontrol-instance-prefix

### Core Settings

| Environment Variable                | App.config                          | Default | Description                                                                                        |
|-------------------------------------|-------------------------------------|---------|----------------------------------------------------------------------------------------------------|
| `{PREFIX}_AUTHENTICATION_ENABLED`   | `{PREFIX}/Authentication.Enabled`   | `false` | Enable JWT authentication                                                                          |
| `{PREFIX}_AUTHENTICATION_AUTHORITY` | `{PREFIX}/Authentication.Authority` | (none)  | OpenID Connect authority URL (e.g., `https://login.microsoftonline.com/{tenant-id}/v2.0`)          |
| `{PREFIX}_AUTHENTICATION_AUDIENCE`  | `{PREFIX}/Authentication.Audience`  | (none)  | The audience identifier (typically your API identifier or client ID, e.g., `api://servicecontrol`) |

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

## Identity Provider Setup

When registering ServiceControl with your identity provider, you will need the following information:

| Setting                       | Description                                                                                                      |
|-------------------------------|------------------------------------------------------------------------------------------------------------------|
| Application type              | Web application / API (confidential client)                                                                      |
| Redirect URI                  | Not required for API-only registration                                                                           |
| Audience                      | A unique identifier for the ServiceControl API (e.g., `api://servicecontrol` or a custom URI)                    |
| Scopes                        | Define at least one scope that ServicePulse can request (e.g., `api.access`)                                     |
| Allowed token audiences       | Must include the audience configured in ServiceControl                                                           |

Additionally, a separate application registration is required for ServicePulse. See [ServicePulse Identity Provider Setup](/servicepulse/security/configuration/authentication.md#identity-provider-setup) for those requirements.

### Identity Provider Guides

For step-by-step instructions on configuring specific identity providers, see:

- [Microsoft Entra ID](../entra-id-authentication.md)

### Configuration examples

The following examples show complete authentication configurations for common identity providers.

#### Microsoft Entra ID

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

#### Auth0

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

#### Keycloak

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

## Troubleshooting

### 401 Unauthorized responses

This error occurs when:

1. The token is missing or malformed
2. The token has expired
3. The token audience doesn't match the configured `Authentication.Audience`
4. The token issuer doesn't match the configured `Authentication.Authority`

**Solution:** Verify that the `Authority` and `Audience` settings match exactly what is configured in your identity provider. Check the ServiceControl logs for detailed validation error messages.

### Token validation fails with "IDX10205: Issuer validation failed"

This typically means:

1. The `Authority` URL is incorrect or doesn't match the token's `iss` claim
2. For Microsoft Entra ID, ensure the authority URL includes or excludes `/v2.0` consistently with your token version

**Solution:** Compare the `iss` claim in your JWT token (decode it at [jwt.ms](https://jwt.ms)) with your configured authority URL.

### Token validation fails with "IDX10214: Audience validation failed"

This occurs when:

1. The `Audience` setting doesn't match the token's `aud` claim
2. The identity provider is issuing tokens for a different audience

**Solution:** Verify the `aud` claim in your token matches the `Authentication.Audience` configuration exactly.

### Unable to retrieve OpenID Connect metadata

This error appears when ServiceControl cannot reach the identity provider's discovery endpoint:

1. Network connectivity issues to the identity provider
2. Firewall blocking outbound HTTPS connections
3. Invalid `Authority` URL
4. TLS/SSL certificate issues

**Solution:** Verify network connectivity to `{authority}/.well-known/openid-configuration`. For local development, you may need to set `RequireHttpsMetadata` to `false`.

### ServicePulse cannot authenticate

If ServicePulse shows authentication errors:

1. Verify the ServicePulse-specific settings are configured on the **primary** ServiceControl instance
2. Ensure the `ServicePulse.ClientId` matches the SPA application registration
3. Check that `ServicePulse.ApiScopes` includes the correct scope(s) for your API

**Solution:** See the [ServicePulse authentication troubleshooting](/servicepulse/security/configuration/authentication.md#troubleshooting) for client-side issues.

### Clock skew causing token validation failures

JWT tokens are time-sensitive. If server clocks are not synchronized:

1. Tokens may be rejected as "not yet valid" or "expired"
2. This commonly occurs in virtual machines or containers

**Solution:** Ensure all servers are using NTP to synchronize their clocks.

## Limitations

### Anonymous endpoints

The following endpoints are accessible without authentication, even when authentication is enabled:

| Endpoint                            | Purpose                                                            |
|-------------------------------------|--------------------------------------------------------------------|
| `/api/authentication/configuration` | Returns authentication configuration for clients like ServicePulse |

This endpoint must remain accessible so clients can obtain the authentication configuration needed to acquire tokens before authenticating.

### Scatter-gather endpoints

The Primary ServiceControl instance communicates with other ServiceControl instances to aggregate data. The following endpoints support this scatter-gather communication and are accessible without authentication:

| Endpoint                     | Purpose                                                   |
|------------------------------|-----------------------------------------------------------|
| `/api`                       | API root/discovery - returns available endpoints          |
| `/api/instance-info`         | Returns instance configuration information                |
| `/api/configuration`         | Returns instance configuration information (alias)        |
| `/api/configuration/remotes` | Returns remote instance configurations for scatter-gather |

These endpoints allow the Primary instance to discover remote instances.

### Same Authentication Configuration Required

When using scatter-gather with authentication enabled:

- All instances (Primary, Audit, Monitoring) must use the **same** Authority and Audience
- Client tokens must be valid for all instances
- There is no service-to-service authentication mechanism; client tokens are forwarded directly

### Token Forwarding Security Considerations

- Client tokens are forwarded to remote instances in their entirety
- Remote instances see the same token as the primary instance
- Token scope/claims are not modified during forwarding
