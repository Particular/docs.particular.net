---
title: SQL Server transport SQL statements
summary: Overview of the SQL statements used to manage the SQL Server transport
component: SqlTransport
reviewed: 2018-05-28
versions: '[3,)'
redirects:
- nservicebus/sqlserver/runtime-sql
- transports/sqlserver/runtime-sql
- transports/sql/runtime-sql
---

## Installation

At installation time the queue creation script is executed

snippet: CreateQueueTextSql


### Creating table structure in production

There are some special considerations for creating the queue tables in production environments.


#### NServiceBus installers

When using NServiceBus [installers](/nservicebus/operations/installers.md) the queue tables are created automatically before the endpoint is started.

the user account under which the installation of the host is performed must have `CREATE TABLE` and `VIEW DEFINITION` permissions on the database where the queues are to be created. The account under which the service runs does not have to have these permissions. Standard read/write/delete permissions (e.g. `db_datawriter` and `db_datareader` roles) are enough.


#### Scripted

Using NServiceBus installers does not allow review of the actual T-SQL statements that are going be executed. For that reason, some prefer to store the actual scripts in a version control system.  
 
The script above is parametrized at execution time with the queue name so it cannot be used as-is. Alternatively, the scripts could be generated from the development or staging environments, then directly executed on a production environment by DBAs to replicate that table structure. 

To capture the script for later execution use SQL Server Management Studio. Connect to the server (e.g. development or staging) and right-click the database with the queue tables. From "Tasks" menu choose "Generate Scripts..." and generate the scripts for relevant tables.

![](generating-ddl.png)

Store these scripts so they can be executed as part of the production deployment.


## Runtime

The following are the T-SQL statements used by the transport at runtime.


### Peek message

Checks if there are messages in the queue.

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

NOTE: The `CorrelationId`, `ReplyToAddress` and `Recoverable` columns are required for backwards compatibility with version 1 of the transport. When receiving messages sent by endpoints that use later versions, the values of correlation ID and reply-to address should be read from the headers (`NServiceBus.CorrelationId` and `NServiceBus.ReplyToAddress`) instead. The value `Recoverable` can be ignored as it is always `true`/`1`.


### Send message

Places a message on the queue.

snippet: SendTextSql

NOTE: The `CorrelationId`, `ReplyToAddress` and `Recoverable` columns are required for backwards compatibility with version 1 of the transport. When sending messages to endpoints that use later versions, the values of correlation ID and reply-to address columns could be set to `NULL` and the actual values provided in the headers (`NServiceBus.CorrelationId` and `NServiceBus.ReplyToAddress`). The value `Recoverable` should always be `true`/`1`.


### Missing index warning

Used to log a warning if a required index is missing. See also [Upgrade from version 2 to 3](/transports/upgrades/sqlserver-2to3.md#namespace-changes-indexes).

snippet: CheckIfExpiresIndexIsPresentSql


### Check column type

Used to log a warning if the message headers data type is non-unicode. See also [Supporting unicode characters in headers](/transports/upgrades/sqlserver-unicode-headers.md).

snippet: CheckHeadersColumnTypeSql



partial: extra
