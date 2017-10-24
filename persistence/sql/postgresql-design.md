---
title: PostgreSQL dialect design
component: SqlPersistence
related:
reviewed: 2017-10-23
---


Design decisions made in SQL Persistence's PostgreSQL dialect implementation:


## Case sensitivity - UpperCamelCase

Unless explicitly specified, PostgreSQL automatically lower-cases all identifier names (e.g. column or table names, etc.). To enforce case-sensitivity, it is necessary to quote all names.

SQL persistence internally honors the UpperCamelCase (also called PascalCase) convention, which is the standard default in other popular database engines, e.g. MS SQL Server. SQL persistence uses quoted identifier names in stored procedures, queries, etc.


## Supported name lengths

The SQL Persistence provides autonomy between endpoints by using separate tables for every endpoint based on the endpoint name. By default PostgreSQL [limits object names to 63 characters](https://www.postgresql.org/docs/current/static/sql-syntax-lexical.html#sql-syntax-identifiers).

By default SQL persistence uses an endpoint's name as a table prefix, the maximum length of the table prefix is 20 characters. The table prefix [can be customized](/persistence/sql/#installation-table-prefix).


## Using `pg_temp.` schema in installation scripts

The table creation for the SQL Persistence requires some dynamic SQL script execution. To achieve this in PostgreSql it is necessary to create temporary functions. These functions are created in the PostgreSql temporary-table schema, commonly referred to as `pg_temp`.

As per [Client Connection Defaults](https://www.postgresql.org/docs/9.2/static/runtime-config-client.html) `pg_temp` is:

> the current session's temporary-table schema, pg_temp_nnn, is always searched if it exists. It can be explicitly listed in the path by using the alias pg_temp