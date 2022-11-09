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
throughput-counter sqlserver --connectionString "Server=SERVER;Database=DATABASE;User=USERNAME;Password=PASSWORD;"
```

The tool will run for slightly longer than 24 hours in order to capture a beginning and ending `RowVersion` value for each queue table. A value can only be detected when a message is waiting in the queue to be processed, and not from an empty queue, so the tool may execute multiple SQL queries for each table. The tool will use a backoff mechanism to avoid putting undue pressure on the SQL Server instance.

## Options

All options are required:

| Option | Description |
|-|-|
| <nobr>`--connectionString`</nobr> | The database connection string that will provide at least read access to all queue tables. |