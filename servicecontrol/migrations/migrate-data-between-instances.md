---
title: Migrate ServiceControl data between instances
summary: Instructions on how to migrate ServiceControl data between instances.
reviewed: 2025-06-12
component: ServiceControl
related:
    - servicecontrol/migrations/replacing-audit-instances
    - servicecontrol/migrations/replacing-error-instances
    - servicecontrol/backup-sc-database
redirects:
    - servicecontrol/data-migration
---

ServiceControl, which exists to serve the management of distributed systems, is itself a distributed system. As a result, migrating all ServiceControl data between deployments means each piece of the system that accesses and writes data is migrated separately.

This document describes in general terms how to migrate ServiceControl data, and links to more specific information on how to accomplish data migration for each piece of the system.

## Overview

ServiceControl data consists of audit and error message data, managed by [audit instances](/servicecontrol/audit-instances/) and [error instances](/servicecontrol/servicecontrol-instances/) respectively. To migrate all ServiceControl data from one deployment to another means replacing both audit and error instances.

[Monitoring instances](/servicecontrol/monitoring-instances/) do not use a persistent data store, and therefore do not require data migration.

> [!NOTE]
> It is worth assessing whether audit and error message data retention is required. For scenarios where retaining audit and error message data is not required (e.g. transient or test environment data that does not merit effort to retain), this process is not necessary.

### Audit data

Migrating audit data involves:

1. Adding the new audit instance as a remote
2. Disabling audit queue ingestion on the old audit instance
3. Decommission of the old audit instance _**after**_ all audit information is expired

Follow the [detailed audit instance replacement process](/servicecontrol/migrations/replacing-audit-instances/) to accomplish migration of this data.

### Error data

Migrating error data involves:

1. Disabling error message ingestion so that new error messages will be temporarily held in the error queue
2. Retrying or archiving any failed messages so that the old error instance contains only ephemeral data like heartbeats and custom checks
3. Creating a new error instance configured to use the same audit instance(s)

Follow the [detailed error instance replacement process](/servicecontrol/migrations/replacing-error-instances/) to accomplish migration of this data.

## Alternative migration strategies

Data migration can also be achieved by [backing up the ServiceControl data](/servicecontrol/backup-sc-database.md) and restoring it to the new instance's deployment.

> [!WARNING]
> The [restrictions](/servicecontrol/backup-sc-database.md#important-notes-and-restrictions) should be strongly considered before moving forward with this approach.
