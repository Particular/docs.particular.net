---
title: Installing ServiceControl
reviewed: 2017-02-23
tags:
 - Installation
redirects:
 - servicecontrol/multi-transport-support
---

The ServiceControl installation file consists of an embedded MSI bootstrapper EXE and an embedded MSI. This installation can be executed standalone or via the [Platform Installer](/platform/installer/). The installation package include a utility to manage the installation, upgrade and remove of ServiceControl services. This utility is launched as the final step in the installation process and is also available via the Windows Start Menu.


## Prerequisites

The ServiceControl Installation has the following prerequisites:

 * [Microsoft .NET 4.5.2 Runtime](https://www.microsoft.com/en-us/download/details.aspx?id=42643)

If ServiceControl is installed via the Platform Installer then the installation and configuration of these prerequisites are managed by the installer.

NOTE: We advise to install ServiceControl on a seperate (virtual) machine in a production environment to isolate the audit and error queue message ingestion. These operations have a big impact other other processes. Treat ServiceControl as you would setup a database server meaning sufficient CPU, RAM (6GM minimum), and storage suitable for low latency write operations.


### RavenDB Prerequisites

* [Silverlight 5](https://www.microsoft.com/silverlight/) 

ServiceControl makes use of an embedded version of RavenDB. To carry out [maintenance activities](/servicecontrol/use-ravendb-studio.md) on this database the Microsoft Silverlight plugin must be installed within a browser on the server. Without the plugin the RavenDB Management Studio will not load.


## Transport Support

In Versions 1.7 and above the transport DLLs are managed by the installation and do not need to be downloaded from NuGet. ServiceControl can be configured to use one of the supported [transports](/transports/) listed below using the ServiceControl Management application:

 * [Microsoft Message Queuing (MSMQ)](/transports/msmq/)
 * [Azure Storage Queues](/transports/azure-storage-queues/)
 * [Azure Service Bus](/transports/azure-service-bus/)
 * [SQL Server](/transports/sql/)
 * [RabbitMQ](/transports/rabbitmq/)

Adding third party transports via the Management Utility is not supported at this stage. If MSMQ is the selected transport then ensure the service has been installed and configured as outlined in [Installing The Platform Components Manually](/platform/installer/offline.md#platform-installer-components-nservicebus-prerequisites).

Installing MSMQ is optional in the Platform Installer. See [Platform Installer - MSMQ](/platform/installer/#select-items-to-install-configure-microsoft-message-queuing).


## Performance Counter

Metrics are reported via the [Performance Counters](/nservicebus/operations/performance-counters.md) if the counters are installed.

For instructions on how to install the Performance Counters without the Platform Installer refer to [Installing The Platform Components Manually](/platform/installer/offline.md)

The installation of the NServiceBus Performance Counters is optional for Versions 1.7 and above.

Performance Counters are not installed by the [Platform Installer](/platform/installer/).


## Using ServiceControl Management to upgrade ServiceControl instances

ServiceControl Management provides a simple means of setting up one or more instances of the ServiceControl service. For production systems it is recommended to limit the number of instances per machine to one.

WARNING: The ability to add multiple instances is primarily intended to assist development and test environments.

ServiceControl Management can be launched automatically at the end of the installation process to enable adding or upgrading ServiceControl instances.

ServiceControl Management will display the instances of the ServiceControl service installed. If the version of the binaries used by an instance are older that those shipped with ServiceControl Management an upgrade link will be shown against the version label.

![](managementutil-upgradelink.png 'width=500')

To upgrade the service just click the upgrade link next to the Service name

Clicking the upgrade link will

 * Prompt for any additional information that is required such as values for new mandatory settings introduced in the newer version.
 * Stop the Service.
 * Remove the old binaries for ServiceControl and the configured Transport.
 * Run the new binaries to create any required queues.
 * Start the Service.


## Using ServiceControl Management to add ServiceControl instances

If this is a new installation of ServiceControl click on the `Add New Instance` button in the center of the screen or the "New Instance" link at the top of the screen,  both options launch the same "New instance form". Complete the form to register a new ServiceControl service.


## Service Name and Plugins

When adding the first instance of the ServiceControl service the default service name is "Particular.ServiceControl". It is possible choose to change this name to a custom service name. In doing so this is also changing the queue name associated with this instance of ServiceControl.

The endpoint plugins such as the heartbeat and custom check plugins assume that the ServiceControl queue name is the default. If a custom service name was used then see [ServiceControl Endpoint Plugins](/servicecontrol/plugins/) for more details on how to configure the endpoint plugins to use the custom queue name.
