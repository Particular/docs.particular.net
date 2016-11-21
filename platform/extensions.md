---
title: Extensions
summary: A list of all extensions to NServiceBus including all community contributions and external integrations.
reviewed: 2016-04-05
---

This is a curated list of all the extensions to NServiceBus developed by both the community and Particular. If any extension has been abandoned and no longer maintained [raise an issue](https://github.com/Particular/docs.particular.net/issues).

**<img style="margin-bottom: 0.25em" src="particular-project.png"> Particular run project**

**<img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> Community run project**


## Transports


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [AmazonSQS](https://github.com/ahofman/NServiceBus.AmazonSQS)

https://www.nuget.org/packages/NServiceBus.AmazonSQS

Provides support for sending messages over [Amazon SQS](https://aws.amazon.com/sqs/).


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [Azure Service Bus](/nservicebus/azure-service-bus/)

https://www.nuget.org/packages/NServiceBus.Azure.Transports.WindowsAzureServiceBus

Provides support for sending messages over [Azure Service Bus](https://azure.microsoft.com/en-us/services/service-bus/).


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [Azure Storage Queues](/nservicebus/azure-storage-queues/)

https://www.nuget.org/packages/NServiceBus.Azure.Transports.WindowsAzureStorageQueues

Provides support for sending messages over [Azure Storage Queue](https://docs.microsoft.com/en-us/azure/storage/storage-dotnet-how-to-use-queues).


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [MSMQ](/nservicebus/msmq/)

https://www.nuget.org/packages/NServiceBus

Provides support for sending messages over [Microsoft Message Queuing (MSMQ)](https://msdn.microsoft.com/en-us/library/ms711472.aspx). This is the default transport in the NServiceBus core.


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [OracleAQ](https://github.com/rosieks/NServiceBus.OracleAQ)

https://www.nuget.org/packages/NServiceBus.OracleAQ

Provides support for sending messages over [Oracle Advanced Queuing (Oracle AQ)](http://docs.oracle.com/cd/B10500_01/appdev.920/a96587/qintro.htm).


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [RabbitMQ](/nservicebus/rabbitmq/)

https://www.nuget.org/packages/NServiceBus.RabbitMQ

Provides support for sending messages over [RabbitMQ](http://www.rabbitmq.com/) using the [RabbitMQ .NET Client](https://www.nuget.org/packages/RabbitMQ.Client/).


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [SqlServer](/nservicebus/sqlserver/)

https://www.nuget.org/packages/NServiceBus.SqlServer

Provides support for sending messages over [Microsoft Sql Server](https://www.microsoft.com/en-us/sql-server/) using SQL tables as the storage mechanism for messages.


## Serializers


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [Core Json](/)

https://www.nuget.org/packages/NServiceBus

Using an ILMeged copy of [Json.NET](http://www.newtonsoft.com/json) built into the NServiceBus core.


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [Newtonsoft Json](/nservicebus/serialization/newtonsoft.md)

https://www.nuget.org/packages/NServiceBus.Newtonsoft.Json

Using an external copy of [Json.NET](http://www.newtonsoft.com/json) so the full programmatic API of Json.NET can be leveraged.


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [Jil](https://github.com/SimonCropp/NServiceBus.Jil)

https://www.nuget.org/packages/NServiceBus.Jil

The [Jil Project](https://github.com/kevin-montrose/Jil) is a fast JSON serializer built on [Sigil](https://github.com/kevin-montrose/Sigil) with a number of somewhat crazy optimization tricks.


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [Wire](https://github.com/hmemcpy/NServiceBus.Wire)

https://www.nuget.org/packages/NServiceBus.Wire

[Wire](https://github.com/rogeralsing/Wire) is a high performance polymorphic serializer for the .NET framework, built by Roger Johansson of [Akka.NET](https://github.com/akkadotnet/akka.net).


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [ProtoBuf](https://github.com/SimonCropp/NServiceBus.ProtoBuf)

https://www.nuget.org/packages/NServiceBus.ProtoBuf

[ProtoBuf](https://github.com/mgravell/protobuf-net) is [Googles](https://developers.google.com/protocol-buffers/) binary serializer designed to be small, fast and simple.


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [MessagePack](https://github.com/SimonCropp/NServiceBus.MessagePack)

https://www.nuget.org/packages/NServiceBus.MessagePack

[MessagePack](http://msgpack.org/) is a binary serializer designed to be both compact and fast.


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [SystemXml](https://github.com/fhalim/NServiceBus.Serializers.SystemXml)

https://www.nuget.org/packages/NServiceBus.Serializers.SystemXml

Using the .NET [System.Xml.Serialization](https://msdn.microsoft.com/en-us/library/system.xml.serialization.aspx) to serialize messages. It allows better interoperability with non-NServiceBus peers.


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [Xml](/)

https://www.nuget.org/packages/NServiceBus

A custom XML serializer built into the NServiceBus core.


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [ZeroFormatter](https://github.com/SimonCropp/NServiceBus.ZeroFormatter)

https://www.nuget.org/packages/NServiceBus.ZeroFormatter

Using [ZeroFormatter](https://github.com/neuecc/ZeroFormatter/) to serialize messages.


## Persisters


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [NServiceBus.MongoDB](https://github.com/sbmako/NServiceBus.MongoDB)

https://www.nuget.org/packages/NServiceBus.MongoDB


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [NServiceBus.Persistence.MongoDb](https://github.com/tekmaven/NServiceBus.Persistence.MongoDb)

https://www.nuget.org/packages/NServiceBus.Persistence.MongoDb


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [NHibernate](/nservicebus/nhibernate/)

https://www.nuget.org/packages/NServiceBus.NHibernate


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [PostgreSQL](https://github.com/fhalim/NServiceBus.PostgreSQL)

https://www.nuget.org/packages/NServiceBus.PostgreSQL

Leverages the [JSONB](https://www.postgresql.org/docs/devel/static/datatype-json.html) data type for storing data in [PostgreSQL](https://www.postgresql.org/).


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [RavenDB](/nservicebus/ravendb/)

https://www.nuget.org/packages/NServiceBus.RavenDB


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [Azure Storage](/nservicebus/azure-storage-persistence/)

https://www.nuget.org/packages/NServiceBus.Azure


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [Entity Framework](https://github.com/benjaminramey/GoodlyFere.NServiceBus.EntityFramework)

https://www.nuget.org/packages/GoodlyFere.NServiceBus.EntityFramework


## Logging


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [Log4Net](/nservicebus/logging/#log4net)

https://www.nuget.org/packages/NServiceBus.Log4Net


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [NLog](/nservicebus/logging/#nlog)

https://www.nuget.org/packages/NServiceBus.NLog


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [CommonLogging](/nservicebus/logging/#commonlogging)

https://www.nuget.org/packages/NServiceBus.CommonLogging


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [Serilog](https://github.com/SimonCropp/NServiceBus.Serilog)

https://www.nuget.org/packages/NServiceBus.Serilog

Support for logging NServiceBus information to [Serilog](https://serilog.net/) logging library and the [Seq](https://getseq.net/) monitoring system both of which built on the concepts structured logging.


## Containers


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [Autofac](/nservicebus/containers/)

https://www.nuget.org/packages/NServiceBus.Autofac


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [CastleWindsor](/nservicebus/containers/)

https://www.nuget.org/packages/NServiceBus.CastleWindsor


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [Ninject](/nservicebus/containers/)

https://www.nuget.org/packages/NServiceBus.Ninject


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [SimpleInjector](/nservicebus/containers/)

https://www.nuget.org/packages/NServiceBus.SimpleInjector


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [Spring](/nservicebus/containers/)

https://www.nuget.org/packages/NServiceBus.Spring


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [StructureMap](/nservicebus/containers/)

https://www.nuget.org/packages/NServiceBus.StructureMap


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [Unity](/nservicebus/containers/)

https://www.nuget.org/packages/NServiceBus.Unity


## Other


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [Aggregates.NET](https://github.com/volak/Aggregates.NET)

https://www.nuget.org/packages/Aggregates.NET

.NET event sourced domain driven design model via [NEventStore](http://www.appccelerate.com/distributedeventbroker.html).


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [Azure Host](/nservicebus/hosting/cloud-services-host/)

https://www.nuget.org/packages/NServiceBus.Hosting.Azure

The process used when hosting an endpoint on [Azure](https://azure.microsoft.com/en-us/).


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [Multi Endpoint Azure Host](/nservicebus/hosting/cloud-services-host/)

https://www.nuget.org/packages/NServiceBus.Hosting.Azure.HostProcess

The process used when sharing an [Azure](https://azure.microsoft.com/en-us/) instance between multiple endpoints.


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" title="A Community run project"> [Distributed Event Broker](https://github.com/appccelerate/distributedeventbroker.nservicebus)

https://www.nuget.org/packages/Appccelerate.DistributedEventBroker.NServiceBus

Allows sending events over the [Appccelerate.EventBroker](http://www.appccelerate.com/distributedeventbroker.html) infrastructure.


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [Distributor](/nservicebus/msmq/distributor/)

https://www.nuget.org/packages/NServiceBus.Distributor.MSMQ

Distributor for the MSMQ transport.


#### <img style="margin-bottom: 0.25em" src="particular-project.png" title="A Particular run project"> [Gateway](/nservicebus/gateway/)

https://www.nuget.org/packages/NServiceBus.Gateway


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [Mandrill](https://github.com/feinoujc/NServiceBus.Mandrill)

https://www.nuget.org/packages/NServiceBus.Mandrill

[Mandrill](https://mandrill.com) is a email infrastructure service built by [MailChimp](https://mailchimp.com). This extension allow for sending Mandrill emails as messages.


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [Mailer](https://github.com/SimonCropp/NServiceBus.Mailer)

https://www.nuget.org/packages/NServiceBus.Mailer

Extension to enable sending SMTP emails as messages.


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [MessageRouting](https://github.com/jbogard/NServiceBus.MessageRouting)

https://www.nuget.org/packages/NServiceBus.MessageRouting

An implementation the [Routing Slip](http://www.enterpriseintegrationpatterns.com/patterns/messaging/RoutingTable.html) pattern that enables routing a message to one or more destinations


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [HandlerOrdering](https://github.com/SimonCropp/HandlerOrdering)

https://www.nuget.org/packages/NServiceBus.HandlerOrdering

Allows a more expressive way to order handlers.


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [NES (.NET Event Sourcing)](https://github.com/elliotritchie/NES)

https://www.nuget.org/packages/NES.NServiceBus

NES that helps build domain models when doing event sourcing. It attempts to fill in the gaps between NServiceBus and NEventStore.


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [Rabbit Operations](http://rabbitoperations.southsidesoft.com)

Operations support for RabbitMQ applications including support for NServiceBus. Reads messages from error and audit queues and indexes them for search and analysis. Supports replay of error messages.


#### <img style="margin-bottom: 0.25em" src="/content/images/community_v1.svg" /> [SignalR](https://github.com/roycornelissen/SignalR.NServiceBus)

Backplane for [SignalR](http://signalr.net/)