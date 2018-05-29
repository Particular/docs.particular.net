## Format

The SQL Server Transport address has following canonical form for version 3.0.x

```
table@[schema]
```

while for Versions 3.1.x it includes also the optional catalog

```
table@[schema]@[catalog]
```

include: addressing_parts_3


## Resolution

The address is resolved into a fully-qualified table name that includes table name, its schema and catalog. In the address the table name is the only mandatory part. An address containing only a table name is a valid address e.g. `MyTable`.


### Schema

include: addressing_schema_3


### Catalog

The catalog is only supported by Version 3.1.x and higher. In order to use multiple catalogs in Version 3.0.x use [multiple connection strings](/transports/sql/connection-settings.md?version=SqlTransport_3#multiple-connection-strings) in the multi-instance mode.

include: addressing_catalog_3


### Backwards compatibility

include: addressing-compat-3

include: addressing-compat-1-and-2