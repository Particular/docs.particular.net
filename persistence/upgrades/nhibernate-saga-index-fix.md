---
title: NHibernate Persistence - Missing unique constraints
summary: Instructions on how to add missing unique constraints on saga correlation property columns for affected versions.
component: NHibernate
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
---


## Summary

This guidance explains how add missing unique constraints on saga correlation property columns

- https://github.com/Particular/NServiceBus.NHibernate/issues/280

This issue may cause multiple rows in saga data table that represent the same logical saga instance. The duplicated rows are inserted as a result of race condition during saga creation and missing unique constraint on correlation property column.


## Compatibility

This issue has been resolved in the following patch versions as defined in the NServiceBus [support policy](/nservicebus/upgrades/support-policy.md):

- NServiceBus.NHibernate 7.2.1
- NServiceBus.NHibernate 7.1.6

Affected versions (7.1 and later) that are still [supported](/nservicebus/upgrades/supported-versions.md#persistence-packages-nservicebus-nhibernate) should be updated to the latest patch release.

## Upgrade steps

Steps:

 * For each saga data table
    * Check if any duplicate values exist in the correlation property column
    * Merge duplicate rows so that there are no duplicate values in the correlation property column
    * Add unique constraint on correlation property column
 * Update endpoint to latest patch release
 * Deploy the new version

### Checking for duplicate correlation property values

The following query detects duplicate rows in the saga data table - it requires providing values for `sagaDataTableName` and `correlationPropertyColumnName` variables.

Each row in the result set represents a single logical saga instance that was duplicated. First column of the result will show the correlation property value for logical saga instance.

#### Microsoft SQL Server

snippet: DetectSagaDataDuplicates_MsSqlServer

#### Oracle

snippet:  DetectSagaDataDuplicates_Oracle

### Add unique constraint on correlation property column

Unique constraint on correlation property column can be added with following query - it requires providing values for `sagaDataTableName` and `correlationPropertyColumnName` variables.

#### Microsoft SQL Server

NOTE: If adding a unique constraint fails with the error `The CREATE UNIQUE INDEX statement terminated because a duplicate key was found for the object name ...` ensure that all duplicated rows detected have been merged.

snippet: AddUniqueConstraintOnSagaDataTable_MsSqlServer

#### Oracle

NOTE: Oracle requires that a unique constraint has a name. Use the `uniqueConstraintName` variable to set the name of the constraint.

snippet: AddUniqueConstraintOnSagaDataTable_Oracle