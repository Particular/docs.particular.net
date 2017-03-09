---
title: SQL Server Transport Runtime SQL
component: SqlServer
reviewed: 2017-03-08
versions: '[3,)'
---

SQL used at runtime to perform various operations.


## Create Queue

Performs queue creation

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

NOTE: T-sql statements for sending and receiving messges execute with [`NOCOUNT ON`](https://msdn.microsoft.com/en-us/library/ms189837.aspx) option. This does not affect original value of the setting. It's value is saved at the beginning and resored at the end of the statement. 

Retrieves a message from the queue.

snippet: ReceiveTextSql


## Send message

Places a message on the queue.

snippet: SendTextSql


## Missing index warning

Used to log a warning if a required index is missing. See also [Upgrade from version 2 to 3](/nservicebus/upgrades/sqlserver-2to3.md#namespace-changes-indexes).

snippet: CheckIfExpiresIndexIsPresentSql
