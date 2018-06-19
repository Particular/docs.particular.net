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
