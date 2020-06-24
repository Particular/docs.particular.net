### User provided transactions

#### Native transactions

When sending messages it is possible to provide custom `SqlTransaction` instance that will be used when executing transport operations. This enables sending two or more message in a single, atomic transaction or share the same transaction between transport and relational data store. 

This API can be used both with `MessageSession` and in the message receive context eg. in a handler.

snippet: UseCustomSqlConnectionAndTransaction

#### Transaction scope

When sending messages it is possible to provide custom `SqlConnection` instance that will be used when executing transport operations. This can be useful in scenarios when the connection enlists in a `TransactionScope` before it's passed to the send operations.

This API can be used both with `MessageSession` and in the message receive context eg. in a handler. 

snippet: UseCustomSqlConnection