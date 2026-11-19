### ServiceControl/ForwardedHeaders.TrustAllProxies

_Added in version 6.11.0_

Trusts forwarded headers from any source. This setting is overwritten to `false` when using `KnownProxies` or `KnownNetworks`.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_FORWARDEDHEADERS_TRUSTALLPROXIES` |
| **App config key** | `ServiceControl/ForwardedHeaders.TrustAllProxies` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

> [!WARNING]
> For production environments behind a reverse proxy, set this to `false` and configure `KnownProxies` or `KnownNetworks` to restrict which proxies are trusted.
