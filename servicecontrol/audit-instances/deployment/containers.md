---
title: Deploying ServiceControl Audit instances using containers
reviewed: 2026-10-07
component: ServiceControl
versions: '[5.3, )'
---

ServiceControl Audit instances are deployed using the [`particular/servicecontrol-audit` image](https://hub.docker.com/r/particular/servicecontrol-audit), as shown in this minimal example using `docker run`, assuming a RabbitMQ container named `rabbitmq`:

```shell
docker run -d --name audit -p 44444:44444 \
    -e TRANSPORTTYPE=RabbitMQ.QuorumConventionalRouting \
    -e CONNECTIONSTRING="host=rabbitmq" \
    -e RAVENDB_CONNECTIONSTRING="http://audit-db:8080" \
    particular/servicecontrol-audit:latest
```

include: platform-container-examples

## Initial setup

include: servicecontrol-container-setup-explanation

### Skipping queue creation

include: servicecontrol-container-skip-queue-creation

```shell
# Using docker run
docker run --rm {OPTIONS} particular/servicecontrol-audit --setup --skip-queue-creation
```

## Required settings

The following environment settings are required to run a ServiceControl audit instance.

include: servicecontrol-container-transport
include: servicecontrol-container-ravenconnectionstring
include: servicecontrol-container-license

## Ports

`44444` is the canonical port exposed by the audit instance API within the container, though this port can be mapped to any desired external port.

## Health checks

The container image has a health check that calls `/api/configuration`. This endpoint allows anonymous requests, also when authentication is enabled.

Use this endpoint for health probes, for example Kubernetes liveness probes or load balancer health checks. A health probe only checks if the instance is available. Do not use other API endpoints for health probes: the API is for ServicePulse only, and the other endpoints require a valid token when [authentication](/servicecontrol/security/configuration/authentication.md) is enabled.

## Volumes

The Audit instance is stateless and does not require any mounted volumes.

## Additional settings

Additional optional settings are documented in [Audit Instance Configuration Settings](/servicecontrol/audit-instances/configuration.md), which describes all available settings, allowed values, and the environment variable keys used to configure the container.

When using tools such as Docker Compose that can share environment information across many containers, the `SERVICECONTROL_AUDIT_` prefix can be dropped from an environment variable name, and the value will still be understood by the container. This facilitates sharing values such as `TRANSPORTTYPE` when all instances will be configured with the same values.

In the event of a naming collision, a fully qualified key such as `SERVICECONTROL_AUDIT_TRANSPORTTYPE` will be preferred over the shared `TRANSPORTTYPE` variant.

include: servicecontrol-container-settings-caveat

## Upgrading

include: servicecontrol-container-upgrading-explanation
