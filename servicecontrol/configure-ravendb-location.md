---
title: RavenDB Embedded Location
summary: Increase space for monitored data by configuring ServiceControl to save data in a location other than the default
reviewed: 2023-07-08
---

ServiceControl [Error](/servicecontrol/servicecontrol-instances/) and [Audit](/servicecontrol/audit-instances/) instances deployed via the ServiceControl Management utility or the ServiceControl PowerShell module stores their data in a RavenDB embedded database. The location of the database is set at install time.

To see the current database location, open ServiceControl Management and view the location listed in the instance details. <!-- TODO: PowerShell instructions -->

![](managementutil-instance-datapath.png 'width=500')

### Setting a different location for the RavenDB embedded database

The ServiceControl Management utility and the ServiceControl PowerShell module do not provide a means of moving the ServiceControl database. To move the database to a different disk location, follow this process:

1. Stop the ServiceControl instance service. If deployed using the ServiceControl Management utility this can be done in that tool.
1. The current database path is listed in the utility. Copy the contents of this directory to the new location. <!-- TODO: How to find the path in Powershell? -->
1. The new database location should not be a subfolder of one of the existing locations (e.g. Installation path, Log Path, etc).
1. Ensure that the service account used for ServiceControl has read/write access to the new location.
1. Manually edit the configuration and specify the new location by changing or creating the `ServiceControl\DBPath` setting for [Error](/servicecontrol-instances/configuration.md#host-settings-servicecontroldbpath) or [Audit](/servicecontrol/audit-instances/configuration.md#database-servicecontrol-auditdbpath) instances.
1. Start the ServiceControl instance service.
1. Remove the old database directory and contents.
