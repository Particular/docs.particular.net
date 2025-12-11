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

## Configuration

Each ServiceControl instance must be configured separately to enable authentication. Configuration is applied by editing the instance's App.config file or by setting environment variables. ServicePulse retrieves these settings automatically from the connected ServiceControl instances, so no separate configuration is required. Read the [configuring authentication](configuration.md) guide for more details.
