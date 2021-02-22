---
title: SQLServer Transport Scripting
summary: Example code and scripts to facilitate deployment and operational actions against the SQLServer Transport.
component: SqlTransport
reviewed: 2021-02-22
related:
 - nservicebus/operations
redirects:
 - nservicebus/sqlserver/operations-scripting
 - transports/sqlserver/operations-scripting
---

The following are example code and scripts to facilitate deployment and operations against the SQL Server Transport.

## Inspecting messages in the queue

The following script returns messages waiting in a given queue:

snippet: inspect-queue

NOTE: Some columns have been removed for clarity as they are only required for wire-level compatibility with previous versions of SQL Server transport.

The `BodyString` column is a computed value that allows inspecting of the message body when a text-based serializer is used (e.g. Json or XML).

partial: sqlstring-column

## Native Send

### The native send helper methods

A send involves the following actions:

 * Create and serialize headers.
 * Write a message body directly to SQL Server Transport.

#### In C&#35;

snippet: sqlserver-nativesend

In this example, the value `MyNamespace.MyMessage` represents the .NET type of the message. See the [headers documentation](/nservicebus/messaging/headers.md) for more information on the `EnclosedMessageTypes` header.

#### In PowerShell

snippet: sqlserver-powershell-nativesend

### Using the native send helper methods

snippet: sqlserver-nativesend-usage

## Create queues

Queue creation can be done for a specific endpoint or queues shared between multiple endpoints.

### The create queue helper methods

#### In C&#35;

snippet: create-queues

#### In PowerShell

snippet: create-queues-powershell

### Creating queues for an endpoint

To create all queues for a given endpoint name.

#### In C&#35;

snippet: create-queues-for-endpoint

#### In PowerShell

snippet: create-queues-for-endpoint-powershell

### Using the create endpoint queues

#### In C&#35;

snippet: create-queues-endpoint-usage

#### In PowerShell

snippet: create-queues-endpoint-usage-powershell

partial: add-sqlstring-column

### To create shared queues

#### In C&#35;

snippet: create-queues-shared-usage

#### In PowerShell

snippet: create-queues-shared-usage-powershell

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

NOTE: Since the connection information for the endpoint that failed is not contained in the error queue table, that information is explicitly passed in.

snippet: sqlserver-return-to-source-queue

### Using the retry helper methods

snippet: sqlserver-return-to-source-queue-usage

### Archiving SqlTransport audit log to long-term storage

There are several ways to achieve archiving of the audit log, including using techniques like [Table Partitioning](https://docs.microsoft.com/en-us/sql/relational-databases/partitions/create-partitioned-tables-and-indexes) and [Snapshot Replication](https://docs.microsoft.com/en-us/sql/relational-databases/replication/snapshot-replication). In this example, [BCP utility](https://docs.microsoft.com/en-us/sql/tools/bcp-utility) will be used.

#### Create helper "archive" table

Create an `audit_archive` table with this SQL script.

snippet: audit-archive

#### Move records to archive table

This script moves the contents of the audit table into `audit_archive` table.

snippet: delete-audit

This can be run with a scheduled job to clear the archive regularly.

#### Execute BCP

Once that query completes, the records can be archived to disk. In a command prompt, use the BCP to create an archive on disk.

```dos
bcp samples.dbo.audit_archive out archive.csv -c -q -T -S .\SQLExpress
```

#### Truncate the archive table

The audit records will still have to clear using the following script:

snippet: truncate-audit