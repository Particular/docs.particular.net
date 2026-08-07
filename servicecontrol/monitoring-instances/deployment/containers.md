---
title: Deploying ServiceControl Monitoring instances using containers
reviewed: 2026-08-07
component: ServiceControl
versions: '[5.3, )'
---

ServiceControl Monitoring instances are deployed using the [`particular/servicecontrol-monitoring` image](https://hub.docker.com/r/particular/servicecontrol-monitoring), as shown in this minimal example using `docker run`, assuming a RabbitMQ container named `rabbitmq`:

```shell
docker run -d --name monitoring -p 33633:33633 \
    -e TRANSPORTTYPE=RabbitMQ.QuorumConventionalRouting \
    -e CONNECTIONSTRING="host=rabbitmq" \
    particular/servicecontrol-monitoring:latest
```

include: platform-container-examples

## Initial setup

include: servicecontrol-container-setup-explanation

## Required settings

The following environment settings are required to run a ServiceControl monitoring instance:

include: servicecontrol-container-transport
include: servicecontrol-container-license

## Ports

`33633` is the canonical port exposed by the monitoring instance API within the container, though this port can be mapped to any desired external port.

## Volumes

The monitoring instance is stateless and does not require any mounted volumes.

## Additional settings

Additional optional settings are documented in [Monitoring Instance Configuration Settings](/servicecontrol/monitoring-instances/configuration.md) which describes all available settings, allowed values, and the environment variable keys used to configure the container.

When using tools such as Docker Compose that can share environment information between many containers, the prefix `MONITORING_` can be dropped from an environment variable name, and the value will still be understood by the container. This facilitates sharing values such as `TRANSPORTTYPE` when all instances will be configured with the same values.

In the event of a naming collision, a fully qualified key such as `MONITORING_TRANSPORTTYPE` will be preferred over the shared `TRANSPORTTYPE` variant.

include: servicecontrol-container-settings-caveat

## Upgrading

include: servicecontrol-container-upgrading-explanation
