---
title: NHibernate Persistence - Resolving incorrect timeout table indexes
summary: Instructions on how to resolve incorrect index definitions that can cause performance degradation for affected versions 6 to 7.
reviewed: 2021-02-22
component: NHibernate
isUpgradeGuide: true
redirects:
 - nservicebus/upgrades/nhibernate-timeouts-index
upgradeGuideCoreVersions:
 - 5
 - 6
---


## Summary

This guidance explains how to resolve an incorrectly created index when [using a custom NHibernate configuration](/persistence/nhibernate/#customizing-the-configuration-passing-configuration-in-code). This is described in the issue [Incorrect schema creation for timeout entity causes performance degradation](https://github.com/Particular/NServiceBus.NHibernate/issues/252).

This issue causes performance degradation if the table contains a large number of rows. Inserts and queries are inefficient due to the incorrect order of columns in the index. This results in unnecessary locking which limits the processing throughput of timeouts.


## Compatibility

This issue has been resolved in the following patch versions of the [NHibernate Persistence](/persistence/nhibernate/) as defined in the NServiceBus [support policy](/nservicebus/upgrades/support-policy.md):

 * [7.1.4](https://github.com/Particular/NServiceBus.NHibernate/releases/tag/7.1.4)
 * [7.0.6](https://github.com/Particular/NServiceBus.NHibernate/releases/tag/7.0.6)
 * [6.2.8](https://github.com/Particular/NServiceBus.NHibernate/releases/tag/6.2.8)


If any of the supported affected minor versions (7.1.x, 7.0.x, or 6.2.8) are used, these should be updated to the latest patch release. If an older non-supported affected version is used, this should be updated to a newer minor (in case of 6.1.x or 6.0.x) or major version (any version prior to 6.x).


## Upgrade steps

Steps:

 * Update to latest patch release.
 * Deploy the new version.
 * Check if a warning related to this schema issue is visible.
   * Or manually inspect the schema in the database.
 * Follow the procedure on how to resolve schema issues for the database engine used (Microsoft SQL Server or Oracle).
   * If any other database engine is used, then these changes must be applied manually.


## Check at startup

If there are endpoints that created an incorrect index definition, then this is detected in all fixed supported versions for 6.2.x, 7.0.x and 7.1.x. The detection routine is run when the endpoint instance is created and started. If that instance is affected, the following warning is logged:

> Could not find TimeoutEntity_EndpointIdx index. This may cause significant performance degradation of message deferral. Consult NServiceBus NHibernate persistence documentation for details on how to create this index.

If this log event is written to the log file, then read the following guidance on how to apply corrections.


## Potential issues

Any of the following issues can be present:

 * table `TimeoutEntity` has a clustered primary key (on Microsoft SQL Server).
 * index `TimeoutEntity_EndpointIdx` is non-clustered (on Microsoft SQL Server).
 * index `TimeoutEntity_EndpointIdx` has an incorrect column order (should be Endpoint, Time).


The approach to applying corrections depends on the database engine that is used.


## Resolving schema issues on Microsoft SQL Server

This assumes that both the index column order and clustered index are incorrect. To resolve this, all existing indexes need to be dropped and recreated.

NOTE: This procedure does not require any downtime. It is advisable to execute it when affected endpoint instances are not under heavy load.

NOTE: Make sure that the correct database is selected. If a custom schema name is used, then update the `dbo` schema with the custom schema identifier.

WARNING: Run this script on a testing or staging environment first to verify that it works as expected.

```sql
declare @schema nvarchar(max) = 'dbo' -- Update 'dbo' with custom schema if needed
declare @sql nvarchar(max)
declare @pkindex nvarchar(max)

select @pkindex = si.name
from sys.tables st
  join sys.indexes si on st.object_id = si.object_id
  join sys.schemas ss on st.schema_id = ss.schema_id
where st.name = 'TimeoutEntity'
  and si.is_primary_key = 1
  and ss.name = @schema

begin tran

select @sql = 'drop index[TimeoutEntity_SagaIdIdx] on [' + @schema + '].[TimeoutEntity]'
exec sp_executeSQL @sql

select @sql = 'drop index [TimeoutEntity_EndpointIdx] on [' + @schema + '].[TimeoutEntity]'
exec sp_executeSQL @sql

select @sql = 'alter table [' + @schema + '].[TimeoutEntity] drop constraint ' + @pkindex
exec sp_executeSQL @sql

select @sql = 'alter table [' + @schema + '].[TimeoutEntity] add constraint ' + @pkindex + ' primary key nonclustered (Id)'
exec sp_executeSQL @sql

select @sql = 'create nonclustered index [TimeoutEntity_SagaIdIdx] on [' + @schema + '].[TimeoutEntity]([SagaId]);'
exec sp_executeSQL @sql

select @sql = 'create clustered index [TimeoutEntity_EndpointIdx] on [' + @schema + '].[TimeoutEntity]([Endpoint], [Time]);'
exec sp_executeSQL @sql

commit tran
```


## Resolving incorrect index definition on Oracle

The incorrect index definition on Oracle only applies to the column order. An existing `TIMEOUTENTITY_ENDPOINTIDX` index has to be dropped, and a new index with correct column order needs to be created.

NOTE: This procedure does not require any downtime. It is advisable to execute it when affected endpoint instances are not under heavy load.

WARNING: Run this script on a testing or staging environment first to verify that it works as expected.

```sql
drop index TIMEOUTENTITY_ENDPOINTIDX;

create index TIMEOUTENTITY_ENDPOINTIDX on TIMEOUTENTITY (ENDPOINT asc, TIME asc);
```
