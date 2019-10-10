---
title: Business Continuity / Disaster Recovery
summary: Solutions for business continuity / disaster recovery with ServiceControl
reviewed: 2019-10-08
tags:
- Backup
related:
- servicecontrol/backup-sc-database
- servicecontrol/deploying-servicecontrol-in-a-cluster
---

Multiple ServiceControl instances can be used to create a business continuity / disaster recovery configuration. By forwarding failed and audited messages to backup instances of ServiceControl it is possible to resume operations in the case that the primary instances are no longer available.

```mermaid
graph TD
EPB[Endpoints] 
E((error queue))
EPB --failed messages -->E
E --ingested by-->SCEA
EPB --audit messages-->A
A((audit queue))
A --ingested by-->SCAA
subgraph Primary
SCEA["ServiceControl Instance"]
SCAA["ServiceControl Audit Instance"]
end
subgraph Backup
SCEB["ServiceControl Instance"]
SCAB["ServiceControl Audit Instance"]
end
EB((error_backup<br/>queue))
SCEA --forwards to-->EB
EB --ingested by-->SCEB
AB((audit_backup<br/>queue))
SCAA --forwards to-->AB
AB --ingested by-->SCAB
```

This configuration works by combining multiple instances with [aduit log forwarding](creating-config-file.md#servicecontrolforwardauditmessages) and [error log forwarding](creating-config-file.md#servicebuserrorlogqueue). Failed and audited messages will be forwarded by the primary instances to the backup instances through log forwarding queues.

To install this configuration perform the following actions:

1. Create the standard error and audit queues.
1. Create the backup error and audit queues.
1. Set up the primary ServiceControl instances and configure them to forward errors and audit messages to the backup queues.
1. Setup the backup ServiceControl instances and configure them to use the backup queues.

WARN: Make sure the names of the backup ServiceControl instances are named differently from the primary ServiceControl instances otherwise unexpected behavior may result.

## What happens when you retry a message

Retrying a message from the primary instance will work as usual, however the backup instances will not update the status of the failed messages to retry pending. Once the successful audit message is received the backup instance will be notified as well and the failed message record in both instances will reflect the messages as having been successfully retried.

WARN: It is possible to duplicate messages when retrying from the backup instance if a retry has already been sent from the primary instance.


