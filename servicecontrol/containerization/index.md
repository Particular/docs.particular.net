---
title: Running ServiceControl in containers
reviewed: 2020-12-10
---

Docker images for ServiceControl exist on Dockerhub under the [Particular organization](https://hub.docker.com/u/particular). These can be used to run ServiceControl in docker containers. These docker images are only available for Windows due to ServiceControl's current dependency on Windows ESENT storage.

NOTE: ServiceControl cannot perform a graceful shutdown which can result in manual intervention to repair the underlying datastore. The base Windows operating system images do not support notifying the application that it is being shut down when running `docker stop`. 

## Containers overview

ServiceControl is split into multiple docker images for each instance type. These are:

* The error instance
* The audit instance
* The monitoring instance

The images for each of these containers are further split into an init container and a runtime container per transport.

## Containerization support 

The following table mentions with containerization technology is currently supported for running a production system.

| Environment                             | Supported | Note                                                               |
|-----------------------------------------|-----------|--------------------------------------------------------------------|
| Docker for Windows                      | Yes       |                                                                    |
| Docker (for Linux)                      | No        | ServiceControl can not run in Linux.                           |
| Azure Container Services (ACS)          | No        | Works, but does not support persistent volumes for durable storage |
| Azure Kubernetes Services (AKS)         | No        | Supports minimum of Windows Server 2019-based images               |
| Amazon Elastic Container Services (ECS) | No        | Untested, guidance mentions AMIs available for Windows 2016        |
| Amazon Elastic Kubernetes Service (EKS) | No        | EKS only supports Windows 2019                                     |

## Init containers

The init containers are used to create or upgrade the infrastructure required for ServiceControl and are based on the [Kubernetes init containers](https://kubernetes.io/docs/concepts/workloads/pods/init-containers/). Once the init container has been run, it will shut down and the runtime container can be run. The runtime container will use the infrastructure that has been created and if the init container was not run the runtime container will fail.

The init containers are similar to [installing all ServiceControl service types (regular, error, and monitoring) via the ServiceControl Management Utility or the Powershell Cmdlets](/servicecontrol/installation.md). This includes creating the database as well as the queues that ServiceControl uses and runtime containers are similar to starting the corresponding Windows Services.

Init containers are identical to the corresponding runtime image but with an added entrypoint arguments (`--setup`). It is also possible to use the runtime image and override the entrypoint and add this argument to achieve the same goal.

## Transports

Each supported transport and topology for ServiceControl has further been broken down into it's own set of images. The following transport and topologies are available on dockerhub:

* [Amazon SQS](https://hub.docker.com/search?q=servicecontrol.amazonsqs&type=image)
* [Azure Service Bus](https://hub.docker.com/search?q=servicecontrol.azureservicebus&type=image)
* [Azure Storage Queues](https://hub.docker.com/search?q=servicecontrol.azurestorageque&type=image)
* [RabbitMQ Conventional Topology](https://hub.docker.com/search?q=servicecontrol.rabbitmq.conven&type=image)
* [RabbitMQ Direct Topology](https://hub.docker.com/search?q=servicecontrol.rabbitmq.direct&type=image)
* [Sql Server](https://hub.docker.com/search?q=servicecontrol.sqlserver&type=image)

## Parameters

All parameters that the ServiceControl instance supports can be provided by passing the parameter as an environment variable. E.g.

```cmd
-e "ServiceControl/Port=12345"
```

The most common parameters used are likely to be:

* ServiceControl/ConnectionString
* ServiceControl/LicenseText
* ServiceControl.Audit/ConnectionString
* ServiceControl.Audit/LicenseText
* ServiceControl.Audit/ServiceControlQueueAddress
* ServiceControl/RemoteInstances
* Monitoring/ConnectionString
* Monitoring/LicenseText

NOTE: For hosting in Azure Container Instances parameter names are supported that use only underscores, e.g. ServiceControl_Audit_LicenseText

These parameters can also be added by using a standard Docker environment file. Every parameter has its own line and does not need to be enclosed by quotes. It can then be used by Docker as follows:

```cmd
docker run --env-file servicecontrol.env [dockerimage]
```

An example of the `servicecontrol.env` file:

```env
# License text
ServiceControl/LicenseText=<?xml version="1.0" encoding="utf-8"?><license id="..."></license>

# Connection string
ServiceControl/ConnectionString=data source=[server],1433; user id=username; password=[password]; Initial Catalog=servicecontrol

# Remote audit instances
ServiceControl/RemoteInstances=[{'api_uri':'http://[hostname]:44444/api'}]
```

## Running ServiceControl using Docker

This section uses the SqlServer transport as an example on how to run ServiceControl using docker. The same steps are applicable to other transports.

Each instance type of ServiceControl has a distinct init and runtime container.

* The error instance of ServiceControl can be run as stand-alone
* The audit instance needs an error instance to be running too
* The monitoring instance can be run as stand-alone

### Error instances

The first step is to create the required infrastructure using the `init` container:

```cmd
docker run -e "ServiceControl/ConnectionString=[connectionstring]" -e 'ServiceControl/LicenseText=[licensecontents]' -v d:/servicecontrol/:c:/data/ -d [imagename]
```

The license contents parameter is the contents of the license file with any `"` characters encoded as `\"`.

The init container will run and shut down once the required queues and database has been created.

The runtime instance of ServiceControl can now be run:

```cmd
docker run -e "ServiceControl/ConnectionString=[connectionstring]" -e 'ServiceControl/LicenseText=[licensecontents]' -v d:/servicecontrol/:c:/data/ -p 33333:33333 -d [imagename]
```

ServiceControl can now be accessed over port `33333`.

All [supported parameters](/servicecontrol/creating-config-file.md) for ServiceControl Error can be specified via environment variables.

### Audit instances

A ServiceControl audit instance can be configured Once a ServiceControl error instance is running.

The first step is to create the required infrastructure by executing the `audit.init` container:

```cmd
docker run -e "ServiceControl.Audit/ConnectionString=[connectionstring]" -e 'ServiceControl.Audit/LicenseText=[licensecontents]' -v d:/servicecontrol.audit/:c:/data/ -d [imagename]
```

The license contents parameter is the contents of the license file with any `"` characters encoded as `\"`.

The init container will run and shut down once the required queues and database has been created.

The runtime instance of ServiceControl Audit can now be run:

```cmd
docker run -e "ServiceControl.Audit/ConnectionString=[connectionstring]" -e 'ServiceControl.Audit/LicenseText=[licensecontents]' -e 'ServiceControl.Audit/ServiceControlQueueAddress=Particular.ServiceControl' -v d:/servicecontrol.audit/:c:/data/ -p 44444:44444 -d [imagename]
```

The `ServiceControl.Audit/ServiceControlQueueAddress` environment variable must point to the queue of the error instance of ServiceControl.

To complete the set up, the error instance of ServiceControl must be stopped and run with an additional environment variable - specifically `-e "ServiceControl/RemoteInstances=[{'api_uri':'http://172.28.XXX.XXX:44444/api'}]"` which tells the error instance how to communicate with the audit instance.

ServiceControl can now be accessed over port `44444`.

All [supported parameters](/servicecontrol/audit-instances/creating-config-file.md) for ServiceControl Audit can be specified via environment variables.

### Monitoring Instances

Monitoring instances can be run standalone and therefore only need the init container to be run and then the runtime container.

```cmd
docker run -e "Monitoring/ConnectionString=[connectionstring]" -e 'Monitoring/LicenseText=[licensecontents]' -v d:/servicecontrol.monitoring/:c:/data/ -d [imagename]
```

The init container will run and shut down once the required queues and database has been created.

The runtime instance of ServiceControl Monitoring can now be run:

```cmd
docker run -e "Monitoring/ConnectionString=[connectionstring]" -e 'Monitoring/LicenseText=[licensecontents]' -v d:/servicecontrol.monitoring/:c:/data/ -p 33633:33633 -d [imagename]
```

All [supported parameters](/servicecontrol/monitoring-instances/installation/creating-config-file.md) for ServiceControl Monitoring can be specified via environment variables.

## Volume mappings

A general practice with containers is that no data is persisted inside a container, but rather outside the container on either the host machine, database or something similar. This makes it easier to backup data, share data among containers and more importantly, not lose the data when the container is updated to a later version. Storing files outside of the container can be achieved with volume mappings, or with folder mappings for Windows containers.

Volume mappings can be used with ServiceControl for configuration files, storing logfiles and possibly reading a license file. More information on data storage can be found on the [Docker website](https://docs.docker.com/storage/).

### Configuration and Logging

Every image has a folder in `c:\data\` that can be mapped to a folder outside of the container. 

```cmd
docker run -v d:\servicecontrol\:c:\data\ [imagename]
```

As a result, that container will write both logfiles and its database into that folder in respectively the `Logs` and `DB` folders. Another option is to map each folder specifically, for more control over their locations and divide them over separate drives, for example:

```cmd
docker run -v d:\servicecontroldatabase\:c:\data\DB\ -v e:\logs\servicecontrol\:c:\data\logs\ [imagename]
```

### License file

The license file can be provided via an environment variable on the command-line or via an environment file. Another options is to map the folder where ServiceControl will look for the license file.

```cmd
docker run -v d:\servicecontrol\license\:c:\ProgramData\ParticularSoftware\ [imagename]
```

As the above command shows, ServiceControl will look for the license file in OS specific folders as can be found [in the license documentation](/nservicebus/licensing/#license-management-machine-wide-license-location).

## RavenDB Maintenance Mode

If maintenance is required on the embedded RavenDb database of ServiceControl, this can be enabled on Docker by

1. Stopping the ServiceControl container
2. Starting a new container by adding the `--maintenance --portable` parameters and adding an additional port mapping for port 33334.

```cmd
docker run --interactive --tty --detach -v d:\servicecontrol\:c:\data\ [imagename] --maintenance --portable
```

## Interactive shell

To run tools like `ESENTUTL` to perform low-level maintenance tasks like [compacting the RavenDB database](/servicecontrol/db-compaction.md) the container can be started in interactive mode and override its entrypoint. 

Docker for Windows:
```cmd
docker run --interactive --tty -v d:\servicecontrol\:c:\data --entrypoint cmd [imagename]
```
