---
title: CORS Configuration
summary: How to configure Cross-Origin Resource Sharing for ServiceControl instances
reviewed: 2026-01-13
component: ServiceControl
---

Cross-Origin Resource Sharing (CORS) controls which web applications can make requests to ServiceControl. This is important when ServicePulse is hosted on a different domain than ServiceControl.

## Configuration

ServiceControl instances can be configured via environment variables or App.config. Each instance type uses a different prefix. See the [Hosting Guide](../hosting-guide.md) for example usage of these configuration settings in conjunction with [Authentication](authentication.md), [TLS](tls.md), and [Forward Headers](forward-headers.md) configuration settings in a scenario based format.

include: servicecontrol-instance-prefix

### Settings

| Environment Variable              | App.config                        | Default | Description                                      |
|-----------------------------------|-----------------------------------|---------|--------------------------------------------------|
| `{PREFIX}_CORS_ALLOWANYORIGIN`    | `{PREFIX}/Cors.AllowAnyOrigin`    | `true`  | Allow requests from any origin                   |
| `{PREFIX}_CORS_ALLOWEDORIGINS`    | `{PREFIX}/Cors.AllowedOrigins`    | (none)  | Comma-separated list of allowed origins          |

> [!WARNING]
> The default configuration (`AllowAnyOrigin = true`) allows any website to make requests to ServiceControl. For production deployments, set `AllowAnyOrigin` to `false` and configure `AllowedOrigins` to restrict access to trusted domains only.

## When to configure CORS

CORS configuration is required when:

- ServicePulse is hosted on a different domain than ServiceControl
- ServiceControl is accessed through a reverse proxy with a different domain

## Configuration example

To restrict access to only your ServicePulse domain:

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
