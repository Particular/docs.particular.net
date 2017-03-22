---
title: NHibernate Timeouts Index Persistence Upgrade Version 6 to 7
summary: Instructions on how to upgrade NHibernate Persistence Version 6 to 7.
component: NHibernate
related:
 - nservicebus/upgrades/5to6
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


This guidance explains how to resolve an incorrectly created schema when passing a custom NHibernate configuration to NServiceBus as described in the following issue:

- https://github.com/Particular/NServiceBus.NHibernate/issues/252



## Check at startup

If you have endpoints with an incorrect index then this is detected in all fixed supported versions for 6.2.x, 7.0.x and 7.1.x. The detection routine is run when the endpoint instance is created. If you are affected you will get the following log event with log level warning:

> Could not find TimeoutEntity_EndpointIdx index. This may cause significant performance degradation of message deferral. Consult NServiceBus NHibernate persistence documentation for details on how to create this index.

If this log event is written to your log file then please take the following steps to resolve the issue.


## Corrections to be applied

Corrections that need to be made:

Make sure that:

- index `TimeoutEntity_EndpointIdx` is present
- index `TimeoutEntity_EndpointIdx` has the correct column order (Endpoint, Time)
- index `TimeoutEntity_EndpointIdx` is clustered instead of the primary key


How to correct these depend on the database engine that you are using.


## Microsoft SQL Server

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

CREATE CLUSTERED INDEX [TimeoutEntity_EndpointIdx] ON [dbo].[TimeoutEntity]([Time] ASC, [Endpoint] ASC);
GO

COMMIT TRAN
```


If you get any of the following messages then your current database is incorrect or you are using a custom schema. Please update the schema in the SQL statements.

- Cannot drop the index 'dbo.TimeoutEntity.TimeoutEntity_SagaIdIdx', because it does not exist or you do not have permission.
- Cannot drop the index 'dbo.TimeoutEntity.TimeoutEntity_EndpointIdx', because it does not exist or you do not have permission.
- Cannot find the object "dbo.TimeoutEntity" because it does not exist or you do not have permissions.


## Oracle

TBD