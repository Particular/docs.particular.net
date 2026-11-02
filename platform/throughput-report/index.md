---
title: Usage Report Requirements
summary: Minimal setup required for generating a usage report for licensing of the Particular Service Platform.
reviewed: 2026-10-27
related:
  - nservicebus/licensing
  - servicepulse/usage-reporting-with-servicepulse
  - servicepulse/usage-reporting-setup
---

The Particular Service Platform is licensed based on the number of NServiceBus endpoints and the message throughput of those endpoints. In order to determine these values, a process needs to be run that queries the transport broker for up to 24 hours, and a report generated from this data to be sent to Particular.

The minimal installation required to generate the usage report is a single [ServiceControl error instance](/servicecontrol/servicecontrol-instances/) running ServicePulse in [integrated mode](/servicecontrol/servicecontrol-instances/integrated-servicepulse.md):

- The error instance contains the broker querying logic.
- ServicePulse is the UI used to specify which endpoints are NServiceBus related and to generate the usage report to send to Particular. In integrated mode, ServicePulse runs inside the error instance, so no separate installation is required. Integrated mode is available in ServiceControl 6.13 and later.

Certain transports [cannot query the broker directly](/servicepulse/usage-reporting-setup.md#audit-and-monitoring-data); for these an audit or monitoring instance will also be required and all endpoints will need to be configured to send data to these instances.

## Installation options

> [!WARNING]
> By default, the ServiceControl instance will immediately start reading from the `error` queue when it starts. If this is not desired, e.g. if you have another process that monitors the error queue, then set the [`SERVICECONTROL_INGESTERRORMESSAGES`](/servicecontrol/servicecontrol-instances/configuration.md#recoverability-servicecontrolingesterrormessages) environment variable to `false` in any of the following installation options. This variable needs to be set for the account under which the ServiceControl instance is running or on the container.

ServiceControl can be installed in the following ways, each running ServicePulse in integrated mode:

- [Containers](#container-installation), on Linux, Windows, or in cloud environments
- [Windows](#windows-installation), using the ServiceControl Management Utility or PowerShell

> [!NOTE]
> When installing ServiceControl, a connection string to a [transport](/transports/) is required. Since this may differ in format from the native connection string for the underlying queuing technology, please check the associated `Configuration` or `Connection Settings` page for your selected transport.
>
> - [Azure Service Bus](/transports/azure-service-bus/configuration.md)
> - [Azure Storage Queues](/transports/azure-storage-queues/configuration.md)
> - [Amazon SQS](/transports/sqs/configuration-options.md)
> - [RabbitMQ](/transports/rabbitmq/connection-settings.md)
> - [SQL Server](/transports/sql/connection-settings.md)
> - [PostgreSQL](/transports/postgresql/connection-settings.md)

## Container Installation

Run a [ServiceControl container](/servicecontrol/servicecontrol-instances/deployment/containers.md) with the [`ENABLEINTEGRATEDSERVICEPULSE`](/servicecontrol/servicecontrol-instances/deployment/containers.md#required-settings-enable-integrated-servicepulse) environment variable set to `true`, and provide a database for its data.

### Storage

From ServiceControl 7, the ServiceControl container must select its storage with the [`STORAGETYPE`](/servicecontrol/servicecontrol-instances/deployment/containers.md#required-settings-storage-selection) environment variable; there is no default. Versions before 7 support only RavenDB.

- **SQL Server or PostgreSQL:** Set `STORAGETYPE` to `SQLServer` or `PostgreSQL` and point ServiceControl at an existing database, such as a managed SQL Server or PostgreSQL service from the cloud provider. These options also need an [external message body store](/servicecontrol/storage/external-body/), such as [Azure Blob Storage](/servicecontrol/storage/external-body/azure-blob.md) or [Amazon S3](/servicecontrol/storage/external-body/amazon-s3.md). See [SQL Server storage](/servicecontrol/storage/sqlserver/) or [PostgreSQL storage](/servicecontrol/storage/postgresql/).
- **RavenDB:** Set `STORAGETYPE` to `RavenDB` and run a [RavenDB container](/servicecontrol/storage/ravendb/containers.md), or supply the [connection string](/servicecontrol/storage/ravendb/containers.md#basic-usage-ravendb-connection-string) of another RavenDB server. Examples are available in the [Platform Container Examples repository](https://github.com/Particular/PlatformContainerExamples), including [Kubernetes manifests](https://github.com/Particular/PlatformContainerExamples/blob/main/helm/README.md#ravendb-deployment) for AKS or EKS. Where RavenDB's [storage requirements](https://ravendb.net/docs/article-page/6.2/csharp/start/installation/deployment-considerations#storage-considerations) cannot be met, use [RavenDB Cloud](https://ravendb.net/cloud) to host the database.

## Windows Installation

ServiceControl is installed as a Windows service and starts automatically. From ServiceControl 7, choose a storage type when creating the instance. RavenDB runs embedded in ServiceControl, so it needs no separate database server, while [SQL Server](/servicecontrol/storage/sqlserver/) and [PostgreSQL](/servicecontrol/storage/postgresql/) use an existing database. Versions before 7 always use embedded RavenDB.

### ServiceControl Management Utility (SCMU)

- [Download](https://particular.net/start-servicecontrol-download) the latest SCMU
- Run the executable. This will require Admin privileges.
- Add a new ServiceControl instance
    ![Add new ServiceControl Instance](scmu-1.png 'width=500')
- Choose your transport and supply the connection string
    ![Choose transport](scmu-3.png 'width=500')
- From ServiceControl 7, choose a **STORAGE TYPE**
- Leave **ENABLE INTEGRATED SERVICEPULSE** set to `On`
- Click 'Add'

Once the instance is installed and running, SCMU shows a link to launch ServicePulse.
![ServicePulse link](scmu-5.png 'width=500')

When upgrading an existing instance from an older version of ServiceControl, click the `Enable Integrated ServicePulse` option.
![Enable integrated ServicePulse](scmu-6.png 'width=500')

### PowerShell

- Ensure you meet the [prerequisites](/servicecontrol/servicecontrol-instances/deployment/powershell.md#prerequisites)
- Install and import the [Particular.ServiceControl.Management module](/servicecontrol/servicecontrol-instances/deployment/powershell.md#installing-and-using-the-powershell-module)
- Run the [New-ServiceControlInstance cmdlet](/servicecontrol/servicecontrol-instances/deployment/powershell.md#error-instance-cmdlets-and-aliases-deploying-an-error-instance)
  - Most parameters can remain as shown in the example, which includes `-EnableIntegratedServicePulse`, with the `-Transport` and `-ConnectionString` values set according to your environment. From ServiceControl 7, `-StorageType` is also required.
