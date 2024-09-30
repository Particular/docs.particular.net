---
title: Measuring system throughput using PostgreSQL Transport
summary: Use the Particular throughput tool to measure the throughput of an NServiceBus system.
reviewed: 2024-05-22
related:
  - nservicebus/throughput-tool
---

The Particular throughput tool can be installed locally and run against a production system to discover the throughput of each endpoint in a system over a period of time.

This article details how to collect endpoint and throughput data when the system uses the [PostgreSQL transport](/transports/postgresql/). Refer to the [throughput counter main page](./) for information how to install/uninstall the tool or for other data collection options.

## Running the tool

Once installed, execute the tool with the database connection string used by PostgreSQL endpoints.

If the tool was [installed as a .NET tool](/nservicebus/throughput-tool/#installation-net-tool-recommended):

```shell
throughput-counter postgresql [options] --connectionString "Server=SERVER;Database=DATABASE;Port=5432;User Id=USERID;Password=PASSWORD;"
```

Or, if using the [self-contained executable](/nservicebus/throughput-tool/#installation-self-contained-executable):

```shell
Particular.EndpointThroughputCounter.exe postgresql [options] --connectionString "Server=SERVER;Database=DATABASE;Port=5432;User Id=USERID;Password=PASSWORD;"
```

The tool will run for slightly longer than 24 hours in order to capture a beginning and ending identity value for each queue table.

## Options

Either the `--connectionString` or `--connectionStringSource` must be used to provide the tool with connection string information.

| Option | Description |
|-|-|
| <nobr>`--connectionString`</nobr> | A single database connection string<sup>1</sup> that will provide at least read access to all queue tables. |
| <nobr>`--addCatalogs`</nobr> | When the `--connectionString` parameter points to a single database, but multiple database catalogs on the same server also contain NServiceBus message queues, the `--addCatalogs` parameter specifies additional database catalogs to search. The tool replaces the `Database` parameter in the connection string with the additional catalog and queries all of them. With this option, only a single database server is supported.<br/><br/>Example: `--connectionString <Catalog1String> --addCatalogs Catalog2 Catalog3 Catalog4` |
| <nobr>`--connectionStringSource` | Provide a file containing database connection strings (one per line) instead of specifying a single connection string as a tool argument. The tool will scan the databases provided by all connection strings in the file for NServiceBus queue tables. With this option, multiple catalogs in multiple database servers are supported.<br/><br/>Example: `--connectionStringSource <PathToFile>` |
include: throughput-tool-global-options

<sup>1</sup> See [examples of PostgreSQL connection strings](https://www.connectionstrings.com/postgresql/). Authentication is often via username/password `User Id=myUsername;Password=myPassword`.

## What the tool does

The tool executes the following PostgreSQL queries on the database connection strings provided.

### Find queues

The tool uses this query to discover what tables in a PostgreSQL database catalog have the table structure that matches an NServiceBus queue table. This query is executed only once when the tool is first run.

```sql
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT C.TABLE_SCHEMA as TableSchema, C.TABLE_NAME as TableName
FROM information_schema.columns C
WHERE
    (C.COLUMN_NAME = 'id' AND C.DATA_TYPE = 'uuid') OR
    (C.COLUMN_NAME = 'expires' AND C.DATA_TYPE = 'timestamp without time zone') OR
    (C.COLUMN_NAME = 'headers' AND C.DATA_TYPE = 'text') OR
    (C.COLUMN_NAME = 'body' AND C.DATA_TYPE = 'bytea') OR
    (C.COLUMN_NAME = 'seq' AND C.DATA_TYPE = 'integer')
GROUP BY C.TABLE_SCHEMA, C.TABLE_NAME
HAVING COUNT(*) = 5
```

### Get snapshot

The tool uses this query to get a snapshot of the identity value for each queue table. It is executed once per queue table when the tool is first run, then again at the end of the tool execution. The snapshots are compared to determine how many messages were processed in that table while the tool was running.

```sql
select last_value from "TABLE_NAME.SEQUENCE_NAME";
```
