---
title: How to backup ServiceControl database
summary: How to backup the ServiceControl RavenDB Embedded instance
tags:
- ServiceControl
- RavenDB
- Backup
---
ServiceControl uses an embedded instance of RavenDB for data storage. In order to backup this database instance proceed as following.

### Backup

* Stop the ServiceControl service;
* copy the ServiceControl data directory, that by default is located in `%SystemDrive%\ProgramData\Particular\ServiceControl\localhost-33333`;
* start the ServiceControl service;

**note**: the default database location will change if ports and host names are customized.

### Restore

* Stop the ServiceControl service;
* copy the previouly backed-up data into the ServiceControl data directory, that by default is located in `%SystemDrive%\ProgramData\Particular\ServiceControl\localhost-33333`;
* start the ServiceControl service;

### Important Notes

The backup method outlined above requires ServiceControl service to be stopped during this time ServiceControl data collection is paused and no connections can be made from ServicePulse, ServiceInsight or any custom software that depends on the ServiceControl API endpoints.

Due to the nature of messaging, messages directed to ServiceControl will remain in its queues until the service is restarted, as soon as ServiceControl service is restared normal messages processing is restored and all the pending messages will be processed, without loosing any information.