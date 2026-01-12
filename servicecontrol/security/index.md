---
title: ServiceControl security overview
summary: Secure ServiceControl and ServicePulse with OAuth 2.0 and OpenID Connect
reviewed: 2025-12-11
component: ServiceControl
---

ServiceControl and [ServicePulse](/servicepulse/) support standards-based authentication using [OAuth 2.0](https://oauth.net/2/) with [JSON Web Tokens (JWT)](https://en.wikipedia.org/wiki/JSON_Web_Token) and [OpenID Connect (OIDC)](https://openid.net/developers/how-connect-works/). When enabled, users must sign in through the configured identity provider before accessing ServicePulse.

> [!WARNING]
> Authentication is disabled by default to maintain backward compatibility with existing deployments. Until authentication is enabled, ServicePulse and ServiceControl are accessible without credentials. Enable authentication to restrict access.

## Supported identity providers

Any OpenID Connect (OIDC) compliant identity provider can be used, including:

- Active Directory Federation Services (ADFS)
- Auth0
- AWS IAM Identity Center
- Duende IdentityServer
- Google Workspace
- Keycloak
- Microsoft Entra ID
- Okta
- Ping Identity

## Enabling authentication

Each ServiceControl instance must be configured separately to enable authentication. Configuration is applied by editing the instance's App.config file or by setting environment variables. ServicePulse retrieves these settings automatically from the connected ServiceControl instances, so no separate configuration is required. Read the [configuring authentication](configuration/authentication.md) guide for more details.

### How it Works

When authentication is enabled:

1. ServicePulse retrieves authentication configuration from the ServiceControl `/api/authentication/configuration` endpoint
2. API requests must include a valid JWT bearer token in the `Authorization` header
3. ServiceControl validates the token against the configured authority
4. The token must have the correct audience and not be expired

TODO: Add sequence diagram

## Transport Layer Security (TLS)

When authentication is enabled, ServiceControl and ServicePulse rely on OAuth 2.0 and OpenID Connect flows that involve exchanging access tokens. To protect these tokens and other sensitive data, **TLS must be enabled on every ServiceControl instance and on any reverse proxy in front of it**.

> [!IMPORTANT]
> Without TLS, tokens and other sensitive information are transmitted in clear text. This exposes the system to interception, session hijacking, and unauthorized access. Always secure ServiceControl with HTTPS in production environments.

ServiceControl supports TLS at either:

- **Kestrel**: Configure HTTPS directly on the ServiceControl instance
- **A reverse proxy**: Terminate SSL at a reverse proxy such as NGINX, Apache, IIS, F5, or Azure App Gateway

See [HTTPS Configuration](configuration/https.md) for detailed configuration options including HTTPS certificates, HSTS, and reverse proxy settings, and the [hosting guide](hosting-guide.md) for example usage of the configuration.

## ServiceInsight compatibility

[ServiceInsight has been sunset](/serviceinsight/) and will not be updated to support OAuth 2.0 or OpenID Connect authentication. If authentication is enabled on a ServiceControl instance, **ServiceInsight will not be able to connect**.

Users relying on ServiceInsight have two options:

1. **[Use ServicePulse](http://localhost:55666/servicepulse/installation) instead of ServiceInsight** (Recommended. Functionality previously available in ServiceInsight has been migrated to ServicePulse).
2. **Do not enable authentication** on the relevant ServiceControl instance
