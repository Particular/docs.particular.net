---
title: NHibernate Persistence - Missing unique constraints
summary: Instructions on how to add missing unique constraints on saga correlation property columns for affected versions.
reviewed: 2021-02-22
component: NHibernate
isUpgradeGuide: true
redirects:
 - nservicebus/upgrades/nhibernate-saga-index-fix
upgradeGuideCoreVersions:
 - 6
---


## Summary

This guidance explains how add missing unique constraints on saga correlation property columns. This is described in the issue [Unique constraints are not generated for saga correlation properties](https://github.com/Particular/NServiceBus.NHibernate/issues/280).

This issue may cause multiple rows in saga data tables that represent the same logical saga instance. The duplicated rows are inserted as a result of a race condition during saga creation and missing unique constraints on correlation property columns.


## Compatibility

This issue has been resolved in the following patch versions of the [NHibernate Persistence](/persistence/nhibernate/) as defined in the NServiceBus [support policy](/nservicebus/upgrades/support-policy.md):

 * [7.2.1](https://github.com/Particular/NServiceBus.NHibernate/releases/tag/7.2.1)
 * [7.1.6](https://github.com/Particular/NServiceBus.NHibernate/releases/tag/7.1.6)

Affected versions (7.1 and later) that are still [supported](/nservicebus/upgrades/supported-versions.md#persistence-packages-nservicebus-nhibernate) should be updated to the latest patch release.


## Upgrade steps

Steps:

 * For each saga data table:
    * Check if any duplicate values exist in the correlation property column.
    * Merge duplicate rows so that there are no duplicate values in the correlation property column.
    * Add unique constraint on correlation property column
 * Update endpoint to latest patch release.
 * Deploy the new version.


### Checking for duplicate correlation property values

The following query detects duplicate rows in the saga data table.

NOTE: It requires providing values for `sagaDataTableName` and `correlationPropertyColumnName` variables.

Each row in the result set represents a single logical saga instance that was duplicated. The first column of the result will show the correlation property value for the logical saga instance.


#### Microsoft SQL Server

```sql
declare @sagaDataTableName nvarchar(max) = '...'
declare @correlationPropertyColumnName nvarchar(max) = '...'
declare @sql nvarchar(max)

select @sql = 'select ' + @correlationPropertyColumnName + ', count(*) as SagaRows from ' + @sagaDataTableName + ' group by ' + @correlationPropertyColumnName + ' having count(*) > 1'
exec sp_executeSQL @sql
```


#### Oracle

```sql
declare
  sagaDataTableName varchar2(30) := '...';
  correlationPropertyColumnName varchar2(30) := '...';
  detectDuplicates varchar2(500);
  type SagaCursorType is ref cursor;
  sagaCursor SagaCursorType;
  correlationProperty <type of correlationPropertyColumnName>;
  instanceCount number(10);
begin
    detectDuplicates :=
       'select ' || correlationPropertyColumnName || ', count(*)
        from ' || sagaDataTableName || '
        group by ' || correlationPropertyColumnName || '
        having count(*) > 1';
        
    open sagaCursor for detectDuplicates;
    
    loop
      fetch sagaCursor into correlationProperty, instanceCount;
      exit when sagaCursor%NOTFOUND;
      
      dbms_output.put_line(correlationProperty);
      
    end loop;
    
    close sagaCursor;
end;
```


### Add unique constraint on correlation property column

The unique constraint on the correlation property column can be added with following query.

NOTE: It requires providing values for `sagaDataTableName` and `correlationPropertyColumnName` variables.


#### Microsoft SQL Server

NOTE: If adding a unique constraint fails with the error `The CREATE UNIQUE INDEX statement terminated because a duplicate key was found for the object name ...`, ensure that all duplicated rows detected have been merged.

```sql
declare @sagaDataTableName nvarchar(max) = '...'
declare @correlationPropertyColumnName nvarchar(max) = '...'
declare @sql nvarchar(max)

select @sql = 'alter table ' + @sagaDataTableName + ' add unique nonclustered ( ' + @correlationPropertyColumnName + ' asc )with (pad_index = off, statistics_norecompute = off, sort_in_tempdb = off, ignore_dup_key = off, online = off, allow_row_locks = on, allow_page_locks = on)'
exec sp_executeSQL @sql
```


#### Oracle

NOTE: Oracle requires that a unique constraint has a name. Use the `uniqueConstraintName` variable to set the name of the constraint.

```sql
declare
  sagaDataTableName varchar2(30) := '...';
  correlationPropertyColumnName varchar2(30) := '...';
  uniqueConstraintName varchar2(30) := '...';
  addUniqueConstraint varchar2(500);
begin
    addUniqueConstraint :=
       'alter table ' || sagaDataTableName || ' add constraint ' || uniqueConstraintName || ' unique (' || correlationPropertyColumnName || ')';
       
    execute immediate addUniqueConstraint;
end;
```
