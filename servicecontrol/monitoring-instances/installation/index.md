---
title: Installing Monitoring Instances
reviewed: 2017-07-26
tags:
 - Installation
---

The ServiceControl installation file consists of an embedded MSI bootstrapper EXE and an embedded MSI. This installation can be executed standalone or via the [Platform Installer](/platform/installer/). The installation package include a utility to manage the installation, upgrade and remove of ServiceControl services, including Monitoring instances. This utility is launched as the final step in the installation process and is also available via the Windows Start Menu.


## Prerequisites

The ServiceControl installation has the following prerequisites:

* [Microsoft .NET 4.6.1 Runtime](https://www.microsoft.com/en-us/download/details.aspx?id=49982)

If ServiceControl is installed via the Platform Installer, then the installer will manage installation and configuration of the prerequisites.

NOTE: Each environment should contain a single [ServiceControl instance](/servicecontrol/servicecontrol-instances/) and a single [Monitoring instance](/servicecontrol/monitoring-instances/). In high-throughput scenarios, it is recommended that these instances each run on a separate dedicated machine.


## Transport support

Monitoring instances can be configured to use one of the supported [transports](/transports/) listed below using the ServiceControl Management application:

* [Azure Service Bus](/transports/azure-service-bus)
* [Azure Service Bus - Endpoint-oriented topology](/transports/azure-service-bus/legacy/topologies.md#versions-7-and-above-endpoint-oriented-topology)
* [Azure Service Bus - Forwarding topology](/transports/azure-service-bus/legacy/topologies.md#versions-7-and-above-forwarding-topology)
* [Azure Storage Queues](/transports/azure-storage-queues/)
* [Amazon Simple Queue Service (SQS)](/transports/sqs/)
* [Microsoft Message Queuing (MSMQ)](/transports/msmq/)
* [RabbitMQ - Conventional routing topology](/transports/rabbitmq/routing-topology.md#conventional-routing-topology)
* [RabbitMQ - Direct routing topology](/transports/rabbitmq/routing-topology.md#direct-routing-topology)
* [SQL Server](/transports/sql/)

### Transport-specific features

Certain transport features are not supported natively by ServiceControl and will require a [transport adapter](/servicecontrol/transport-adapter). Contact support@particular.net for further guidance.

Adding third-party transports via the ServiceControl Management application is not supported.

### MSMQ

If MSMQ is the selected transport, then ensure the service has been installed and configured as outlined in [Installing The Platform Components Manually](/platform/installer/offline.md#platform-installer-components-nservicebus-prerequisites).

Installing MSMQ is optional in the Platform Installer. See [Platform Installer - MSMQ](/platform/installer/#select-items-to-install-configure-microsoft-message-queuing).


## Using ServiceControl Management to upgrade Monitoring instances

ServiceControl Management provides a simple means of setting up one or more Monitoring instances.

WARNING: The ability to add multiple instances is primarily intended to assist development and test environments.

ServiceControl Management can be launched automatically at the end of the installation process to enable adding or upgrading ServiceControl or Monitoring instances.

ServiceControl Management will display the list of installed ServiceControl and Monitoring instances. If the version of the binaries used by an instance are older that those shipped with ServiceControl Management, an upgrade link will be shown against the version label.

![](/servicecontrol/managementutil-upgradelink.png 'width=500')

To upgrade the service, click the upgrade link next to the service name.

Clicking the upgrade link will:

* Prompt for any new information that is required to run properly on the transport of choice.
* Stop the Monitoring instance Windows Service.
* Remove the old binaries.
* Run the new binaries to upgrade the Monitoring instance by applying new settings and/or creating any required additional queues.
* Start the Monitoring instance Windows Service.


## Using ServiceControl Management to add Monitoring instances

Click on the `+ NEW` link at the top of the screen and select "Monitoring instance" to launch the "New instance form". Complete the form to register a new Monitoring instance service.


## Service Name and Plugins

When adding the first Monitoring instance service the default service name is `Particular.Monitoring`. It is possible to change this name to a custom service name. In doing so this is also changing the queue name associated with this Monitoring instance.

The metrics endpoint plugin will need the Monitoring instance name in order to send metric data to the Monitoring instance.