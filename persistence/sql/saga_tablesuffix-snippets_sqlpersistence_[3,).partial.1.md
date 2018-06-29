On an NServiceBus `Saga<T>`, the table suffix can be overridden by decorating the saga class with an attribute:

snippet: table-suffix-with-attribute

The table suffix can also be defined more easily using a property override [when using a `SqlSaga<T>` base class](sqlsaga.md#table-suffix).