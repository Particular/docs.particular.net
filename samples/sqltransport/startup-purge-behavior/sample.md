---
title: SQL Server Transport Startup Purge Behavior
summary: A sample showing how to truncate large transport tables at startup for SQL Server Transport.
reviewed: 2020-11-13
component: SqlTransport
related:
- transports/sql
---


## Prerequisites

include: sql-prereq

The database created by this sample is called `SQLServerTruncate`.


## Running the project

partial: running-the-project

## Code walk-through

When the endpoint starts up, it runs all `INeedInitialization` instances. In this sample, the `TruncateTableAtStartup` class is used to truncate the table if it exists at startup.

snippet: TruncateTableAtStartup

### Configure the SQL Server transport

snippet: TransportConfiguration
