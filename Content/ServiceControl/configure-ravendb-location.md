---
title: Customize RavenDB Embedded path and drive
summary: How to configure ServiceControl to save data in a different location other than the deault in order to have more space available for monitored data
tags:
- ServiceControl
- RavenDB Embedded
- Configuration
---
ServiceControl stores its own data in a RavenDB Embedded instance that by default is located in:

`%SystemDrive\ProgramData\Particular\ServiceControl\localhost-33333`

In many scenarios it may be is desirable to place the Embedded RavenDB database in a different location (for example: if the system drive storage capacity is not sufficient).

### Set a different location for RavenDB Embedded

In order to configure ServiceControl to store data in a different location right after the installation it is enought to:

* stop the ServiceControl service;
* locate the ServiceControl.dll.config configuration file in the ServiceControl installation folder;
* edit the configuration file to add a new setting:

	`xml
	<add key="ServiceControl/DbPath" value="X:\your\new\path" />
	`
* start the ServiceControl service;

### Move an existing installation to a different location

There are situations where it is required to move ServiceControl data directory presenving already collected data, the procedure is the same as the one to configure ServiceControl from scratch, the only additional step, befor starting the ServiceControl service, is to move the existing data directory to the new location.