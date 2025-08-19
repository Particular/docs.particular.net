---
title: PostgreSQL dialect design
summary: An SQL persister dialect that specifically targets PostgreSQL, including AWS Aurora PostgreSQL.
component: SqlPersistence
related:
reviewed: 2024-10-09
versions: "[3,)"
redirects:
  - nservicebus/sql-persistence/postgresql-design
---

> [!WARNING]
> Even though PostgreSQL is a free product, it is recommended to ensure that support agreements are in place when running the SQL persistence for PostgreSQL in production. For details see [PostgreSQL Commercial support](https://www.postgresql.org/support/professional_support/).

## Supported database versions

SQL persistence supports:

- [PostgreSQL 10](https://www.postgresql.org/docs/10/release-10.html) and above
- AWS Aurora PostgreSQL

### PostGIS / NetTopologySuite

[PostGIS](https://postgis.net/) and [NetTopologySuite](https://github.com/NetTopologySuite/NetTopologySuite) are popular extensions. The [PostGIS/NetTopologySuite Type Plugin guidance at npgsql.org](https://www.npgsql.org/doc/types/nts.html) explains how to setup these extensions with PostgreSQL. 

## Usage

Use the [Npgsql NuGet Package](https://www.nuget.org/packages/Npgsql/).

snippet: sqlpersistenceusagepostgresql

### Token Authentication

To connect using token credentials, a User ID must be supplied in the connection string with the password supplied from the access token. Given that the token is only short-lived, a [data source builder must be utilized to handle password refreshes](https://devblogs.microsoft.com/dotnet/using-postgre-sql-with-dotnet-and-entra-id/). The following example uses Microsoft Entra ID.

snippet: SqlPersistenceUsagePostgreSqlEntra

### Passing Jsonb as NpgsqlDbType

[Npgsql](https://www.npgsql.org) requires parameters that pass [JSONB](https://www.postgresql.org/docs/9.4/datatype-json.html) data to have [NpgsqlParameter.NpgsqlDbType](https://www.npgsql.org/doc/api/Npgsql.NpgsqlParameter.html#Npgsql_NpgsqlParameter__ctor_System_String_NpgsqlTypes_NpgsqlDbType_) explicitly set to [Npgsql​Db​Type.Jsonb](https://www.npgsql.org/doc/api/NpgsqlTypes.NpgsqlDbType.html). Npgsql does not infer this based on the DB column type. It is not possible for the Sql Persistence to control this setting while still avoiding a reference to Npgsql.

As such it is necessary to explicitly set `NpgsqlParameter.NpgsqlDbType` to `NpgsqlDbType.Jsonb`:

snippet: JsonBParameterModifier

### Json.Net TypeNameHandling

When using Json.Net `$type` feature via [TypeNameHandling](https://www.newtonsoft.com/json/help/html/P_Newtonsoft_Json_JsonSerializerSettings_TypeNameHandling.htm), [MetadataPropertyHandling ](https://www.newtonsoft.com/json/help/html/P_Newtonsoft_Json_JsonSerializerSettings_MetadataPropertyHandling.htm) should be set to [ReadAhead](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_MetadataPropertyHandling.htm).

snippet: PostgresTypeNameHandling

This is because Json.Net normally expects the `$type` metadata to be the first property of each object for best efficiency in deserialization. If the `$type` does not appear first, then Json.Net assumes it isn't there. When using the [PostgreSQL Jsonb](https://www.postgresql.org/docs/9.4/datatype-json.html) the JSON stored does not preserve the order of object keys. This can result in storing the `$type` in a non-first position.

## Unicode support

include: unicode-support

## Schema support

The PostgreSQL dialect supports multiple schemas. By default, when schema is not specified, it uses `public` schema when referring to database objects.

snippet: PostgreSqlSchema

## Case sensitivity

Unless explicitly specified, PostgreSQL automatically lower-cases all identifier names (e.g. column or table names, etc.). To enforce case sensitivity, it is necessary to quote all names.

SQL persistence internally honors the UpperCamelCase (also called PascalCase) convention, which is the standard default in other popular database engines, e.g. MS SQL Server. SQL persistence uses quoted identifier names in stored procedures, queries, etc.

## Supported name lengths

include: name-lengths

The SQL Persistence provides autonomy between endpoints by using separate tables for every endpoint based on the endpoint name. By default PostgreSQL [limits object names to 63 characters](https://www.postgresql.org/docs/current/sql-syntax-lexical.html#sql-syntax-identifiers).

By default, SQL persistence uses an endpoint's name as a table prefix, the maximum length of the table prefix is 20 characters. The table prefix [can be customized](/persistence/sql/install.md#table-prefix).

## Using `pg_temp.` schema in installation scripts

The table creation for the SQL Persistence requires some dynamic SQL script execution. To achieve this in PostgreSql it is necessary to create temporary functions. These functions are created in the PostgreSql temporary-table schema, commonly referred to as `pg_temp`.

As per [Client Connection Defaults](https://www.postgresql.org/docs/9.2/runtime-config-client.html) `pg_temp` is:

> The current session's temporary-table schema, pg_temp_nnn, is always searched if it exists. It can be explicitly listed in the path by using the alias pg_temp
