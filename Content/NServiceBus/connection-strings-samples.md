---
title: NServiceBus Connection Strings samples
summary: Provides a list of samples connections strings for different transports supported by NServiceBus.
tags:
- NServiceBus
- Connection strings
- Transports
---

NServiceBus supports the following transports out of the box:

* MSMQ;
* ActiveMQ;
* RabbitMQ;
* SQL Server;
* Microsoft Azure ServiceBus;
* Microsoft Azure Storage Queues;

You can configure NServiuceBus endpoints to use a specific transport by:

* Setting a connection string, named `NServiceBus/Transport`, in the endpoint's configuration file;
* Installing the relevant transport NuGet package;

###MSMQ

* Nuget transport package: not required, MSMQ is supported out of the box;
* Connection string sample: `deadLetter=true;
   journal=true;useTransactionalQueues=true;
   cacheSendConnection=true`;

###ActiveMQ

* Nuget transport package: []();
* Connection string sample: `ServerUrl=activemq:tcp://localhost:61616`;

###RabbitMQ

* Nuget transport package: []();
* Connection string sample: `host=localhost`;

###SQL Server

* Nuget transport package: []();
* Connection string sample: `Data Source=.\SQLEXPRESS;Initial Catalog=nservicebus;
   Integrated Security=True`;

###Microsoft Azure ServiceBus

* Nuget transport package: []();
* Connection string sample: `Endpoint=sb://[namespace].servicebus.windows.net; SharedSecretIssuer=owner;SharedSecretValue=someSecret`;

###Microsoft Azure Storage Queues

* Nuget transport package: []();
* Connection string sample: `DefaultEndpointsProtocol=https;AccountName=myAccount;AccountKey=myKey;`;
