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

**note**: the default database location will change if ports and host names are customized.

### Restore

* Stop the ServiceControl service;
* copy the previouly backed-up data into the ServiceControl data directory, that by default is located in `%SystemDrive%\ProgramData\Particular\ServiceControl\localhost-33333`;
* Start the ServiceControl service;

### Important Notes

The backup method outlined above requires ServiceControl service to be stopped during this time ServiceControl data collection is paused and no connections can be made from ServicePulse, ServiceInsight or any custom software that depends on the ServiceControl API endpoints.

Due to the nature of messaging, and ServiceControl is no exception, messages directed to ServiceControl will remain in its queues until the service is restarted, as soon as ServiceControl service is restared normal messages processing is restored and all the pending messages will be processed, without loosing any information.