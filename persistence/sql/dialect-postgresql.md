---
title: SQL Persistence - PostgreSQL dialect
summary: An SQL persister dialect that specifically targets PostgreSQL, including AWS Aurora PostgreSQL.
component: SqlPersistence
reviewed: 2026-06-17
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

## Usage

Use the [Npgsql NuGet Package](https://www.nuget.org/packages/Npgsql/).

snippet: sqlpersistenceusagepostgresql

### Token Authentication

To connect using token credentials, a User ID must be supplied in the connection string with the password supplied from the access token. Given that the token is only short-lived, a [data source builder must be utilized to handle password refreshes](https://devblogs.microsoft.com/dotnet/using-postgre-sql-with-dotnet-and-entra-id/). The following example uses Microsoft Entra ID.

snippet: SqlPersistenceUsagePostgreSqlEntra

### Passing Jsonb as NpgsqlDbType

When handling parameters that pass [JSONB](https://www.postgresql.org/docs/current/datatype-json.html) data, it is [necessary to `NpgsqlParameter.NpgsqlDbType` to `NpgsqlDbType.Jsonb`](https://www.npgsql.org/doc/types/json.html?q=jsonb&tabs=datasource#string-mapping):

snippet: JsonBParameterModifier

### Newtonsoft.Json TypeNameHandling

When using Newtonsoft.Json as serializer and the `$type` feature via [TypeNameHandling](https://www.newtonsoft.com/json/help/html/P_Newtonsoft_Json_JsonSerializerSettings_TypeNameHandling.htm), the [MetadataPropertyHandling](https://www.newtonsoft.com/json/help/html/P_Newtonsoft_Json_JsonSerializerSettings_MetadataPropertyHandling.htm) should be set to [ReadAhead](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_MetadataPropertyHandling.htm).

snippet: PostgresTypeNameHandling

This is because Newtonsoft.Json normally expects the `$type` metadata to be the first property of each object for best efficiency in deserialization. If the `$type` does not appear first, then it assumes the property isn't there. 
However, when using [Jsonb data](https://www.postgresql.org/docs/current/datatype-json.html) the stored object does not preserve the order of its keys, which can result in storing the `$type` property in any other position, causing for Newtonsoft.Json to assume that the object doesn't have the property.

### Spatial data

In order to handle spatial data, it is necessary to add the [Npgsql.NetTopologySuite](https://www.nuget.org/packages/Npgsql.NetTopologySuite) NuGet package.
This [guide on npgsql](https://www.npgsql.org/doc/types/nts.html) explains how to set it up to work with PostgreSQL.

## Unicode support

include: unicode-support

## Schema support

The PostgreSQL dialect supports multiple schemas. By default, when a schema is not specified, it uses `public` schema when referring to database objects.

snippet: PostgreSqlSchema

## Case sensitivity

Unless explicitly specified, PostgreSQL uses lower cases for all identifier names (e.g. column or table names, etc.). To enforce case sensitivity, it is necessary to quote all names.

SQL persistence internally honors the UpperCamelCase (also called PascalCase) convention. SQL persistence uses quoted identifier names in stored procedures, queries, etc.

## Supported name lengths

include: name-lengths

The SQL Persistence provides autonomy between endpoints by using separate tables for every endpoint based on the endpoint name. By default PostgreSQL [limits object names to 63 characters](https://www.postgresql.org/docs/current/sql-syntax-lexical.html#SQL-SYNTAX-IDENTIFIERS).

By default, SQL persistence uses an endpoint's name as a table prefix, the maximum length of the table prefix is 20 characters. The table prefix [can be customized](/persistence/sql/install.md#table-prefix).

## Using `pg_temp.` schema in installation scripts

The table creation for the SQL Persistence requires some dynamic SQL script execution. To achieve this in PostgreSql it is necessary to create temporary functions. These functions are created in the PostgreSql temporary-table schema, commonly referred to as `pg_temp`.

As per [Client Connection Defaults](https://www.postgresql.org/docs/current/runtime-config-client.html) `pg_temp` is:

> The current session's temporary-table schema, pg_temp_nnn, is always searched if it exists. It can be explicitly listed in the path by using the alias pg_temp
