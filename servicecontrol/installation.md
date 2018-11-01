---
title: Installing ServiceControl
reviewed: 2018-06-15
tags:
 - Installation
redirects:
 - servicecontrol/multi-transport-support
---

The ServiceControl installation file consists of an embedded MSI bootstrapper EXE and an embedded MSI. This installation can be executed standalone or via the [Platform Installer](/platform/installer/). The installation package include a utility to manage the installation, upgrade, and removal of ServiceControl services. This utility is launched as the final step in the installation process and is also available via the Windows Start Menu.


## Prerequisites

The ServiceControl installation has the following prerequisites:

* [Microsoft .NET 4.6.1 Runtime](https://www.microsoft.com/en-us/download/details.aspx?id=49982)

If ServiceControl is installed via the Platform Installer, then the installer will manage installation and configuration of the prerequisites.

NOTE: ServiceControl should be installed on a separate dedicated machine with dedicated storage in a production environment to isolate the audit and error queue message ingestion. These operations have a big impact on other processes. ServiceControl must be given sufficient CPU, RAM (6GB minimum) and storage suitable for low latency write operations, similar to how a database server would be provisioned. See [ServiceControl Capacity Planning](capacity-and-planning.md) for more guidance.


## Transport support

In ServiceControl version 1.7 and above, the transport packages are managed by the installation and do not need to be downloaded from NuGet. ServiceControl can be configured to use one of the supported [transports](/transports/) listed below using the ServiceControl Management application:

* [Azure Service Bus](/transports/azure-service-bus-netstandard)
* [Azure Service Bus - Endpoint-oriented topology](/transports/azure-service-bus/topologies/#versions-7-and-above-endpoint-oriented-topology)
* [Azure Service Bus - Forwarding topology](/transports/azure-service-bus/topologies/#versions-7-and-above-forwarding-topology)
* [Azure Storage Queues](/transports/azure-storage-queues/)
* [Amazon Simple Queue Service (SQS)](/transports/sqs/)
* [Microsoft Message Queuing (MSMQ)](/transports/msmq/)
* [RabbitMQ - Conventional routing topology](/transports/rabbitmq/routing-topology.md#conventional-routing-topology)
* [RabbitMQ - Direct routing topology](/transports/rabbitmq/routing-topology.md#direct-routing-topology)
* [SQL Server](/transports/sql/)


### Transport-specific features

Certain transport features are not supported natively by ServiceControl and will require a [transport adapter](/servicecontrol/transport-adapter). Contact support@particular.net for further guidance.

Adding third-party transports via the Management Utility is not supported.


### MSMQ Specifics

If MSMQ is the selected transport, then ensure the service has been installed and configured as outlined in [Installing The Platform Components Manually](/platform/installer/offline.md#platform-installer-components-nservicebus-prerequisites).

Installing MSMQ is optional in the Platform Installer. See [Platform Installer - MSMQ](/platform/installer/#select-items-to-install-configure-microsoft-message-queuing).


## Performance counter

Metrics are reported via the [performance counters](/monitoring/metrics/performance-counters.md) if the counters are installed.

For instructions on how to install the performance counters without the Platform Installer refer to [Installing The Platform Components Manually](/platform/installer/offline.md)

The installation of the NServiceBus performance counters is optional for ServiceControl version 1.7 and above.

Performance counters are not installed by the [Platform Installer](/platform/installer/).


## Using ServiceControl Management to upgrade ServiceControl instances

ServiceControl Management provides a simple means of setting up one or more instances of the ServiceControl service. For production systems, it is recommended to limit the number of instances per machine to one.

WARNING: The ability to add multiple instances is primarily intended to assist development and test environments.

ServiceControl Management can be launched automatically at the end of the installation process to enable adding or upgrading ServiceControl instances.

ServiceControl Management will display the instances of the ServiceControl service installed. If the version of the binaries used by an instance are older that those shipped with ServiceControl Management, an upgrade link will be shown next to the version label.

![](managementutil-upgradelink.png 'width=500')

To upgrade the service, click the upgrade link next to the service name.

Clicking the upgrade link will:

 * Prompt for additional information that is required, such as values for new mandatory settings introduced in the newer version.
 * Stop the service.
 * Remove the old binaries for ServiceControl and the configured transport.
 * Run the new binaries to create required queues.
 * Start the service.


## Using ServiceControl Management to add ServiceControl instances

If this is a new installation of ServiceControl click on the `Add New Instance` button in the center of the screen or the "New Instance" link at the top of the screen; both options launch the "New instance form". Complete the form to register a new ServiceControl service.


## Service name and plugins

When adding the first instance of the ServiceControl service, the default service name is "Particular.ServiceControl". It is possible to change this name to a custom service name. This will also change the queue name associated with this instance of ServiceControl.

The ServiceControl queue name is required for configuring the endpoint plugins. See [Install Heartbeats Plugin](/monitoring/heartbeats/install-plugin.md) and [Install Custom Checks Plugin](/monitoring/custom-checks/install-plugin.md) for more information.


## Removing ServiceControl

To perform a clean uninstallation of ServiceControl from a machine:

1. Remove each ServiceControl instance using ServiceControl Management (or Powershell)
2. Uninstall ServiceControl Management using Add or Remove programs


### Remove ServiceControl instances

To remove a ServiceControl instance, click the Advanced Options button and then select Remove. If applicable, select the option to remove the database and logs directories and then confirm. This will stop the running instance (if it is running) and remove all files related to the instance from the local file system.


#### Remaining artifacts

Even after a ServiceControl instance has been removed, there are artifacts left behind. Each ServiceControl instance leaves queues in the configured transport. The queue names will depend on the configuration of the ServiceControl instance.

- _instance name_ - contains control messages for the ServiceControl instance.
- _instance name_.errors - contains control messages that the ServiceControl instance was not able to process.
- _instance name_.staging - temporarily contains failed messages while they are being retried.
- _audit queue name_ - contains messages that have been processed by endpoints and then forwarded to the audit queue. These messages have not been ingested by the ServiceControl instance yet.
- _error queue name_ - contains messages that failed processing in endpoints and have passed immediate and delayed retries. These messages have not been ingested by the ServiceControl instance yet.
- _audit log queue name_ if audit log forwarding was configured - contains messages that have been ingested from the audit queue, processed by this ServiceControl instance, and then forwarded to the audit log queue.
- _error log queue name_ if error log forwarding was configured - contains messages that have been ingested from the error queue, processed by this ServiceControl instance, and then forwarded to the error log queue.

If the option to delete the database/log folders was not selected when removing the instance, then these folders and their contents are left on disk.

NOTE: If the instance was configured to run under a service account then that account may have been granted _Logon as a Service_ privileges. This is not reversed when the instance is removed.


### Uninstall ServiceControl Management

To uninstall ServiceControl Management, use the Apps & features settings in Windows. 

NOTE: Uninstalling ServiceControl Management will not remove any existing instances. Remove all ServiceControl instances using ServiceControl Management before uninstalling it.