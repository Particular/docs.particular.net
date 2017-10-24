
### PostgreSQL

Using the [Npgsql NuGet Package](https://www.nuget.org/packages/Npgsql/).

snippet: sqlpersistenceusagepostgresql

[Npgsql](http://www.npgsql.org) requires that parameters that pass [JSONB](https://www.postgresql.org/docs/9.4/static/datatype-json.html) data explicitly have [NpgsqlParameter.NpgsqlDbType](http://www.npgsql.org/api/Npgsql.NpgsqlParameter.html#Npgsql_NpgsqlParameter_NpgsqlDbType) set to [Npgsql​Db​Type.Jsonb](http://www.npgsql.org/api/NpgsqlTypes.NpgsqlDbType.html). Npgsql does not infer this based on the DB column type. It is not possible for the Sql Persistence to control this setting while still avoiding a reference to Npgsql.

As such it is necessary to explicitly set `NpgsqlParameter.NpgsqlDbType` to `NpgsqlDbType.Jsonb`:

snippet: JsonBParameterModifier