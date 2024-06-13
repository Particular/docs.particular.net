---
title: Deploying ServiceControl Error instances using SCMU
summary: How to install different types of ServiceControl instances
reviewed: 2024-06-14
component: ServiceControl
related:
 - servicecontrol/servicecontrol-instances/hardware
 - servicecontrol/capacity-and-planning
redirects:
 - servicecontrol/multi-transport-support
---

//TODO: Review/Editorialize. Some of this should be in the index and less specific to SCMU deployments

Every component in the Particular Service Platform (not including NServiceBus), including ServiceControl, must be [downloaded](https://particular.net/downloads) and installed.

#if-version [,5)
After installation, there is no ServiceControl instance running yet. Instances can be installed, upgraded, and removed using the ServiceControl Management Utility. This utility is launched as the final step in the installation process and is also available via the Windows start menu.
#end-if

## Install methods

ServiceControl can be installed in multiple ways. This article describes using the visual ServiceControl Management application. Scripted installs and upgrades can be done via the [ServiceControl Management PowerShell module](powershell.md). Advanced installation guidance to support high-load and high-availability is available on [active/active remote setups](/servicecontrol/servicecontrol-instances/remotes.md) and [active/passive clustering](/servicecontrol/deploying-servicecontrol-in-a-cluster.md).

There is also a [community-managed puppet module](https://forge.puppet.com/tragiccode/nservicebusservicecontrol).

partial: prereqs //TODO move file, appropriate here?

## Planning

> [!NOTE]
> In production environments, make sure to review the [environment considerations](/servicecontrol/servicecontrol-instances/hardware.md) when setting up a machine with ServiceControl.

The ServiceControl Management Utility provides a simple way to set up one or more ServiceControl instances (error, audit, and monitoring). For production systems, it is recommended to limit the number of instances per machine to one of each type. The ability to add multiple instances *of the same type on a single machine* is primarily intended to assist development and test environments.

See [ServiceControl Capacity Planning](/servicecontrol/capacity-and-planning.md) and [Hardware Considerations](/servicecontrol/servicecontrol-instances/hardware.md) for more guidance.

## Installing ServiceControl instances

There are [three types](/servicecontrol/#servicecontrol-instance-types) of ServiceControl instances that can be installed using the ServiceControl Management utility. As the error and audit instance usually go side-by-side, they are installed and configured at the same time.

1. Open the ServiceControl Management Utility.
2. Click the `New` button at the top-right and a popup window appears.
3. Select either `Add ServiceControl and Audit instances` or `Add monitoring instance`.
4. Provide a name and transport.
   1. The default name is `Particular.ServiceControl`.
      The name is used to derive names for the error and audit instances. The name of each instance can be adjusted from its default if required.
   2. The transport should match the endpoint's transport.
      If a transport is not available, a [transport adapter](/servicecontrol/transport-adapter/incompatible-features.md) can be used.
5. For additional instance settings, open the `ServiceControl` and `ServiceControl Audit` sections.
   1. The name of the instance can be modified; the defaults for error and audit are `Particular.ServiceControl` and `Particular.ServiceControl.Audit`.
      1. The name of the error instance is especially important to [enable plugins to send information](/servicecontrol/servicecontrol-instances/configuration.md<!-- //TODO #servicecontrolinternalqueuename -->) to ServiceControl.
      2. If multiple instances for different systems are installed on the same server, a name like `Particular.SystemName` is a common option.
   2. Select the appropriate user account.
      Note that ServiceControl instances will run as Windows Services in the background.
   3. Be aware of the port numbers as these are used by ServicePulse and ServiceInsight to connect to ServiceControl.
   4. Configure the [retention period](/servicecontrol/how-purge-expired-data.md) for each instance.
   5. Configure the name of the queue that messages should arrive in.
      This queue is important to endpoints that send error and audit messages to these ServiceControl instances, as well as [plugins](/servicecontrol/servicecontrol-instances/configuration.md<!-- //TODO #servicecontrolinternalqueuename -->).
   6. If needed, configure [forwarding queues](/servicecontrol/errorlog-auditlog-behavior.md).
   7. Full-text search can be turned off for [performance reasons](/servicecontrol/capacity-and-planning.md#storage-performance) if it's not needed.

A monitoring instance differs from error and audit instances in its configuration:

1. There is no configuration for storage as it only stores data in memory.
1. There is no forwarding queue as the messages are specific to ServiceControl monitoring.
1. The queue to forward messages that can't be processed to is different, as described in [monitoring setup](/tutorials/monitoring-setup/#component-overview).

As an example, after configuring an error, audit and monitoring instance for the SQL Server transport, the ServiceControl Management Utility will look similar to this:

![ServiceControl Management interface that shows the link for upgrading](/servicecontrol/managementutil.png 'width=500')

## Upgrading ServiceControl instances

ServicePulse will show a notification when a new version of ServiceControl is available. New versions can then be [downloaded](https://particular.net/downloads) and installed.

The ServiceControl Management Utility displays the instances of the ServiceControl service installed on the current machine. If a ServiceControl instance is running on an outdated version, an upgrade link will be shown next to the version label.

![ServiceControl Management interface that shows the link for upgrading](/servicecontrol/managementutil-upgradelink.png 'width=500')

To upgrade the service, click the upgrade link next to the service name.

Clicking the upgrade link will:

* Prompt for additional required information, i.e. new mandatory settings introduced in the newer version
* Stop the service
* Remove the old binaries for ServiceControl and the configured transport
* Run the new binaries to create required queues
* Start the service

## Upgrading multiple major versions

A ServiceControl installation should be upgraded one major version at a time. Check the currently installed version of ServiceControl to look up the upgrade path in the table below. For each entry in the upgrade path, install the listed version and upgrade all ServiceControl instances to that version. Once all instances are upgraded and running, install the listed version in the next row until all instances are upgraded to the latest version.

| Current Version | Upgrade Path                                       |
|--------------------------------|------------------------------------------|
| 1.x.y | Install [1.48.0](https://github.com/Particular/ServiceControl/releases/tag/1.48.0), and update all instances|
| 2.x.y | Install [2.1.5](https://github.com/Particular/ServiceControl/releases/tag/2.1.5), and update all instances|
| 3.x.y | Install [3.8.4](https://github.com/Particular/ServiceControl/releases/tag/3.8.4), and update all instances|
| 4.x.y | Install [4.33.0](https://github.com/Particular/ServiceControl/releases/tag/4.33.0), and update all instances|
| 5.x.y | Download the [latest release](https://particular.net/start-servicecontrol-download), and update all instances|

All versions are available at <https://github.com/Particular/ServiceControl/releases>

> [!NOTE]
> Upgrades might take a while to run. Account for the unavailability of ServiceControl and plan the upgrade during maintenance windows if necessary.

## ServiceControl plugins

Endpoint plugins like heartbeats and custom checks require sending information to ServiceControl. The name of the queue is the same name as the error instance.

See [Install Heartbeats Plugin](/monitoring/heartbeats/install-plugin.md) and [Install Custom Checks Plugin](/monitoring/custom-checks/install-plugin.md) for more information.

## Migrating to a new host

To migrate an instance of ServiceControl to another host:

1. Ensure the same Windows version is used on the new machine as storage is not compatible between Windows versions
1. Stop and disable the current instance of ServiceControl in the Windows Service Manager
1. Install and configure a new instance of ServiceControl using the same name on the new server
1. Ensure the name is identical
1. Stop the newly installed instance of ServiceControl
1. Copy the data from the previous instance to the new instance server (moving the data is not recommended)
1. Start the new service
1. Remove the old instance of ServiceControl

> [!NOTE]
> If the database of the instance being migrated is very large, or no downtime can be tolerated, or the destination uses a different Windows version, or the instance uses a different name, then consider [scaling ServiceControl out via remote instances](/servicecontrol/servicecontrol-instances/remotes.md).

Things to remember:

* Ensure the instance being migrated is on the [latest version](https://github.com/Particular/ServiceControl/releases).
* Optionally, use the same installer as the current version. Database schemas are not guaranteed to be compatible between versions.
* Optionally, [script ServiceControl installations via powershell](/servicecontrol/servicecontrol-instances/deployment/powershell.md) instead of running installers manually

ServiceControl configuration settings are accessible via the Service Control Management Utility or by navigating to the configuration files on the file system stored in `ServiceControl.exe.config`, `ServiceControl.Audit.exe.config`, and `ServiceControl.Monitoring.exe.config`.

> [!NOTE]
> If ServiceControl was previously installed via the ServiceControl Management Utility then all instances are installed on a single machine by default. If the system has considerable load or has a large retention period, consider installing a single instance type on a server to scale out. This can be done via Powershell.

> [!WARNING]
> Take care when planning to move ServiceControl from one server to another. Moving databases between servers can be challenging. The embedded RavenDB does not support moving from a new version of Windows back to older versions of Windows. See [Getting error while restoring backup file in raven DB](https://stackoverflow.com/questions/25625910/getting-error-while-restoring-backup-file-in-raven-db) for more details.

partial: removing //TODO: Move file, appropriate here?

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

> [!NOTE]
> If the instance was configured to run under a service account then that account may have been granted _Logon as a Service_ privileges. This is not reversed when the instance is removed.

#if-version [,5)

### Uninstall the ServiceControl Management Utility

To uninstall the ServiceControl Management Utility, use the `Apps & features` settings in Windows.

> [!NOTE]
> Uninstalling the ServiceControl Management Utility will not remove existing instances. Remove all ServiceControl instances using the ServiceControl Management Utility before uninstalling the application itself.

#end-if
