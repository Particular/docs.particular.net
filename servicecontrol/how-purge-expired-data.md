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

Expiration of error and audit data is implemented by runnig a reccuring task that checks the documents for old data and deleting the rows that should be deleted. Changing the expiration setting results in it being picked up during the next time the service is run. 
In the ServiceControl.Audit version 4.26 and up expiration is done using the persistance mechanisms, that results in metadata key to be stored with every message. Thanks to that, purging of the data can be handled fully by database itself. The drawback is that changing of expiration setting is only applied to new rows. 
