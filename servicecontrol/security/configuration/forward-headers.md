---
title: Forward Headers Configuration
summary: How to enable and configure forward headers for ServiceControl instances
reviewed: 2026-01-12
component: ServiceControl
---

When ServiceControl instances are deployed behind a reverse proxy (like NGINX, Traefik, or a cloud load balancer) that terminates SSL/TLS, you need to configure forwarded headers so ServiceControl correctly understands the original client request.

## Configuration

ServiceControl instances can be configured via environment variables or App.config. Each instance type uses a different prefix. See the [Hosting Guide](../hosting-guide.md) for example usage of these configuration settings in conjustion with [Authentication](authentication.md) and [HTTPS](https.md) configuration settings in a scenario based format.

include: servicecontrol-instance-prefix

### Settings

| Environment Variable                        | App.config                                  | Default | Description                                                                  |
|---------------------------------------------|---------------------------------------------|---------|------------------------------------------------------------------------------|
| `{PREFIX}_FORWARDEDHEADERS_ENABLED`         | `{PREFIX}/ForwardedHeaders.Enabled`         | `true`  | Enable forwarded headers processing                                          |
| `{PREFIX}_FORWARDEDHEADERS_TRUSTALLPROXIES` | `{PREFIX}/ForwardedHeaders.TrustAllProxies` | `true`  | Trust all proxies (auto-disabled if known proxies/networks set)              |
| `{PREFIX}_FORWARDEDHEADERS_KNOWNPROXIES`    | `{PREFIX}/ForwardedHeaders.KnownProxies`    | (none)  | Comma-separated IP addresses of trusted proxies (e.g., `127.0.0.1,10.0.0.5`) |
| `{PREFIX}_FORWARDEDHEADERS_KNOWNNETWORKS`   | `{PREFIX}/ForwardedHeaders.KnownNetworks`   | (none)  | Comma-separated CIDR networks (e.g., `10.0.0.0/8,172.16.0.0/12`)             |

> [!WARNING]
> The default configuration (`TrustAllProxies = true`) is suitable for development and trusted container environments only. For production deployments accessible from untrusted networks, its recommended to configure `KnownProxies` or `KnownNetworks` to restrict which sources can set forwarded headers. Failing to do so can allow attackers to spoof client IP addresses.

## What Headers are Processed

When enabled, ServiceControl instances processes:

- `X-Forwarded-For` - Original client IP address
- `X-Forwarded-Proto` - Original protocol (http/https)
- `X-Forwarded-Host` - Original host header

When the proxy is trusted:

- `Request.Scheme` will be set from `X-Forwarded-Proto` (e.g., `https`)
- `Request.Host` will be set from `X-Forwarded-Host` (e.g., `servicecontrol.example.com`)
- Client IP will be available from `X-Forwarded-For`

When the proxy is **not** trusted (incorrect `KnownProxies`):

- `X-Forwarded-*` headers are **ignored** (not applied to the request)
- `Request.Scheme` remains `http`
- `Request.Host` remains the internal hostname
- The request is still processed (not blocked)

## HTTP to HTTPS Redirect

When using a reverse proxy that terminates SSL, you can configure ServiceControl instances to redirect HTTP requests to HTTPS. This works in combination with forwarded headers:

1. The reverse proxy forwards both HTTP and HTTPS requests to ServiceControl
2. The proxy sets `X-Forwarded-Proto` to indicate the original protocol
3. ServiceControl reads this header (via forwarded headers processing)
4. If the original request was HTTP and redirect is enabled, ServiceControl returns a redirect to HTTPS

To enable HTTP to HTTPS redirect, see [HTTPS Configuration](https.md) for details on how to do this.

## Proxy Chain Behavior (ForwardLimit)

When processing `X-Forwarded-For` headers with multiple IPs (proxy chains), the behavior depends on trust configuration:

| Configuration             | ForwardLimit      | Behavior                                      |
|---------------------------|-------------------|-----------------------------------------------|
| `TrustAllProxies = true`  | `null` (no limit) | Processes all IPs, returns original client IP |
| `TrustAllProxies = false` | `1` (default)     | Processes only the last proxy IP              |

For example, with `X-Forwarded-For: 203.0.113.50, 10.0.0.1, 192.168.1.1`:

- **TrustAllProxies = true**: Returns `203.0.113.50` (original client)
- **TrustAllProxies = false**: Returns `192.168.1.1` (last proxy)
