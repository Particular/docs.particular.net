---
title: Accessing data via SQL persistence
summary: How to access business data using connections managed by NServiceBus SQL persistence.
component: SqlPersistence
reviewed: 2020-12-03
related:
 - nservicebus/handlers/accessing-data
 - samples/sqltransport-sqlpersistence
 - samples/entity-framework
redirects:
 - nservicebus/sql-persistence/accessing-data
---

Accessing business data

SQL persistence supports a mechanism that allows using the same data context used by NServiceBus internals to also store business data. This ensures atomicity of changes done across multiple handlers and sagas involved in processing of the same message. See [accessing data](/nservicebus/handlers/accessing-data.md) to learn more about other ways of accessing the data in the handlers.

The current [DbConnection](https://msdn.microsoft.com/en-us/library/system.data.common.dbconnection.aspx) and [DbTransaction](https://msdn.microsoft.com/en-us/library/system.data.common.dbtransaction.aspx) can be accessed via the current context.

partial: caveats


### Using SQL data context

snippet: handler-sqlPersistenceSession

partial: di

### Using Entity Framework

When using Entity Framework (or another object/relational mapper) to access business data, there is the option to create an Entity Framework data context within a handler and use the Synchronized Storage Session to reuse the connection to the database.

Another option is to inject the Entity Framework data context into the handler. When NServiceBus has finished processing a message it will publish an in-process event that provides the ability to call the `SaveChanges` method on the Entity Framework data context. More information can be found in the sample for using [samples/entity-framework](/samples/entity-framework-core/).


### Using in a saga

include: saga-business-data-access

snippet: saga-sqlPersistenceSession


Regardless of how the database connection is accessed, it is fully managed by NServiceBus.

include: transport-transaction-scope
