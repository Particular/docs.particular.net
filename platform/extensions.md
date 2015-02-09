---
title: Extensions and community contributions
summary: A list of all extensions to NServiceBus including all community contributions and external integrations
---

## Transports 

### [AmazonSQS](https://github.com/ahofman/NServiceBus.AmazonSQS) <a href="http://www.nuget.org/packages/NServiceBus.AmazonSQS/"><img src="http://img.shields.io/nuget/v/NServiceBus.AmazonSQS.svg?" alt="NuGet Status"></a>

An [AWS SQS](http://aws.amazon.com/sqs/) transport.

### [Azure Service Bus](/nservicebus/windows-azure-transport.md)  <a href="http://www.nuget.org/packages/NServiceBus.Azure.Transports.WindowsAzureServiceBus/"><img src="http://img.shields.io/nuget/v/NServiceBus.Azure.Transports.WindowsAzureServiceBus.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

An [Azure Service Bus](http://azure.microsoft.com/en-us/services/service-bus/) transport.

### [Azure Storage Queues](/nservicebus/windows-azure-transport.md) <a href="http://www.nuget.org/packages/NServiceBus.Azure.Transports.WindowsAzureStorageQueues/"><img src="http://img.shields.io/nuget/v/NServiceBus.Azure.Transports.WindowsAzureStorageQueues.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

An [Azure Storage Queue](http://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-queues/) transport.

### [MSMQ](/nservicebus/msmqtransportconfig.md) <a href="http://www.nuget.org/packages/NServiceBus/"><img src="http://img.shields.io/nuget/v/NServiceBus.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

The default transport in the NServiceBus core for [MSMQ](https://msdn.microsoft.com/en-us/library/ms711472%28v=vs.85%29.aspx).

### [OracleAQ](https://github.com/rosieks/NServiceBus.OracleAQ) <a href="http://www.nuget.org/packages/NServiceBus.OracleAQ/"><img src="http://img.shields.io/nuget/v/NServiceBus.OracleAQ.svg?" alt="NuGet Status"></a>

An [Oracle Advanced Queuing (Oracle AQ)](http://docs.oracle.com/cd/B10500_01/appdev.920/a96587/qintro.htm) transport.

### [RabbitMQ](/nservicebus/rabbitmq/) <a href="http://www.nuget.org/packages/NServiceBus.RabbitMQ/"><img src="http://img.shields.io/nuget/v/NServiceBus.RabbitMQ.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

An [RabbitMQ](http://www.rabbitmq.com/) transport.

### [SqlServer](/nservicebus/sqlserver/) <a href="http://www.nuget.org/packages/NServiceBus.SqlServer/"><img src="http://img.shields.io/nuget/v/NServiceBus.SqlServer.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

Support for [Microsoft Sql Server](http://www.microsoft.com/en-us/server-cloud/products/sql-server/)

## Serializers

### JSON

There are several implementation of [JSON](http://en.wikipedia.org/wiki/JSON) serializers.

#### [Core Json](/) <a href="http://www.nuget.org/packages/NServiceBus/"><img src="http://img.shields.io/nuget/v/NServiceBus.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

Using an merged copy of [Json.NET](http://www.newtonsoft.com/json) built into the NServiceBus core.

#### [External Json](/) <a href="http://www.nuget.org/packages/NServiceBus.Newtonsoft.Json/"><img src="http://img.shields.io/nuget/v/NServiceBus.Newtonsoft.Json.svg?" alt="NuGet Status"></a>

Using an external copy of [Json.NET](http://www.newtonsoft.com/json).

#### [Jil](https://github.com/SimonCropp/NServiceBus.Jil) <a href="http://www.nuget.org/packages/NServiceBus.Jil/"><img src="http://img.shields.io/nuget/v/NServiceBus.Jil.svg?" alt="NuGet Status"></a>

Using the [Jil](https://github.com/kevin-montrose/Jil) Json serializer.

### [ProtoBuf](https://github.com/SimonCropp/NServiceBus.ProtoBuf) <a href="http://www.nuget.org/packages/NServiceBus.ProtoBuf/"><img src="http://img.shields.io/nuget/v/NServiceBus.ProtoBuf.svg?" alt="NuGet Status"></a>

Using the [ProtoBuf](https://code.google.com/p/protobuf-net/) binary serializer.

### [MessagePack](https://github.com/SimonCropp/NServiceBus.MessagePack) <a href="http://www.nuget.org/packages/NServiceBus.MessagePack/"><img src="http://img.shields.io/nuget/v/NServiceBus.MessagePack.svg?" alt="NuGet Status"></a>

Using the [msgpack-cli](https://github.com/msgpack/msgpack-cli) to serialize via the [MessagePack](http://msgpack.org/) format.

### [SystemXml](https://github.com/fhalim/NServiceBus.Serializers.SystemXml) <a href="http://www.nuget.org/packages/NServiceBus.Serializers.SystemXml/"><img src="http://img.shields.io/nuget/v/NServiceBus.Serializers.SystemXml.svg?" alt="NuGet Status"></a>

Using the .net [System.Xml.Serialization](http://msdn.microsoft.com/en-us/library/system.xml.serialization.aspx) to serialize.

### [Xml](/) <a href="http://www.nuget.org/packages/NServiceBus/"><img src="http://img.shields.io/nuget/v/NServiceBus.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

A custom XML serializer built into the NServiceBus core.

## Persisters

### MongoDB

There are current two projects supporting [MongoDB](http://www.mongodb.org/)

#### [NServiceBus.MongoDB](https://github.com/sbmako/NServiceBus.MongoDB) <a href="http://www.nuget.org/packages/NServiceBus.MongoDB/"><img src="http://img.shields.io/nuget/v/NServiceBus.MongoDB.svg?" alt="NuGet Status"></a>

#### [NServiceBus.Persistence.MongoDb](https://github.com/tekmaven/NServiceBus.Persistence.MongoDb) <a href="http://www.nuget.org/packages/NServiceBus.Persistence.MongoDb/"><img src="http://img.shields.io/nuget/v/NServiceBus.Persistence.MongoDb.svg?" alt="NuGet Status"></a>

### [NHibernate](/nservicebus/relational-persistence-using-nhibernate.md) <a href="http://www.nuget.org/packages/NServiceBus.NHibernate/"><img src="http://img.shields.io/nuget/v/NServiceBus.NHibernate.svg?" alt="NuGet Status"></a> [![A Particular run project](particular-project.png)](http://particular.net/)

Support for [NHibernate](http://nhforge.org/)

### [PostgreSQL](https://github.com/fhalim/NServiceBus.PostgreSQL) <a href="http://www.nuget.org/packages/NServiceBus.PostgreSQL/"><img src="http://img.shields.io/nuget/v/NServiceBus.PostgreSQL.svg?" alt="NuGet Status"></a>

Support for [PostgreSQL](http://www.postgresql.org/)

### [RavenDB](/nservicebus/ravendb/) <a href="http://www.nuget.org/packages/NServiceBus.RavenDB/"><img src="http://img.shields.io/nuget/v/NServiceBus.RavenDB.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

Support for [RavenDB](http://ravendb.net/)

### [DocumentDB](https://github.com/synhershko/NServiceBus.DocumentDB)

A proof-of-concept project that shows the possibility of support for [Azure DocumentDB](http://azure.microsoft.com/en-us/services/documentdb/)

## Logging

### [Log4Net](/nservicebus/logging-in-nservicebus.md#log4net)  <a href="http://www.nuget.org/packages/NServiceBus.Log4Net/"><img src="http://img.shields.io/nuget/v/NServiceBus.Log4Net.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

Support for [Log4Net](http://logging.apache.org/log4net/)

### [NLog](/nservicebus/logging-in-nservicebus.md#nlog) <a href="http://www.nuget.org/packages/NServiceBus.NLog/"><img src="http://img.shields.io/nuget/v/NServiceBus.NLog.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

Support for [NLog](http://nlog-project.org/)

### [CommonLogging](/nservicebus/logging-in-nservicebus.md#commonlogging) <a href="http://www.nuget.org/packages/NServiceBus.CommonLogging/"><img src="http://img.shields.io/nuget/v/NServiceBus.CommonLogging.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

Support for [CommonLogging](https://github.com/net-commons/common-logging)

### [Serilog](https://github.com/SimonCropp/NServiceBus.Serilog) <a href="http://www.nuget.org/packages/NServiceBus.Serilog/"><img src="http://img.shields.io/nuget/v/NServiceBus.Serilog.svg?" alt="NuGet Status"></a>

Support for [Serilog](http://serilog.net/) and [Seq](http://getseq.net/)

## Containers

### [Autofac](/nservicebus/containers.md) <a href="http://www.nuget.org/packages/NServiceBus.Autofac/"><img src="http://img.shields.io/nuget/v/NServiceBus.Autofac.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

Support for [Ninject](http://autofac.org/)

### [CastleWindsor](/nservicebus/containers.md) <a href="http://www.nuget.org/packages/NServiceBus.CastleWindsor/"><img src="http://img.shields.io/nuget/v/NServiceBus.CastleWindsor.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

Support for [CastleWindsor](http://docs.castleproject.org/Windsor.MainPage.ashx)

### [Ninject](/nservicebus/containers.md) <a href="http://www.nuget.org/packages/NServiceBus.Ninject/"><img src="http://img.shields.io/nuget/v/NServiceBus.Ninject.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

Support for [Ninject](http://www.ninject.org/)

### [StructureMap](/nservicebus/containers.md)  <a href="http://www.nuget.org/packages/NServiceBus.StructureMap/"><img src="http://img.shields.io/nuget/v/NServiceBus.StructureMap.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

Support for [StructureMap](http://structuremap.github.io/)

### [Spring](/nservicebus/containers.md) <a href="http://www.nuget.org/packages/NServiceBus.Spring/"><img src="http://img.shields.io/nuget/v/NServiceBus.Spring.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

Support for [Spring](http://www.nuget.org/packages/Spring.Core/)

### [Unity](/nservicebus/containers.md) <a href="http://www.nuget.org/packages/NServiceBus.Unity/"><img src="http://img.shields.io/nuget/v/NServiceBus.Unity.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

Support for [Unity](http://unity.codeplex.com/)

## Other

### [Aggregates.NET](https://github.com/volak/Aggregates.NET) <a href="http://www.nuget.org/packages/Aggregates.NET/"><img src="http://img.shields.io/nuget/v/Aggregates.NET.svg?" alt="NuGet Status"></a>

.NET event sourced domain driven design model via [NEventStore](http://www.appccelerate.com/distributedeventbroker.html).

### Azure Hosting

#### [Azure Host](/nservicebus/hosting-nservicebus-in-windows-azure.md) <a href="http://www.nuget.org/packages/NServiceBus.Hosting.Azure/"><img src="http://img.shields.io/nuget/v/NServiceBus.Hosting.Azure.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

The process used when hosting an endpoint on [Azure](http://azure.microsoft.com/en-us/).

#### [Multi Endpoint Azure Host](/nservicebus/hosting-nservicebus-in-windows-azure.md) <a href="http://www.nuget.org/packages/NServiceBus.Hosting.Azure.HostProcess/"><img src="http://img.shields.io/nuget/v/NServiceBus.Hosting.Azure.HostProcess.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

The process used when sharing an [Azure](http://azure.microsoft.com/en-us/) instance between multiple endpoints.

### [Distributed Event Broker](https://github.com/appccelerate/distributedeventbroker.nservicebus) <a href="http://www.nuget.org/packages/Appccelerate.DistributedEventBroker.NServiceBus/"><img src="http://img.shields.io/nuget/v/Appccelerate.DistributedEventBroker.NServiceBus.svg?" alt="NuGet Status"></a> 

Allows sending events over the [Appccelerate.EventBroker](http://www.appccelerate.com/distributedeventbroker.html) infrastructure.

### [Distributor](/nservicebus/load-balancing-with-the-distributor.md) <a href="http://www.nuget.org/packages/NServiceBus.Distributor.MSMQ/"><img src="http://img.shields.io/nuget/v/NServiceBus.Distributor.MSMQ.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

Distributor for the MSMQ transport.

### [Gateway](/nservicebus/introduction-to-the-gateway.md) <a href="http://www.nuget.org/packages/NServiceBus.Gateway/"><img src="http://img.shields.io/nuget/v/NServiceBus.Gateway.svg?" alt="NuGet Status"></a> <a href="http://particular.net/"><img src="particular-project.png" alt="A Particular run project"></a>

### [Mandrill](https://github.com/feinoujc/NServiceBus.Mandrill) <a href="http://www.nuget.org/packages/Aggregates.NET/"><img src="http://img.shields.io/nuget/v/Aggregates.NET.svg?" alt="NuGet Status"></a>

Allow for sending [Mandrill](https://mandrillapp.com/api/docs/) emails as messages.

### [Mailer](https://github.com/SimonCropp/NServiceBus.Mailer) <a href="http://www.nuget.org/packages/NServiceBus.Mailer/"><img src="http://img.shields.io/nuget/v/NServiceBus.Mailer.svg?" alt="NuGet Status"></a>

Extension to enable sending emails as messages.

### [MessageRouting](https://github.com/jbogard/NServiceBus.MessageRouting) <a href="http://www.nuget.org/packages/NServiceBus.MessageRouting/"><img src="http://img.shields.io/nuget/v/NServiceBus.MessageRouting.svg?" alt="NuGet Status"></a>

An implementation the [Routing Slip](http://www.enterpriseintegrationpatterns.com/RoutingTable.html) pattern that enables you to route a message to one or more destinations

### [HandlerOrdering](https://github.com/SimonCropp/HandlerOrdering) <a href="http://www.nuget.org/packages/HandlerOrdering/"><img src="http://img.shields.io/nuget/v/HandlerOrdering.svg?" alt="NuGet Status"></a>

Allows a more expressive way to order handlers.

### [NES (.NET Event Sourcing)](https://github.com/elliotritchie/NES) <a href="http://www.nuget.org/packages/NES.NServiceBus/"><img src="http://img.shields.io/nuget/v/NES.NServiceBus.svg?" alt="NuGet Status"></a>

NES (.NET Event Sourcing) is a lightweight framework that helps you build domain models when you're doing event sourcing.

### [SignalR](https://github.com/roycornelissen/SignalR.NServiceBus)

Backplane for [SignalR](http://signalr.net/)

### [NServiceBus.SLR](https://github.com/KenBerg75/NServiceBus.SLR) <a href="http://www.nuget.org/packages/NServiceBus.SLR/"><img src="http://img.shields.io/nuget/v/NServiceBus.SLR.svg?" alt="NuGet Status"></a>

A plugin to NServiceBus that allows configuration of Second Level Retries based on the Exception Type.