The meaning of the parts is the following:

 * `table` is an unquoted delimited identifier without the surrounding square brackets. Whitespace and special characters are allowed and are not escaped, e.g. `my table` and `my]table` are legal values. The identifier is quoted automatically by SQL Server transport when executing the SQL statements. `@` is a separator between the table and schema parts and thus is not a valid character.
 * `schema` is either an unquoted delimited identifier without the surrounding square brackets or a standard bracket-delimited identifier. In the second form, it is always surrounded by brackets, and any right brackets (`]`) inside are escaped, e.g. `[my]]schema]`. `@` is only allowed in the bracket-delimited form; otherwise, it is treated as a separator.
 * `catalog` has the same syntax as `schema`.
