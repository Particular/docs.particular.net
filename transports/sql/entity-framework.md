---
title: Using the SQL Server transport with Entity Framework
reviewed: 2021-05-14
component: SqlTransport
redirects:
 - nservicebus/sqlserver/entity-framework
 - transports/sqlserver/entity-framework
---

To avoid escalating transactions to the [Distributed Transaction Coordinator (DTC)](https://en.wikipedia.org/wiki/Microsoft_Distributed_Transaction_Coordinator), operations using [Entity Framework](https://docs.microsoft.com/en-us/ef/) must share their connection string with the SQL Server transport.

However, the connection string used for the SQL Server transport cannot be used directly by Entity Framework when the _Database/Model First_ approach is used. In this case, Entity Framework requires a [special connection string](https://docs.microsoft.com/en-us/ef/ef6/fundamentals/configuring/connection-strings#databasemodel-first-with-connection-string-in-appconfigwebconfig-file) containing specific metadata.

The metadata can be added using `EntityConnectionStringBuilder`. The modified connection string can then be used to create an `EntityConnection`, which can then be used to create an instance of the generated `DbContext` type:

snippet: EntityConnectionCreationAndUsage

The `DbContext` generated Entity Framework does not have a constructor with an `EntityConnection`, but since it is a partial class, the constructor can be added:

snippet: DbContextPartialWithEntityConnection

NOTE: The code snippets above assume that the created entity data model is named `MySample`. The references should match the names used in the project.
