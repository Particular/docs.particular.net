---
title: RavenDB Embedded Location
summary: Increase space for monitored data by configuring ServiceControl to save data in a location other than the default
reviewed: 2020-06-23
---

Each ServiceControl service stores its data in a RavenDB embedded database. The location of the database is set at install time.

To see the current database location, open ServiceControl Management and view the location listed in the instance details.

![](managementutil-instance-datapath.png 'width=500')


### Setting a different location for the RavenDB embedded database

ServiceControl Management does not provide a means of moving the ServiceControl database. To move the database to a different disk location, follow this process:

 * Open ServiceControl Management
 * Stop the service
 * The current database path is listed in the utility. Copy the contents of this directory to the new location
 * The new database location should not be a subfolder of one of the existing locations (e.g. Installation path, Log Path, etc)
 * Ensure that the service account used for ServiceControl has read/write access to the new location.
 * Manually edit the configuration and specify the new location by changing or adding the `ServiceControl\DBPath` setting. See [Configuration Settings](creating-config-file.md)
 * Restart the ServiceControl service
 * Remove the old database directory and contents

This will generate a new instance of the RavenDB embedded instance in the specified path.
