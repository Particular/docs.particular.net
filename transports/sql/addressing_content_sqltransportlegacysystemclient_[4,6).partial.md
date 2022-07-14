## Format

The SQL Server Transport address has the following canonical form:

```
table@[schema]@[catalog]
```

include: addressing_parts_3


## Resolution

The address is resolved into a fully-qualified table name that includes the table name, its schema, and catalog. In the address, the table name is the only mandatory part. An address containing only a table name is a valid address e.g. `MyTable`.


### Schema

include: addressing_schema_3


### Catalog


include: addressing_catalog_3