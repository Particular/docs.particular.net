---
title: Deploying ServiceControl Monitoring instances using SCMU
reviewed: 2025-04-17
redirects:
 - servicecontrol/monitoring-instances/installation
---

The ServiceControl installation package includes a utility to manage the installation, upgrade, and removal of ServiceControl instances, including monitoring instances. This utility is launched as the final step in the installation process and is also available via the Windows Start Menu.

## Usage

Use a Monitoring instance to vizualize [endpoint performance metrics](/monitoring/#endpoint-performance) in ServicePulse.

Monitoring instances are not as [resource intensive as the Error and Audit instances](/servicecontrol/servicecontrol-instances/hardware.md) since they do not rely on a RavenDB instance for storing state.

## Transport support

Monitoring supports the same transports as [ServiceControl](/servicecontrol/transports.md).

## Using ServiceControl Management to upgrade monitoring instances

ServiceControl Management provides a simple means of setting up one or more monitoring instances.

ServiceControl Management can be launched automatically at the end of the installation process to enable adding or upgrading ServiceControl or monitoring instances.

ServiceControl Management will display the list of installed ServiceControl and monitoring instances. If the version of the binaries used by an instance are older than those shipped with ServiceControl Management, an upgrade link will be shown against the version label.

![](/servicecontrol/managementutil-upgradelink.png 'width=500')

To upgrade the instance, click the upgrade link next to the instance name.

Clicking the upgrade link will:

* Prompt for any new information that is required to run properly on the transport of choice.
* Stop the monitoring instance Windows Service.
* Remove the old binaries.
* Run the new binaries to upgrade the instance by applying new settings and/or creating any required additional queues.
* Start the monitoring instance Windows Service.

## Using ServiceControl Management to add monitoring instances

Click on the `+ NEW` link at the top of the screen and select "Monitoring instance" to launch the "New instance form". Complete the form to register a new monitoring instance.

## Service name and plugins

When adding a monitoring instance, the default Windows Service name is `Particular.Monitoring`. It is possible to change this name to a custom Windows Service name. In doing so, this is also changing the queue name associated with this instance.

The [metrics endpoint plugin](/monitoring/metrics/install-plugin.md) will need the monitoring instance name to send metric data to the monitoring instance.
