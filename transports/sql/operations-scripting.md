---
title: Scripting
summary: Example code and scripts to facilitate deployment and operational actions against the SQLServer Transport.
component: SqlServer
reviewed: 2017-06-28
redirects:
 - nservicebus/sqlserver/operations-scripting
 - transports/sqlserver/operations-scripting
---

The following are example code and scripts to facilitate deployment and operations against the SQL Server Transport.


## Native Send


### The native send helper methods

A send involves the following actions:

 * Create and serialize headers.
 * Write a message body directly to SQL Server Transport.


#### In C&#35;

snippet: sqlserver-nativesend

In this example, the value `MyNamespace.MyMessage` represents the .NET type of the message. See the [headers documentation](/nservicebus/messaging/headers.md) for more information on the `EnclosedMessageTypes` header.


#### In PowerShell;

snippet: sqlserver-powershell-nativesend


### Using the native send helper methods

snippet: sqlserver-nativesend-usage


## Create queues

Queue creation can be done for a specific endpoint or queues shared between multiple endpoints.


### The create queue helper methods

snippet: sqlserver-create-queues


### Creating queues for an endpoint

To create all queues for a given endpoint name:

snippet: sqlserver-create-queues-for-endpoint

snippet: sqlserver-create-queues-endpoint-usage


### To create shared queues

snippet: sqlserver-create-queues-shared-usage


## Delete queues


### The delete helper queue methods

snippet: sqlserver-delete-queues


### To delete all queues for a given endpoint

snippet: sqlserver-delete-queues-for-endpoint

snippet: sqlserver-delete-queues-endpoint-usage


### To delete shared queues

snippet: sqlserver-delete-queues-shared-usage


## Return message to source queue


### The retry helper methods

A retry involves the following actions:

 * Read a message from the error queue table.
 * Forward that message to another queue table to be retried.

NOTE: Since the connection information for the endpoint that failed is not contained in the error queue table that information is explicitly passed in.

snippet: sqlserver-return-to-source-queue


### Using the retry helper methods

snippet: sqlserver-return-to-source-queue-usage


### Archiving SqlTransport audit log to long term storage

There are several ways to achieve this including using techniques like [Table Partitioning](https://docs.microsoft.com/en-us/sql/relational-databases/partitions/create-partitioned-tables-and-indexes) and [Snapshot Replication](https://docs.microsoft.com/en-us/sql/relational-databases/replication/snapshot-replication). In this example [BCP utility](https://docs.microsoft.com/en-us/sql/tools/bcp-utility) will be used.


#### Create helper "archive" table

Create an `audit_archive` table with this SQL script.

snippet: audit-archive


#### Move records to archive table

This script moves the contents of the audit table into `audit_archive` table.

snippet: delete-audit

This can be run with a scheduled job to clear the archive regularly.


#### Execute BCP

Once that query completes the records can be archived to disk. In a command prompt use the BCP to create an archive on disk.

```dos
bcp samples.dbo.audit_archive out archive.csv -c -q -T -S .\SQLExpress
```


#### Truncate the archive table

The audit records will still have to clear using the following script.

snippet: truncate-audit