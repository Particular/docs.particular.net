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


https://github.com/Particular/PlatformDevelopment/issues/1145


## Check at startup

This guidance explains how to resolve SQL index issues as mentioned in XXX. If you have endpoints with an incorrect index then this is detected in all supported versions for 6.2.x, 7.0.x and 7.1.x. The detection routine is run when the endpoint instance is created. If you are affected you will get the following log event with log level warning:

> TimeoutEntity indexes are non-optimal


If this log event is written to your log file then please take the following steps to resolve the issue.

## Correct 

Corrections that need to be made:

Make sure that:

- index `TimeoutEntity_EndpointIdx` is present
- index `TimeoutEntity_EndpointIdx` has the correct column order (Endpoint, Time)
- index `TimeoutEntity_EndpointIdx` is clustered instead of the primary key


If 

```sql
DROP INDEX [TimeoutEntity_SagaIdIdx] ON [dbo].[TimeoutEntity];
```



```sql
CREATE TABLE [dbo].[TimeoutEntity] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Destination] NVARCHAR (1024)  NULL,
    [SagaId]      UNIQUEIDENTIFIER NULL,
    [State]       VARBINARY (MAX)  NULL,
    [Time]        DATETIME         NULL,
    [Headers]     NVARCHAR (MAX)   NULL,
    [Endpoint]    NVARCHAR (440)   NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC)
);

GO
CREATE CLUSTERED INDEX [TimeoutEntity_EndpointIdx]
    ON [dbo].[TimeoutEntity]([Endpoint] ASC, [Time] ASC);

GO
CREATE NONCLUSTERED INDEX [TimeoutEntity_SagaIdIdx]
    ON [dbo].[TimeoutEntity]([SagaId] ASC);

```
