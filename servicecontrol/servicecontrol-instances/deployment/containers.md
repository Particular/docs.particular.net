---
title: Deploying ServiceControl Error instances using Containers
summary: A guide to setting up and deploying ServiceControl Error instances using Containers
reviewed: 2026-11-02
component: ServiceControl
redirects:
  - servicecontrol/containerization
  - samples/platformtools-docker-compose
---

ServiceControl Error instances are deployed using the [`particular/servicecontrol` image](https://hub.docker.com/r/particular/servicecontrol).

The following minimal example uses `docker run` to host an Error instance on AWS, with [Amazon SQS](/servicecontrol/transports.md#amazon-sqs) as the transport, Amazon Aurora PostgreSQL for [PostgreSQL storage](/servicecontrol/storage/postgresql/), and [Amazon S3](/servicecontrol/storage/external-body/amazon-s3.md) for message bodies:

```shell
docker run -d --name error -p 33333:33333 \
    -e AWS_REGION=us-east-1 \
    -e TRANSPORTTYPE=AmazonSQS \
    -e CONNECTIONSTRING="Region=us-east-1" \
    -e STORAGETYPE=PostgreSQL \
    -e DATABASE_CONNECTIONSTRING="Host=my-cluster.cluster-xxxxxxxxxxxx.us-east-1.rds.amazonaws.com;Database=ServiceControl;Username=servicecontrol;Token Authentication=AwsRdsIam;SSL Mode=Require" \
    -e MESSAGEBODY_STORAGETYPE=S3 \
    -e MESSAGEBODY_S3_BUCKETNAME=my-servicecontrol-bodies \
    -e REMOTEINSTANCES='[{"api_uri":"http://audit:44444/api"}]' \
    -e ENABLEINTEGRATEDSERVICEPULSE="true" \
    particular/servicecontrol:latest
```

The example contains no AWS keys or database password. SQS, S3, and the [IAM database login](/servicecontrol/storage/postgresql/#authentication-token-authentication) use the AWS SDK default credential chain, such as an IAM role attached to the host or task, and the region set in `AWS_REGION`.

This example shows one combination of transport, storage, and message body store. [Required settings](#required-settings) lists the variables for each storage type.

include: platform-container-examples

## Initial setup

include: servicecontrol-container-setup-explanation

Do not use `--setup` or `--setup-and-run` with an [ingestion-only worker](/servicecontrol/servicecontrol-instances/scaling-out-ingestion.md). Only the Error instance runs setup.

### Skipping queue creation

include: servicecontrol-container-skip-queue-creation

```shell
# Using docker run
docker run --rm {OPTIONS} particular/servicecontrol --setup --skip-queue-creation
```

## Required settings

The following environment settings are required to run a ServiceControl error instance.

include: servicecontrol-container-transport
include: servicecontrol-container-storage

### Remote instances

_Environment variable:_ `REMOTEINSTANCES`

A JSON structure that provides URLs for the Error instance to access any [remote audit instances](/servicecontrol/servicecontrol-instances/remotes.md). When requesting audit data via the ServiceControl API, the Error instance will communicate with each remote audit instance in a scatter-gather pattern, then return the combined results. The URLs must be accessible directly by the Error instance, not constructed to be accessible from an external browser.

### Enable integrated ServicePulse

_Environment variable:_ `ENABLEINTEGRATEDSERVICEPULSE`

A boolean value specifying whether to enable the [integrated ServicePulse](/servicecontrol/servicecontrol-instances/integrated-servicepulse.md) for this Error instance.

include: servicecontrol-container-license

## Ports

`33333` is the canonical port exposed by the error instance API within the container, though this port can be mapped to any desired external port.

## Health checks

The container image has a health check that calls `/health`. This endpoint allows anonymous requests, also when authentication is enabled. It reports if the process is running.

`/health/ready` also reports if the instance ingests failed messages.

Use these endpoints for health probes: `/health` for liveness probes, and `/health/ready` for readiness probes and load balancers. Do not restart the container because `/health/ready` fails. A health probe only checks if the instance is available. Do not use other API endpoints for health probes: the API is for ServicePulse only, and the other endpoints require a valid token when [authentication](/servicecontrol/security/configuration/authentication.md) is enabled.

Both endpoints are available from version 6.20.

## Volumes

The Error instance container keeps no data of its own. Whether it needs a mounted volume depends on the storage type:

- **SQL Server or PostgreSQL storage with file system message body storage:** mount a durable volume at the path set by `SERVICECONTROL_MESSAGEBODY_FILESYSTEM_STORAGEPATH`. Do not share the volume path with another instance. The exception is [ingestion-only workers](/servicecontrol/servicecontrol-instances/scaling-out-ingestion.md), which mount the same volume as their Error instance. See the [file system message body storage guide](/servicecontrol/storage/external-body/file.md).
- **SQL Server or PostgreSQL storage with Azure Blob Storage or Amazon S3:** no volume is needed. The data is in the database and in the message body store.
- **RavenDB storage:** no volume is needed for the data, which is in the RavenDB database. To connect to RavenDB with a client certificate stored in a file, mount the file and point `RAVENDB_CLIENTCERTIFICATEPATH` to it. A certificate supplied as a Base64-encoded environment variable needs no volume. See [RavenDB client certificate](/servicecontrol/storage/ravendb/containers.md#additional-settings-ravendb-client-certificate).

> [!WARNING]
> The container runs as a non-root user with UID `1654`, which must be able to write to the body storage volume. Setup succeeds even when the directory is not writable, and the instance then fails to start up. Give the volume suitable ownership or permissions before starting the instance.

## Additional settings

Additional optional settings are documented in [Error Instance Configuration Settings](/servicecontrol/servicecontrol-instances/configuration.md), which describes all available settings, allowed values, and the environment variable keys used to configure the container.

When using tools such as Docker Compose that share environment information across many containers, the `SERVICECONTROL_` prefix can be dropped from an environment variable name, and the value will still be understood by the container. This facilitates sharing values such as `TRANSPORTTYPE` when all instances will be configured with the same values.

In the event of a naming collision, a fully qualified key such as `SERVICECONTROL_TRANSPORTTYPE` will be preferred over the shared `TRANSPORTTYPE` variant.

include: servicecontrol-container-settings-caveat

## Upgrading

include: servicecontrol-container-upgrading-explanation
