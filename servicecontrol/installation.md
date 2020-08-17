---
title: Installing ServiceControl
reviewed: 2020-03-16
redirects:
 - servicecontrol/multi-transport-support
---

The ServiceControl installation file consists of an embedded MSI bootstrapper EXE and an embedded MSI. This installation can be executed standalone or via the [Platform Installer](/platform/installer/). The installation package includes a utility to install, upgrade, and remove ServiceControl services. This utility is launched as the final step in the installation process and is also available via the Windows Start Menu.

NOTE: A [community managed puppet module](https://forge.puppet.com/tragiccode/nservicebusservicecontrol) is available to install ServiceControl.

## Prerequisites

The ServiceControl installation has the following prerequisites:

* [Microsoft .NET 4.6.1 Runtime](https://www.microsoft.com/en-us/download/details.aspx?id=49982)

If ServiceControl is installed via the Platform Installer, then the installer will manage installation and configuration of the prerequisites.

NOTE: ServiceControl should be installed on a separate dedicated machine with dedicated storage in a production environment to isolate the audit and error queue message ingestion. These operations have a big impact on other processes. ServiceControl must be given sufficient CPU, RAM (6GB minimum) and storage suitable for low-latency write operations, similar to how a database server would be provisioned. See [ServiceControl Capacity Planning](capacity-and-planning.md) for more guidance.


## Transport support

In ServiceControl version 1.7 and above, the transport packages are managed by the installation and do not need to be downloaded from NuGet. ServiceControl can be configured to use one of the supported [transports](/transports/) listed below using the ServiceControl Management application:

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

#### Transport adapters

Certain transport features are not supported natively by ServiceControl and will require a [transport adapter](/servicecontrol/transport-adapter). Contact support@particular.net for further guidance.

Adding third-party transports via the ServiceControl Management application is not supported.

#### MSMQ

If MSMQ is the selected transport, ensure the MSMQ service has been installed and configured as outlined in [Installing The Platform Components](/platform/installer/offline.md#platform-installer-components-nservicebus-prerequisites).

#### RabbitMQ

In addition to the [connection string options of the transport](/transports/rabbitmq/connection-settings.md) the following ServiceControl specific options are available in versions 4.4 and above:

* `UseExternalAuthMechanism=true|false(default)` - Specifies that an [external authentication mechanism should be used for client authentication](/transports/rabbitmq/connection-settings.md#transport-layer-security-support-external-authentication).
* `DisableRemoteCertificateValidation=true|false(default)` - Allows ServiceControl to connect to the broker [even if the remote server certificate is invalid](/transports/rabbitmq/connection-settings.md#transport-layer-security-support-remote-certificate-validation).

#### Azure Service Bus

In addition to the [connection string options of the transport](/transports/azure-service-bus/#configuring-an-endpoint) the following ServiceControl specific options are available in versions 4.4 and above:

* `QueueLengthQueryDelayInterval=<value_in_milliseconds>` - Specifies delay between queue length refresh queries. The default value is 500 ms.

#### SQL 

In addition to the [connection string options of the transport](/transports/sql/connection-settings.md#connection-configuration) the following ServiceControl specific options are available in versions 4.4 and above:

* `Queue Schema=<schema_name>` - Specifies custom schema for the ServiceControl input queue.

#### Amazon SQS 

The following ServiceControl connection string options are available in versions 4.4 and above:

* `AccessKeyId=<value>` - AssessKeyId value,
* `SecretAccessKey=<value>` - SecretAccessKey value,
* `Region=<value>` - Region transport [option](/transports/sqs/configuration-options.md#region),
* `QueueNamePrefix=<value>` - Queue name prefix transport [option](/transports/sqs/configuration-options.md#queuenameprefix),
* `S3BucketForLargeMessages=<value>` - S3 bucket for large messages [option](/transports/sqs/configuration-options.md#s3bucketforlargemessages),
* `S3KeyPrefix=<value>` - S3 key prefic [option](/transports/sqs/configuration-options.md#s3bucketforlargemessages-s3keyprefix).


## Performance counter

Metrics are reported via the [performance counters](/monitoring/metrics/performance-counters.md) if the counters are installed.

For instructions on how to install the performance counters without the Platform Installer, refer to [Installing The Platform Components Manually](/platform/installer/offline.md)

The installation of the NServiceBus performance counters is optional for ServiceControl version 1.7 and above.


## Using ServiceControl Management to upgrade ServiceControl instances

ServiceControl Management provides a simple means of setting up one or more instances of ServiceControl service types (error, audit, and monitoring). For production systems, it is recommended to limit the number of instances per machine to one.

WARNING: The ability to add multiple instances *of the same type on a single machine* is primarily intended to assist development and test environments.

ServiceControl Management can be launched automatically at the end of the installation process to enable adding or upgrading ServiceControl instances.

ServiceControl Management will display the instances of the ServiceControl service installed. If the version of the binaries used by an instance are older than those shipped with ServiceControl Management, an upgrade link will be shown next to the version label.

![ServiceControl Management interface that shows the link for upgrading](managementutil-upgradelink.png 'width=500')

To upgrade the service, click the upgrade link next to the service name.

Clicking the upgrade link will:

 * Prompt for additional required information, i.e. new mandatory settings introduced in the newer version
 * Stop the service
 * Remove the old binaries for ServiceControl and the configured transport
 * Run the new binaries to create required queues
 * Start the service


## Using ServiceControl Management to add ServiceControl instances

If this is a new installation of ServiceControl, click on the `Add New Instance` button in the center of the screen or the "New Instance" link at the top of the screen; both options launch the "New instance form". Complete the form to register a new ServiceControl service.


## Service name and plugins

When adding the first instance of the ServiceControl service, the default service name is "Particular.ServiceControl". This can be changed to a custom service name. This will also change the queue name associated with this instance of ServiceControl.

The ServiceControl queue name is required for configuring the endpoint plugins. See [Install Heartbeats Plugin](/monitoring/heartbeats/install-plugin.md) and [Install Custom Checks Plugin](/monitoring/custom-checks/install-plugin.md) for more information.


## Removing ServiceControl

To perform a clean uninstallation of ServiceControl from a machine:

1. Remove each ServiceControl instance using ServiceControl Management (or PowerShell)
2. Uninstall ServiceControl Management using Add or Remove programs


### Remove ServiceControl instances

To remove a ServiceControl instance, click the Advanced Options button and select Remove. If applicable, select the option to remove the database and logs directories. This will stop the running instance (if it is running) and remove all files related to the instance from the local file system.


#### Remaining artifacts

Even after a ServiceControl instance has been removed, there are artifacts left behind. Each ServiceControl instance leaves queues in the configured transport. The queue names will depend on the configuration of the ServiceControl instance.

- _instance name_ - control messages for the ServiceControl instance
- _instance name_.errors - control messages that the ServiceControl instance was not able to process
- _instance name_.staging - temporary failed messages while they are being retried
- _audit queue name_ - messages that have been processed by endpoints and then forwarded to the audit queue. These messages have not yet been ingested by the ServiceControl instance.
- _error queue name_ - messages that failed processing in endpoints and have passed immediate and delayed retries. These messages have not yet been ingested by the ServiceControl instance.
- _audit log queue name_ if audit log forwarding was configured - messages that have been ingested from the audit queue, processed by this ServiceControl instance, and then forwarded to the audit log queue.
- _error log queue name_ if error log forwarding was configured - messages that have been ingested from the error queue, processed by this ServiceControl instance, and then forwarded to the error log queue.

If the option to delete the database/log folders was not selected when removing the instance, then these folders and their contents are left on disk.

NOTE: If the instance was configured to run under a service account then that account may have been granted _Logon as a Service_ privileges. This is not reversed when the instance is removed.


### Uninstall ServiceControl Management

To uninstall ServiceControl Management, use the `Apps & features` settings in Windows. 

NOTE: Uninstalling the ServiceControl Management application will not remove existing instances. Remove all ServiceControl instances using ServiceControl Management before uninstalling the application itself.