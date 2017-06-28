## Format

The SQL Server address has following canonical form

```
table@[schema]@[catalog]
```

where:

 * `table` is an unquoted delimited identifier without the surrounding square brackets. Whitespace and special characters are allowed and are not escaped e.g. `my table` and `my]table` are legal values. The identifier is quoted automatically by SQL Server transport when executing the SQL statements. `@` is a separator between the table and schema parts and thus is not a valid character. 
 * `schema` is either an unquoted delimited identifier without the surrounding square brackets or a standard bracket-delimited identifier. In the second form it is always surrounded by brackets and any right brackets (`]`) inside are escaped e.g. `[my]]schema]`. `@` is only allowed in the bracket-delimited form, otherwise it is treated as separator.
 * `catalog` has the same syntax as `schema`.


## Resolution

The address is resolved into a fully-qualified table name that includes table name, its schema and catalog. In the address the table name is the only mandatory part. An address containing only a table name is a valid address e.g. `MyTable`.


### Schema

Schema is optional. Even if it is present in the address, it can be overridden by configuration. The algorithm for calculating the schema is following:

 * If schema is configured for a given queue via `UseSchemaForQueue`, the configured value is used.
 * If [logical routing](/nservicebus/messaging/routing.md#command-routing) is is used and schema is configured for a given endpoint via `UseSchemaForEndpoint`, the configured schema is used.
 * If destination address contains schema, the schema from address is used.
 * If default schema is configured via `DefaultSchema`, the configured value is used.
 * Otherwise `dbo` is used as a default schema.

NOTE: Because both schema and catalog are independent and optional, it is legal to specify only table and catalog names in the address. In such case the schema part should be delimited and empty i.e. `table@[]@[catalog]`.


### Catalog

Catalog is optional. Even if it is present in the address, it can be overridden by configuration. The algorithm for calculating the schema is following:

 * If catalog is configured for a given queue via `UseCatalogForQueue`, the configured value is used.
 * If [logical routing](/nservicebus/messaging/routing.md#command-routing) is is used and catalog is configured for a given endpoint via `UseCatalogForEndpoint`, the configured catalog is used.
 * If destination address contains catalog, the catalog from address is used.
 * Otherwise the catalog configured as `Initial catalog` or `Database` in the connection string is used.


### Backwards compatibility


#### Version 3.0

Version 3.0 of SQL Server transport did not recognize the catalog part of the address. If such an endpoint receives  a three-part address, e.g. `MyTable@[MySchema]@[MyCatalog]` (either as a reply-to address or as a subscriber address), the Version 3.0 transport endpoint will drop the last part (catalog) when parsing the address. 

If the communicating endpoints use different catalogs, the Version 3.0 endpoint needs to be configured to use [multi-instance mode](/transports/sql/deployment-options.md#modes-overview-multi-instance) with `MyTable@[MySchema]` address bound to a connection string that specifies `MyCatalog` as an initial catalog.


#### Versions 1 and 2

Versions 1 and 2 of SQL Server transport only recognize the table name part of the address. If such an endpoint receives a three-part address e.g. `MyTable@[MySchema]@[MyCatalog]` (either as a reply-to address or as a subscriber address), it will ignore anything past the first occurrence of `@` when using that address as a destination.

Both schema and catalog have to be configured for `MyTable` at that legacy endpoint in order for it to be able to reply or publish to the correct queue.