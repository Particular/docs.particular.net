---
title: Deploying ServiceControl Audit instances using containers
reviewed: 2026-11-02
component: ServiceControl
---

ServiceControl Audit instances are deployed using the [`particular/servicecontrol-audit` image](https://hub.docker.com/r/particular/servicecontrol-audit).

The following minimal example uses `docker run` to host an Audit instance on Azure, with [Azure Service Bus](/servicecontrol/transports.md#azure-service-bus) as the transport, Azure SQL Database for [SQL Server storage](/servicecontrol/storage/sqlserver/), and [Azure Blob Storage](/servicecontrol/storage/external-body/azure-blob.md) for message bodies:

```shell
docker run -d --name audit -p 44444:44444 \
    -e TRANSPORTTYPE=NetStandardAzureServiceBus \
    -e CONNECTIONSTRING="Endpoint=sb://my-namespace.servicebus.windows.net/;SharedAccessKeyName=...;SharedAccessKey=..." \
    -e STORAGETYPE=SQLServer \
    -e DATABASE_CONNECTIONSTRING="Server=tcp:my-server.database.windows.net,1433;Database=ServiceControl.Audit;User ID=...;Password=...;Encrypt=True" \
    -e MESSAGEBODY_STORAGETYPE=AzureBlob \
    -e MESSAGEBODY_AZURE_CONNECTIONSTRING="DefaultEndpointsProtocol=https;AccountName=mystorageaccount;AccountKey=...;EndpointSuffix=core.windows.net" \
    particular/servicecontrol-audit:latest
```

Each of these services can also use [managed identity](/servicecontrol/transports.md#azure-service-bus-enabling-managed-identity) instead of the keys and passwords shown; see [SQL Server authentication](/servicecontrol/storage/sqlserver/#authentication) and [Azure Blob Storage authentication](/servicecontrol/storage/external-body/azure-blob.md#authentication).

The `STORAGETYPE` environment variable is required. SQL Server and PostgreSQL storage also require an [external message body store](/servicecontrol/storage/external-body/), selected with `MESSAGEBODY_STORAGETYPE`: [file system](/servicecontrol/storage/external-body/file.md), [Azure Blob Storage](/servicecontrol/storage/external-body/azure-blob.md), or [Amazon S3](/servicecontrol/storage/external-body/amazon-s3.md). For PostgreSQL, set `STORAGETYPE` to `PostgreSQL` and use a [PostgreSQL connection string](/servicecontrol/storage/postgresql/). For RavenDB, set `STORAGETYPE` to `RavenDB` and replace the database and message body variables with `RAVENDB_CONNECTIONSTRING`, as described in [RavenDB container setup](/servicecontrol/storage/ravendb/containers.md). See [Storage selection](#required-settings-storage-selection).

SQL Server and PostgreSQL storage, and the `STORAGETYPE` setting, are available from ServiceControl 7. Earlier versions support only RavenDB storage: leave out `STORAGETYPE` and the database and message body variables, and set `RAVENDB_CONNECTIONSTRING` instead.

include: platform-container-examples

## Initial setup

Before running the container image normally, it must run in setup mode to create the required message queues and perform upgrade tasks.

The container image will run in setup mode by adding the `--setup` argument. For example:

```shell
# Using docker run
docker run --rm {OPTIONS} particular/servicecontrol-audit --setup
```

Setup mode may require different settings, such as a different transport connection string with permissions to create queues.

After setup is complete, the container will exit, and the `--rm` (or equivalent) option can be used to automatically remove it.

The setup process should be repeated any time the container is [updated to a new version](#upgrading).

### Simplified setup

Instead of running `--setup` as a separate container, the setup and run operations can be combined using the `--setup-and-run` argument:

```shell
# Using docker run
docker run {OPTIONS} particular/servicecontrol-audit --setup-and-run
```

The `--setup-and-run` argument runs the setup process when the container starts, after which the application runs normally. This simplifies deployment by removing the need for a separate init container in environments where the setup process does not need different settings.

Using `--setup-and-run` removes the need to repeat a setup process when the container is updated to a new version.

### Skipping queue creation

include: servicecontrol-container-skip-queue-creation

```shell
# Using docker run
docker run --rm {OPTIONS} particular/servicecontrol-audit --setup --skip-queue-creation
```

## Required settings

The following environment settings are required to run a ServiceControl audit instance.

include: servicecontrol-container-transport
include: servicecontrol-container-storage
include: servicecontrol-container-license

## Ports

`44444` is the canonical port exposed by the audit instance API within the container, though this port can be mapped to any desired external port.

## Health checks

The container image has a health check that calls `/api/configuration`. This endpoint allows anonymous requests, also when authentication is enabled.

Use this endpoint for health probes, for example Kubernetes liveness probes or load balancer health checks. A health probe only checks if the instance is available. Do not use other API endpoints for health probes: the API is for ServicePulse only, and the other endpoints require a valid token when [authentication](/servicecontrol/security/configuration/authentication.md) is enabled.

## Volumes

The Audit instance container keeps no data of its own. With RavenDB storage, the data is in the RavenDB database. With SQL Server or PostgreSQL storage, the data is in the database and in the message body store. A mounted volume is needed only in these cases:

- **File system message body storage:** mount a durable volume at the path set by `SERVICECONTROL_AUDIT_MESSAGEBODY_FILESYSTEM_STORAGEPATH`. Do not share the volume path with another instance, unless the instances [share a database](/servicecontrol/audit-instances/scaling-out-ingestion.md#sql-storage-shared-database), in which case they must share the volume. See the [file system message body storage guide](/servicecontrol/storage/external-body/file.md). Azure Blob Storage and Amazon S3 need no volume.
- **RavenDB client certificate file:** to connect to RavenDB with a client certificate stored in a file, mount the file and point `RAVENDB_CLIENTCERTIFICATEPATH` to it. A certificate supplied as a Base64-encoded environment variable needs no volume. See [RavenDB client certificate](/servicecontrol/storage/ravendb/containers.md#additional-settings-ravendb-client-certificate).

> [!WARNING]
> The container runs as a non-root user with UID `1654`, which must be able to write to the body storage volume. Setup succeeds even when the directory is not writable, and the instance then fails to start up. Give the volume suitable ownership or permissions before starting the instance.

## Additional settings

Additional optional settings are documented in [Audit Instance Configuration Settings](/servicecontrol/audit-instances/configuration.md), which describes all available settings, allowed values, and the environment variable keys used to configure the container.

When using tools such as Docker Compose that can share environment information across many containers, the `SERVICECONTROL_AUDIT_` prefix can be dropped from an environment variable name, and the value will still be understood by the container. This facilitates sharing values such as `TRANSPORTTYPE` when all instances will be configured with the same values.

In the event of a naming collision, a fully qualified key such as `SERVICECONTROL_AUDIT_TRANSPORTTYPE` will be preferred over the shared `TRANSPORTTYPE` variant.

Not all settings are relevant to audit instances running in a container. For example, HTTP hostname and port use standard values inside the container, and are mapped to real hosts and ports by infrastructure external to the container. Be sure to carefully review the documentation for each configuration setting to ensure it is relevant in a container context.

## Upgrading

A ServiceControl audit instance is upgraded by removing the container for the old version and replacing it with a container built using the new version. However, the container should be run in [setup mode](#initial-setup) each time it is upgraded. For example:

```shell
docker stop audit
docker rm audit
docker pull particular/servicecontrol-audit:latest
docker run --rm {OPTIONS} particular/servicecontrol-audit:latest --setup
docker run -d {OPTIONS} particular/servicecontrol-audit:latest
```

Note that Docker can cache the `latest` tag as well as the major/minor tags (such as `5` or `5.3`) unless the tag is pulled again. To be certain, use the full version tag.
