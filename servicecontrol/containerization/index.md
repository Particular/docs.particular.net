---
title: Running ServiceControl in containers
reviewed: 2020-12-10
---

Docker images for ServiceControl exist on Dockerhub under the [Particular organization](https://hub.docker.com/u/particular). These can be used to run ServiceControl in docker containers. These docker images are only available for Windows.

## Containers overview

ServiceControl is split into multiple docker images for each instance type. These are:

* The error instance
* The audit instance
* The monitoring instance

The images for each of these containers are further split into an init container and a runtime container. This means that there are 6 images for all instances of ServiceControl.

The init container is used to create or upgrade the infrastructure required for ServiceControl. This includes creating the database as well as the queues that ServiceControl uses. Once the init container has been run, it will shut down and the runtime container can be run. The runtime container will use the infrastructure that has been created.

### Transports

Each supported transport and topology for ServiceControl has further been broken down into it's own set of images. The following transport and topologies are available on dockerhub:

* [Amazon SQS](https://hub.docker.com/search?q=servicecontrol.amazonsqs&type=image)
* [Azure Service Bus](https://hub.docker.com/search?q=servicecontrol.azureservicebus&type=image)
* [Azure Storage Queues](https://hub.docker.com/search?q=servicecontrol.azurestorageque&type=image)
* [RabbitMQ Conventional Topology](https://hub.docker.com/search?q=servicecontrol.rabbitmq.conven&type=image)
* [RabbitMQ Direct Topology](https://hub.docker.com/search?q=servicecontrol.rabbitmq.direct&type=image)
* [Sql Server](https://hub.docker.com/search?q=servicecontrol.sqlserver&type=image)

## Parameters

All parameters that the ServiceControl instance supports can be provided by passing the parameter as an environment variable. E.g.

`-e "ServiceControl/Port=12345"`

## Running ServiceControl using Docker

This section uses the SqlServer transport as an example on how to run ServiceControl using docker. The same steps are applicable to other transports.

Each instance type of ServiceControl has a distinct init and runtime container.

* The error instance of ServiceControl can be run as stand-alone
* The audit instance needs an error instance to be running too
* The monitoring instance can be run as stand-alone

### Error instances

The first step is to create the required infrastructure using the `init` container:

`docker run -e "ServiceControl/ConnectionString=[connectionstring]" -e 'ServiceControl/LicenseText=[licensecontents]' -v c:/data/:c:/data/ -v c:/logs/:c:/logs/ -d particular/servicecontrol.sqlserver.init-windows:4`

The license contents parameter is the contents of the license file with any `"` characters encoded as `\"`.

The init container will run and shut down once the required queues and database has been created.

The runtime instance of ServiceControl can now be run:

`docker run -e "ServiceControl/ConnectionString=[connectionstring]" -e 'ServiceControl/LicenseText=[licensecontents]' -v c:/data/:c:/data/ -v c:/logs/:c:/logs/ -p 33333:33333 -d particular/servicecontrol.sqlserver-windows:4`

ServiceControl can now be accessed over port `33333`.



### Audit instances

A ServiceControl audit instance can be configured Once a ServiceControl error instance is running.

The first step is to create the required infrastructure by executing the `audit.init` container:

`docker run -e "ServiceControl.Audit/ConnectionString=[connectionstring]" -e 'ServiceControl.Audit/LicenseText=[licensecontents]' -v c:/auditdata/:c:/data/ -v c:/auditlogs/:c:/logs/ -d particular/servicecontrol.sqlserver.audit.init-windows:4`

The license contents parameter is the contents of the license file with any `"` characters encoded as `\"`.

The init container will run and shut down once the required queues and database has been created.

The runtime instance of ServiceControl Audit can now be run:

`docker run -e "ServiceControl.Audit/ConnectionString=[connectionstring]" -e 'ServiceControl.Audit/LicenseText=[licensecontents]' -e 'ServiceControl.Audit/ServiceControlQueueAddress=Particular.ServiceControl' -v c:/auditdata/:c:/data/ -v c:/auditlogs/:c:/logs/ -p 44444:44444 -d particular/servicecontrol.audit.sqlserver-windows:4`

The `ServiceControl.Audit/ServiceControlQueueAddress` environment variable must point to the queue of the error instance of ServiceControl.

To complete the set up, the error instance of ServiceControl must be stopped and run with an additional environment variable - specifically `-e "ServiceControl/RemoteInstances=[{'api_uri':'http://172.28.XXX.XXX:44444/api'}]"` which tells the error instance how to communicate with the audit instance.

ServiceControl can now be accessed over port `44444`.

All supported parameters for ServiceControl Audit can be found [here](https://docs.particular.net/servicecontrol/audit-instances/creating-config-file).

### Monitoring Instances

Monitoring instances can be run standalone and therefore only need the init container to be run and then the runtime container.

`docker run -e "Monitoring/ConnectionString=[connectionstring]" -e 'Monitoring/LicenseText=[licensecontents]' -v c:/monitoringlogs/:c:/logs/ -d particular/servicecontrol.sqlserver.monitoring.init-windows:4`

The init container will run and shut down once the required queues and database has been created.

The runtime instance of ServiceControl Monitoring can now be run:

`docker run -e "Monitoring/ConnectionString=[connectionstring]" -e 'Monitoring/LicenseText=[licensecontents]' -v c:/data/:c:/data/ -v c:/logs/:c:/logs/ -p 33633:33633 -d particular/servicecontrol.sqlserver.monitoring-windows:4`

All supported parameters for ServiceControl Audit can be found [here](https://docs.particular.net/servicecontrol/monitoring-instances/installation/creating-config-file).