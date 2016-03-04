---
title: Extensions
summary: A list of all extensions to NServiceBus including all community contributions and external integrations
---

This is a curated list of all the extensions to NServiceBus developed by both the community and Particular. If any extension has been abandoned and no longer maintained [raise an issue](https://github.com/Particular/docs.particular.net/issues).

**<img src="particular-project.png"> Particular run project**

**<img src="community-project.png"> Community run project**


## Transports


#### <img src="community-project.png" title="A Community run project"> [AmazonSQS](https://github.com/ahofman/NServiceBus.AmazonSQS)

<a href="https://www.nuget.org/packages/NServiceBus.AmazonSQS/"><img src="https://buildstats.info/nuget/NServiceBus.AmazonSQS" title="NuGet Status"></a>

Provides support for sending messages over [Amazon SQS](http://aws.amazon.com/sqs/).


#### <img src="particular-project.png" title="A Particular run project"> [Azure Service Bus](/nservicebus/azure/azure-transport.md)

<a href="https://www.nuget.org/packages/NServiceBus.Azure.Transports.WindowsAzureServiceBus/"><img src="https://buildstats.info/nuget/NServiceBus.Azure.Transports.WindowsAzureServiceBus" title="NuGet Status"></a>

Provides support for sending messages over [Azure Service Bus](https://azure.microsoft.com/en-us/services/service-bus/).


#### <img src="particular-project.png" title="A Particular run project"> [Azure Storage Queues](/nservicebus/azure/azure-storage-queues-transport.md)

<a href="https://www.nuget.org/packages/NServiceBus.Azure.Transports.WindowsAzureStorageQueues/"><img src="https://buildstats.info/nuget/NServiceBus.Azure.Transports.WindowsAzureStorageQueues" title="NuGet Status"></a>

Provides support for sending messages over [Azure Storage Queue](https://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-queues/).


#### <img src="particular-project.png" title="A Particular run project"> [MSMQ](/nservicebus/msmq/)

<a href="https://www.nuget.org/packages/NServiceBus/"><img src="https://buildstats.info/nuget/NServiceBus" title="NuGet Status"></a>

Provides support for sending messages over [Microsoft Message Queuing (MSMQ)](https://msdn.microsoft.com/en-us/library/ms711472%28v=vs.85%29.aspx). This is the default transport in the NServiceBus core.


#### <img src="community-project.png" title="A Community run project"> [OracleAQ](https://github.com/rosieks/NServiceBus.OracleAQ)

<a href="https://www.nuget.org/packages/NServiceBus.OracleAQ/"><img src="https://buildstats.info/nuget/NServiceBus.OracleAQ" title="NuGet Status"></a>

Provides support for sending messages over [Oracle Advanced Queuing (Oracle AQ)](http://docs.oracle.com/cd/B10500_01/appdev.920/a96587/qintro.htm).


####  <img src="particular-project.png" title="A Particular run project"> [RabbitMQ](/nservicebus/rabbitmq/)

<a href="https://www.nuget.org/packages/NServiceBus.RabbitMQ/"><img src="https://buildstats.info/nuget/NServiceBus.RabbitMQ" title="NuGet Status"></a>

Provides support for sending messages over [RabbitMQ](http://www.rabbitmq.com/) using the [RabbitMQ .NET Client](https://www.nuget.org/packages/RabbitMQ.Client/).


#### <img src="particular-project.png" title="A Particular run project"> [SqlServer](/nservicebus/sqlserver/)

<a href="https://www.nuget.org/packages/NServiceBus.SqlServer/"><img src="https://buildstats.info/nuget/NServiceBus.SqlServer" title="NuGet Status"></a>

Provides support for sending messages over  [Microsoft Sql Server](http://www.microsoft.com/en-us/server-cloud/products/sql-server/) using SQL tables as the storage mechanism for messages.


## Serializers


#### <img src="particular-project.png" title="A Particular run project"> [Core Json](/)

<a href="https://www.nuget.org/packages/NServiceBus/"><img src="https://buildstats.info/nuget/NServiceBus" title="NuGet Status"></a>

Using an ILMeged copy of [Json.NET](http://www.newtonsoft.com/json) built into the NServiceBus core.


#### <img src="particular-project.png" title="A Particular run project"> [Newtonsot Json](/nservicebus/serialization/newtonsoft.md)

<a href="https://www.nuget.org/packages/NServiceBus.Newtonsoft.Json/"><img src="https://buildstats.info/nuget/NServiceBus.Newtonsoft.Json" title="NuGet Status"></a>

Using an external copy of [Json.NET](http://www.newtonsoft.com/json) so the full programmatic API of Json.NET can be leveraged.


#### <img src="community-project.png" title="A Community run project"> [Jil](https://github.com/SimonCropp/NServiceBus.Jil)

<a href="https://www.nuget.org/packages/NServiceBus.Jil/"><img src="https://buildstats.info/nuget/NServiceBus.Jil" title="NuGet Status"></a>

The [Jil Project](https://github.com/kevin-montrose/Jil) is a fast JSON serializer built on [Sigil](https://github.com/kevin-montrose/Sigil) with a number of somewhat crazy optimization tricks.

#### <img src="community-project.png" title="A Community run project"> [Wire](https://github.com/hmemcpy/NServiceBus.Wire)

<a href="https://www.nuget.org/packages/NServiceBus.Wire/"><img src="https://buildstats.info/nuget/NServiceBus.Wire" title="NuGet Status"></a>

[Wire](https://github.com/rogeralsing/Wire) is a high performance polymorphic serializer for the .NET framework, built by Roger Johansson of [Akka.NET](https://github.com/akkadotnet/akka.net).


#### <img src="community-project.png" title="A Community run project"> [ProtoBuf](https://github.com/SimonCropp/NServiceBus.ProtoBuf)

<a href="https://www.nuget.org/packages/NServiceBus.ProtoBuf/"><img src="https://buildstats.info/nuget/NServiceBus.ProtoBuf" title="NuGet Status"></a>

[ProtoBuf](https://github.com/mgravell/protobuf-net) is [Googles](https://developers.google.com/protocol-buffers/) binary serializer designed to be small, fast and simple.


#### <img src="community-project.png" title="A Community run project"> [MessagePack](https://github.com/SimonCropp/NServiceBus.MessagePack)

<a href="https://www.nuget.org/packages/NServiceBus.MessagePack/"><img src="https://buildstats.info/nuget/NServiceBus.MessagePack" title="NuGet Status"></a>

[MessagePack](http://msgpack.org/) is a binary serializer designed to be both compact and fast.


#### <img src="community-project.png" title="A Community run project"> [SystemXml](https://github.com/fhalim/NServiceBus.Serializers.SystemXml)

<a href="https://www.nuget.org/packages/NServiceBus.Serializers.SystemXml/"><img src="https://buildstats.info/nuget/NServiceBus.Serializers.SystemXml" title="NuGet Status"></a>

Using the .NET [System.Xml.Serialization](https://msdn.microsoft.com/en-us/library/system.xml.serialization.aspx) to serialize messages. It allows better interoperability with non-NServiceBus peers.


#### <img src="particular-project.png" title="A Particular run project"> [Xml](/)

<a href="https://www.nuget.org/packages/NServiceBus/"><img src="https://buildstats.info/nuget/NServiceBus" title="NuGet Status"></a>

A custom XML serializer built into the NServiceBus core.


## Persisters


#### <img src="community-project.png" title="A Community run project"> [NServiceBus.MongoDB](https://github.com/sbmako/NServiceBus.MongoDB)

<a href="https://www.nuget.org/packages/NServiceBus.MongoDB/"><img src="https://buildstats.info/nuget/NServiceBus.MongoDB" title="NuGet Status"></a>


#### <img src="community-project.png" title="A Community run project"> [NServiceBus.Persistence.MongoDb](https://github.com/tekmaven/NServiceBus.Persistence.MongoDb)

<a href="https://www.nuget.org/packages/NServiceBus.Persistence.MongoDb/"><img src="https://buildstats.info/nuget/NServiceBus.Persistence.MongoDb" title="NuGet Status"></a>


#### <img src="particular-project.png" title="A Particular run project"> [NHibernate](/nservicebus/nhibernate/)

<a href="https://www.nuget.org/packages/NServiceBus.NHibernate/"><img src="https://buildstats.info/nuget/NServiceBus.NHibernate" title="NuGet Status"></a>


#### <img src="community-project.png" title="A Community run project"> [PostgreSQL](https://github.com/fhalim/NServiceBus.PostgreSQL)

<a href="https://www.nuget.org/packages/NServiceBus.PostgreSQL/"><img src="https://buildstats.info/nuget/NServiceBus.PostgreSQL" title="NuGet Status"></a>

Leverages the [JSONB](http://www.postgresql.org/docs/devel/static/datatype-json.html) data type for storing data in [PostgreSQL](http://www.postgresql.org/).


#### <img src="particular-project.png" title="A Particular run project"> [RavenDB](/nservicebus/ravendb/)

<a href="https://www.nuget.org/packages/NServiceBus.RavenDB/"><img src="https://buildstats.info/nuget/NServiceBus.RavenDB" title="NuGet Status"></a>


#### <img src="particular-project.png" title="A Particular run project"> [Azure Storage](/nservicebus/azure/azure-storage-persistence.md)

<a href="https://www.nuget.org/packages/NServiceBus.Azure/"><img src="https://buildstats.info/nuget/NServiceBus.Azure" title="NuGet Status"></a>


## Logging


#### <img src="particular-project.png" title="A Particular run project"> [Log4Net](/nservicebus/logging/#log4net)

<a href="https://www.nuget.org/packages/NServiceBus.Log4Net/"><img src="https://buildstats.info/nuget/NServiceBus.Log4Net" title="NuGet Status"></a>


#### <img src="particular-project.png" title="A Particular run project"> [NLog](/nservicebus/logging/#nlog)

<a href="https://www.nuget.org/packages/NServiceBus.NLog/"><img src="https://buildstats.info/nuget/NServiceBus.NLog" title="NuGet Status"></a>


#### <img src="particular-project.png" title="A Particular run project"> [CommonLogging](/nservicebus/logging/#commonlogging)

<a href="https://www.nuget.org/packages/NServiceBus.CommonLogging/"><img src="https://buildstats.info/nuget/NServiceBus.CommonLogging" title="NuGet Status"></a>


#### <img src="community-project.png" title="A Community run project"> [Serilog](https://github.com/SimonCropp/NServiceBus.Serilog)

<a href="https://www.nuget.org/packages/NServiceBus.Serilog/"><img src="https://buildstats.info/nuget/NServiceBus.Serilog" title="NuGet Status"></a>

Support for logging NServiceBus information to [Serilog](http://serilog.net/) logging library and the [Seq](http://serilog.net/) monitoring system both of which built on the concepts structured logging.


## Containers


#### <img src="particular-project.png" title="A Particular run project"> [Autofac](/nservicebus/containers/)

<a href="https://www.nuget.org/packages/NServiceBus.Autofac/"><img src="https://buildstats.info/nuget/NServiceBus.Autofac" title="NuGet Status"></a>


#### <img src="particular-project.png" title="A Particular run project"> [CastleWindsor](/nservicebus/containers/)

<a href="https://www.nuget.org/packages/NServiceBus.CastleWindsor/"><img src="https://buildstats.info/nuget/NServiceBus.CastleWindsor" title="NuGet Status"></a>


#### <img src="particular-project.png" title="A Particular run project"> [Ninject](/nservicebus/containers/)

<a href="https://www.nuget.org/packages/NServiceBus.Ninject/"><img src="https://buildstats.info/nuget/NServiceBus.Ninject" title="NuGet Status"></a>


#### <img src="particular-project.png" title="A Particular run project"> [StructureMap](/nservicebus/containers/)

<a href="https://www.nuget.org/packages/NServiceBus.StructureMap/"><img src="https://buildstats.info/nuget/NServiceBus.StructureMap" title="NuGet Status"></a>


#### <img src="particular-project.png" title="A Particular run project"> [Spring](/nservicebus/containers/)

<a href="https://www.nuget.org/packages/NServiceBus.Spring/"><img src="https://buildstats.info/nuget/NServiceBus.Spring" title="NuGet Status"></a>


#### <img src="particular-project.png" title="A Particular run project"> [Unity](/nservicebus/containers/)

<a href="https://www.nuget.org/packages/NServiceBus.Unity/"><img src="https://buildstats.info/nuget/NServiceBus.Unity" title="NuGet Status"></a>


## Other


#### <img src="community-project.png" title="A Community run project"> [Aggregates.NET](https://github.com/volak/Aggregates.NET)

<a href="https://www.nuget.org/packages/Aggregates.NET/"><img src="https://buildstats.info/nuget/Aggregates.NET" title="NuGet Status"></a>

.NET event sourced domain driven design model via [NEventStore](http://www.appccelerate.com/distributedeventbroker.html).


#### <img src="particular-project.png" title="A Particular run project"> [Azure Host](/nservicebus/azure/hosting.md)

<a href="https://www.nuget.org/packages/NServiceBus.Hosting.Azure/"><img src="https://buildstats.info/nuget/NServiceBus.Hosting.Azure" title="NuGet Status"></a>

The process used when hosting an endpoint on [Azure](https://azure.microsoft.com/en-us/).


#### <img src="particular-project.png" title="A Particular run project"> [Multi Endpoint Azure Host](/nservicebus/azure/hosting.md)

<a href="https://www.nuget.org/packages/NServiceBus.Hosting.Azure.HostProcess/"><img src="https://buildstats.info/nuget/NServiceBus.Hosting.Azure.HostProcess" title="NuGet Status"></a>

The process used when sharing an [Azure](https://azure.microsoft.com/en-us/) instance between multiple endpoints.


#### <img src="community-project.png" title="A Community run project"> [Distributed Event Broker](https://github.com/appccelerate/distributedeventbroker.nservicebus)

<a href="https://www.nuget.org/packages/Appccelerate.DistributedEventBroker.NServiceBus/"><img src="https://buildstats.info/nuget/Appccelerate.DistributedEventBroker.NServiceBus" title="NuGet Status"></a>

Allows sending events over the [Appccelerate.EventBroker](http://www.appccelerate.com/distributedeventbroker.html) infrastructure.


#### <img src="particular-project.png" title="A Particular run project"> [Distributor](/nservicebus/scalability-and-ha/distributor/)

<a href="https://www.nuget.org/packages/NServiceBus.Distributor.MSMQ/"><img src="https://buildstats.info/nuget/NServiceBus.Distributor.MSMQ" title="NuGet Status"></a>

Distributor for the MSMQ transport.


#### <img src="particular-project.png" title="A Particular run project">  [Gateway](/nservicebus/gateway/)

<a href="https://www.nuget.org/packages/NServiceBus.Gateway/"><img src="https://buildstats.info/nuget/NServiceBus.Gateway" title="NuGet Status"></a>


#### <img src="community-project.png" title="A Community run project"> [Mandrill](https://github.com/feinoujc/NServiceBus.Mandrill)

<a href="https://www.nuget.org/packages/NServiceBus.Mandrill/"><img src="https://buildstats.info/nuget/NServiceBus.Mandrill" title="NuGet Status"></a>

[Mandrill](http://mandrill.com/) is a email infrastructure service built by [MailChimp](http://mailchimp.com/). This extension allow for sending Mandrill emails as messages.


#### <img src="community-project.png" title="A Community run project"> [Mailer](https://github.com/SimonCropp/NServiceBus.Mailer)

<a href="https://www.nuget.org/packages/NServiceBus.Mailer/"><img src="https://buildstats.info/nuget/NServiceBus.Mailer" title="NuGet Status"></a>

Extension to enable sending SMTP emails as messages.


#### <img src="community-project.png" title="A Community run project"> [MessageRouting](https://github.com/jbogard/NServiceBus.MessageRouting)

<a href="https://www.nuget.org/packages/NServiceBus.MessageRouting/"><img src="https://buildstats.info/nuget/NServiceBus.MessageRouting" title="NuGet Status"></a>

An implementation the [Routing Slip](http://www.enterpriseintegrationpatterns.com/patterns/messaging/RoutingTable.html) pattern that enables you to route a message to one or more destinations


#### <img src="community-project.png" title="A Community run project"> [HandlerOrdering](https://github.com/SimonCropp/HandlerOrdering)

<a href="https://www.nuget.org/packages/HandlerOrdering/"><img src="https://buildstats.info/nuget/HandlerOrdering" title="NuGet Status"></a>

Allows a more expressive way to order handlers.


#### <img src="community-project.png" title="A Community run project"> [NES (.NET Event Sourcing)](https://github.com/elliotritchie/NES)

<a href="https://www.nuget.org/packages/NES.NServiceBus/"><img src="https://buildstats.info/nuget/NES.NServiceBus" title="NuGet Status"></a>

NES that helps you build domain models when you're doing event sourcing. It attempts to fill in the gaps between NServiceBus and NEventStore.


#### <img src="community-project.png" title="A Community run project"> [Rabbit Operations](http://RabbitOperations.southsidesoft.com)

Operations support for RabbitMQ applications including support for NServiceBus. Reads messages from error and audit queues and indexes them for search and analysis. Supports replay of error messages.


#### <img src="community-project.png" title="A Community run project"> [SignalR](https://github.com/roycornelissen/SignalR.NServiceBus)

Backplane for [SignalR](http://signalr.net/)


#### <img src="community-project.png" title="A Community run project"> [NServiceBus.SLR](https://github.com/KenBerg75/NServiceBus.SLR)

<a href="https://www.nuget.org/packages/NServiceBus.SLR/"><img src="https://buildstats.info/nuget/NServiceBus.SLR" title="NuGet Status"></a>

A plugin to NServiceBus that allows configuration of Second Level Retries based on the Exception Type.
