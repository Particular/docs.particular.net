#### Versions 1 and 2

Versions 1 and 2 of the SQL Server transport only recognize the table name part of the address. If such an endpoint receives a three-part address e.g. `MyTable@[MySchema]@[MyCatalog]` (either as a reply-to address or as a subscriber address), it will ignore anything after the first occurrence of `@` when using that address as a destination.

Both schema and catalog have to be configured for `MyTable` at that legacy endpoint in order for it to be able to reply or publish to the correct queue.
