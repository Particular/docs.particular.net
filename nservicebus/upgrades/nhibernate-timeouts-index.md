---
title: NHibernate Persistence - Resolving incorrect timeout table schema (#252)
summary: Instructions on how to resolve incorrect schema that can cause performance issues for affected versions 6 to 7.
component: NHibernate
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## Summary

This guidance explains how to resolve an incorrectly created schema when passing a custom NHibernate configuration to NServiceBus as described in the following issue:

- https://github.com/Particular/NServiceBus.NHibernate/issues/252

This issues causes performance issues if the table contains a large number of rows. Inserts and queries are inefficient due to the incorrect order of columns. This results in unnecessary locking which limits the processing throughput of timeouts.


## Compatibility

This issues has been resolved in following patch versions as defined in our  [support policy](support-policy.md):

- 7.1.4 in use by NServiceBus 6.x
- 7.0.6 (support 7.0.x) in use by NServiceBus 5.x
- 6.2.8 (support 6.2.x) in use by NServiceBus 5.x



If you are using any of the supported minor versions (7.1.x, 7.0.x, or 6.2.8) then you should at least update to the latest patch release. If you are using an older version then you should update to a newer minor (in case of 6.1.x or 6.0.x) or major version (any version prior to 6.x).


## Upgrade steps

Steps:

 * Update to latest patch release
 * Deploy the new version
 * Check if you get a warning related to this schema issue
   * Or manually inspect the schema in your database
 * Follow procedure on how to resolve schema issues for your database engine (Microsoft SQL Server or Oracle)
   * If you are using any other database engine then you have to apply these changes in the database dialect for your engine manually


## Check at startup

If you have endpoints with an incorrect table schema then this is detected in all fixed supported versions (at March 1th, 2017) for 6.2.x, 7.0.x and 7.1.x. The detection routine is run when the endpoint instance is created and started. If you are affected you will get the following log event with log level warning:

> Could not find TimeoutEntity_EndpointIdx index. This may cause significant performance degradation of message deferral. Consult NServiceBus NHibernate persistence documentation for details on how to create this index.

If this log event is written to your log file then read the following guidance on how to apply corrections.


## Potential issues

Any of the following issues can be present:

- table `TimeoutEntity` has a clustered primary key
- index `TimeoutEntity_EndpointIdx` is non-clustered
- index `TimeoutEntity_EndpointIdx` has an incorrect column order (should be Endpoint, Time)


How to correct these depend on the database engine that you are using.


## Resolving schema issues with Microsoft SQL Server

This assumes that both the index column order and clustered index are incorrect. To resolve this we will drop all existing indexes and recreate them.

WARN: This procedure requires downtime. Make sure that all affected endpoint instances are not running or that the database is running in admin mode. This is needed because we are dropping index that guarantee consistency.

NOTE: Make sure that the correct database is selected. If you are using a custom schema name then update the dbo schema with your custom schema identifier.

NOTE: Please first run this script on your testing stage first in order to verify the script works as expected to reduce downtime.

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

If you get any of the following messages then your current database is incorrect or you are using a custom schema. Please update the schema in the SQL statements.

- Cannot drop the index 'dbo.TimeoutEntity.TimeoutEntity_SagaIdIdx', because it does not exist or you do not have permission.
- Cannot drop the index 'dbo.TimeoutEntity.TimeoutEntity_EndpointIdx', because it does not exist or you do not have permission.
- Cannot find the object "dbo.TimeoutEntity" because it does not exist or you do not have permissions.


## Oracle

TBD
