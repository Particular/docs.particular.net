---
title: ServiceControl Audit persistence
summary: Information about ServiceControl Audit persistence options.
reviewed: 2022-10-17
---

ServiceControl audit instances store their data in an RavenDB embedded database. In the ServiceControl.Audit version 4.26 and up, new instances store their data in RavenDB version 5. Instances less than version 4.26 use RavenDB version 3.5.

Upgrading ServiceControl.Audit instances into version 4.26 or higher does not change the database version. So instance that uses RavenDB version 3.5 upgraded to the newest version will still be using RavenDB version 3.5.

WARNING: The ServiceControl Audit RavenDB embedded instance is used exclusively by ServiceControl and is not intended for external manipulation or modifications.

## Document expiration

