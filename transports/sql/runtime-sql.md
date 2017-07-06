---
title: SQL Server Transport Runtime SQL
component: SqlServer
reviewed: 2017-07-07
versions: '[3,)'
redirects:
- nservicebus/sqlserver/runtime-sql
- transports/sqlserver/runtime-sql
---

SQL used at runtime to perform various operations.


## Create Queue

Performs queue creation.

snippet: CreateQueueTextSql


## Peek message

Check if there are messages in the queue.

snippet: PeekTextSql


## Purge expired

Purges expired messages from the queue.

snippet: PurgeBatchOfExpiredMessagesTextSql


## Purge at startup

Used by an endpoint to optionally purge all message on startup.

snippet: PurgeTextSql


## Receive message

NOTE: The T-SQL statements for sending and receiving messges execute with [`NOCOUNT ON`](https://docs.microsoft.com/en-us/sql/t-sql/statements/set-nocount-transact-sql) option. However, this does not affect the original value of this setting. The original value is saved at the beginning and restored after executing the statement.

Retrieves a message from the queue.

snippet: ReceiveTextSql


## Send message

Places a message on the queue.

snippet: SendTextSql


## Missing index warning

Used to log a warning if a required index is missing. See also [Upgrade from version 2 to 3](/transports/upgrades/sqlserver-2to3.md#namespace-changes-indexes).

snippet: CheckIfExpiresIndexIsPresentSql


partial: extra