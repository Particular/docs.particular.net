---
title: Automatic Expiration of Data
summary: Configuring ServiceControl's data retention policy
related:
 - nservicebus/recoverability
 - nservicebus/operations/auditing
 - servicecontrol/audit-instances/persistence
reviewed: 2022-10-18
---

ServiceControl stores audit and error data. Any audit and error data that is older than the specified thresholds is deleted from the embedded RavenDB. The expiration thresholds for both faulted and audited messages must be set during installation. These values can be modified later by launching ServiceControl Management and editing the configuration settings for the instance.

Note: The expiration process curates only the data in the embedded RavenDB. Audit and error forwarding queues are not curated or managed by ServiceControl. To turn these settings off, launch ServiceControl Management and edit the configuration settings for the instance.

Warning: The database will not automatically shrink in size after reducing the retention period. Ensure ServiceControl had time to purge all expired messages and then [Compact the database](db-compaction.md).

## Differences in purge implementations

Expiration of error and audit data is implemented by a reccuring task that checks the documents for old data and deletes documents that have expired. Changing the expiration setting results in it being picked up during the next time the service is run. 
In ServiceControl.Audit version 4.26 and above expiration is handled by the database automatically. Each audited message contains a metadata key that tells the database when to remove it. When the expiration setting changes, the new value is only applied to new audit data. Existing audit meta is not changed. Messages already in the database are not affected by the change to the expiration setting.
