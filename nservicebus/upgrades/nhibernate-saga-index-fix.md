---
title: NHibernate Persistence - Resolving missing unique constraint on saga correlation property column #280
summary: Instructions on how to add missing unique constraints on saga correlation property column for affected versions.
component: NHibernate
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
---


## Summary

This guidance explains how add missing unique constraints on saga correlation property column

- https://github.com/Particular/NServiceBus.NHibernate/issues/280

This issue may cause duplicated rows in saga mapping tables representing single logical saga instance. The duplicated rows are inserted as a result of race condition during saga creation and missing unique constraint on correlation property column.


## Compatibility

This issue has been resolved in the following patch versions as defined in the NServiceBus [support policy](support-policy.md):

- NServiceBus.NHibernate 7.2.1
- NServiceBus.NHibernate 7.1.6


If any of the supported affected minor versions (7.2.x or 7.1.x) are used these should be updated to the latest patch release.


## Upgrade steps

Steps:

 * For all saga mapping table
    * Check if it holds any duplicated rows
    * If any exist manually merge the duplicates
    * Add unique constraint on correlation property column
 * Update endpoint to latest patch release
 * Deploy the new version

### Checking if table hold duplicates

Duplicates in saga mapping table can be detected using following query. It requires providing `sagaMappingTableName` and `correlationPropetyColumnName`:

```sql
declare @sagaMappingTableName nvarchar(max) = ...
declare @correlationPropetyColumnName nvarchar(max) = ...
declare @sql nvarchar(max)

select @sql = 'select ' + @correlationPropetyColumnName + ', count(*) as SagaRows from ' + @sagaMappingTableName + ' group by ' + @correlationPropetyColumnName + ' having count(*) > 1'
exec sp_executeSQL @sql

```

## Add unique constraint on correlation property column

NOTE: If adding unique constraint fails with `The CREATE UNIQUE INDEX statement terminated because a duplicate key was found for the object name ...` message please make sure that all duplicated rows detected have been merged.


Unique constraint on correlation property column can be added with following query. It requires providing `sagaMappingTableName` and `correlationPropetyColumnName`:

```sql
declare @sagaMappingTableName nvarchar(max) = ...
declare @correlationPropetyColumnName nvarchar(max) = ...
declare @sql nvarchar(max)


select @sql = 'alter table ' + @sagaMappingTableName + ' add unique nonclustered ( ' + @correlationPropetyColumnName + ' asc )with (pad_index = off, statistics_norecompute = off, sort_in_tempdb = off, ignore_dup_key = off, online = off, allow_row_locks = on, allow_page_locks = on)'
exec sp_executeSQL @sql
```