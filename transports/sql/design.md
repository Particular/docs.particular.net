---
title: Design
summary: The design and implementation details of SQL Server Transport
reviewed: 2016-08-08
component: SqlTransport
tags:
- Transactions
- Transport
redirects:
 - nservicebus/sqlserver/design
 - transports/sqlserver/design
---

### Primary queue

Each endpoint has a single table representing the primary queue. The name of the primary queue matches the name of the endpoint.

In a scale out scenario this single queue is shared by all instances.

partial: callback


### Other queues

Each endpoint also has queues required by timeout (the exact names and number of queues created depends on the version of the transport) and retry mechanisms.

Error and audit queues are usually shared among multiple endpoints.


### Queue table structure

Following SQL DDL is used to create a table and its index for a queue:

```sql
CREATE TABLE [schema].[queuename](
	[Id] [uniqueidentifier] NOT NULL,
	[CorrelationId] [varchar](255),
	[ReplyToAddress] [varchar](255),
	[Recoverable] [bit] NOT NULL,
	[Expires] [datetime],
	[Headers] [varchar](max) NOT NULL,
	[Body] [varbinary](max),
	[RowVersion] [bigint] IDENTITY(1,1) NOT NULL
)
ON [PRIMARY];

CREATE CLUSTERED INDEX [Index_RowVersion]
ON [schema].[queuename]
(
	[RowVersion] ASC
)

CREATE NONCLUSTERED INDEX [Index_Expires]
ON [schema].[queuename]
(
	[Expires] ASC
)
INCLUDE
(
	[Id],
	[RowVersion]
)
```

Receiving messages is conducted by a `DELETE` statement from the top of the table (the oldest row according to the `[RowVersion]` column).

The tables are created by [installers](/nservicebus/operations/installers.md) when the application is started for the first time. It is required that the user account under which the installation of the host is performed has `CREATE TABLE` as well as `VIEW DEFINITION` permissions on the database in which the queues are to be created. The account under which the service runs does not have to have these permissions. Standard read/write/delete permissions (e.g. being member of `db_datawriter` and `db_datareader` roles) are enough.


### Creating table structure in Production

The scripts above, to generate the queues, do not have queue names. This may cause confusion when reviewed by a DBA. The scripts could, alternatively, be generated off the Development or Staging environment, and then directly executed on Production environment by DBAs to replicate that table structure. 

To generate this DDL script, right-click the database and from "Tasks" menu choose "Generate Scripts..." and generate the scripts for relevant tables.

![](generating-ddl.png)

partial: indexes
