### User provided transactions

When sending messages outside of a handler context it is possible to provide custom `SqlConnection` and `SqlTransaction` instances that will be used to perform transport operations. This enables sending two or more message in a single, atomic transaction or share the same transaction between transport and relational data store.

snippet: UseCustomSqlConnectionAndTransaction