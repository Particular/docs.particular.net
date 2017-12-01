---
title: SQL Server dialect design
component: SqlPersistence
related:
reviewed: 2017-11-23
---

## Unicode support

include: unicode-support

Refer to the dedicated [SQL Server documentation](https://docs.microsoft.com/en-us/sql/relational-databases/collations/collation-and-unicode-support) for details.


## Supported name lengths

SQL Server supports [max. 128 characters](https://docs.microsoft.com/en-us/sql/sql-server/maximum-capacity-specifications-for-sql-server).

include: name-length-validation-off


## Usage

include: usage

snippet: SqlPersistenceUsageSqlServer

include: mssql-dtc-warning


## Schema support

The SQL Server dialect supports multiple schemas. By default, when schema is not specified, it uses `dbo` schema when referring to database objects. When schema is specified multiple endpoints can co-exist within the same database without affecting each other.

snippet: MsSqlSchema



