---
title: SQL Gateway Storage
summary: SQL deduplication storage for the Gateway component
component: GatewaySql
reviewed: 2020-02-05
related:
 - samples/gateway
---

SQL Gateway Storage provides deduplication storage for the [Gateway component](/nservicebus/gateway/) in Microsoft SQL Server using only a configured `DbConnection`.

## Usage

By default, a default schema and table name of `[dbo].[GatewayDeduplication]` will be used.

snippet: DefaultUsage

### Customizing schema and table name

The schema name and/or table name can be customized.

snippet: CustomizeSchemaAndTableName

Both [System.Data.SqlClient](https://www.nuget.org/packages/System.Data.SqlClient) and [Microsoft.Data.SqlClient](https://www.nuget.org/packages/Microsoft.Data.SqlClient) are supported.

NOTE: While it is possible to use the same GatewayDeduplication table for all endpoints within a single logical site, the Gateway assumes that _different_ logical sites (which are generally physically separated as well) will use separate storage infrastructure. Because sending a message to multiple sites will result in messages with the same message id delivered to each site, if those sites for some reason share a single deduplication table, the deduplication will not work correctly. In that case, separate the storage by using different table names as shown above.

### Using endpoint name

By including the endpoint name in the table name, the resulting table can mimic the pattern used by table names in [SQL Persistence](/persistence/sql/).

snippet: WithEndpointName

## Table creation

The deduplication table will be created if [installers](/nservicebus/operations/installers.md) are enabled, which is useful during development time.

In controlled environments the creation of the table should be scripted and executed by a user with rights to create database schema.

This script can be used to create the deduplication table, replacing the schema/table name if necessary:

```sql
if not exists (
	select * from sys.objects
	where
		object_id = object_id('[dbo].[GatewayDeduplication]')
		and type = 'U'
)
begin

	create table [dbo].[GatewayDeduplication] (
		Id nvarchar(255) not null primary key clustered,
		TimeReceived datetime null
	)
end
if not exists (
	select *
	from sys.indexes
	where
		name = 'Index_TimeReceived'
		and object_id = object_id('[dbo].[GatewayDeduplication]')
)
begin
	create index Index_TimeReceived
	on [dbo].[GatewayDeduplication] (TimeReceived asc)
end
```

## Cleaning up old records

After some period when duplicates are no longer likely, deduplication data should be cleaned up. However, SQL Gateway Storage provides no built-in mechanism to do this. Duplication data should be cleaned by an outside process like [SQL Agent](https://docs.microsoft.com/en-us/sql/ssms/agent/sql-server-agent?).

A script similar to the following should be run to delete records in batches to prevent excessive database locking:

```sql
declare @BatchSize int = 5000
declare @ReceivedBefore datetime = dateadd(day, -7, getutcdate())

while 1=1
begin

	set rowcount @BatchSize
	delete from [dbo].[GatewayDeduplication]
	where TimeReceived < @ReceivedBefore

	if @@ROWCOUNT < @BatchSize
		break;
end
```