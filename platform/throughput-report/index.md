---
title: Usage Report Requirements
summary: Minimal setup required for generating a usage report for licensing of the Particular Service Platform.
reviewed: 2026-08-14
related:
  - nservicebus/licensing
  - servicepulse/usage
  - servicepulse/usage-config
---

The Particular Service Platform is licensed based on the number of NServiceBus endpoints and the message throughput of those endpoints. In order to determine these values, a process needs to be run that queries the transport broker for up to 24 hours and a report generated from this data to be sent to Particular.

The minimal installation required to generate the usage report is:

- [ServiceControl](/servicecontrol/). Only a single [error instance](/servicecontrol/servicecontrol-instances/) is required, as this is the primary service that contains the broker querying logic.
- [ServicePulse](/servicepulse/). This is the UI that interfaces with the ServiceControl service to allow users to specify which endpoints are NServiceBus related and generate the usage report to send to Particular.

> [!NOTE]
> ServicePulse can run in an [integrated mode](/servicecontrol/servicecontrol-instances/integrated-servicepulse.md) and be started from within ServiceControl, in which case no additional installation is required.
> This [can be enabled](/servicecontrol/servicecontrol-instances/configuration.md#host-settings-servicecontrolenableintegratedservicepulse) when installing via the ServiceControl Management Utility as well as when running via Containers

## Installation options

> [!WARNING]
> By default, the ServiceControl instance will immediately start reading from the `error` queue when it starts. If this is not desired, e.g. if you have another process that monitors the error queue, then set the [`SERVICECONTROL_INGESTERRORMESSAGES`](/servicecontrol/servicecontrol-instances/configuration.md#recoverability-servicecontrolingesterrormessages) environment variable to `false` in any of the following installation options. This variable needs to be set for the account under which the ServiceControl instance is running or on the container.

The following methods can be used to install these requirements:

- [Linux, Windows or cloud environments via Containers](#container-installation)
  - ServiceControl and ServicePulse through [Containers](#container-installation)
- [Windows Only](#windows-installation)
  - ServiceControl through [ServiceControl Management Utility](#windows-installation-servicecontrol-servicecontrol-management-utility-scmu)
  - ServiceControl through [Powershell](#windows-installation-servicecontrol-powershell)
  - ServicePulse running in [an integrated mode from within ServiceControl Management Utility](#windows-installation-servicepulse-integrated-into-servicecontrol-management-utility-scmu)
  - ServicePulse as a [stand alone Windows service](#windows-installation-servicepulse-stand-alone)

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

When installing ServiceControl directly, through one of the [Windows installation options below](#windows-installation), it includes an embedded RavenDB instance that stores all the data required for generating usage reports (and any other functions of ServiceControl that you may use). When using containers, a separate RavenDB database is required that can also be containerized. An example of this can be found in the [Platform Container Examples repository](https://github.com/Particular/PlatformContainerExamples).

The containers required for generating a usage report are:

- [RavenDB](/servicecontrol/ravendb/containers.md)
  - Alternatively another RavenDB source can be used. The [connection string](/servicecontrol/servicecontrol-instances/deployment/containers.md#required-settings-ravendb-connection-string) must be supplied when installing ServiceControl
- [ServiceControl](/servicecontrol/servicecontrol-instances/deployment/containers.md)
- [ServicePulse](/servicepulse/containerization/) (if **not** running in [integrated mode](/servicecontrol/servicecontrol-instances/configuration#host-settings-servicecontrolenableintegratedservicepulse))

### Cloud environments

When hosting containers in Kubernetes in any of the major Cloud providers, it is possible to host RavenDB in Kubernetes using the recommended storage providers by the Cloud infrastructure, see [these example manifests](https://github.com/Particular/PlatformContainerExamples/blob/main/helm/README.md#ravendb-deployment) for deployments in AKS or EKS.
In other hosting environments where the RavenDB [storage requirements](https://ravendb.net/docs/article-page/6.2/csharp/start/installation/deployment-considerations#storage-considerations) cannot be met, it is suggested to use [RavenDB Cloud](https://ravendb.net/cloud) to host the database.

## Windows Installation

### ServiceControl

ServiceControl is installed as a Windows service, and starts automatically. It includes an embedded RavenDB instance that stores all the data required for generating usage reports (and any other functions of ServiceControl that you may use).

#### ServiceControl Management Utility (SCMU)

- [Download](https://particular.net/start-servicecontrol-download) the latest SCMU
- Run the executable. This will require Admin privileges
- Add a new ServiceControl instance
    ![Add new ServiceControl Instance](scmu-1.png 'width=500')
- Uncheck the `ServiceControl Audit` node, since this isn't required for usage reports
    ![Uncheck Audit Instance](scmu-2.png 'width=500')
- Choose your transport and supply the connection string
    ![Choose transport](scmu-3.png 'width=500')
- Click 'Add'

#### Powershell

- Ensure you meet the [prerequisites](/servicecontrol/servicecontrol-instances/deployment/powershell.md#prerequisites)
- Install and import the [Particular.ServiceControl.Management module](/servicecontrol/servicecontrol-instances/deployment/powershell.md#installing-and-using-the-powershell-module)
- Run the [New-ServiceControlInstance cmdlet](/servicecontrol/servicecontrol-instances/deployment/powershell.md#error-instance-cmdlets-and-aliases-deploying-an-error-instance)
  - Most of the parameters can be left as per the example, with the `-Transport` and `-ConnectionString` parameters set according to your environment.

### ServicePulse

ServicePulse can be installed separately as a Windows service, or it can run in an [integrated mode](/servicecontrol/servicecontrol-instances/integrated-servicepulse.md) from within the ServiceControl Management Utility (SCMU).

#### Integrated into ServiceControl Management Utility (SCMU)

This is the easiest and recommended way to run ServicePulse for usage report purposes.

##### New installations

When installing a new instance of ServiceControl via SCMU, once the service is installed and running, a link to launch an integrated version of ServicePulse will be visible in SCMU.
![ServicePulse link](scmu-5.png 'width=500')

##### Upgrades

When upgrading from an older version of ServiceControl via SCMU, ensure you click the `Enable Integrated ServicePulse` option.
![Enable integrated ServicePulse](scmu-6.png 'width=500')

#### Stand alone

Follow the [installation instructions](/servicepulse/installation.md) and ensure ServicePulse is [configured](/servicepulse/host-config.md#configuring-connections-via-the-servicepulse-ui) to point to the port of the ServiceControl instance installed above.
