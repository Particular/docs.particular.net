---
title: SQL Persistence Upgrade Version 1.0.0 to 1.0.1
summary: Instructions on how to upgrade to SQL Persistence version 1.0.1
reviewed: 2017-02-27
component: SqlPersistence
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
---



## Convert Outbox Index to Nonclustered

WARNING: This upgrade is only required by Endpoints that are using both [Microsoft SQL Server](/nservicebus/sql-persistence/#usage-sql-server) and [Outbox](/nservicebus/outbox/).

NOTE: This is a optional performance optimization that is only necessary for high throughput endpoints. All new endpoints created with Version 1.0.1 and above will have this optimization applied.

As the `MessageId` is not guaranteed to be sequential a [nonclustered index](https://msdn.microsoft.com/en-AU/library/ms190457.aspx) gives better performance. Applying this change results in the table being treated as a [heap](https://msdn.microsoft.com/en-AU/library/hh213609.aspx).


### Performing the upgrade

Perform the steps described in this section for all endpoints that that are using [Microsoft SQL Server](/nservicebus/sql-persistence/#usage-sql-server), [Outbox](/nservicebus/outbox/) and Sql Persistence Version 1.0.0.

NOTE: Since Version 1.0.1 does not require the nonclustered index to function, the conversion of indexes over to nonclustered can be done before **or** after the upgrade to 1.0.1.


#### Stop endpoint

Stop the affected endpoint.

NOTE: This process can be done on a per-endpoint basis or in bulk for all affected endpoints.


#### Convert to Nonclustered

Run the following upgrade script

snippet: ConvertOutboxToNonclustered

This script takes a [tablePrefix](/nservicebus/sql-persistence/#installation-table-prefix) as a parameter and then performs the following actions:

 * Find the index name by querying [sys.tables](https://msdn.microsoft.com/en-us/library/ms187406.aspx) and [sys.indexes](https://msdn.microsoft.com/en-us/library/ms173760.aspx).
 * Execute a dynamic [DROP CONSTRAINT](https://docs.microsoft.com/en-us/sql/relational-databases/tables/delete-check-constraints) command.
 * Execute a dynamic [ADD CONSTRAINT](https://msdn.microsoft.com/en-us/library/ms190024.aspx) command.

This script can be executed as part of a deployment using the following code:

snippet: ExecuteConvertOutboxToNonclustered


#### Start endpoint

Start the effected endpoint.
