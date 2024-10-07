---
title: Running ServicePulse in containers
reviewed: 2024-10-07
component: ServicePulse
---

ServicePulse can be deployed on containers using the [`particular/servicepulse` image](https://hub.docker.com/r/particular/servicepulse).

The container can be created as shown in this minimal example using `docker run`:

```shell
docker run -d --name servicepulse -p 9090:9090 \
    -e SERVICECONTROL_URL="http://servicecontrol:33333" \
    -e MONITORING_URL="http://servicecontrol-monitoring:33633" \
    particular/servicepulse:latest
```

## Reverse proxy

The ServicePulse container image includes a reverse proxy feature that allows ServicePulse to act as a single ingress/egress point for a system containing ServiceControl containers. This allows existing container hosting infrastructure to layer additional features onto the ServicePulse ingress point, such as SSL or authentication. It is enabled by default, but [it can be disabled](#settings-enable-reverse-proxy).

## Ports

`9090` is the canonical port exposed by ServicePulse within the container, though this port can be mapped to any desired external port.

## Volumes

ServicePulse is stateless and does not require any mounted volumes.

## Settings

The settings on this page are unique to the container deployment of ServicePulse.

### ServiceControl URL

The ServiceControl URL points to the ServiceControl (Error) instance URL. ServicePulse requests to `/api/*` will be proxied to this URL in order to fetch ServiceControl data used by ServicePulse.

| | |
|-|-|
| **Environment variable** | `SERVICECONTROL_URL` |
| **Type** | URL string |
| **Default** | `http://localhost:33333` |

### Monitoring URL

The Monitoring URL points to the ServiceControl Monitoring instance URL. ServicePulse requests to `/monitoring-api/*` will be proxied to this URL in order to fetch monitoring data used by ServicePulse.

| | |
|-|-|
| **Environment variable** | `MONITORING_URL` |
| **Type** | URL string |
| **Default** | `http://localhost:33633` |

### Monitoring URL array

> [!NOTE]
> This setting is deprecated. The [Monitoring URL setting](#settings-monitoring-url) (singular) is preferred and will be used if present.

A JSON array of URLs to monitoring instances.

| | |
|-|-|
| **Environment variable** | `MONITORING_URLS` |
| **Type** | JSON Array of URLs |
| **Default** | `['http://localhost:33633']` |

### Default route

The default page that should be displayed when visiting the ServicePulse site.

| | |
|-|-|
| **Environment variable** | `DEFAULT_ROUTE` |
| **Type** | string |
| **Default** | `/dashboard` |

### Show pending retries

Set to `true` to show details of pending retries.

| | |
|-|-|
| **Environment variable** | `SHOW_PENDING_RETRY` |
| **Type** | bool |
| **Default** | `false` |

### Enable reverse proxy

Controls whether the proxy that forwards requests to the ServiceControl and Monitoring instances is enabled or not. Set to `false` to disable.

_Added in version 1.44.0_

| | |
|-|-|
| **Environment variable** | `ENABLE_REVERSE_PROXY` |
| **Type** | bool |
| **Default** | `true` |
