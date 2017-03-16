
## Saga Finder

The SQL Persistence exposes a specific API to enable creating [Saga Finders](/nservicebus/sagas/saga-finding.md).


### Microsoft SQL Server

snippet: SagaFinder-sqlServer


### MySql

snippet: SagaFinder-MySql


This will result in a SQL Select query being executed with the where statement appended to the following:

snippet: MsSqlServer_SagaSelectSql

Converting the returned information into a `IContainSagaData` will then be performed by the SQL Persister. See also [SQL Persistence Saga Finder Sample](https://docs.particular.net/samples/saga/sql-sagafinder/).