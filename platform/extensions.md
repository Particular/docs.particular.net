---
title: Extensions and community contributions
summary: A list of all extensions to NServiceBus including all community contributions and external integrations
---

## Transports 

### [AmazonSQS](https://github.com/ahofman/NServiceBus.AmazonSQS) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.AmazonSQS.svg?)](http://www.nuget.org/packages/NServiceBus.AmazonSQS/)

An [AWS SQS](http://aws.amazon.com/sqs/) transport.

### [Azure Service Bus](/nservicebus/windows-azure-transport.md) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Azure.Transports.WindowsAzureServiceBus.svg?)](http://www.nuget.org/packages/NServiceBus.Azure.Transports.WindowsAzureServiceBus/) [![A Particular run project](particular-project.png)](http://particular.net/)  

An [Azure Service Bus](http://azure.microsoft.com/en-us/services/service-bus/) transport.

### [Azure Storage Queues](/nservicebus/windows-azure-transport.md) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Azure.Transports.WindowsAzureStorageQueues.svg?)](http://www.nuget.org/packages/NServiceBus.Azure.Transports.WindowsAzureStorageQueues/) [![A Particular run project](particular-project.png)](http://particular.net/)

An [Azure Storage Queue](http://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-queues/) transport.

### [MSMQ](/nservicebus/msmqtransportconfig.md) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.svg?)](http://www.nuget.org/packages/NServiceBus/) [![A Particular run project](particular-project.png)](http://particular.net/)

The default transport in the NServiceBus core for [MSMQ](https://msdn.microsoft.com/en-us/library/ms711472%28v=vs.85%29.aspx).

### [OracleAQ](https://github.com/rosieks/NServiceBus.OracleAQ) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.OracleAQ.svg?)](http://www.nuget.org/packages/NServiceBus.OracleAQ/)

An [Oracle Advanced Queuing (Oracle AQ)](http://docs.oracle.com/cd/B10500_01/appdev.920/a96587/qintro.htm) transport.

### [RabbitMQ](/nservicebus/rabbitmq/) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.RabbitMQ .svg?)](http://www.nuget.org/packages/NServiceBus.RabbitMQ/) [![A Particular run project](particular-project.png)](http://particular.net/)

An [RabbitMQ](http://www.rabbitmq.com/) transport.

### [SqlServer](/nservicebus/sqlserver/) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.SqlServer.svg?)](http://www.nuget.org/packages/NServiceBus.SqlServer/) [![A Particular run project](particular-project.png)](http://particular.net/)

Support for [Microsoft Sql Server](http://www.microsoft.com/en-us/server-cloud/products/sql-server/)

## Serializers

### JSON

There are several implementation of [JSON](http://en.wikipedia.org/wiki/JSON) serializers.

#### [Core Json](/) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.svg?)](http://www.nuget.org/packages/NServiceBus/) [![A Particular run project](particular-project.png)](http://particular.net/)

Using an merged copy of [Json.NET](http://www.newtonsoft.com/json) built into the NServiceBus core.

#### [External Json](/) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Newtonsoft.Json.svg?)](http://www.nuget.org/packages/NServiceBus.Newtonsoft.Json/)

Using an external copy of [Json.NET](http://www.newtonsoft.com/json).

#### [Jil](https://github.com/SimonCropp/NServiceBus.Jil) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Jil.svg?)](http://www.nuget.org/packages/NServiceBus.Jil/)

Using the [Jil](https://github.com/kevin-montrose/Jil) Json serializer.

### [ProtoBuf](https://github.com/SimonCropp/NServiceBus.ProtoBuf) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.ProtoBuf.svg?)](http://www.nuget.org/packages/NServiceBus.ProtoBuf/)

Using the [ProtoBuf](https://code.google.com/p/protobuf-net/) binary serializer.

### [MessagePack](https://github.com/SimonCropp/NServiceBus.MessagePack) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.MessagePack.svg?)](http://www.nuget.org/packages/NServiceBus.MessagePack/)

Using the [msgpack-cli](https://github.com/msgpack/msgpack-cli) to serialize via the [MessagePack](http://msgpack.org/) format.

### [SystemXml](https://github.com/fhalim/NServiceBus.Serializers.SystemXml) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Serializers.SystemXml.svg?)](http://www.nuget.org/packages/NServiceBus.Serializers.SystemXml/)

Using the .net [System.Xml.Serialization](http://msdn.microsoft.com/en-us/library/system.xml.serialization.aspx) to serialize.

### [Xml](/) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.svg?)](http://www.nuget.org/packages/NServiceBus/) [![A Particular run project](particular-project.png)](http://particular.net/)

A custom XML serializer built into the NServiceBus core.

## Persisters

### MongoDB

There are current two projects supporting [MongoDB](http://www.mongodb.org/)

#### [NServiceBus.MongoDB](https://github.com/sbmako/NServiceBus.MongoDB) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.MongoDB.svg?)](http://www.nuget.org/packages/NServiceBus.MongoDB/)

#### [NServiceBus.Persistence.MongoDb](https://github.com/tekmaven/NServiceBus.Persistence.MongoDb) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Persistence.MongoDb.svg?)](http://www.nuget.org/packages/NServiceBus.Persistence.MongoDb/)

### [NHibernate](/nservicebus/relational-persistence-using-nhibernate.md) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.NHibernate.svg?)](http://www.nuget.org/packages/NServiceBus.NHibernate/) [![A Particular run project](particular-project.png)](http://particular.net/)

Support for [NHibernate](http://nhforge.org/)

### [PostgreSQL](https://github.com/fhalim/NServiceBus.PostgreSQL) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.PostgreSQL.svg?)](http://www.nuget.org/packages/NServiceBus.PostgreSQL/)

Support for [PostgreSQL](http://www.postgresql.org/)

### [RavenDB](/nservicebus/ravendb/) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.RavenDB.svg?)](http://www.nuget.org/packages/NServiceBus.RavenDB/) [![A Particular run project](particular-project.png)](http://particular.net/)

Support for [RavenDB](http://ravendb.net/)

### [DocumentDB](https://github.com/synhershko/NServiceBus.DocumentDB)

A proof-of-concept project that shows the possibility of support for [Azure DocumentDB](http://azure.microsoft.com/en-us/services/documentdb/)

## Logging

### [Log4Net](/nservicebus/logging-in-nservicebus.md#log4net) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Log4Net.svg?)](http://www.nuget.org/packages/NServiceBus.Log4Net/) [![A Particular run project](particular-project.png)](http://particular.net/)

Support for [Log4Net](http://logging.apache.org/log4net/)

### [NLog](/nservicebus/logging-in-nservicebus.md#nlog) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.NLog.svg?)](http://www.nuget.org/packages/NServiceBus.NLog/) [![A Particular run project](particular-project.png)](http://particular.net/)

Support for [NLog](http://nlog-project.org/)

### [CommonLogging](/nservicebus/logging-in-nservicebus.md#commonlogging) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.CommonLogging.svg?)](http://www.nuget.org/packages/NServiceBus.CommonLogging/) [![A Particular run project](particular-project.png)](http://particular.net/)

Support for [CommonLogging](https://github.com/net-commons/common-logging)

### [Serilog](https://github.com/SimonCropp/NServiceBus.Serilog) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Serilog.svg?)](http://www.nuget.org/packages/NServiceBus.Serilog/)

Support for [Serilog](http://serilog.net/) and [Seq](http://getseq.net/)

## Containers

### [Autofac](/nservicebus/containers.md) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Autofac.svg?)](http://www.nuget.org/packages/NServiceBus.Autofac/) [![A Particular run project](particular-project.png)](http://particular.net/)

Support for [Ninject](http://autofac.org/)

### [CastleWindsor](/nservicebus/containers.md) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.CastleWindsor.svg?)](http://www.nuget.org/packages/NServiceBus.CastleWindsor/) [![A Particular run project](particular-project.png)](http://particular.net/)

Support for [CastleWindsor](http://docs.castleproject.org/Windsor.MainPage.ashx)

### [Ninject](/nservicebus/containers.md) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Ninject.svg?)](http://www.nuget.org/packages/NServiceBus.Ninject/) [![A Particular run project](particular-project.png)](http://particular.net/)

Support for [Ninject](http://www.ninject.org/)

### [StructureMap](/nservicebus/containers.md) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.StructureMap.svg?)](http://www.nuget.org/packages/NServiceBus.StructureMap/) [![A Particular run project](particular-project.png)](http://particular.net/)

Support for [StructureMap](http://structuremap.github.io/)

### [Spring](/nservicebus/containers.md) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Spring.svg?)](http://www.nuget.org/packages/NServiceBus.Spring/) [![A Particular run project](particular-project.png)](http://particular.net/)

Support for [Spring](http://www.nuget.org/packages/Spring.Core/)

### [Unity](/nservicebus/containers.md) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Unity .svg?)](http://www.nuget.org/packages/NServiceBus.Unity/) [![A Particular run project](particular-project.png)](http://particular.net/)

Support for [Unity](http://unity.codeplex.com/)

## Other

### [Aggregates.NET](https://github.com/volak/Aggregates.NET) [![NuGet Status](http://img.shields.io/nuget/v/Aggregates.NET.svg?)](http://www.nuget.org/packages/Aggregates.NET/)

.NET event sourced domain driven design model via [NEventStore](http://www.appccelerate.com/distributedeventbroker.html).

### Azure Hosting

#### [Azure Host](/nservicebus/hosting-nservicebus-in-windows-azure.md) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Hosting.Azure.svg?)](http://www.nuget.org/packages/NServiceBus.Hosting.Azure/) [![A Particular run project](particular-project.png)](http://particular.net/)

The process used when hosting an endpoint on [Azure](http://azure.microsoft.com/en-us/).

#### [Multi Endpoint Azure Host](/nservicebus/hosting-nservicebus-in-windows-azure.md) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Hosting.Azure.HostProcess.svg?)](http://www.nuget.org/packages/NServiceBus.Hosting.Azure.HostProcess/) [![A Particular run project](particular-project.png)](http://particular.net/)

The process used when sharing an [Azure](http://azure.microsoft.com/en-us/) instance between multiple endpoints.

### [Distributed Event Broker](https://github.com/appccelerate/distributedeventbroker.nservicebus) [![NuGet Status](http://img.shields.io/nuget/v/Appccelerate.DistributedEventBroker.NServiceBus.svg?)](http://www.nuget.org/packages/Appccelerate.DistributedEventBroker.NServiceBus/)

Allows sending events over the [Appccelerate.EventBroker](http://www.appccelerate.com/distributedeventbroker.html) infrastructure.

### [Distributor](/nservicebus/load-balancing-with-the-distributor.md) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Distributor.MSMQ.svg?)](http://www.nuget.org/packages/NServiceBus.Distributor.MSMQ/) [![A Particular run project](particular-project.png)](http://particular.net/)

Distributor for the MSMQ transport.

### [Gateway](/nservicebus/introduction-to-the-gateway.md) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Gateway.svg?)](http://www.nuget.org/packages/NServiceBus.Gateway/) [![A Particular run project](particular-project.png)](http://particular.net/)

### [Mandrill](https://github.com/feinoujc/NServiceBus.Mandrill) [![NuGet Status](http://img.shields.io/nuget/v/Aggregates.NET.svg?)](http://www.nuget.org/packages/Aggregates.NET/)

Allow for sending [Mandrill](https://mandrillapp.com/api/docs/) emails as messages.

### [Mailer](https://github.com/SimonCropp/NServiceBus.Mailer) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Mailer.svg?)](http://www.nuget.org/packages/NServiceBus.Mailer/)

Extension to enable sending emails as messages.

### [MessageRouting](https://github.com/jbogard/NServiceBus.MessageRouting) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.MessageRouting.svg?)](http://www.nuget.org/packages/NServiceBus.MessageRouting/)

An implementation the [Routing Slip](http://www.enterpriseintegrationpatterns.com/RoutingTable.html) pattern that enables you to route a message to one or more destinations

### [HandlerOrdering](https://github.com/SimonCropp/HandlerOrdering) [![NuGet Status](http://img.shields.io/nuget/v/HandlerOrdering.svg?)](http://www.nuget.org/packages/HandlerOrdering/)

Allows a more expressive way to order handlers.

### [NES (.NET Event Sourcing)](https://github.com/elliotritchie/NES) [![NuGet Status](http://img.shields.io/nuget/v/NES.NServiceBus.svg?)](http://www.nuget.org/packages/NES.NServiceBus/)

NES (.NET Event Sourcing) is a lightweight framework that helps you build domain models when you're doing event sourcing.

### [SignalR](https://github.com/roycornelissen/SignalR.NServiceBus)

Backplane for [SignalR](http://signalr.net/)

### [NServiceBus.SLR](https://github.com/KenBerg75/NServiceBus.SLR) [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.SLR.svg?)](http://www.nuget.org/packages/NServiceBus.SLR/)

A plugin to NServiceBus that allows configuration of Second Level Retries based on the Exception Type.