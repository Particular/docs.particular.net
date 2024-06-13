---
title: Deploying ServiceControl Audit instances using containers
reviewed: 2024-07-08
component: ServiceControl
versions: '[5.3, )'
---

ServiceControl Audit instances are deployed using the [`particular/servicecontrol-audit` image](https://hub.docker.com/r/particular/servicecontrol-audit), as shown in this minimal example using `docker run`:

```shell
docker run -d -p 44444:44444 \
    -e TRANSPORTTYPE=RabbitMQ.QuorumConventionalRouting \
    -e CONNECTIONSTRING="host=host.docker.internal" \
    -e RAVENDB_CONNECTIONSTRING="http://host.docker.internal:8080" \
    particular/servicecontrol-audit:latest
```
## Initial setup

Before running the container image normally, it must be run in setup mode to create the required message queues.

The container image will run in setup mode by adding the `--setup` argument. For example:

```shell
# Using docker run
docker run --rm {OPTIONS} particular/servicecontrol-audit --setup
```

Depending on the requirements of the message transport, setup mode may require different connection settings that have permissions to create queues, which are not necessary during non-setup runtime.

After setup is complete, the container will exit, and the `--rm` (or equivalent) option may be used to automatically remove the container.

The initial setup should be repeated any time the container is [updated to a new version](#upgrading).

## Required settings

The following environment settings are required to run a ServiceControl audit instance:

| Environment Variable | Description |
|-|-|
| `TRANSPORTTYPE` | Determines the message transport used to communicate with message endpoints. See [TODO]() for valid TransportType values. |
| `CONNECTIONSTRING` | Provides the connection information to connect to the chosen transport. The form of this connection string is different for every message transport. See [ServiceControl transport support](/servicecontrol/transports.md) for more details on options available to each message transport. |
| `RAVENDB_CONNECTIONSTRING` | Provides the URL to connect to the [database container](/servicecontrol/ravendb/deployment/containers.md) that stores the audit instance's data. The database container should be exclusive to the error instance, and not shared by any other ServiceControl instances. |
| `PARTICULARSOFTWARE_LICENSE` | The Particular Software license. The environment variable should contain the full multi-line contents of the license file. |

## Ports

`44444` is the canonical port exposed by the audit instance API within the container, though this port can be mapped to any desired external port.

## Volumes

The Audit instance is stateless and does not require any mounted volumes.

## Additional settings

Additional optional settings are documented in [Audit Instance Configuration Settings](/servicecontrol/audit-instances/configuration.md) which describes all available settings, allowed values, and the environment variable keys used to configure the container.

When using tools such as Docker Compose that can share environment information between many containers, the prefix `SERVICECONTROL_AUDIT_` can be dropped from an environment variable name, and the value will still be understood by the container. This facilitates sharing values such as `TRANSPORTTYPE` when all instances will be configured with the same values.

In the event of a naming collision, a fully qualified key such as `SERVICECONTROL_AUDIT_TRANSPORTTYPE` will be preferred over the shared `TRANSPORTTYPE` variant.

Not all settings are relevant to audit instances running in a container. For example, HTTP hostname and port use standard values inside the container, and mapped to real hosts and ports by infrastructure external to the container. Be sure to check the documentation for each configuration setting carefully to ensure it is relevant in a container context.

## Upgrading

An ServiceControl audit instance is upgraded by removing the container for the old version and replacing it with a container built using the new version. However, the container should be run in [setup mode](#initial-setup) each time it is upgraded. For example:

```shell
docker stop audit
docker rm audit
docker pull particular/servicecontrol-audit:latest
docker run -rm {OPTIONS} particular/servicecontrol-audit:latest --setup
docker run -d {OPTIONS} particular/servicecontrol-audit:latest
```

Note that Docker can cache the `latest` tag as well as the major/minor tags (such as `5` or `5.3`) unless the tag is pulled again. To be certain, use the full version tag.