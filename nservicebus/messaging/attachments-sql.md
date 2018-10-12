---
title: SQL Attachments
summary: Use SQL Server varbinary to store attachments for messages.
reviewed: 2018-04-06
component: AttachmentsSql
related:
 - samples/attachments-sql
 - samples/attachments-fileshare
---

Uses a SQL Server [varbinary](https://docs.microsoft.com/en-us/sql/t-sql/data-types/binary-and-varbinary-transact-sql) to store attachments for messages.


## Usage

Two settings are required as part of the default usage:

 * A connection factory that returns an open instance of a [SqlConnection](https://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.aspx). Note that any Exception that occurs during opening the connection should be handled by the factory.
 * A default time to keep for attachments.

snippet: EnableAttachments


### Recommended Usage

Extract out the connection factory to a helper method

snippet: OpenConnection

Also uses the `NServiceBus.Attachments.Sql.TimeToKeep.Default` method for attachment cleanup.

This usage results in the following:

snippet: EnableAttachmentsRecommended


### Using ambient connectivity

Attachments can leverage the ambient SQL connectivity from either the [transport](/transports/) and/or the [persister](/persistence/).

If both `UseSynchronizedStorageSessionConnectivity` and `UseTransportConnectivity` are defined, the `SynchronizedStorageSession` will be used first, followed by the `TransportTransaction`.


#### Use SynchronizedStorageSession connectivity

To use the ambient [SynchronizedStorageSession persister](/nservicebus/handlers/accessing-data.md#using-nservicebus-persistence):

snippet: UseSynchronizedStorageSessionConnectivity

This approach attempts to use the SynchronizedStorageSession using the following steps:

 * For the current context attempt to retrieve an instance of `SynchronizedStorageSession`. If no `SynchronizedStorageSession` exists, don't continue and fall back to the [SqlConnection](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection) retrieved by the `connectionFactory`.
 * Attempt to retrieve a property named 'Transaction' that is a [SqlTransaction](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqltransaction) from the `SynchronizedStorageSession`. If it exists, use it for all SQL operations in the current pipeline.
 * Attempt to retrieve a property named 'Connection' that is a [SqlConnection](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection) from the `SynchronizedStorageSession`. If it exists, use it for all SQL operations in the current pipeline.

The properties are retrieved using [reflection](https://docs.microsoft.com/en-us/dotnet/framework/reflection-and-codedom/reflection) since there is no API in NServiceBus to access SynchronizedStorageSession data via type.


#### Use transport connectivity

To use the ambient [transport transaction](/transports/transactions.md):

snippet: UseTransportConnectivity

This approach attempts to use the transport transaction using the following steps:

 * For the current context, attempt to retrieve an instance of `TransportTransaction`. If no `TransportTransaction` exists, don't continue and fall back to using the [SqlConnection](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection) retrieved by the `connectionFactory`.
 * Attempt to retrieve an instance of [Transaction](https://docs.microsoft.com/en-us/dotnet/api/system.transactions.transaction) from the `TransportTransaction`. If it exists, use it in [SqlConnection.EnlistTransaction](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection.enlisttransaction) with an instance of [SqlConnection](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection) retrieved by the `connectionFactory`. Then use that [SqlConnection](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection) for all SQL operations in the current pipeline.
 * Attempt to retrieve an instance of [SqlTransaction](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqltransaction) from the `TransportTransaction`. If it exists, use it for all SQL operations in the current pipeline.
 * Attempt to retrieve an instance of [SqlConnection](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection) from the `TransportTransaction`. If it exists, use it for all SQL operations in the current pipeline.


## Installation


### Script execution runs by default at endpoint startup

To streamline development the attachment installer is, by default, executed at endpoint startup, in the same manner as all other [installers](/nservicebus/operations/installers.md).

snippet: ExecuteAtStartup

NOTE: Note that this is also a valid approach for higher level environments.


### Optionally take control of script execution

However in higher level environment scenarios, where standard installers are being run, but the SQL attachment installation has been executed as part of a deployment, it may be necessary to explicitly disable the attachment installer executing while leaving standard installers enabled.

snippet: DisableInstaller


## Table Name

The default table name and schema is `dbo.MessageAttachments`. It can be changed with the following:

snippet: UseTableName



include: attachments
