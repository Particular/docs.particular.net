---
title: Managing ServiceControl RavenDB instances via Containers
reviewed: 2024-06-11
component: ServiceControl
---

When ServiceControl is hosted in containers, the [`particular/servicecontrol-ravendb` image](https://hub.docker.com/r/particular/servicecontrol-ravendb) provides the database storage to the Error and Audit instances.

The database container extends the [official RavenDB container](https://hub.docker.com/r/ravendb/ravendb) and is provided to easy version parity with ServiceControl instances. In other words, for any version `x.y.z` version of ServiceControl, the same version `x.y.z` of the database container should be used to ensure data storage compatibility.

> [!WARNING]
> A single database container should not be shared between multiple ServiceControl instances in production scenarios.

## Basic usage

This minimal example creates a database container using `docker run`:

```shell
docker run -d --name servicecontrol-db \
    -v <DATA_DIRECTORY>:/opt/RavenDB/Server/RavenData \
    particular/servicecontrol-ravendb:latest
```

Once the database container is running, the connection string `http://servicecontrol-db:8080` can be used for the `RAVENDB_CONNECTIONSTRING` value for an [Error instance](/servicecontrol/servicecontrol-instances/deployment/containers.md) or [Audit instance](/servicecontrol/audit-instances/deployment/containers.md).

## Required settings

A volume must be mounted to `/opt/RavenDB/Server/RavenData` to provide persistent storage for database contents between container updates. Failure to specify a path for the volume will result in loss of all data when the container is removed.

## Additional settings

As the ServiceControl RavenDB container extends the official RavenDB container, additional configuration details can be found in the RavenDB Docker container documentation, according to this version map:

| ServiceControl Versions | RavenDB Version | Container Documentation |
|:-:|:-:|:-:|
| 5.4 and higher | 5.4 | [RavenDB 5.4 container docs](https://ravendb.net/docs/article-page/5.4/csharp/start/installation/running-in-docker-container) |

> [!NOTE]
> The [RavenDB container overview on DockerHub](https://hub.docker.com/r/ravendb/ravendb) is specific to the most recent version of RavenDB which may not match the version used by ServiceControl.
