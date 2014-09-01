---
title: Backup the ServiceControl Data
summary: How to backup the ServiceControl RavenDB Embedded instance
tags:
- ServiceControl
- RavenDB
- Backup
---
ServiceControl utilizes an embedded instance of RavenDB for data storage. To backup this database instance, proceed as follows.

### Backup

1. Stop the ServiceControl service using the following command from an elevated command prompt `net stop "Particular ServiceControl"` or manually via the "Services" management console snap-in.
1. Copy the ServiceControl data directory, which by default is located in `%SystemDrive%\ProgramData\Particular\ServiceControl\localhost-33333`.
1. Start the ServiceControl service `net start "Particular ServiceControl"` or manually via the "Services" management console snap-in.

NOTE: The default database location changes if ports and hostname are customized.

### Restore

1. Stop the ServiceControl service using the following command from an elevated command prompt `net stop "Particular ServiceControl"` or manually via the "Services" management console snap-in.
1. Copy the previously backed-up data into the ServiceControl data directory, which by default is located in `%SystemDrive%\ProgramData\Particular\ServiceControl\localhost-33333`.
1. Start the ServiceControl service `net start "Particular ServiceControl"`Stop the ServiceControl service using the following command from an elevated command prompt `net stop "Particular ServiceControl"` or manually via the "Services" management console snap-in.

### Important Notes

While backup is in progress, the ServiceControl service must be stopped. During this time, ServiceControl data collection pauses and no connections can be made from ServicePulse, ServiceInsight, or any custom software that depends on the ServiceControl API endpoints.

Due to the nature of messaging, messages directed to ServiceControl remain in the queues until the ServiceControl service restarts, when normal message processing is restored and all the pending messages are processed, with no loss of information.
