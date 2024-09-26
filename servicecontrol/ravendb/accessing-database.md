---
title: Accessing the ServiceControl database
summary: How to get direct access to the database used by ServiceControl Error and Audit instances
reviewed: 2024-07-11
redirects:
  - servicecontrol/maintenance-mode
  - servicecontrol/audit-instances/maintenance-mode
---

ServiceControl Error and Audit instances store data in a RavenDB database. Some advanced operations require accessing that database directly.

> [!NOTE]
> Accessing RavenDB Studio requires a modern web browser. Internet Explorer is not supported.
>
> The database is intended to be used only by the ServiceControl instance and is not intended for external manipulation or modifications.

How the database is deployed depends on the version of ServiceControl, the version of RavenDB used by that version, and whether ServiceControl is [deployed on Windows](#windows-deployment) or on [containerized infrastructure](#container-deployment).

## Windows deployment

Windows deployments differ based on what version of RavenDB is used to persist ServiceControl data.

### RavenDB 5

RavenDB 5 is used as the data storage for all ServiceControl version 5.x and higher, and was available as an option for ServiceControl Audit instances of version 4.26.0 and above.

When logged on to the server where ServiceControl is installed, the database is always available, as it is implemented as a separate process.

Access the database at:

* Error Instance: [http://localhost:33334](http://localhost:33334) (or the configured [database maintenance port](/servicecontrol/servicecontrol-instances/configuration.md#host-settings-servicecontroldatabasemaintenanceport))
* Audit Instance: [http://localhost:44445](http://localhost:44445) (or the configured [database maintenance port](/servicecontrol/audit-instances/configuration.md#host-settings-servicecontrol-auditdatabasemaintenanceport))

The database can be accessed without stopping or restarting the ServiceControl instance, though [maintenance mode](#windows-deployment-maintenance-mode) can still be used for some operations.

### RavenDB 3.5

RavenDB 3.5 is used on all ServiceControl 4.x instances except for ServiceControl Audit instances initially created using version 4.26.0 or later.

In these versions, ServiceControl serves the database in-process and only exposes the database if [maintenance mode](#windows-deployment-maintenance-mode) is enabled.

### Maintenance mode

When running in Maintenance Mode, a Windows ServiceControl instance will only run the database. The API will not be available, and messages will not be ingested from any queues. This is useful to be able to inspect and perform administrative functions on the database without any modifications from the application being possible.

To enable maintenance mode:

1. Launch ServiceControl Management.
2. Click the wrench <kbd> :wrench: </kbd> icon to open Advanced Options.
3. Click **Start Maintenance Mode**.
4. Click the **Launch RavenDB Studio** link to access the RavenDB interface.
5. Click **Stop Maintenance Mode** to resume normal operation.

![Running in Maintenance Mode](/servicecontrol/maintenance-mode.gif)

## Container deployment

When deployed using containers, the database runs in a separate container than the ServiceControl instance. The database can be accessed at any time on port `8080` of the database container.

The container equivalent of [maintenance mode](#windows-deployment-maintenance-mode) can be achieved by stopping the ServiceControl container while leaving the database container running.