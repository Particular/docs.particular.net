---
title: SQL Server Transport Upgrade - Non-clustered index in the input queue
summary: How to migrate to the input queue schema in the SQL Server transport without a clustered index
component: SqlTransport
reviewed: 2020-06-12
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 3
 - 4
 - 5
 - 6
---


This document explains how to patch a system using the [SQL Server transport](/transports/sql/) to [remove clustered index on the queue table](https://github.com/Particular/NServiceBus.SqlServer/pull/613) for improved performance. 

## Upgrade steps
 
Existing queue tables need to be migrated manually to the new schema. This can be done by executing the following script for each input queue table in the system:

```sql
DROP INDEX [Index_RowVersion] ON <schema>.<table> WITH ( ONLINE = ON )

CREATE NONCLUSTERED INDEX Index_RowVersion ON <schema>.<table>
(
    [RowVersion] ASC
)
```

This will result in dropping the clustered index `Index_RowVersion` and creating a new non-clustered index with the same name.

New index schema creation script has been introduced in the following patch version of the [SQL Server transport](/transports/sql/):

 * [6.0.1](https://github.com/Particular/NServiceBus.SqlServer/releases/tag/6.0.1)
 * [5.0.3](https://github.com/Particular/NServiceBus.SqlServer/releases/tag/5.0.3)
 * [4.3.2](https://github.com/Particular/NServiceBus.SqlServer/releases/tag/4.3.2)
 * [3.1.5](https://github.com/Particular/NServiceBus.SqlServer/releases/tag/3.1.5)