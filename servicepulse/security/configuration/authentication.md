---
title: ServicePulse Authentication Configuration
summary: How to enable and configure authentication for ServicePulse
reviewed: 2026-01-14
component: ServicePulse
related:
- servicepulse/security/hosting-guide
- servicepulse/security/configuration/authorization
- servicecontrol/security/configuration/authentication
- servicecontrol/security/entra-id-authentication
---

ServicePulse supports authentication using [OpenID Connect (OIDC)](https://openid.net/developers/how-connect-works/) and works with any OpenID Connect compliant identity provider (Microsoft Entra ID, Okta, Auth0, Keycloak, etc.). When enabled, users must sign in with their identity provider before accessing the dashboard.

> [!IMPORTANT]
> Ensure ServiceControl has been setup with [authentication](/servicecontrol/security/configuration/authentication.md) and [TLS encryption](/servicecontrol/security/configuration/tls.md). This is a prerequisite for ServicePulse authentication.

## Configuration

Authentication in ServicePulse is [configured in the primary ServiceControl instance](/servicecontrol/security/configuration/authentication.md#configuration). ServicePulse fetches authentication settings on startup from the ServiceControl API and the following settings are set:

| Setting      | Description                                                                   |
|--------------|-------------------------------------------------------------------------------|
| `enabled`    | Enable or disable authentication                                              |
| `authority`  | The OIDC authority URL (identity provider)                                    |
| `client_id`  | The OIDC client ID registered with your identity provider                     |
| `api_scopes` | API scopes to request (space-separated or JSON array)                         |
| `audience`   | The audience claim for the access token (required by some identity providers) |
| `scopes`     | _Added in version 6.18.3._ The complete scope string ServicePulse requests, composed by ServiceControl from `api_scopes` plus `openid profile email` and `offline_access` (see [Required Scopes](#identity-provider-setup-required-scopes)). When talking to a ServiceControl instance older than 6.18.3, this field is absent and ServicePulse falls back to assembling the scope string itself, including `offline_access`. |

## Identity Provider Setup

When registering ServicePulse with your identity provider, configure the following:

| Setting                  | Value                                             |
|--------------------------|---------------------------------------------------|
| Application type         | Single Page Application (SPA)                     |
| Grant type               | Authorization Code with PKCE                      |
| Redirect URI             | `https://your-servicepulse-url/`                  |
| Post-logout redirect URI | `https://your-servicepulse-url/`                  |
| Silent renew URI         | `https://your-servicepulse-url/silent-renew.html` |

### Identity Provider Guides

Step-by-step instructions on configuring some specific identity providers are provided below. For any other identity providers, read their specific documentation and adapt it to the general guidance covered for [Microsoft Entra ID](/servicecontrol/security/entra-id-authentication.md).

- [Microsoft Entra ID](/servicecontrol/security/entra-id-authentication.md)

### Required Scopes

- [ServiceControl API Access](/servicecontrol/security/configuration/authentication.md#identity-provider-setup)

ServicePulse requests the following OIDC scopes in addition to any API scopes configured:

- `openid` - Required for OIDC
- `profile` - User's name and profile information
- `email` - User's email address
- `offline_access` - Enables refresh tokens for silent renewal

`openid`, `profile`, and `email` are always requested. `offline_access` is included unless the operator has disabled it via [`Authentication.ServicePulse.OfflineAccessScopeEnabled`](/servicecontrol/servicecontrol-instances/configuration.md#authentication) on the primary ServiceControl instance — some identity providers reject the entire authorization request if a client requests a scope it isn't permitted to use, and `offline_access` is the scope most likely to trigger that. Disabling it trades away silent renewal via refresh token: see [Silent Renewal](#token-management-silent-renewal) for what that means in practice.

## Token Management

### Storage

User tokens are stored in the browser's `sessionStorage`. This means:

- Tokens are cleared when the browser tab is closed
- Each browser tab maintains its own session
- Tokens are not shared across tabs

### Silent Renewal

ServicePulse automatically renews access tokens before they expire. With `offline_access` requested, it renews over a back-channel call to the token endpoint using a refresh token. Without `offline_access`, some identity providers don't issue a refresh token, so ServicePulse falls back to a hidden iframe (`silent-renew.html`) that depends on the identity provider's session cookie being sent in a third-party context — something browsers that restrict third-party cookies (Safari ITP, Brave, and Chrome's ongoing restrictions) will block.

If silent renewal fails (e.g. session expired at the identity provider, or the iframe fallback is blocked), users are redirected to log in again: a brief round-trip if the identity provider session is still live, or a full return to the login page if it isn't.

## User Interface

When authentication is enabled and the user is signed in, the dashboard header displays:

- User's name (from the `name` claim)
- User's email (from the `email` claim)
- A sign-out button

## Troubleshooting

### "Authentication required" error

This error appears when:

1. Authentication is enabled but no valid token exists
2. The token has expired and silent renewal failed
3. The user cancelled the login flow

**Solution:** Click the login button or refresh the page to initiate authentication.

### Redirect loop or login failures

Common causes:

1. **Incorrect redirect URI** - Ensure the redirect URI registered with your identity provider exactly matches the ServicePulse URL (including trailing slash if present)
2. **CORS issues** - Your identity provider must allow requests from the ServicePulse origin
3. **Clock skew** - Ensure server clocks are synchronized; token validation is time-sensitive

### Silent renewal fails repeatedly

This can occur when:

1. The identity provider session has expired
2. Third-party cookies are blocked (required for iframe-based renewal)
3. The `silent-renew.html` page is not accessible

**Solution:** Check the browser console for specific error messages. Some browsers block third-party cookies by default, which can prevent silent renewal from working.

### Token not included in API requests

Verify that:

1. Authentication is enabled in ServiceControl
2. The user has completed the login flow
3. The token has not expired

Check the browser's Network tab to confirm the `Authorization: Bearer` header is present on API requests.

## Security Considerations

### HTTPS Required

For production deployments, [always use HTTPS](tls.md). OIDC tokens are sensitive credentials that should only be transmitted over encrypted connections.

### Session Duration

Token lifetime is controlled by your identity provider. Consider configuring:

- **Access token lifetime** - Short-lived (e.g. 1 hour) for security
- **Refresh token lifetime** - Longer-lived to enable silent renewal
- **Session policies** - Maximum session duration before re-authentication is required
