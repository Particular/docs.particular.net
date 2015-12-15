---
title: Customize RavenDB Embedded Location
summary: Increase space for monitored data by configuring ServiceControl to save data in a different location other than the default.
tags:
- ServiceControl
- RavenDB Embedded
---

Each ServiceControl service stores its data in a RavenDB embedded database. The location of the database is typically set in the configuration file by the `ServiceControl\DBPath` setting. See [Configuration Settings](creating-config-file.md)

If the setting is not specified then ServiceControl will store the database under `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\` in a sub folder based on the current port and host name. So using the default settings an instance would use `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\localhost-33333` as the path for the database.

If you are manually manipulating the configuration of ServiceControl this needs to be taken into account as a change in port and host name would cause ServiceControl to create a new database if the `ServiceControl\DBPath` is not set.  The new ServiceControl Management Utility that ships with ServiceControl 1.7 sets the database location when creating or modifying the configuration to avoid this dynamic change.

### Setting a Different Location for RavenDB Embedded Database

The ServiceControl Management Utility does not provide a means of moving the ServiceControl database.  To move the database to a different disk location follow this process:

 * Open the ServiceControl Management Utility 
 * Stop the service from the provided options
 * The current database path will be listed in the utility.  Copy the contents of this folder to the new location
 * Ensure that the service account used has read/write access to the new location.
 * Manually edit the configuration and specify the new location by changing/adding the `ServiceControl\DBPath` setting. See [Configuration Settings](creating-config-file.md)
 * Restart the Service
 * Remove the old database directory and contents
 
This will generate a new instance of the RavenDB embedded instance in the specified path.

NOTE: The ServiceControl process must have read/write access to the specified path.