---
title: ServicePulse Security Configuration
summary: How to secure ServicePulse instances
reviewed: 2026-01-14
component: ServicePulse
related:
- servicepulse/security/hosting-guide
- servicecontrol/security/hosting-guide
---

ServicePulse provides several configuration options to secure instances. This section covers the available security settings and how to configure them.

## Configuration topics

### [Authentication](authentication.md)

Authentication settings are fetched from the primary ServiceControl instance, streamlining the authentication setup for ServicePulse.

### [TLS](tls.md)

Enable encrypted connections by configuring ServicePulse to use HTTPS directly. Includes options for certificate management, HTTP to HTTPS redirects, and HTTP Strict Transport Security (HSTS).

### [Forward Headers](forward-headers.md)

Configure forwarded header processing for deployments behind a reverse proxy. Ensures ServicePulse correctly interprets client requests when SSL/TLS is terminated at a load balancer or proxy.
