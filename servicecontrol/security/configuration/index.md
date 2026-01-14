---
title: ServiceControl Security Configuration
summary: How to secure ServiceControl instances
reviewed: 2026-01-12
component: ServiceControl
related:
- servicecontrol/security/hosting-guide
- servicepulse/security/hosting-guide
---

ServiceControl provides several configuration options to secure instances. This section covers the available security settings and how to configure them.

## Configuration topics

### [Authentication](authentication.md)

Configure JWT authentication using OpenID Connect (OIDC) to secure ServiceControl and ServicePulse. Supports identity providers like Microsoft Entra ID, Okta, Auth0, Keycloak, and other OIDC-compliant providers.

### [TLS](tls.md)

Enable encrypted connections by configuring ServiceControl to use HTTPS directly. Includes options for certificate management, HTTP to HTTPS redirects, and HTTP Strict Transport Security (HSTS).

### [Forward Headers](forward-headers.md)

Configure forwarded header processing for deployments behind a reverse proxy. Ensures ServiceControl correctly interprets client requests when SSL/TLS is terminated at a load balancer or proxy.

### [CORS](cors.md)

Configure Cross-Origin Resource Sharing to control which web applications can access the ServiceControl API.

## Configuration methods

ServiceControl instances can be configured via environment variables or App.config. Each instance type uses a different prefix:

include: servicecontrol-instance-prefix
