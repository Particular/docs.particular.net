---
title: Installing ServiceControl
reviewed: 2020-03-16
redirects:
 - servicecontrol/multi-transport-support
---

Every component in the Particular Service Platform, including ServiceControl, needs to be [downloaded](https://particular.net/downloads) and installed.

After installation there is no actual ServiceControl instance running yet. Those instances can be installed, upgraded and removed with the ServiceControl Management Utility. This utility is launched as the final step in the installation process and is also available via the Windows Start Menu.

NOTE: A [community managed puppet module](https://forge.puppet.com/tragiccode/nservicebusservicecontrol) is available to install ServiceControl.

## Prerequisites

The ServiceControl installation has the following prerequisites:

* [Microsoft .NET 4.7.2 Runtime](https://dotnet.microsoft.com/download/dotnet-framework/net472)

## Planning

WARNING: In production be aware of the possible considerations when setting up a machine with ServiceControl.

ServiceControl Management provides a simple means of setting up one or more instances of ServiceControl service types (error, audit, and monitoring). For production systems, it is recommended to limit the number of instances per machine to one. The ability to add multiple instances *of the same type on a single machine* is primarily intended to assist development and test environments.

See [ServiceControl Capacity Planning](capacity-and-planning.md) and [Hardware Considerations](/servicecontrol/servicecontrol-instances/hardware.md) for more guidance.

## Installing ServiceControl instances

There are [three types](/servicecontrol/#servicecontrol-instance-types) of ServiceControl instances that can be installed using the ServiceControl Management utility. As the error and audit instance usually go side-by-side, they are installed and configured at the same time.

1. Open the ServiceControl Management Utility in the Windows start menu.
1. Click the `New` button at the top-right and a popup window appears.
1. Select either `Add ServiceControl and Audit instances` or `Add monitoring instance`.
1. Provide a name and transport.
   1. The default name commonly used is `Particular.ServiceControl`.  
      It has no real use except for the fact that it is being used to finalize the name for the error and audit instances. The name of each instance can be adjusted from its default if required.
   1. The transport should be the same as the transport the endpoints are using.  
      If a transport is not available, a [transport adapter](/servicecontrol/transport-adapter/incompatible-features.md) can be used.
1. For additional instance settings, open the `ServiceControl` and `ServiceControl Audit` sections.
   1. Here the name of the instance can be altered, the defaults for error and audit are `Particular.ServiceControl` and `Particular.ServiceControl.Audit`.
      1. The name for the error instance is particularly important to [enable plugins to send information](/servicecontrol/installation.md#servicecontrol-plugins) to ServiceControl.
      1. If multiple instances for different systems are installed on the same server, a name like `Particular.SystemName` could be an option as well.
   1. Select the appropriate user account.  
      Be aware that ServiceControl instances will run as Windows Services in the background.
   1. Be aware of the port numbers as these are used by ServicePulse and ServiceInsight to connect to ServiceControl.
   1. Configure the [retention period](/servicecontrol/how-purge-expired-data.md) for each instance.
   1. Configure the name of the queue that messages should arrive in.  
      This queue is important to endpoints that should send error and audit messages to these ServiceControl instances, or for [plugins](/servicecontrol/installation.md#servicecontrol-plugins).
   1. Potentially configure [forwarding queues](/servicecontrol/errorlog-auditlog-behavior.md).
   1. Full-text search can be turned off for [performance reasons](/servicecontrol/capacity-and-planning.md#storage-performance).

A monitoring instance differs from error and audit instances in its configuration:

1. Configuration
   1. There is no configuration for storage as it only stores data in-memory.
   1. There is no forwarding queue as the messages are specific to ServiceControl monitoring.
   1. The queue to forward messages to that can't be processed, as described in [monitoring setup](/tutorials/monitoring-setup/#component-overview).

After configuring an error, audit and monitoring instance for the SQL Server transport, the ServiceControl Management Utility should look similar to this:

![ServiceControl Management interface that shows the link for upgrading](managementutil.png 'width=500')

## Upgrading ServiceControl instances

Whether a new version of ServiceControl is available can be verified in the ServicePulse user interface. A new version can then be [downloaded](https://particular.net/downloads) and installed.

ServiceControl Management will display the instances of the ServiceControl service installed. If the version used by an instance are older than those that were downloaded and installed, an upgrade link will be shown next to the version label.

![ServiceControl Management interface that shows the link for upgrading](managementutil-upgradelink.png 'width=500')

To upgrade the service, click the upgrade link next to the service name.

Clicking the upgrade link will:

* Prompt for additional required information, i.e. new mandatory settings introduced in the newer version
* Stop the service
* Remove the old binaries for ServiceControl and the configured transport
* Run the new binaries to create required queues
* Start the service

## ServiceControl plugins

Endpoint plugins like heartbeat and custom checks require sending information to ServiceControl. The name of the queue is the exact name of the error instance.

See [Install Heartbeats Plugin](/monitoring/heartbeats/install-plugin.md) and [Install Custom Checks Plugin](/monitoring/custom-checks/install-plugin.md) for more information.

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
