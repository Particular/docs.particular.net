---
title: Samples supporting .NET Core
summary: Samples containing a .NET Core solution
reviewed: 2018-05-04
---

The following samples contain variants for both .NET Core and .NET Framework.

Both solutions are contained within the same zip file, and use the same underlying C# files, but with separate `.sln` and `.csproj` files.

For example, the [Full Duplex sample](/samples/fullduplex/) download will contain:

* **FullDuplex.sln** - Solution for .NET Framework
* **FullDuplex.Core.sln** - Solution for .NET Core
* **Server** - Project directory
    * **Server.csproj** - Project for .NET Framework
    * **Server.Core.csproj** - Project for .NET Core


## Recommended samples

To get started with .NET Core and NServiceBus, the following samples are recommended:

### General samples

* [Full Duplex](/samples/fullduplex/?version=core_7) - sending and receiving with NServiceBus endpoints
* [Publish/Subscribe](/samples/pubsub/?version=core_7) - publishing events to multiple subscribers
* [Simple Saga Usage](/samples/saga/simple/?version=core_7)
* [Using NServiceBus in an ASP.NET Core WebAPI Application](/samples/web/send-from-aspnetcore-webapi/?version=core_7)
* [Hosting in a Windows Service](/samples/hosting/windows-service/?version=core_7)

### Message transport samples

* [Simple RabbitMQ Transport usage](/samples/rabbitmq/simple/?version=rabbit_5)
* [Simple SQL Server Transport Usage](/samples/sqltransport/simple/?version=sqltransport_4)
* [Azure Storage Queues Transport](/samples/azure/storage-queues/?version=asq_8)
* [Simple AmazonSQS Transport usage](/samples/sqs/simple/?version=sqs_4)

### Persistence samples

* [Simple SQL Persistence Usage](/samples/sql-persistence/simple/?version=sqlpersistence_4)
* [SQL Server Transport and SQL Persistence](/samples/sqltransport-sqlpersistence/?version=core_7)
* [Azure Storage Persistence](/samples/azure/storage-persistence/?version=asp_2)

## All .NET Core Samples

This is the comprehensive list of samples that support .NET Core:

include: generated-samples-supporting-netcore