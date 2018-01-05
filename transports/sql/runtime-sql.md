---
title: SQL Server Transport SQL statements
component: SqlTransport
reviewed: 2017-07-07
versions: '[3,)'
redirects:
- nservicebus/sqlserver/runtime-sql
- transports/sqlserver/runtime-sql
---

## Installation

At installation time the queue creation script is executed

snippet: CreateQueueTextSql


### Creating table structure in production

There are some special considerations for creating the queue tables in higher environments e.g. production.


#### NServiceBus installers

When using NServiceBus [installers](/nservicebus/operations/installers.md) the queue tables are created automatically before the endpoint is started.

It is required that the user account under which the installation of the host is performed has `CREATE TABLE` as well as `VIEW DEFINITION` permissions on the database in which the queues are to be created. The account under which the service runs does not have to have these permissions. Standard read/write/delete permissions (e.g. being member of `db_datawriter` and `db_datareader` roles) are enough.


#### Scripted

Using NServiceBus installers not allow to review the actual T-SQL statements that are going be executed. For that reason some prefer to store the actual scripts in a version control system.  
 
The script above is parametrized at execution time with the queue name so it cannot be used as-is. The scripts could, alternatively, be generated off the Development or Staging environment, and then directly executed on Production environment by DBAs to replicate that table structure. 

To generate this DDL script, right-click the database and from "Tasks" menu choose "Generate Scripts..." and generate the scripts for relevant tables.

![](generating-ddl.png)


## Runtime

Following are the T-SQL statements used by the transport at runtime.


### Peek message

Check if there are messages in the queue.

snippet: PeekTextSql


### Purge expired

Purges expired messages from the queue.

snippet: PurgeBatchOfExpiredMessagesTextSql


### Purge at startup

Used by an endpoint to optionally purge all message on startup.

snippet: PurgeTextSql


### Receive message

NOTE: The T-SQL statements for sending and receiving messges execute with [`NOCOUNT ON`](https://docs.microsoft.com/en-us/sql/t-sql/statements/set-nocount-transact-sql) option. However, this does not affect the original value of this setting. The original value is saved at the beginning and restored after executing the statement.

Retrieves a message from the queue.

snippet: ReceiveTextSql


### Send message

Places a message on the queue.

snippet: SendTextSql


### Missing index warning

Used to log a warning if a required index is missing. See also [Upgrade from version 2 to 3](/transports/upgrades/sqlserver-2to3.md#namespace-changes-indexes).

snippet: CheckIfExpiresIndexIsPresentSql


### Check column type

Used to log a warning if the message headers data type is non unicode. See also [Supporting Unicode characters in headers](/transports/upgrades/sqlserver-unicode-headers.md).

snippet: CheckHeadersColumnTypeSql



partial: extra