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

This example shows one combination of transport, storage, and message body store. [Required settings](#required-settings) lists the variables for each storage type.

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
include: servicecontrol-container-storage
include: servicecontrol-container-license

## Ports

`44444` is the canonical port exposed by the audit instance API within the container, though this port can be mapped to any desired external port.

## Health checks

The container image has a health check that calls `/api/configuration`. This endpoint allows anonymous requests, also when authentication is enabled.

Use this endpoint for health probes, for example Kubernetes liveness probes or load balancer health checks. A health probe only checks if the instance is available. Do not use other API endpoints for health probes: the API is for ServicePulse only, and the other endpoints require a valid token when [authentication](/servicecontrol/security/configuration/authentication.md) is enabled.

## Volumes

The Audit instance container keeps no data of its own. Whether it needs a mounted volume depends on the storage type:

- **SQL Server or PostgreSQL storage with file system message body storage:** mount a durable volume at the path set by `SERVICECONTROL_AUDIT_MESSAGEBODY_FILESYSTEM_STORAGEPATH`. Do not share the volume path with another instance, unless the instances [share a database](/servicecontrol/audit-instances/scaling-out-ingestion.md#sql-storage-shared-database), in which case they must share the volume. See the [file system message body storage guide](/servicecontrol/storage/external-body/file.md).
- **SQL Server or PostgreSQL storage with Azure Blob Storage or Amazon S3:** no volume is needed. The data is in the database and in the message body store.
- **RavenDB storage:** no volume is needed for the data, which is in the RavenDB database. To connect to RavenDB with a client certificate stored in a file, mount the file and point `RAVENDB_CLIENTCERTIFICATEPATH` to it. A certificate supplied as a Base64-encoded environment variable needs no volume. See [RavenDB client certificate](/servicecontrol/storage/ravendb/containers.md#additional-settings-ravendb-client-certificate).

> [!WARNING]
> The container runs as a non-root user with UID `1654`, which must be able to write to the body storage volume. Setup succeeds even when the directory is not writable, and the instance then fails to start up. Give the volume suitable ownership or permissions before starting the instance.

## Additional settings

Additional optional settings are documented in [Audit Instance Configuration Settings](/servicecontrol/audit-instances/configuration.md), which describes all available settings, allowed values, and the environment variable keys used to configure the container.

When using tools such as Docker Compose that can share environment information across many containers, the `SERVICECONTROL_AUDIT_` prefix can be dropped from an environment variable name, and the value will still be understood by the container. This facilitates sharing values such as `TRANSPORTTYPE` when all instances will be configured with the same values.

In the event of a naming collision, a fully qualified key such as `SERVICECONTROL_AUDIT_TRANSPORTTYPE` will be preferred over the shared `TRANSPORTTYPE` variant.

include: servicecontrol-container-settings-caveat

## Upgrading

include: servicecontrol-container-upgrading-explanation
