---
title: How to backup ServiceControl database
summary: How to backup the ServiceControl RavenDB Embeeded instance
tags:
- ServiceControl
- RavenDB
- Backup
---
ServiceControlo utilizes, as storage backend RavenDB Embedded, in order to backup ServiceControl data proceed as following.

### Backup

* Stop the ServiceControl service;
* copy the ServiceControl data directory, that by default is located in `%SystemDrive%\ProgramData\Particular\ServiceControl\localhost-33333`;
* Start the ServiceControl service;

### Restore

* Stop the ServiceControl service;
* copy the previouly backed-up data into the ServiceControl data directory, that by default is located in `%SystemDrive%\ProgramData\Particular\ServiceControl\localhost-33333`;
* Start the ServiceControl service;

### Notes

--------

* The location of the RavenDB embedded folder by default
* The directory path localhost-33333 will change based on customizing port numbers of host names
* Comment on the fact that this backup method indeed requires a momentary shutdown of the SC service, but due to the nature of async messaging, no data will be lost