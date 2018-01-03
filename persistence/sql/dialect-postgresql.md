---
title: PostgreSQL dialect design
component: SqlPersistence
related:
reviewed: 2017-11-23
versions: "[3,)"
redirects:
 - nservicebus/sql-persistence/postgresql-design
---

{{WARNING: Even though PostgreSQL is a free product, it is recommended to ensure that support agreements are in place when running the SQL persistence for PostgreSQL in production. For details see [PostgreSQL Commercial support](https://www.postgresql.org/support/professional_support/).
}}


## Supported database versions

SQL persistence supports [PostgreSQL 10](https://www.postgresql.org/docs/10/static/release-10.html) and above.


## Usage

include: usage

Using the [Npgsql NuGet Package](https://www.nuget.org/packages/Npgsql/).

snippet: sqlpersistenceusagepostgresql


### Passing Jsonb as NpgsqlDbType

[Npgsql](http://www.npgsql.org) requires that parameters that pass [JSONB](https://www.postgresql.org/docs/9.4/static/datatype-json.html) data explicitly have [NpgsqlParameter.NpgsqlDbType](http://www.npgsql.org/api/Npgsql.NpgsqlParameter.html#Npgsql_NpgsqlParameter_NpgsqlDbType) set to [Npgsql​Db​Type.Jsonb](http://www.npgsql.org/api/NpgsqlTypes.NpgsqlDbType.html). Npgsql does not infer this based on the DB column type. It is not possible for the Sql Persistence to control this setting while still avoiding a reference to Npgsql.

As such it is necessary to explicitly set `NpgsqlParameter.NpgsqlDbType` to `NpgsqlDbType.Jsonb`:

snippet: JsonBParameterModifier


### Json.Net TypeNameHandling

When using Json.Net `$type` feature via [TypeNameHandling](https://www.newtonsoft.com/json/help/html/P_Newtonsoft_Json_JsonSerializerSettings_TypeNameHandling.htm), then [MetadataPropertyHandling ](https://www.newtonsoft.com/json/help/html/P_Newtonsoft_Json_JsonSerializerSettings_MetadataPropertyHandling.htm) should be set to [ReadAhead](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_MetadataPropertyHandling.htm).

snippet: PostgresTypeNameHandling

The reason for this is Json.Net normally expects the `$type` metadata to be the first property of each object for best efficiency in deserialization. If the `$type` does not appear first, then Json.Net assumes it isn't there. When using the [PostgreSQL Jsonb](https://www.postgresql.org/docs/9.4/static/datatype-json.html) the JSON stored does not preserve the order of object keys. This can result in the `$type` being stored in a non-first position.


## Unicode support

include: unicode-support


## Schema support

The PostgreSQL dialect supports multiple schemas. By default, when schema is not specified, it uses `public` schema when referring to database objects.

snippet: PostgreSqlSchema


## Case sensitivity - UpperCamelCase

Unless explicitly specified, PostgreSQL automatically lower-cases all identifier names (e.g. column or table names, etc.). To enforce case-sensitivity, it is necessary to quote all names.

SQL persistence internally honors the UpperCamelCase (also called PascalCase) convention, which is the standard default in other popular database engines, e.g. MS SQL Server. SQL persistence uses quoted identifier names in stored procedures, queries, etc.


## Supported name lengths

include: name-lengths

The SQL Persistence provides autonomy between endpoints by using separate tables for every endpoint based on the endpoint name. By default PostgreSQL [limits object names to 63 characters](https://www.postgresql.org/docs/current/static/sql-syntax-lexical.html#sql-syntax-identifiers).

By default SQL persistence uses an endpoint's name as a table prefix, the maximum length of the table prefix is 20 characters. The table prefix [can be customized](/persistence/sql/install.md#table-prefix).


## Using `pg_temp.` schema in installation scripts

The table creation for the SQL Persistence requires some dynamic SQL script execution. To achieve this in PostgreSql it is necessary to create temporary functions. These functions are created in the PostgreSql temporary-table schema, commonly referred to as `pg_temp`.

As per [Client Connection Defaults](https://www.postgresql.org/docs/9.2/static/runtime-config-client.html) `pg_temp` is:

> the current session's temporary-table schema, pg_temp_nnn, is always searched if it exists. It can be explicitly listed in the path by using the alias pg_temp