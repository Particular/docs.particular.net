snippet: handler-sqlPersistenceSession-DI

NOTE: A helper type `ISqlStorageSession` is used in the example above because by default `DbConnection` and `DbTransaction` are not registered in the DI container. Users who wish to register these types need to add the following lines to the endpoint configuration:

snippet: sqlPersistenceSession-DI-register