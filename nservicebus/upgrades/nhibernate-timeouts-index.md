---
title: NHibernate Persistence - Resolving incorrect timeout table indexes #252
summary: Instructions on how to resolve incorrect index definitions that can cause performance degradation for affected versions 6 to 7.
component: NHibernate
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## Summary

This guidance explains how to resolve an incorrectly created index when passing a custom NHibernate configuration to NServiceBus as described in the following issue:

- https://github.com/Particular/NServiceBus.NHibernate/issues/252

This issue causes performance degradation if the table contains a large number of rows. Inserts and queries are inefficient due to the incorrect order of columns. This results in unnecessary locking which limits the processing throughput of timeouts.


## Compatibility

This issue has been resolved in the following patch versions as defined in our [support policy](support-policy.md):

- NServiceBus.NHibernate 7.1.4
- NServiceBus.NHibernate 7.0.6
- NServiceBus.NHibernate 6.2.8


If any of the supported affected minor versions (7.1.x, 7.0.x, or 6.2.8) are used these should be updated to the latest patch release. If an older version - non supported - affected version is used, this should be updated to a newer minor (in case of 6.1.x or 6.0.x) or major version (any version prior to 6.x).


## Upgrade steps

Steps:

 * Update to latest patch release
 * Deploy the new version
 * Check if a warning related to this schema issue is visible
   * Or manually inspect the schema in your database
 * Follow the procedure on how to resolve schema issues for the database engine used (Microsoft SQL Server or Oracle)
   * If any other database engine is used then these changes must be applied manually


## Check at startup

If there are endpoints that created an incorrect index definition then this is detected in all fixed supported versions for 6.2.x, 7.0.x and 7.1.x. The detection routine is run when the endpoint instance is created and started. If you are affected you will get the following log event with log level warning:

> Could not find TimeoutEntity_EndpointIdx index. This may cause significant performance degradation of message deferral. Consult NServiceBus NHibernate persistence documentation for details on how to create this index.

If this log event is written to your log file then read the following guidance on how to apply corrections.


## Potential issues

Any of the following issues can be present:

- table `TimeoutEntity` has a clustered primary key
- index `TimeoutEntity_EndpointIdx` is non-clustered
- index `TimeoutEntity_EndpointIdx` has an incorrect column order (should be Endpoint, Time)


How to correct these depend on the database engine that you are using.


## Resolving schema issues on Microsoft SQL Server

This assumes that both the index column order and clustered index are incorrect. To resolve this all existing indexes need to be dropped and recreated.

WARNING: This procedure requires downtime. Make sure that all affected endpoint instances are not running or that the database is running in single-user mode. This is needed because indexes are temporarily removed that guarantee consistency.

NOTE: Make sure that the correct database is selected. If a custom schema name is used then update the dbo schema with the custom schema identifier.

NOTE: Run this script on a testing or staging environment first to verify that it works as expected.

```sql
BEGIN TRAN

DROP INDEX [TimeoutEntity_SagaIdIdx] ON [dbo].[TimeoutEntity];
GO

DROP INDEX [TimeoutEntity_EndpointIdx] ON [dbo].[TimeoutEntity];
GO

ALTER TABLE [dbo].[TimeoutEntity] DROP CONSTRAINT PK__TimeoutE__3214EC06D068BEFC
GO

ALTER TABLE [dbo].[TimeoutEntity] ADD CONSTRAINT PK__TimeoutE__3214EC06D068BEFC PRIMARY KEY NONCLUSTERED (Id)
GO

CREATE NONCLUSTERED INDEX [TimeoutEntity_SagaIdIdx] ON [dbo].[TimeoutEntity]([SagaId] ASC);
GO

CREATE CLUSTERED INDEX [TimeoutEntity_EndpointIdx] ON [dbo].[TimeoutEntity]([Endpoint] ASC, [Time] ASC);
GO

COMMIT TRAN
```

If any of the following messages is shown then the currently selected database is incorrect or the schema in the scripts is incorrect because a custom schema is used. Update the schema in the SQL statements to resolve this issue.

- Cannot drop the index 'dbo.TimeoutEntity.TimeoutEntity_SagaIdIdx', because it does not exist or you do not have permission.
- Cannot drop the index 'dbo.TimeoutEntity.TimeoutEntity_EndpointIdx', because it does not exist or you do not have permission.
- Cannot find the object "dbo.TimeoutEntity" because it does not exist or you do not have permissions.


## Resolving incorrect index definition on Oracle

The incorrect index definition on Oracle only applies to the column order. An existing `TIMEOUTENTITY_ENDPOINTIDX` index has to be dropped, and a new index with correct column order needs to be created:

NOTE: This procedure does not require any downtime. It is still advisable to execute it when corresponding endpoint instances are not under heavy load.

NOTE: Run this script on a testing or staging environment first to verify that it works as expected.

```sql
DROP INDEX TIMEOUTENTITY_ENDPOINTIDX;

CREATE INDEX TIMEOUTENTITY_ENDPOINTIDX ON TIMEOUTENTITY (ENDPOINT ASC, TIME ASC);
```
