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

## Configuration examples

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

## Troubleshooting

### ServicePulse cannot connect to ServiceControl

**Symptom**: ServicePulse displays connection errors or fails to load data, and browser developer tools show CORS errors like "Access-Control-Allow-Origin" or "blocked by CORS policy".

**Cause**: The ServicePulse origin is not in the allowed origins list, or the origin URL doesn't match exactly (including protocol and port).

**Solutions**:

- Check browser developer tools (F12 > Console) for the exact CORS error message
- Verify the origin in `AllowedOrigins` matches exactly, including:
  - Protocol (`https://` vs `http://`)
  - Domain name (no trailing slash)
  - Port number if non-standard (e.g. `https://servicepulse.example.com:8080`)
- For testing, temporarily set `AllowAnyOrigin` to `true` to confirm CORS is the issue.

> [!CAUTION]
> Do not leave `AllowAnyOrigin` set to `true` in any production environment. This removes a key browser security feature.

### CORS preflight requests failing

**Symptom**: Simple GET requests work, but POST/PUT/DELETE requests fail with CORS errors. Browser shows OPTIONS request failures.

**Cause**: The browser sends a preflight OPTIONS request for complex requests, which may be blocked by a firewall or reverse proxy.

**Solutions**:

- Ensure the reverse proxy forwards OPTIONS requests to ServiceControl
- Check that no firewall or WAF is blocking OPTIONS requests
- Verify ServiceControl is responding to OPTIONS requests (check response headers)

### Origin mismatch with reverse proxy

**Symptom**: CORS errors occur even though the origin appears to be configured correctly.

**Cause**: When using a reverse proxy, the origin seen by ServiceControl may differ from what's configured. The browser sends the origin based on where ServicePulse is loaded from.

**Solutions**:

- Check which URL the browser is using to access ServicePulse (this is the origin)
- Ensure the configured origin matches the ServicePulse URL exactly
- If using different domains for internal and external access, add both origins to `AllowedOrigins`

### Credentials not being sent

**Symptom**: Authenticated requests fail even when CORS is configured. The `Authorization` header is not sent.

**Cause**: When using authentication with CORS, credentials are only sent if the response includes appropriate CORS headers and the origin is explicitly allowed (not wildcard).

**Solutions**:

- Ensure `AllowAnyOrigin` is set to `false` when using authentication
- Add the specific origin to `AllowedOrigins` (wildcards don't support credentials)
- Verify the `Access-Control-Allow-Credentials` header is present in responses
