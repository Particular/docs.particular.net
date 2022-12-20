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
  
NOTE: In recent versions of Microsoft's Sql Server drivers encryption has been enabled by default. When trying to connect to a Sql Server instance that uses a self-signed cerftificate, the tool may display an exception stating *[The certificate chain was issued by an authority that is not trusted](https://learn.microsoft.com/en-us/troubleshoot/sql/connect/certificate-chain-not-trusted?tabs=ole-db-driver-19)*. To bypass this exception update the connection string to include "*;Trust Server Certificate=true*".

## SQL queries

The tool executes the following SQL queries on the database connection strings provided.

### Find queues

The tool uses this query to discover what tables in a SQL database catalog have the table structure that matches an NServiceBus queue table. This query is executed only once when the tool is first run.

```sql
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT DISTINCT t.TABLE_SCHEMA AS TableSchema, t.TABLE_NAME as TableName
FROM [INFORMATION_SCHEMA].[TABLES] t WITH (NOLOCK)
INNER JOIN [INFORMATION_SCHEMA].[COLUMNS] cId WITH (NOLOCK)
    ON t.TABLE_NAME = cId.TABLE_NAME AND cId.COLUMN_NAME = 'Id' AND cId.DATA_TYPE = 'uniqueidentifier'
INNER JOIN [INFORMATION_SCHEMA].[COLUMNS] cCorrelationId WITH (NOLOCK)
    ON t.TABLE_NAME = cCorrelationId.TABLE_NAME AND cCorrelationId.COLUMN_NAME = 'CorrelationId' AND cCorrelationId.DATA_TYPE = 'varchar'
INNER JOIN [INFORMATION_SCHEMA].[COLUMNS] cReplyToAddress WITH (NOLOCK)
    ON t.TABLE_NAME = cReplyToAddress.TABLE_NAME AND cReplyToAddress.COLUMN_NAME = 'ReplyToAddress' AND cReplyToAddress.DATA_TYPE = 'varchar'
INNER JOIN [INFORMATION_SCHEMA].[COLUMNS] cRecoverable WITH (NOLOCK)
    ON t.TABLE_NAME = cRecoverable.TABLE_NAME AND cRecoverable.COLUMN_NAME = 'Recoverable' AND cRecoverable.DATA_TYPE = 'bit'
INNER JOIN [INFORMATION_SCHEMA].[COLUMNS] cExpires WITH (NOLOCK)
    ON t.TABLE_NAME = cExpires.TABLE_NAME AND cExpires.COLUMN_NAME = 'Expires' AND cExpires.DATA_TYPE = 'datetime'
INNER JOIN [INFORMATION_SCHEMA].[COLUMNS] cHeaders WITH (NOLOCK)
    ON t.TABLE_NAME = cHeaders.TABLE_NAME AND cHeaders.COLUMN_NAME = 'Headers'
INNER JOIN [INFORMATION_SCHEMA].[COLUMNS] cBody WITH (NOLOCK)
    ON t.TABLE_NAME = cBody.TABLE_NAME AND cBody.COLUMN_NAME = 'Body' AND cBody.DATA_TYPE = 'varbinary'
INNER JOIN [INFORMATION_SCHEMA].[COLUMNS] cRowVersion WITH (NOLOCK)
    ON t.TABLE_NAME = cRowVersion.TABLE_NAME AND cRowVersion.COLUMN_NAME = 'RowVersion' AND cRowVersion.DATA_TYPE = 'bigint'
WHERE t.TABLE_TYPE = 'BASE TABLE'
```

### Get ROWVERSION

The tool uses these queries to discover the current `ROWVERSION` in each queue table. The start query is used at the beginning of tool execution, and the end query is used at the end of tool execution roughly 24 hours later.

These queries will be executed at minimum once per queue table. Because it is impossible to get a value if the queue is empty (there are no rows in the table) the tool may need to retry several times in order to get a value for each queue. The retries are time-limited to a 15 minute collection window at the beginning and end of the tool execution, and uses a backoff mechanism to avoid putting any undue stress on the SQL Server instance.

#### Start query

```sql
select min(RowVersion) from [SCHEMA_NAME].[TABLE_NAME] with (nolock);
```

#### End query

```sql
select max(RowVersion) from [SCHEMA_NAME].[TABLE_NAME] with (nolock);
```
