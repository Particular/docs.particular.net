---
title: Deploying ServiceControl Error instances using Containers
summary: A guide to setting up and deploying ServiceControl Error instances using Containers
reviewed: 2024-07-08
component: ServiceControl
versions: '[5.3, )'
redirects:
  - servicecontrol/containerization
  - samples/platformtools-docker-compose
---

ServiceControl Error instances are deployed using the [`particular/servicecontrol` image](https://hub.docker.com/r/particular/servicecontrol), as shown in this minimal example using `docker run`, assuming a RabbitMQ container named `rabbitmq`:

```shell
docker run -d --name servicecontrol -p 33333:33333 \
    -e TRANSPORTTYPE=RabbitMQ.QuorumConventionalRouting \
    -e CONNECTIONSTRING="host=rabbitmq" \
    -e RAVENDB_CONNECTIONSTRING="http://servicecontrol-db:8080" \
    -e REMOTEINSTANCES='[{"api_uri":"http://audit:44444/api"}]' \
    particular/servicecontrol:latest
```

include: platform-container-examples

## Initial setup

Before running the container image normally, it must run in setup mode to create the required message queues and perform upgrade tasks.

The container image will run in setup mode by adding the `--setup` argument. For example:

```shell
# Using docker run
docker run --rm {OPTIONS} particular/servicecontrol --setup
```

Setup mode may require different settings, such as a different transport connection string with permissions to create queues.

After setup is complete, the container will exit, and the `--rm` (or equivalent) option may be used to automatically remove the container.

The setup process should be repeated any time the container is [updated to a new version](#upgrading).

### Simplified setup

Instead of running `--setup` as a separate container, the setup and run operations can be combined using the `--setup-and-run` argument:

```shell
# Using docker run
docker run {OPTIONS} particular/servicecontrol --setup-and-run
```

The `--setup-and-run` argument will run the setup process when the container is run, after which the application will run normally. This simplifies deployment by removing the need for a separate init container in environments where the setup process does not need different settings.

Using `--setup-and-run` removes the need to repeat a setup process when the container is updated to a new version.

## Required settings

The following environment settings are required to run a ServiceControl error instance.

include: servicecontrol-container-transport
include: servicecontrol-container-ravenconnectionstring

### Remote instances

_Environment variable:_ `REMOTEINSTANCES`

A JSON structure that provides URLs for the Error instance to access any [remote audit instances](/servicecontrol/servicecontrol-instances/remotes.md). When requesting audit data via the ServiceControl API, the Error instance will communicate to each of the remote audit instances in a scatter-gather pattern and then return the combined results. The URLs must be accessible by the Error instance directly, not constructed to be accessible from an external browser.

include: servicecontrol-container-license

## Ports

`33333` is the canonical port exposed by the error instance API within the container, though this port can be mapped to any desired external port.

## Volumes

The Error instance is stateless and does not require any mounted volumes.

## Additional settings

Additional optional settings are documented in [Error Instance Configuration Settings](/servicecontrol/servicecontrol-instances/configuration.md) which describes all available settings, allowed values, and the environment variable keys used to configure the container.

When using tools such as Docker Compose that can share environment information between many containers, the prefix `SERVICECONTROL_` can be dropped from an environment variable name, and the value will still be understood by the container. This facilitates sharing values such as `TRANSPORTTYPE` when all instances will be configured with the same values.

In the event of a naming collision, a fully qualified key such as `SERVICECONTROL_TRANSPORTTYPE` will be preferred over the shared `TRANSPORTTYPE` variant.

Not all settings are relevant to error instances running in a container. For example, HTTP hostname and port use standard values inside the container, and mapped to real hosts and ports by infrastructure external to the container. Be sure to check the documentation for each configuration setting carefully to ensure it is relevant in a container context.

## Upgrading

An ServiceControl error instance is upgraded by removing the container for the old version and replacing it with a container built using the new version. However, the container should be run in [setup mode](#initial-setup) each time it is upgraded. For example:

```shell
docker stop error
docker rm error
docker pull particular/servicecontrol:latest
docker run -rm {OPTIONS} particular/servicecontrol:latest --setup
docker run -d {OPTIONS} particular/servicecontrol:latest
```

Note that Docker can cache the `latest` tag as well as the major/minor tags (such as `5` or `5.3`) unless the tag is pulled again. To be certain, use the full version tag.
