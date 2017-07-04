## Format

The SQL Server address has following canonical form

```
table@[schema]
```

where:

 * `table` is an unquoted delimited identifier without the surrounding square brackets. Whitespace and special characters are allowed and are not escaped e.g. `my table` and `my]table` are legal values. The identifier is quoted automatically by SQL Server transport when executing the SQL statements. `@` is a separator between the table and schema parts and thus is not a valid character.
 * `schema` is either an unquoted delimited identifier without the surrounding square brackets or a standard bracket-delimited identifier. In the second form it is always surrounded by brackets and any right brackets (`]`) inside are escaped e.g. `[my]]schema]`. `@` is only allowed in the bracket-delimited form, otherwise it is treated as separator.


## Resolution

The address is resolved into a qualified table name that includes both table name and its schema. In the address the table name is the only mandatory part. An address containing only a table name is a valid address e.g. `MyTable`.


### Schema

include: addressing_schema_3

### Backwards compatibility

Versions 1 and 2 of SQL Server transport only recognize the table name part of the address. If such an endpoint receives a two-part address e.g. `MyTable@[MySchema]` (either as a reply-to address or as a subscriber address), it will ignore anything past the first occurrence of `@` when using that address as a destination.

The schema has to be configured for `MyTable` at that legacy endpoint in order for it to be able to reply or publish to the correct queue.
