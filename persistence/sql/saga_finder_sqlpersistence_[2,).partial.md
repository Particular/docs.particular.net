
## Saga Finder

The SQL Persistence exposes an API to enable creating [Saga Finders](/nservicebus/sagas/saga-finding.md).


### Usage

The API is exposed as an extension method on `SynchronizedStorageSession` and can be called as follows:


#### Microsoft SQL Server

WARNING: On Microsoft SQL Server, the saga finder feature requires the `JSON_VALUE` function that is only avaiable starting with SQL Server 2016.

snippet: SagaFinder-sqlServer


#### MySql

snippet: SagaFinder-MySql


### Parameters


### context

Used to ensure the concurrency metadata is stored in the current session.


#### whereClause

This text will be appended to a standard Saga select statement:

snippet: MsSqlServer_SagaSelectSql


#### appendParameters

`appendParameters` allows [DbParameter](https://msdn.microsoft.com/en-us/library/system.data.common.dbparameter.aspx)s to be appended to the underlying [DbCommand](https://msdn.microsoft.com/en-us/library/system.data.common.dbcommand.aspx) that will perform the query.

**builder**: calls through to [DbCommand.CreateParameter](https://msdn.microsoft.com/en-us/library/system.data.common.dbcommand.createparameter.aspx) to allow construction on a [DbParameter](https://msdn.microsoft.com/en-us/library/system.data.common.dbparameter.aspx).

**append**: calls through to [DbParameterCollection.Add](https://msdn.microsoft.com/en-us/library/system.data.common.dbparametercollection.add.aspx) to add the parameter to the underlying [DbCommand](https://msdn.microsoft.com/en-us/library/system.data.common.dbcommand.aspx).


### IContainSagaData Construction

Converting the returned information into an `IContainSagaData` will then be performed by the SQL Persister. 

See also [SQL Persistence Saga Finder Sample](/samples/saga/sql-sagafinder/).
