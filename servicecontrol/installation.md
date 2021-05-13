---
title: Installing ServiceControl
reviewed: 2020-03-16
redirects:
 - servicecontrol/multi-transport-support
---

The ServiceControl installation file consists of an embedded MSI bootstrapper EXE and an embedded MSI. The installation package includes a utility to install, upgrade, and remove ServiceControl services. This utility is launched as the final step in the installation process and is also available via the Windows Start Menu.

NOTE: A [community managed puppet module](https://forge.puppet.com/tragiccode/nservicebusservicecontrol) is available to install ServiceControl.

## Prerequisites

The ServiceControl installation has the following prerequisites:

* [Microsoft .NET 4.7.2 Runtime](https://dotnet.microsoft.com/download/dotnet-framework/net472)

See [ServiceControl Capacity Planning](capacity-and-planning.md) and [Hardware Considerations](/servicecontrol/servicecontrol-instances/hardware.md) for more guidance.

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

If MSMQ is the selected transport, ensure the MSMQ service has been installed and configured as outlined in [Installing The Platform Components](/platform/installer/offline.md#msmq-prerequisites).

#### RabbitMQ

In addition to the [connection string options of the transport](/transports/rabbitmq/connection-settings.md) the following ServiceControl specific options are available in versions 4.4 and above:

* `UseExternalAuthMechanism=true|false(default)` - Specifies that an [external authentication mechanism should be used for client authentication](/transports/rabbitmq/connection-settings.md#transport-layer-security-support-external-authentication).
* `DisableRemoteCertificateValidation=true|false(default)` - Allows ServiceControl to connect to the broker [even if the remote server certificate is invalid](/transports/rabbitmq/connection-settings.md#transport-layer-security-support-remote-certificate-validation).

#### Azure Service Bus

In addition to the [connection string options of the transport](/transports/azure-service-bus/#configuring-an-endpoint) the following ServiceControl specific options are available in versions 4.4 and above:

* `QueueLengthQueryDelayInterval=<value_in_milliseconds>` - Specifies delay between queue length refresh queries. The default value is 500 ms.

* `TopicName=<topic-bundle-name>` - Specifies [topic name](/transports/azure-service-bus/configuration.md#entity-creation) to be used by the instance. The default value is `bundle-1`.

#### SQL

In addition to the [connection string options of the transport](/transports/sql/connection-settings.md#connection-configuration) the following ServiceControl specific options are available in versions 4.4 and above:

* `Queue Schema=<schema_name>` - Specifies custom schema for the ServiceControl input queue.
* `SubscriptionRouting=<subscription_table_name>` - Specifies SQL subscription table name.  

#### Amazon SQS

The following ServiceControl connection string options are available in versions 4.4 and above:

* `AccessKeyId=<value>` - AssessKeyId value,
* `SecretAccessKey=<value>` - SecretAccessKey value,
* `Region=<value>` - Region transport [option](/transports/sqs/configuration-options.md#region),
* `QueueNamePrefix=<value>` - Queue name prefix transport [option](/transports/sqs/configuration-options.md#queuenameprefix),
* `TopicNamePrefix=<value>` - Topic name prefix transport [option](/transports/sqs/configuration-options.md#topicnameprefix)
* `S3BucketForLargeMessages=<value>` - S3 bucket for large messages [option](/transports/sqs/configuration-options.md#s3bucketforlargemessages),
* `S3KeyPrefix=<value>` - S3 key prefic [option](/transports/sqs/configuration-options.md#s3bucketforlargemessages-s3keyprefix).

## Performance counter

Metrics are reported via the [performance counters](/monitoring/metrics/performance-counters.md) if the counters are installed.

For instructions on how to install the performance counters, refer to [Installing The Platform Components Manually](/platform/installer/offline.md)

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

## Migrating to a new host

These are the steps to perform to migrate an instance of ServiceControl to another host:

1. Ensure the same Windows version is used on the new machine as storage is not compatible between Windows versions.
1. Stop and disable the current instance of ServiceControl in the Windows Service Manager.
1. Install and configure a new instance of ServiceControl using the same name on the new server.
1. Ensure the name is identical
1. Stop the newly installed instance of ServiceControl.
1. Copy the data from the previous instance to the new instance server (moving the data is not recommended).
1. Start the new service.
1. Remove the old instance of ServiceControl.

NOTE: If the database of the instance being migrated is very large, or no downtime can be tolerated, or the destination uses a different Windows version, or the instance uses a different name, then consider [scaling ServiceControl out via remote instances](/servicecontrol/servicecontrol-instances/remotes.md).

Things to remember:

* First update the current instance to the [latest version](https://github.com/Particular/ServiceControl/releases).
* Optionally, use the same installer as the current version. Database schemas are not guaranteed to be compatible between different versions.
* Optionally, [script ServiceControl installations via powershell](/servicecontrol/installation-powershell.md) instead of running installers manually

ServiceControl configuration settings are accessible via the  Service Control Management Utility or by navigating to the configuration files on the file system stored in `ServiceControl.exe.config`, `ServiceControl.Audit.exe.config`, and `ServiceControl.Monitoring.exe.config`.

Note: If ServiceControl was previously installed via the ServiceControl Management Utility then all instances are installed on a single machine by default. If the system has considerable load or has a large retention period, consider installing a single instance type on a server to scale out. This can be done via Powershell.

WARNING: Care should be taken when planning to move ServiceControl from one server to another. Moving databases between servers can be challenging. The embedded RavenDB does not support moving from a new versions of Windows back to older versions of Windows. See [Getting error while restoring backup file in raven DB](https://stackoverflow.com/questions/25625910/getting-error-while-restoring-backup-file-in-raven-db) for more details.

## Removing ServiceControl

To perform a clean uninstallation of ServiceControl from a machine:

1. Remove each ServiceControl instance using ServiceControl Management (or PowerShell)
2. Uninstall ServiceControl Management using Add or Remove programs

### Remove ServiceControl instances

To remove a ServiceControl instance, click the Advanced Options button and select Remove. If applicable, select the option to remove the database and logs directories. This will stop the running instance (if it is running) and remove all files related to the instance from the local file system.

#### Remaining artifacts

Even after a ServiceControl instance has been removed, there are artifacts left behind. Each ServiceControl instance leaves queues in the configured transport. The queue names will depend on the configuration of the ServiceControl instance.

* _instance name_ - control messages for the ServiceControl instance
* _instance name_.errors - control messages that the ServiceControl instance was not able to process
* _instance name_.staging - temporary failed messages while they are being retried
* _audit queue name_ - messages that have been processed by endpoints and then forwarded to the audit queue. These messages have not yet been ingested by the ServiceControl instance.
* _error queue name_ - messages that failed processing in endpoints and have passed immediate and delayed retries. These messages have not yet been ingested by the ServiceControl instance.
* _audit log queue name_ if audit log forwarding was configured - messages that have been ingested from the audit queue, processed by this ServiceControl instance, and then forwarded to the audit log queue.
* _error log queue name_ if error log forwarding was configured - messages that have been ingested from the error queue, processed by this ServiceControl instance, and then forwarded to the error log queue.

If the option to delete the database/log folders was not selected when removing the instance, then these folders and their contents are left on disk.

NOTE: If the instance was configured to run under a service account then that account may have been granted _Logon as a Service_ privileges. This is not reversed when the instance is removed.

### Uninstall ServiceControl Management

To uninstall ServiceControl Management, use the `Apps & features` settings in Windows.

NOTE: Uninstalling the ServiceControl Management application will not remove existing instances. Remove all ServiceControl instances using ServiceControl Management before uninstalling the application itself.
