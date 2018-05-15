---
title: ServiceControl Upgrade Version 1 to 2
reviewed: 2018-04-24
component: ServiceControl
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 5
---


## RavenDB data files

ServiceControl Version 1 uses RavenDB 2.5 while ServiceControl Version 2 uses RavenDB 3.5. Data file formats of RavenDB 2.5 and 3.5 are different and require migration process. The migration is conducted automatically by RavenDB server (embedded in the ServiceControl process) when the database engine detects that the data files are in the old format.

WARN: The migration is managed by the ServiceControl Management Utility instance upgrade process and does not require manual intervention. It is recommended, however, to [back up the RavenDB](/servicecontrol/backup-sc-database.md) database prior to attempting the upgrade.


## Database maintenance port

In Version 1 the ServiceControl running in the maintenance mode exposed the underlying RavenDB database on the same port as it exposed the API during normal operation. This approach is no longer possible with RavenDB 3.5 hence ServiceControl Version 2 requires explicit configuration of a database maintenance port which needs to be different from the API port.

ServiceControl Management Utility automatically detects the need for this new configuration setting and is going to ask the user to provide it when performing the upgrade of a ServiceControl instance.