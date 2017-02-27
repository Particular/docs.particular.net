---
title: Sql Persistence Upgrade Version 1.0.0 to 1.0.1
summary: Instructions on how to patch SQL injection vulnerability in SQL Server Transport version 1.x
reviewed: 2017-02-27
component: SqlPersistence
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
---



## Convert Outbox Index to Nonclustered

WARNING: Only required by Endpoints that are using both [Microsoft SQL Server](/nservicebus/sql-persistence/#usage-sql-server) and [Outbox](/nservicebus/outbox/).


As the `MessageId` is not guaranteed to be sequential a [Nonclustered Index](https://msdn.microsoft.com/en-AU/library/ms190457.aspx) gives better performance. Applying this change results in the table being treated as a [Heap](https://msdn.microsoft.com/en-AU/library/hh213609.aspx).


### Performing the upgrade

For all endpoint that that are using both [Microsoft SQL Server](/nservicebus/sql-persistence/#usage-sql-server) and [Outbox](/nservicebus/outbox/) and have been deployed using the 1.0.0 version of the Sql Persistence NuGet package.

NOTE: Since Version 1.0.1 does not require the Nonclustered to function, the conversion of indexes over to Nonclustered can be done before **or** after the upgrade to 1.0.1.


#### Stop endpoint

Stop the effected endpoint.

NOTE: This process can be done on a per-endpoint basis or in bulk for all effected endpoints.


#### Convert to Nonclustered

Run the following upgrade script

snippet: ConvertOutboxToNonclustered

This script takes a [tablePrefix](/nservicebus/sql-persistence/#installation-table-prefix) as a parameter and then performs the following actions:

 * Find the index name by querying [sys.tables](https://msdn.microsoft.com/en-us/library/ms187406.aspx) and [sys.indexes](https://msdn.microsoft.com/en-us/library/ms173760.aspx).
 * Execute a dynamic [DROP CONSTRAINT](https://msdn.microsoft.com/en-us/library/ms187626.aspx) command.
 * Execute a dynamic [Add CONSTRAINT](https://msdn.microsoft.com/en-us/library/ms190024.aspx) command.

This script can be executed as part of a deployment using the following code:

snippet: ExecuteConvertOutboxToNonclustered


#### Start endpoint

Start the effected endpoint.
