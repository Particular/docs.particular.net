---
title: Managing ServiceControl RavenDB instances via Containers
reviewed: 2026-10-27
component: ServiceControl
versions: '[5,)'
redirects:
  - servicecontrol/ravendb/containers
  - servciecontrol/ravendb/deployment/containers
---

When ServiceControl is hosted in containers, the [`particular/servicecontrol-ravendb` image](https://hub.docker.com/r/particular/servicecontrol-ravendb) provides the database storage to the Error and Audit instances.

The database container extends the [official RavenDB container](https://hub.docker.com/r/ravendb/ravendb) and is provided to ensure compatibility with ServiceControl instances. In other words, for any version `x.y.z` version of ServiceControl, the same version `x.y.z` of the database container should be used to ensure data storage compatibility.

> [!NOTE]
The NServiceBus license covers the license for the embedded RavenDB that ships with ServiceControl. A separate RavenDB license is not required in this case.

> [!WARNING]
> A single database container should not be shared between multiple ServiceControl instances in production scenarios.

## Basic usage

This minimal example creates a database container using `docker run`:

#if-version [5, 6)
```shell
docker run -d --name servicecontrol-db \
    -v db-config:/opt/RavenDB/config \
    -v db-data:/opt/RavenDB/Server/RavenData \
    particular/servicecontrol-ravendb:latest
```
#end-if
#if-version [6, )
```shell
docker run -d --name servicecontrol-db \
    -v db-config:/etc/ravendb \
    -v db-data:/var/lib/ravendb/data \
    particular/servicecontrol-ravendb:latest
```
#end-if

Once the database container is running, the connection string `http://servicecontrol-db:8080` can be used for the `RAVENDB_CONNECTIONSTRING` value for an [Error instance](/servicecontrol/servicecontrol-instances/deployment/containers.md) or [Audit instance](/servicecontrol/audit-instances/deployment/containers.md).

### RavenDB connection string

_Environment variable:_ `RAVENDB_CONNECTIONSTRING`

Set this variable on the Error or Audit container to the URL of its RavenDB server.

## Required settings

#if-version [5, 6)
A volume must be mounted to `/opt/RavenDB/Server/RavenData` to provide persistent storage for database contents between container updates. Failure to specify a path for the volume will result in loss of all data when the container is removed.
#end-if
#if-version [6,)
A volume must be mounted to `/var/lib/ravendb/data` to provide persistent storage for database contents between container updates. Failure to specify a path for the volume will result in loss of all data when the container is removed.
#end-if

> [!NOTE]
> RavenDB has specific requirements about the types of storage it supports. See the [RavenDB deployment storage considerations documentation](https://ravendb.net/docs/article-page/6.2/csharp/start/installation/deployment-considerations#storage-considerations) for more information.
> 
> If providing these storage requirements is not possible, an externally-hosted, separately-licensed RavenDB server can also be used starting with ServiceControl version 6.0. The external server must be using the same Major and Minor version noted in the version map below.

## Additional settings

As the ServiceControl RavenDB container extends the official RavenDB container, additional configuration details can be found in the RavenDB Docker container documentation, according to this version map:

| ServiceControl Versions | RavenDB Version | Container Documentation |
|:-:|:-:|:-:|
| 6.x | 6.2 | [RavenDB 6.2 container docs](https://ravendb.net/docs/article-page/6.2/csharp/start/containers/general-guide)
| 5.4 to 5.11 | 5.4 | [RavenDB 5.4 container docs](https://ravendb.net/docs/article-page/5.4/csharp/start/installation/running-in-docker-container) |

### RavenDB client certificate

_Added in ServiceControl version 6.3.0_

Configure the following settings on the Error or Audit container, not the RavenDB database container. When connecting to an external RavenDB instance, the client certificate settings are attempted in this order:

1. An environment variable `RAVENDB_CLIENTCERTIFICATEBASE64` can be used to supply the client certificate as a Base64-encoded string.
2. An environment variable `RAVENDB_CLIENTCERTIFICATEPATH` can be used to identify the local path to a certificate that has been added to the container via a mounted volume.
3. The app will attempt to load the certificate from `/app/raven-client-certificate.pfx`.
4. The app will attempt to access the database without a client certificate.

In any of the above cases, the certificate can be password-protected, in which case the password can be supplied using the `RAVENDB_CLIENTCERTIFICATEPASSWORD` environment variable.

> [!NOTE]
> The [RavenDB container overview on Docker Hub](https://hub.docker.com/r/ravendb/ravendb) is specific to the most recent version of RavenDB which may not match the version used by ServiceControl.

> [!NOTE]
> To host ServiceControl and ServicePulse in Azure Container Apps, use Service Control v6 with [RavenDB Cloud](https://ravendb.net/cloud) for hosting the database
