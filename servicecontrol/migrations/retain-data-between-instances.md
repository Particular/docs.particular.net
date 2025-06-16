---
title: Retain ServiceControl data for new instances
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

ServiceControl, which exists to serve the management of distributed systems, is itself a distributed system. As a result, retaining all ServiceControl data from one instance to another means each piece of the system that accesses and writes data is migrated separately.

> [!NOTE]
> This process is not part of standard ServiceControl instance upgrades unless noted in the [upgrade documentation](/servicecontrol/upgrades/).

Situations may arise where data retention may be preferable, or even necessary instead of upgrading ServiceControl. For example:

- ServiceControl instances are deployed on aging hardware and need to be moved
- ServiceControl instances are being migrated to a different kind of deployment, such as moving from on-premises to cloud-hosted servers or from ServiceControl Management Utility installation to containers
- ServiceControl instance upgrades are not possible

This document describes in general terms how to retain ServiceControl data, and links to more specific information on how to accomplish data retention for each piece of the system.

## Overview

ServiceControl data consists of audit and error message data, managed by [audit instances](/servicecontrol/audit-instances/) and [error instances](/servicecontrol/servicecontrol-instances/) respectively. To retain all ServiceControl data means replacing both audit and error instances.

[Monitoring instances](/servicecontrol/monitoring-instances/) do not use a persistent data store, and therefore have no data to retain.

> [!NOTE]
> It is worth assessing whether audit and error message data retention is required. For scenarios where retaining audit and error message data is not required (e.g. transient or test environment data that does not merit effort to retain), this process is not necessary.

### Audit data

Retaining audit data involves:

1. Adding the new audit instance as a remote
2. Disabling audit queue ingestion on the old audit instance
3. Decommission of the old audit instance _**after**_ all audit information has expired

Follow the [detailed audit instance replacement process](/servicecontrol/migrations/replacing-audit-instances/) to achieve retention of this data.

### Error data

Retaining error data involves:

1. Disabling error message ingestion so that new error messages will be temporarily held in the error queue
2. Retrying or archiving any failed messages so that the old error instance contains only ephemeral data like heartbeats and custom checks
3. Creating a new error instance configured to use the same audit instance(s)

Follow the [detailed error instance replacement process](/servicecontrol/migrations/replacing-error-instances/) to achieve retention of this data.

## Alternative retention strategies

Data retention can also be achieved by [backing up the ServiceControl data](/servicecontrol/backup-sc-database.md) and restoring it to the new instance.

> [!WARNING]
> The [restrictions](/servicecontrol/backup-sc-database.md#important-notes-and-restrictions) should be strongly considered before moving forward with this approach.
