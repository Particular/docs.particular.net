---
title: SQL Persistence Upgrade Version 1.0.0 to 1.0.1
summary: Instructions on how to upgrade to SQL Persistence version 1.0.1
reviewed: 2020-08-24
component: SqlPersistence
isUpgradeGuide: true
redirects:
 - nservicebus/upgrades/sqlpersistence-1.0.0-1.0.1
 - persistence/upgrades/sqlpersistence-1.0.0-1.0.1
upgradeGuideCoreVersions:
 - 6
---



## Convert Outbox Index to Nonclustered

WARNING: This upgrade is only required by Endpoints that are using both [Microsoft SQL Server](/persistence/sql/dialect-mssql.md) and [Outbox](/nservicebus/outbox/).

NOTE: This is a optional performance optimization that is only necessary for high throughput endpoints. All new endpoints created with Version 1.0.1 and above will have this optimization applied.

As the `MessageId` is not guaranteed to be sequential a [nonclustered index](https://docs.microsoft.com/en-us/sql/relational-databases/indexes/clustered-and-nonclustered-indexes-described) gives better performance. Applying this change results in the table being treated as a [heap](https://docs.microsoft.com/en-us/sql/relational-databases/indexes/heaps-tables-without-clustered-indexes).


### Performing the upgrade

Perform the steps described in this section for all endpoints that that are using [Microsoft SQL Server](/persistence/sql/dialect-mssql.md), [Outbox](/nservicebus/outbox/) and Sql Persistence Version 1.0.0.

NOTE: Since Version 1.0.1 does not require the nonclustered index to function, the conversion of indexes over to nonclustered can be done before **or** after the upgrade to 1.0.1.


#### Stop endpoint

Stop the affected endpoint.

NOTE: This process can be done on a per-endpoint basis or in bulk for all affected endpoints.


#### Convert to Nonclustered

Run the following upgrade script

snippet: ConvertOutboxToNonclustered

This script takes a [tablePrefix](/persistence/sql/install.md#table-prefix) as a parameter and then performs the following actions:

 * Find the index name by querying [sys.tables](https://docs.microsoft.com/en-us/sql/relational-databases/system-catalog-views/sys-tables-transact-sql) and [sys.indexes](https://docs.microsoft.com/en-us/sql/relational-databases/system-catalog-views/sys-indexes-transact-sql).
 * Execute a dynamic [DROP CONSTRAINT](https://docs.microsoft.com/en-us/sql/relational-databases/tables/delete-check-constraints) command.
 * Execute a dynamic [ADD CONSTRAINT](https://docs.microsoft.com/en-us/sql/relational-databases/tables/create-unique-constraints) command.

This script can be executed as part of a deployment using the following code:

snippet: ExecuteConvertOutboxToNonclustered


#### Start endpoint

Start the effected endpoint.
