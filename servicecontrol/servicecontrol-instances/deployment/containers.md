---
title: Deploying ServiceControl Error instances using Containers
summary: A guide to setting up and deploying ServiceControl Error instances using Containers
reviewed: 2026-08-07
component: ServiceControl
versions: '[5.3, )'
redirects:
  - servicecontrol/containerization
  - samples/platformtools-docker-compose
---

ServiceControl Error instances are deployed using the [`particular/servicecontrol` image](https://hub.docker.com/r/particular/servicecontrol), as shown in this minimal example using `docker run`, assuming a RabbitMQ container named `rabbitmq`:

```shell
docker run -d --name error -p 33333:33333 \
    -e TRANSPORTTYPE=RabbitMQ.QuorumConventionalRouting \
    -e CONNECTIONSTRING="host=rabbitmq" \
    -e RAVENDB_CONNECTIONSTRING="http://servicecontrol-db:8080" \
    -e REMOTEINSTANCES='[{"api_uri":"http://audit:44444/api"}]' \
    -e ENABLEINTEGRATEDSERVICEPULSE="true" \
    particular/servicecontrol:latest
```

include: platform-container-examples

## Initial setup

include: servicecontrol-container-setup-explanation

## Required settings

The following environment settings are required to run a ServiceControl error instance.

include: servicecontrol-container-transport
include: servicecontrol-container-ravenconnectionstring

### Remote instances

_Environment variable:_ `REMOTEINSTANCES`

A JSON structure that provides URLs for the Error instance to access any [remote audit instances](/servicecontrol/servicecontrol-instances/remotes.md). When requesting audit data via the ServiceControl API, the Error instance will communicate with each remote audit instance in a scatter-gather pattern, then return the combined results. The URLs must be accessible directly by the Error instance, not constructed to be accessible from an external browser.

### Enable integrated ServicePulse

_Environment variable:_ `ENABLEINTEGRATEDSERVICEPULSE`

A boolean value specifying whether to enable the [integrated ServicePulse](/servicecontrol/servicecontrol-instances/integrated-servicepulse.md) for this Error instance.

include: servicecontrol-container-license

## Ports

`33333` is the canonical port exposed by the error instance API within the container, though this port can be mapped to any desired external port.

## Volumes

The Error instance is stateless and does not require any mounted volumes.

## Additional settings

Additional optional settings are documented in [Error Instance Configuration Settings](/servicecontrol/servicecontrol-instances/configuration.md), which describes all available settings, allowed values, and the environment variable keys used to configure the container.

When using tools such as Docker Compose that share environment information across many containers, the `SERVICECONTROL_` prefix can be dropped from an environment variable name, and the value will still be understood by the container. This facilitates sharing values such as `TRANSPORTTYPE` when all instances will be configured with the same values.

In the event of a naming collision, a fully qualified key such as `SERVICECONTROL_TRANSPORTTYPE` will be preferred over the shared `TRANSPORTTYPE` variant.

include: servicecontrol-container-settings-caveat

## Upgrading

include: servicecontrol-container-upgrading-explanation
