---
title: Measuring system throughput using SQL Transport
summary: Use the Particular throughput tool to measure the throughput of an NServiceBus system.
reviewed: 2022-11-09
related:
  - nservicebus/throughput-tool
---

The Particular throughput tool can be installed locally and run against a production system to discover the throughput of each endpoint in a system over a period of time.

This article details how to collect endpoint and throughput data when the system uses the [SQL transport](/transports/sql/). Refer to the [throughput counter main page](./) for information how to install/uninstall the tool or for other data collection options.

## Running the tool

Once installed, execute the tool with the database connection string used by SQL Server endpoints, as in this example:

```shell
throughput-counter sqlserver --connectionString "Server=SERVER;Database=DATABASE;User=USERNAME;Password=PASSWORD;Trust Server Certificate=true;"
```

The tool will run for slightly longer than 24 hours in order to capture a beginning and ending `RowVersion` value for each queue table. A value can only be detected when a message is waiting in the queue to be processed, and not from an empty queue, so the tool may execute multiple SQL queries for each table. The tool will use a backoff mechanism to avoid putting undue pressure on the SQL Server instance.

## Options

Either the `--connectionString` or `--connectionStringSource` must be used to provide the tool with connection string information.

| Option | Description |
|-|-|
| <nobr>`--connectionString`</nobr> | A single database connection string that will provide at least read access to all queue tables. |
| <nobr>`--addCatalogs`</nobr> | When the `--connectionString` parameter points to a single database, but multiple database catalogs on the same server also contain NServiceBus message queues, the `--addCatalogs` parameter specifies additional database catalogs to search. The tool replaces the `Database` or `Initial Catalog` parameter in the connection string with the additional catalog and queries all of them. With this option, only a single database server is supported.<br/><br/>Example: `--connectionString <Catalog1String> --addCatalogs Catalog2 Catalog3 Catalog4` |
| <nobr>`--connectionStringSource` | Provide a file containing database connection strings (one per line) instead of specifying a single connection string as a tool argument. The tool will scan the databases provided by all connection strings in the file for NServiceBus queue tables. With this option, multiple catalogs in multiple database servers are supported.<br/><br/>Example: `--connectionStringSource <PathToFile>` |
include: throughput-tool-global-options
  
NOTE: In recent versions of Microsoft's Sql Server drivers encryption has been enabled by default. When trying to connect to a Sql Server instance that uses a self-signed cerftificate, the tool may display an exception stating *[The certificate chain was issued by an authority that is not trusted](https://learn.microsoft.com/en-us/troubleshoot/sql/connect/certificate-chain-not-trusted?tabs=ole-db-driver-19)*. To bypass this exception update the connection string to include `;Trust Server Certificate=true`.

## What does the tool do

The tool executes the following SQL queries on the database connection strings provided.

### Find queues

The tool uses this query to discover what tables in a SQL database catalog have the table structure that matches an NServiceBus queue table. This query is executed only once when the tool is first run.

```sql
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
  
SELECT C.TABLE_SCHEMA as TableSchema, C.TABLE_NAME as TableName
FROM [INFORMATION_SCHEMA].[COLUMNS] C
WHERE
	(C.COLUMN_NAME = 'Id' AND C.DATA_TYPE = 'uniqueidentifier') OR
	(C.COLUMN_NAME = 'CorrelationId' AND C.DATA_TYPE = 'varchar') OR
	(C.COLUMN_NAME = 'ReplyToAddress' AND C.DATA_TYPE = 'varchar') OR
	(C.COLUMN_NAME = 'Recoverable' AND C.DATA_TYPE = 'bit') OR
	(C.COLUMN_NAME = 'Expires' AND C.DATA_TYPE = 'datetime') OR
	(C.COLUMN_NAME = 'Headers') OR
	(C.COLUMN_NAME = 'Body' AND C.DATA_TYPE = 'varbinary') OR
	(C.COLUMN_NAME = 'RowVersion' AND C.DATA_TYPE = 'bigint')
GROUP BY C.TABLE_SCHEMA, C.TABLE_NAME
HAVING COUNT(*) = 8
```

### Get snapshot

The tool uses this query to get a snapshot of the identity value for each queue table. It is executed once per queue table when the tool is first run, then again at the end of the tool execution. The snapshots are compared to determine how many messages were processed in that table while the tool was running.

```sql
select IDENT_CURRENT('[SCHEMA_NAME].[TABLE_NAME]')
```
