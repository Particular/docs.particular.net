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