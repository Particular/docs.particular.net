---
title: SQL Gateway Storage
summary: SQL deduplication storage for the gateway component
component: GatewaySql
reviewed: 2024-06-19
related:
 - samples/gateway
---

SQL Gateway Storage provides deduplication storage for the [gateway component](/nservicebus/gateway/) in Microsoft SQL Server using only a configured `DbConnection`.

## Usage

Both [System.Data.SqlClient](https://www.nuget.org/packages/System.Data.SqlClient) and [Microsoft.Data.SqlClient](https://www.nuget.org/packages/Microsoft.Data.SqlClient) are supported. Using the `ConnectionBuilder` method allows creating and configuring the connection using the desired package.

By default, the component uses a default schema and table name of `[dbo].[GatewayDeduplication]`.

snippet: DefaultUsage

### Token-credentials

Microsoft Entra ID authentication is supported via the [standard connection string options](https://learn.microsoft.com/en-us/sql/connect/ado-net/sql/azure-active-directory-authentication).

> [!NOTE]
> Microsoft Entra ID authentication is only supported when using [Microsoft.Data.SqlClient](https://learn.microsoft.com/en-us/sql/connect/ado-net/sql/azure-active-directory-authentication#overview)

### Customizing schema and table name

The following code shows how to customize the schema and table name:

snippet: CustomizeSchemaAndTableName

> [!NOTE]
> While it is possible to use the same GatewayDeduplication table for all endpoints within a single logical site, the gateway assumes that _different_ logical sites (which are generally physically separated as well) will use separate storage infrastructure. Because sending a message to multiple sites will result in messages with the same message ID delivered to each site, if those sites share a single deduplication table, the deduplication will not work correctly. In that case, separate the storage by using different table names as shown above.

### Using the endpoint name

By including the name of the endpoint in the table name, the resulting table can mimic the pattern used by table names in the [SQL persister](/persistence/sql/).

snippet: WithEndpointName

## Table creation

The deduplication table will be created if [installers](/nservicebus/operations/installers.md) are enabled, which is useful during development.

In controlled environments the creation of the table should be scripted and executed by a user with rights to create database schemas.

The following script can be used to create the deduplication table, replacing the schema/table name if necessary:

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

After a certain amount of time, duplicates are no longer likely and deduplication data should be cleaned up. However, the SQL gateway storage component provides no built-in mechanism to do this. Duplication data should be cleaned by an outside process like [SQL Agent](https://docs.microsoft.com/en-us/sql/ssms/agent/sql-server-agent?).

A script similar to the following will delete records in batches to prevent excessive database locking:

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
