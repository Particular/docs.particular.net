---
title: Scripting SQL Server Transport
summary: Example code and scripts to facilitate deployment and operational actions against the SQLServer Transport.
---

The followings are example codes and scripts to facilitate deployment and operations against the SQL Server Transport.


## Native Send 


### The native send helper methods

The following code shows an example of how to perform the following actions

 * create and serialize headers.
 * write a message body directly to SQL Server Transport.


#### In C&#35;

snippet:sqlserver-nativesend


#### In Powershell;

snippet:sqlserver-powershell-nativesend


### Using the native send helper methods

snippet:sqlserver-nativesend-usage


## Create queues

Queue creation can be done for a specific endpoint or queues shared between multiple endpoints.


### The create queue helper methods

snippet:sqlserver-create-queues


### Using the create helper queue methods

To create all queues for a given endpoint name.

snippet:sqlserver-create-queues-endpoint-usage

To create shared queues.

snippet:sqlserver-create-queues-shared-usage


## Delete queues


### The delete helper queue methods

snippet:sqlserver-delete-queues


### Using the delete queue helper methods

To delete all queues for a given endpoint name.

snippet:sqlserver-delete-queues-endpoint-usage

To delete shared queues

snippet:sqlserver-delete-queues-shared-usage


## Return message to source queue


### The retry helper methods

The following code shows an example of how to perform the following actions

 * read a message from the error queue table.
 * forward that message to another queue table to be retried.

NOTE: Since the connection information for the endpoint that failed is not contained in the error queue table that information is explicitly passed in.

snippet:sqlserver-return-to-source-queue


### Using the retry helper methods

snippet:sqlserver-return-to-source-queue-usage


### Archiving SqlTransport audit log to long term storage

There are several ways to achieve this including using techniques like [Table Partitioning](https://technet.microsoft.com/en-us/library/ms188730.aspx) and [Snapshot Replication](https://technet.microsoft.com/en-us/library/ms151832.aspx). In this example [BCP utility](https://msdn.microsoft.com/en-AU/library/ms162802.aspx) will be used.


#### Create helper "archive" table

Create an audit_archive table with this SQL script.

```
CREATE TABLE [dbo].[audit_archive](
	[Id] [uniqueidentifier] NOT NULL,
	[CorrelationId] [varchar](255) NULL,
	[ReplyToAddress] [varchar](255) NULL,
	[Recoverable] [bit] NOT NULL,
	[Expires] [datetime] NULL,
	[Headers] [varchar](max) NOT NULL,
	[Body] [varbinary](max) NULL,
	[RowVersion] [bigint] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
```


#### Move records to archive table

This script moves the contents of the audit table into `audit_archive` table.

```
DELETE FROM [dbo].[audit]
OUTPUT [deleted].*
INTO [dbo].[audit_archive]
```

This can be run with a scheduled job to clear down the archive regularly.


#### Execute BCP

Once that query completes the records can be archived to disk. In a command prompt use the BCP to create an archive on disk.

`bcp samples.dbo.audit_archive out archive.csv -c -q -T -S .\SQLExpress`


#### Truncate the archive table

You will still have to clear the audit records using the following script.

`TRUNCATE TABLE  [dbo].[audit_archive] ;`
