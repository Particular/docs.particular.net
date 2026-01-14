---
title: ServiceControl CORS Configuration
summary: How to configure Cross-Origin Resource Sharing for ServiceControl instances
reviewed: 2026-01-14
component: ServiceControl
related:
- servicecontrol/security/hosting-guide
---

Cross-Origin Resource Sharing (CORS) controls which web applications can make requests to ServiceControl. This is important when ServicePulse is hosted on a different domain than ServiceControl.

## Configuration

ServiceControl instances can be configured via environment variables or App.config. Each instance type uses a different prefix. See the [Hosting Guide](../hosting-guide.md) for example usage of these configuration settings in conjunction with [Authentication](authentication.md), [TLS](tls.md), and [Forward Headers](forward-headers.md) configuration settings in a scenario based format.

- [Primary Instance](/servicecontrol/servicecontrol-instances/configuration.md#cors)
- [Audit Instance](/servicecontrol/audit-instances/configuration.md#cors)
- [Monitoring Instance](/servicecontrol/monitoring-instances/configuration.md#cors)

## When to configure CORS

CORS configuration is required when:

- ServicePulse is hosted on a different domain than ServiceControl
- ServiceControl is accessed through a reverse proxy with a different domain

## Configuration example

To restrict access to only your ServicePulse domain, using the primary ServiceControl instance:

```xml
<appSettings>
  <add key="ServiceControl/Cors.AllowAnyOrigin" value="false" />
  <add key="ServiceControl/Cors.AllowedOrigins" value="https://servicepulse.yourcompany.com" />
</appSettings>
```

To allow multiple origins:

```xml
<appSettings>
  <add key="ServiceControl/Cors.AllowAnyOrigin" value="false" />
  <add key="ServiceControl/Cors.AllowedOrigins" value="https://servicepulse.yourcompany.com,https://admin.yourcompany.com" />
</appSettings>
```
