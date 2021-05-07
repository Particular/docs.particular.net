---
title: SQL Server Transport Upgrade - Supporting Unicode in Headers
summary: How to add support for Unicode characters in message headers
component: SqlTransport
reviewed: 2021-04-22
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


This document explains how to patch a system using the SQL Server transport to [allow message headers to contain characters not supported by the current SQL Server collation](https://github.com/Particular/NServiceBus.SqlServer/issues/340). The issue may cause data loss when these characters are used in  header names or values.


## Compatibility

This issue has been resolved in the following patch versions of the [SQL Server transport](/transports/sql/) as defined in the NServiceBus [support policy](/nservicebus/upgrades/support-policy.md):

 * [3.1.1](https://github.com/Particular/NServiceBus.SqlServer/releases/tag/3.1.1)
 * [3.0.3](https://github.com/Particular/NServiceBus.SqlServer/releases/tag/3.0.3)
 * [2.2.6](https://github.com/Particular/NServiceBus.SqlServer/releases/tag/2.2.6)

Any of the supported affected minor versions (3.1.x, 3.0.x, or 2.2.x) should be updated to the latest patch release. Older (unsupported) affected versions should be updated to a supported minor (2.1.x or 2.0.x) or major version (1.x).


## Upgrade steps

To upgrade an existing endpoint:

 * Update to the latest patch release
 * Deploy the new version
 * Check endpoint startup logs for a [warning message](/transports/upgrades/sqlserver-unicode-headers.md#check-at-startup)
 * Follow the [queue table schema upgrade procedure](/transports/upgrades/sqlserver-unicode-headers.md#queue-table-schema-upgrade)


## Check at startup

The SQL Server transport detects incorrect definition of the `Headers` column and logs a warning:

> Table [dbo].[SampleEndpoint] stores headers in a non Unicode-compatible column (varchar).
>
> This may lead to data loss when sending non-ASCII characters in headers. SQL Server transport versions 3.1 and newer can take advantage of the `nvarchar` column type for headers. Please change the column type in the database.

If this log event is written to the log file, the following guidance describes how to upgrade the queue table schema.


## Queue table schema upgrade

The incorrect `Headers` column definition on existing queue tables needs to be updated manually using the following SQL statement for every queue table managed by a given endpoint:

NOTE: This procedure does not require any downtime, and it can be executed when affected endpoints are processing messages.

WARNING: Run this script on a testing or staging environment first to verify that it works as expected.


```sql
declare @queueTableName nvarchar(max) = N'...'
declare @sql nvarchar(max) = N'alter table ' + @queueTableName + N' alter column Headers nvarchar(max) not null';

exec sp_executesql @sql;
```

