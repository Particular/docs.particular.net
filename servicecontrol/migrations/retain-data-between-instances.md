---
title: Retain ServiceControl data between instances
summary: Instructions on how to retain ServiceControl data between instances.
reviewed: 2025-06-12
component: ServiceControl
related:
    - servicecontrol/migrations/replacing-audit-instances
    - servicecontrol/migrations/replacing-error-instances
    - servicecontrol/backup-sc-database
redirects:
    - servicecontrol/data-migration
---

ServiceControl, which exists to serve the management of distributed systems, is itself a distributed system. As a result, different pieces of the system manage different subsets of ServiceControl data.

> [!NOTE]
> This process is not part of standard ServiceControl instance upgrades unless noted in the [upgrade documentation](/servicecontrol/upgrades/).

Situations may arise where data retention between instances may be preferable, or even necessary instead of upgrading ServiceControl. For example:

- ServiceControl instances are deployed on aging hardware and need to be moved
- ServiceControl instances are being migrated to a different kind of deployment, such as moving from on-premises to cloud-hosted servers or from ServiceControl Management Utility installation to containers
- ServiceControl instance upgrades are not possible

This document gives an overview of strategies for retaining ServiceControl data, and links to more specific information on how to accomplish data retention for each.

## Overview

ServiceControl data consists of audit and error message data, managed by [audit instances](/servicecontrol/audit-instances/) and [error instances](/servicecontrol/servicecontrol-instances/) respectively.

[Monitoring instances](/servicecontrol/monitoring-instances/) do not use a persistent data store, and therefore have no data to retain.

## Strategies

> [!NOTE]
> It is worth assessing whether audit and error message data retention is required. For scenarios where retaining audit and error message data is not required (e.g. transient or test environment data that does not merit effort to retain), these strategies are not necessary.

Each of the strategies presented here have trade-offs:

- Instance replacement
  - Requires little to no downtime
  - Does not require ServiceControl version, instance name, and operating system to be the same
  - Requires that old instances remain in service for the duration of the audit message retention period

- Database backup and restore
  - Requires downtime from when the database is backed up to when it is restored
  - Requires ServiceControl version, instance name, and operating system to be the same
  - Allows old instance decommission immediately after the new instance is running with restored data

### Instance replacement

This strategy is an incremental approach to data retention. Data is retained by replacing audit and error instances separately and allowing new instances to access old data through remote configuration and stopping ingestion. The following instance replacement guides should be followed for this strategy.

- [Replacing audit instances](/servicecontrol/migrations/replacing-audit-instances/)
- [Replacing error instances](/servicecontrol/migrations/replacing-error-instances/)

### Database backup and restore

This strategy moves both audit and error data at the same time by [backing up the ServiceControl database](/servicecontrol/backup-sc-database.md) and restoring it to the new instance.

> [!WARNING]
> The [restrictions](/servicecontrol/backup-sc-database.md#important-notes-and-restrictions) must be considered before moving forward with this approach.
