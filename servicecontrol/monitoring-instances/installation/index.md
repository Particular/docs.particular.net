---
title: Installing Monitoring Instances
reviewed: 2017-07-26
tags:
 - Installation
---

The ServiceControl installation file consists of an embedded MSI bootstrapper EXE and an embedded MSI. This installation can be executed standalone or via the [Platform Installer](/platform/installer/). The installation package include a utility to manage the installation, upgrade and remove of ServiceControl services, including Monitoring instances. This utility is launched as the final step in the installation process and is also available via the Windows Start Menu.


## Prerequisites

The ServiceControl Installation has the following prerequisites:

 * [Microsoft .NET 4.5.2 Runtime](https://www.microsoft.com/en-us/download/details.aspx?id=42643)

If ServiceControl is installed via the Platform Installer then the installation and configuration of these prerequisites are managed by the installer.

NOTE: Each environment should contain a single [ServiceControl instance](/servicecontrol/servicecontrol-instances/) and a single [Monitoring instance](/servicecontrol/monitoring-instances/). In high-throughput scenarios it is recommended that these instances each run on a separate dedicated machine.


## Transport Support

Monitoring instances can be configured to use one of the supported [transports](/transports/) listed below using the ServiceControl Management application:

 * [Microsoft Message Queuing (MSMQ)](/transports/msmq/)
 * [Azure Storage Queues](/transports/azure-storage-queues/)
 * [Azure Service Bus](/transports/azure-service-bus/)
 * [SQL Server](/transports/sql/)
 * [RabbitMQ](/transports/rabbitmq/)

Adding third party transports via the Management Utility is not supported at this stage. If MSMQ is the selected transport then ensure the service has been installed and configured as outlined in [Installing The Platform Components Manually](/platform/installer/offline.md#platform-installer-components-nservicebus-prerequisites).

Installing MSMQ is optional in the Platform Installer. See [Platform Installer - MSMQ](/platform/installer/#select-items-to-install-configure-microsoft-message-queuing).


## Using ServiceControl Management to upgrade Monitoring instances

ServiceControl Management provides a simple means of setting up one or more Monitoring instances

WARNING: The ability to add multiple instances is primarily intended to assist development and test environments.

ServiceControl Management can be launched automatically at the end of the installation process to enable adding or upgrading ServiceControl or Monitoring instances.

ServiceControl Management will display the list of installed ServiceControl and Monitoring instances. If the version of the binaries used by an instance are older that those shipped with ServiceControl Management an upgrade link will be shown against the version label.

![](/servicecontrol/managementutil-upgradelink.png 'width=500')

To upgrade the service just click the upgrade link next to the Service name

Clicking the upgrade link will

 * Prompt for any additional information that is required such as values for new mandatory settings introduced in the newer version.
 * Stop the Service.
 * Remove the old binaries.
 * Run the new binaries to create any required queues.
 * Start the Service.


## Using ServiceControl Management to add Monitoring instances

Click on the `+ NEW` link at the top of the screen and select "Monitoring instance" to launch the "New instance form". Complete the form to register a new Monitoring instance service.


## Service Name and Plugins

When adding the first Monitoring instance service the default service name is "Particular.Monitoring". It is possible to change this name to a custom service name. In doing so this is also changing the queue name associated with this Monitoring instance.

The metrics endpoint plugin will need the Monitoring instance name in order to send metric data to the Monitoring instance.