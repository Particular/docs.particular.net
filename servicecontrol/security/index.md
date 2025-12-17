---
title: ServiceControl security
summary: Secure ServiceControl and ServicePulse with OAuth 2.0 and OpenID Connect
reviewed: 2025-12-11
component: ServiceControl
---

ServiceControl and [ServicePulse](/servicepulse/) support standards-based authentication using OAuth 2.0 and OpenID Connect (OIDC). When enabled, users must sign in through the configured identity provider before accessing ServicePulse.

> [!WARNING]
> Authentication is disabled by default to maintain backward compatibility with existing deployments. Until authentication is enabled, ServicePulse and the ServiceControl are accessible without credentials. Enable authentication in production environments to restrict access.

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

Each ServiceControl instance must be configured separately to enable authentication. Configuration is applied by editing the instance's App.config file or by setting environment variables. ServicePulse retrieves these settings automatically from the connected ServiceControl instances, so no separate configuration is required. Read the [configuring authentication](configuration.md) guide for more details.

## Transport Layer Security (TLS)

When authentication is enabled, ServiceControl and ServicePulse rely on OAuth 2.0 and OpenID Connect flows that involve exchanging access tokens. To protect these tokens and other sensitive data, **TLS must be enabled on every ServiceControl instance and on any reverse proxy in front of it**.

> [!IMPORTANT]
> Without TLS, tokens and other sensitive information are transmitted in clear text. This exposes the system to interception, session hijacking, and unauthorized access. Always secure ServiceControl with HTTPS in production environments.

ServiceControl supports TLS termination at either:

- **Kestrel**: Configure HTTPS directly on the ServiceControl instance
- **A reverse proxy**: Such as NGINX, Apache, IIS, or Azure App Gateway

See the [hosting and security guide](hosting-guide.md) for detailed configuration options including HTTPS certificates, HSTS, and reverse proxy settings.

## ServiceInsight compatibility

ServiceInsight does not support OAuth 2.0 or OpenID Connect authentication. If authentication is enabled on a ServiceControl instance, **ServiceInsight will not be able to connect**.

Users relying on ServiceInsight have two options:

1. **Do not enable authentication** on the relevant ServiceControl instance, or
2. **Use ServicePulse instead of ServiceInsight**.

The second option is recommended, as the functionality previously available in ServiceInsight has been migrated to ServicePulse.