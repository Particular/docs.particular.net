---
title: Installing ServiceControl Monitoring Instances
reviewed: 2021-04-22
---

The ServiceControl installation package includes a utility to manage the installation, upgrade, and removal of ServiceControl services, including monitoring instances. This utility is launched as the final step in the installation process and is also available via the Windows Start Menu.

## Prerequisites

The ServiceControl installation has the following prerequisites:

* [Microsoft .NET 4.7.2 Runtime](https://dotnet.microsoft.com/download/dotnet-framework/net472)

NOTE: Each environment should contain a single [ServiceControl instance](/servicecontrol/servicecontrol-instances/) and a single [monitoring instance](/servicecontrol/monitoring-instances/). In high-throughput scenarios, it is recommended that these instances each run on a separate dedicated machine.


## Transport support

Monitoring supports the same transports as [Service Control](/servicecontrol/installation.md#transport-support).

## Using ServiceControl Management to upgrade monitoring instances

ServiceControl Management provides a simple means of setting up one or more monitoring instances.

WARNING: The ability to add multiple instances is primarily intended to assist development and test environments.

ServiceControl Management can be launched automatically at the end of the installation process to enable adding or upgrading ServiceControl or monitoring instances.

ServiceControl Management will display the list of installed ServiceControl and monitoring instances. If the version of the binaries used by an instance are older than those shipped with ServiceControl Management, an upgrade link will be shown against the version label.

![](/servicecontrol/managementutil-upgradelink.png 'width=500')

To upgrade the service, click the upgrade link next to the service name.

Clicking the upgrade link will:

* Prompt for any new information that is required to run properly on the transport of choice.
* Stop the monitoring instance Windows Service.
* Remove the old binaries.
* Run the new binaries to upgrade the instance by applying new settings and/or creating any required additional queues.
* Start the monitoring instance Windows Service.


## Using ServiceControl Management to add monitoring instances

Click on the `+ NEW` link at the top of the screen and select "Monitoring instance" to launch the "New instance form". Complete the form to register a new monitoring instance service.


## Service name and plugins

When adding the first monitoring instance service, the default service name is `Particular.Monitoring`. It is possible to change this name to a custom service name. In doing so, this is also changing the queue name associated with this instance.

The metrics endpoint plugin will need the monitoring instance name to send metric data to the monitoring instance.
