---
title: Managing ServiceControl RavenDB instances via Containers
reviewed: 2024-06-11
component: ServiceControl
versions: '[5,)'
redirects:
  - servciecontrol/ravendb/deployment/containers
---

When ServiceControl is hosted in containers, the [`particular/servicecontrol-ravendb` image](https://hub.docker.com/r/particular/servicecontrol-ravendb) provides the database storage to the Error and Audit instances.

The database container extends the [official RavenDB container](https://hub.docker.com/r/ravendb/ravendb) and is provided to easy version parity with ServiceControl instances. In other words, for any version `x.y.z` version of ServiceControl, the same version `x.y.z` of the database container should be used to ensure data storage compatibility.

> [!WARNING]
> A single database container should not be shared between multiple ServiceControl instances in production scenarios.

## Basic usage

This minimal example creates a database container using `docker run`:

#if-version [5, 6)
```shell
docker run -d --name servicecontrol-db \
    -v <DATA_DIRECTORY>:/opt/RavenDB/Server/RavenData \
    particular/servicecontrol-ravendb:latest
```
#end-if
#if-version [6, )
```shell
docker run -d --name servicecontrol-db \
    -v <DATA_DIRECTORY>:/var/lib/ravendb/data \
    particular/servicecontrol-ravendb:latest
```
#end-if

Once the database container is running, the connection string `http://servicecontrol-db:8080` can be used for the `RAVENDB_CONNECTIONSTRING` value for an [Error instance](/servicecontrol/servicecontrol-instances/deployment/containers.md) or [Audit instance](/servicecontrol/audit-instances/deployment/containers.md).

## Required settings

#if-version [5, 6)
A volume must be mounted to `/opt/RavenDB/Server/RavenData` to provide persistent storage for database contents between container updates. Failure to specify a path for the volume will result in loss of all data when the container is removed.
#end-if
#if-version [6,7)
A volume must be mounted to `/var/lib/ravendb/data` to provide persistent storage for database contents between container updates. Failure to specify a path for the volume will result in loss of all data when the container is removed.
#end-if

> [!NOTE]
> RavenDB has specific requirements about the types of storage it supports. See the [RavenDB container requirements documentation](https://ravendb.net/docs/article-page/6.2/csharp/start/installation/running-in-docker-container#requirements) for more information.

## Additional settings

As the ServiceControl RavenDB container extends the official RavenDB container, additional configuration details can be found in the RavenDB Docker container documentation, according to this version map:

| ServiceControl Versions | RavenDB Version | Container Documentation |
|:-:|:-:|:-:|
| 6.x | 6.2 | [RavenDB 6.2 container docs](https://ravendb.net/docs/article-page/6.2/csharp/start/installation/running-in-docker-container)
| 5.4 to 5.x | 5.4 | [RavenDB 5.4 container docs](https://ravendb.net/docs/article-page/5.4/csharp/start/installation/running-in-docker-container) |

> [!NOTE]
> The [RavenDB container overview on DockerHub](https://hub.docker.com/r/ravendb/ravendb) is specific to the most recent version of RavenDB which may not match the version used by ServiceControl.
