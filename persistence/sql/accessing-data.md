---
title: Accessing data via SQL persistence
summary: How to access business data using connections managed by NServiceBus SQL persistence.
component: SqlPersistence
reviewed: 2017-04-24
related:
 - nservicebus/handlers/accessing-data
 - samples/sqltransport-sqlpersistence
redirects:
 - nservicebus/sql-persistence/accessing-data
---

SQL persistence supports a mechanism that allows using the same data context used by NServiceBus internals to also store business data. This ensures atomicity of changes done across multiple handlers and sagas involved in processing of the same message. See [accessing data](/nservicebus/handlers/accessing-data.md) to learn more about other ways of accessing the data in the handlers.

The current [DbConnection](https://msdn.microsoft.com/en-us/library/system.data.common.dbconnection.aspx) and [DbTransaction](https://msdn.microsoft.com/en-us/library/system.data.common.dbtransaction.aspx) can be accessed via the current context.

NOTE: If different connections strings were used for particular persistence features such as sagas, timeouts, etc. then `context.SynchronizedStorageSession.SqlPersistenceSession()` will expose connection for sagas.


### Using in a Handler

snippet: handler-sqlPersistenceSession


### Using in a Saga

include: saga-business-data-access

snippet: saga-sqlPersistenceSession


Regardless of how the database connection is accessed, it is fully managed by NServiceBus.

include: transport-transaction-scope
