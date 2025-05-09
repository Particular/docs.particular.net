---
title: Usage Report Requirements
summary: Minimal setup required for generating a usage report for licensing of the Particular Service Platform.
reviewed: 2025-05-08
related:
  - nservicebus/licensing
  - servicepulse/usage
---

The Particular Service Platform is licensed based on the number of NServiceBus endpoints and the message throughput of those endpoints. In order to determine these values, a monitor needs to be run over the transport broker for a representative period of time (24 hours) and a report generated from this data to be sent to Particular.

The minimal installation required to generate the usage report is:

- [ServiceControl](/servicecontrol/). Only a single [error instance](/servicecontrol/servicecontrol-instances/) is required, as this is the primary service that contains the monitoring logic. 
- [ServicePulse](/servicepulse/). This is the UI that interfaces with the ServiceControl service to allow users to specify which endpoints are NServiceBus related and generate the usage report to send to Particular.

The following methods can be used to install these requirements:

- Windows Only
  - ServiceControl through [ServiceControl Management Utility](#windows-installation-servicecontrol-servicecontrol-management-utility-scmu)
  - ServiceControl through [Powershell](#windows-installation-servicecontrol-powershell)
  - [ServicePulse](#windows-installation-servicepulse)
- Linux or Windows
  - ServiceControl and ServicePulse through [Containers](#container-installation)


## Windows Installation

### ServiceControl

ServiceControl is installed as a Windows service, and starts automatically. It includes an embedded RavenDB instance that stores all the data required for generating usage reports (and any other functions of ServiceControl that you may use).

#### ServiceControl Management Utility (SCMU)

- [Download](https://particular.net/start-servicecontrol-download) the latest SCMU 
- Run the executable. This will require Admin priveleges
- Add a new ServiceControl and Audit instance
![Add new ServiceControl and Audit Instance](scmu-1.png 'width=500')
- Uncheck the `ServiceControl Audit` node, since this isn't required for usage reports
![Uncheck Audit Instance](scmu-2.png 'width=500')
- Choose your transport and supply the connection string
![Choose transport](scmu-3.png 'width=500')
> [!WARNING]
> By default, the ServiceControl instance will immediately start reading from the `error` queue when it starts. If this is not desired, e.g. if you have another process that monitors the error queue, then expand the `ServiceControl` node and change the value of `Error Queue Name` to a non-existing queue name before clicking 'Add'
> ![Change Error Queue Name](scmu-4.png 'width=500')
> Alternatively, set the [`SERVICECONTROL_INGESTERRORMESSAGES`](/servicecontrol/servicecontrol-instances/configuration.md#recoverability-servicecontrolingesterrormessages) environment variable to `false` before clicking `Add`
- Click 'Add'

#### Powershell

- Ensure you meet the [prerequisites](/servicecontrol/servicecontrol-instances/deployment/powershell.md#prerequisites)
- Install and import the [Particular.ServiceControl.Management module](/servicecontrol/servicecontrol-instances/deployment/powershell.md#installing-and-using-the-powershell-module)
- Run the [New-ServiceControlInstance cmdlet](/servicecontrol/servicecontrol-instances/deployment/powershell.md#error-instance-cmdlets-and-aliases-deploying-an-error-instance)
  - Most of the parameters can be left as per the example, with the `-Transport` and `-ConnectionString` parameters set according to your environment.
> [!WARNING]
> By default, the ServiceControl instance will immediately start reading from the `error` queue when it starts. If this is not desired, e.g. if you have another process that monitors the error queue, either set the `-ErrorQueue` parameter to a non-existing queue name, or set the [`SERVICECONTROL_INGESTERRORMESSAGES`](/servicecontrol/servicecontrol-instances/configuration.md#recoverability-servicecontrolingesterrormessages) environment variable to `false` before running the cmdlet

### ServicePulse

Follow the [installation instructions](/servicepulse/installation.md) and ensure ServicePulse is [configured](/servicepulse/host-config.md#configuring-connections-via-the-servicepulse-ui) to point to the port of the ServiceControl instance installed above.

## Container Installation

When installing ServiceControl directly, through one of the [above solutions](#windows-installation), it includes an embedded RavenDB instance that stores all the data required for generating usage reports (and any other functions of ServiceControl that you may use). When using containers, a separate RavenDB database is required. This can also be containerized, allowing for a single docker compose to install all dependencies. An example of this can be found in the [Platform Container Examples repository](https://github.com/Particular/PlatformContainerExamples).

The containers required for generating a usage report are:

- [RavenDB](/servicecontrol/ravendb/containers.md)
  - Alternatively another RavenDB source can be used. The [connection string](/servicecontrol/servicecontrol-instances/deployment/containers.md#required-settings-ravendb-connection-string) must be supplied when installing ServiceControl
- [ServiceControl](/servicecontrol/servicecontrol-instances/deployment/containers.md)
> [!WARNING]
> By default, the ServiceControl instance will immediately start reading from the `error` queue when it starts. If this is not desired, e.g. if you have another process that monitors the error queue, then one of the following parameters should be supplied to the `docker run` command
>  - -e SERVICEBUS_ERRORQUEUE=<<name of non-existing queue, e.g. `errornotused`>> 
>  - -e SERVICECONTROL_INGESTERRORMESSAGES=false
- [ServicePulse](/servicepulse/containerization/)

### Cloud

When hosting containers in the cloud, it is suggested to use [RavenDB Cloud](https://ravendb.net/cloud) to host the database. This is because the [storage requirements](https://ravendb.net/docs/article-page/6.2/csharp/start/installation/deployment-considerations#storage-considerations) of RavenDB generally cannot be met on most cloud offerings.