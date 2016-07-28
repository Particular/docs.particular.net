---
title: Customize RavenDB Embedded Location
summary: Increase space for monitored data by configuring ServiceControl to save data in a different location other than the default.
tags:
- ServiceControl
- RavenDB Embedded
---

Each ServiceControl service stores its data in a RavenDB embedded database. The location of the database is set at install time.

To see the current database location, open the ServiceControl Management Utility and the location is listed under the instance details.

![](managementutil-instance-datapath.png 'width=500')


### Setting a Different Location for RavenDB Embedded Database

The ServiceControl Management Utility does not provide a means of moving the ServiceControl database. To move the database to a different disk location follow this process:

 * Open the ServiceControl Management Utility
 * Stop the service from the provided options
 * The current database path will be listed in the utility. Copy the contents of this folder to the new location
 * Ensure that the service account used has read/write access to the new location.
 * Manually edit the configuration and specify the new location by changing/adding the `ServiceControl\DBPath` setting. See [Configuration Settings](creating-config-file.md)
 * Restart the Service
 * Remove the old database directory and contents

This will generate a new instance of the RavenDB embedded instance in the specified path.

NOTE: The ServiceControl process must have read/write access to the specified path.