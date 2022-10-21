---
title: ServiceControl Audit persistence
summary: Information about ServiceControl Audit persistence options.
reviewed: 2022-10-17
---

ServiceControl audit instances store their data in a RavenDB embedded database. In ServiceControl.Audit version 4.26 and above new instances use RavenDB version 5. Instances created by version 4.25 and below use RavenDB version 3.5.

Upgrading ServiceControl.Audit instances to version 4.26 or higher does not change the database version. Instances using RavenDB version 3.5, when upgraded to the newest version, will still be using RavenDB version 3.5. For more details see [upgrade guide to new persistence format](servicecontrol/upgrades/new-persistence.md)

WARNING: The ServiceControl Audit RavenDB embedded database is used exclusively by ServiceControl and is not intended for external manipulation or modifications.
