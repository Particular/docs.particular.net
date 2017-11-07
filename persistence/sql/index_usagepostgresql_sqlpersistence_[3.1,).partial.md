
### PostgreSQL

Using the [Npgsql NuGet Package](https://www.nuget.org/packages/Npgsql/).

snippet: sqlpersistenceusagepostgresql


#### Passing Jsonb as NpgsqlDbType

[Npgsql](http://www.npgsql.org) requires that parameters that pass [JSONB](https://www.postgresql.org/docs/9.4/static/datatype-json.html) data explicitly have [NpgsqlParameter.NpgsqlDbType](http://www.npgsql.org/api/Npgsql.NpgsqlParameter.html#Npgsql_NpgsqlParameter_NpgsqlDbType) set to [Npgsql​Db​Type.Jsonb](http://www.npgsql.org/api/NpgsqlTypes.NpgsqlDbType.html). Npgsql does not infer this based on the DB column type. It is not possible for the Sql Persistence to control this setting while still avoiding a reference to Npgsql.

As such it is necessary to explicitly set `NpgsqlParameter.NpgsqlDbType` to `NpgsqlDbType.Jsonb`:

snippet: JsonBParameterModifier


#### Json.Net TypeNameHandling

When using Json.Net `$type` feature via [TypeNameHandling](https://www.newtonsoft.com/json/help/html/P_Newtonsoft_Json_JsonSerializerSettings_TypeNameHandling.htm), then [MetadataPropertyHandling ](https://www.newtonsoft.com/json/help/html/P_Newtonsoft_Json_JsonSerializerSettings_MetadataPropertyHandling.htm) should be set to [ReadAhead](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_MetadataPropertyHandling.htm).

snippet: PostgresTypeNameHandling

The reason for this is Json.Net normally expects the `$type` metadata to be the first property of each object for best efficiency in deserialization. If the `$type` does not appear first, then Json.Net assumes it isn't there. When using the [PostgreSQL Jsonb](https://www.postgresql.org/docs/9.4/static/datatype-json.html) the JSON stored does not preserve the order of object keys. This can result in the `$type` being stored in a non-first position.