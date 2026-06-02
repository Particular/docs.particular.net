---
title: Securing ServicePulse
summary: Enable HTTPS for ServicePulse
reviewed: 2026-05-27
component: ServicePulse
related:
- servicecontrol/security
---

## Authentication

ServicePulse authentication is configured in ServiceControl, not in ServicePulse itself. When authentication is enabled on ServiceControl, ServicePulse automatically retrieves the OIDC configuration from the primary ServiceControl instance and presents users with a sign-in page.

To set up authentication, follow the [ServiceControl security guide](/servicecontrol/security).

## Enable HTTPS on ServicePulse

ServicePulse should be served over HTTPS to protect user sessions and tokens in transit. Configure a TLS certificate so ServicePulse serves HTTPS directly.

See [TLS configuration](configuration/tls.md) for certificate options and configuration examples.

## Advanced deployment patterns

For deployments that use a reverse proxy for TLS termination, or require end-to-end encryption between a proxy and ServicePulse, see the [hosting guide](hosting-guide.md).

## Reference

- [TLS configuration](configuration/tls.md)
- [Forward headers for reverse proxy](configuration/forward-headers.md)
- [Hosting guide](hosting-guide.md)
- [ServiceControl security](/servicecontrol/security)
