---
title: Deploying ServiceControl Monitoring instances using containers
reviewed: 2024-07-08
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

Before running the container image normally, it must run in setup mode to create the required message queues and perform upgrade tasks.

The container image will run in setup mode by adding the `--setup` argument. For example:

```shell
# Using docker run
docker run --rm {OPTIONS} particular/servicecontrol-monitoring --setup
```

Setup mode may require different settings, such as a different transport connection string with permissions to create queues.

After setup is complete, the container will exit, and the `--rm` (or equivalent) option may be used to automatically remove the container.

The setup process should be repeated any time the container is [updated to a new version](#upgrading).

### Simplified setup

Instead of running `--setup` as a separate container, the setup and run operations can be combined using the `--setup-and-run` argument:

```shell
# Using docker run
docker run {OPTIONS} particular/servicecontrol-monitoring --setup-and-run
```

The `--setup-and-run` argument will run the setup process when the container is run, after which the application will run normally. This simplifies deployment by removing the need for a separate init container in environments where the setup process does not need different settings.

Using `--setup-and-run` removes the need to repeat a setup process when the container is updated to a new version.

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

Not all settings are relevant to audit instances running in a container. For example, HTTP hostname and port use standard values inside the container, and mapped to real hosts and ports by infrastructure external to the container. Be sure to check the documentation for each configuration setting carefully to ensure it is relevant in a container context.

## Upgrading

A ServiceControl monitoring instance is upgraded by removing the container for the old version and replacing it with a container built using the new version. However, the container should be run in [setup mode](#initial-setup) each time it is upgraded. For example:

```shell
docker stop monitoring
docker rm monitoring
docker pull particular/servicecontrol-monitoring:latest
docker run -rm {OPTIONS} particular/servicecontrol-monitoring:latest --setup
docker run -d {OPTIONS} particular/servicecontrol-monitoring:latest
```

Note that Docker can cache the `latest` tag as well as the major/minor tags (such as `5` or `5.3`) unless the tag is pulled again. To be certain, use the full version tag.