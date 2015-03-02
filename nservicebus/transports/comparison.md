---
title: NServiceBus transports comparison
summary: Comparison of supported NServiceBus transports.
tags:
- Transports
---

NServiceBus has the concept of transport, a transport is the underlying infrastructure required to deliver a message from one endpoint to another. Both endpoints need to be configured to use the same transport.

NServiceBus supports the following transports:

* [MSMQ](#msmq);
* [SQL Server](#sql-server-transport);
* [RabbitMQ](#rabbitmq);
* [Microsoft Azure Service Bus](#azure-service-bus);
* [Microsoft Azure Storage Queues](#azure-storage-queues);

Choosing the correct transport can be challenging and the choice can depend on several factors, the following table list a set of transport attributes and details the support each transport provides:

 |MSMQ|SQL Server|RabbitMQ|Azure ServiceBus|Azure Storage Queues
|---              |---         |---               |---              |---                           |---
|[Architecture](#architecture)|Store & Forward|Store & Forward|Broker|Broker|Broker
|[Local Transactions](#local-transactions)|&#10004;|&#10004;|&#10004;/&#10006;|&#10004;|&#10006;
|[MSDTC](#msdtc)|&#10004;|&#10004;|&#10006;|&#10006;|&#10006;
|[Ordering](#ordering)|Fi-Fo|Fi-Fo|&#10006;|Fi-Fo\*|&#10006;
|[De-Duplication](#de-duplication)|&#10006;|&#10006;|&#10006;|&#10004;\*|&#10006;
|[Durable](#durable)|&#10004;|&#10004;|&#10004;\*|&#10004;|&#10006;
|[Delivery guarantee](#delivery-guarantee)|Once|Once|At-Least-Once|At-Least-Once / At-Most-Once\*|At-Least-Once
|[Dead-lettering](#dead-lettering)|&#10004;|&#10006;|&#10004;|&#10004;|&#10006;
|[Poison messages](#poison-messages)|&#10004;|&#10006;|&#10006;|&#10004;|&#10004;
|[Max queue size](#max-queue-size)|All Queues on a single machine 4Gb (by default)|?|User defined|1Gb to 80Gb|200TB
|[Max message size](#max-message-size)|4Mb|?|Unlimited|256Kb|64Kb
|[Max message TTL](#max-message-ttl)|Unlimited|?|Unlimited|Unlimited|7 days
|[Max number of queues](#max-number-of-queues)|HW Dependent|?|Unlimited|10,000\*|Unlimited
|[Max number of concurrent clients](#max-number-of-concurrent-clients)|Unlimited|?|Configurable|Unlimited|Unlimited
|[Max throughput](#max-throughput)| | | |Up to 2,000 messages per second|Up to 2,000 messages per second
|[Average latency](#average-latency)| | | |10 ms|20-25 ms
|[Throttling](#throttling)|&#10006;|?|?|&#10004;|&#10004;
|[Throttling behavior](#throttling)| \--| | |Reject HTTP.503|Reject exception/HTTP.503
|[Authentication](#security)|&#10006;|?|Plugin based|Symmetric key|Symmetric key
|[Access control model](#security)|ACL|?|Host & Resource based|Delegated access via SAS tokens|RBAC via ACS
|[HA support](#ha-support)|&#10006;|User|&#10004;\*|&#10004;|&#10004;
|[3rd party SDK](#3rd-party-sdk)|&#10006;|&#10006;|RabbitMQ Client|Azure SDK|Azure SDK
|[Express messages](#express-messages)|&#10004;|&#10006;|&#10006;|&#10006;|&#10006;
|[Competing consumers](#competing-consumers)|&#10006;|&#10004;|&#10004;|&#10004;|&#10004;
|[Topics support](#topics-support)|&#10006;|&#10006;|&#10004;|&#10004;|&#10006;

(each of the followings should be shown in a fancy box as glossary of the above matrix attributes)

##### Architecture

A store-and-forward queueing system is inherently distributed, each machine having its own instance of it. Messages that are being sent are first stored in an outgoing queue of the local machine. At this point the enqueue operation finishes and the sender can move on. The queueing system will do its best to forward the stored message to its destination using whatever routing rules it implements.

A broker is a centralized solution (although can be clustered to achieve either higher performance or for high availability). Clients connect to the broker over a network and use its APIs to enqueue and dequeue messages. Brokers frequently implement higher level messaging patterns such as publish/subscriber or multicast.

##### Local Transactions

Most queueing systems offer transactional semantics of dequeue and enqueue operations, such as
 * allow to enqueue more than one message as an atomic operation (either both messages are put into the queue or none is)
 * ensure that at only one of multiple competing queue consumers can dequeue a given message

##### MSDTC

MS Distributed Transaction Coordinator (MS DTC) is a Windows service that allows execution of distributed transactions (transactions that involved more than one resource) using [Two-phase commit](http://en.wikipedia.org/wiki/Two-phase_commit_protocol) protocol. It can be used to form an extended transaction between a database server and a queueing system in order to
 * automatically roll back dequeue transaction if database fails to commit the changes
 * automatically roll back the changes done in the database if dequeue transaction fails to commit

TODO notes one network partitions

##### Ordering

description, add a note that ServiceBus requires sessions

##### De-Duplication

description, add a note that ServiceBus requires sessions

##### Durable

description

##### Delivery guarantee

descriptions, comments on RabbitMQ network partitions

##### Dead-lettering

descriptions and notes on different behaviors and permissione where appropriate

##### Poison messages

description

##### Max queue size
##### Max message size
##### Max message TTL
##### Max number of queues
##### Max number of concurrent clients
##### Max throughput
##### Average latency
##### Throttling

description and behavior

##### Security

Authentication and authorization

##### HA support

description and notes on different approaches

##### 3rd party SDK

links

##### Express messages

description and links to docu

##### Competing consumers

Competing consumers is a messaging pattern where more than one process or thread is allowed to dequeue messages from a given queue. It is primarily used to achieve higher processing throughput in cases where time of message processing is much longer than time it takes to dequeue a message. A queueing system that supports competing consumers need to guarantee that each message can be dequeued by only one client.

##### Topics support



### NServiceBus features

NServiceBus does its best to support all its built-in feature on all the supported transports, the following is the list of the NServiceBus major feature and the support provided on each transport:

(each element here will have a link to a document to dive into the topic or to a fancy box with a brief description to avoid to lose context, names are temporary placeholders)

  |MSMQ|SQL Server|RabbitMQ|Azure ServiceBus|Azure Storage Queues
|---              |---         |---               |---              |---                           |---
|Sagas|&#10004;|&#10004;|&#10004;|&#10004;|&#10004;|
|Outbox|&#10004;|&#10004;|&#10004;|&#10004;|&#10004;|
|De-Duplication (When Outbox is enabled)|&#10004;|&#10004;|&#10004;|&#10004;|&#10004;|
|DataBus|&#10004;|&#10004;|&#10004;|&#10004;|&#10004;|
|Gateway|&#10004;|&#10004;|&#10004;|&#10004;|&#10004;|
|Timeouts|&#10004;|&#10004;|&#10004;|&#10004;|&#10004;|
|First Level Retries|&#10004;|&#10004;|&#10004;|&#10004;|&#10004;|
|Second Level Retries|&#10004;|&#10004;|&#10004;|&#10004;|&#10004;|
|ServicePulse manual retry|&#10004;|&#10004;|&#10004;|&#10004;|&#10004;|
|ServiceControl Integration|&#10004;|&#10004;|&#10004;|&#10004;|&#10004;|

(Add comments on the DataBus available implementations and what can be done given the transport)

### MSMQ

Transport package: *not required, built-in the NServiceBus core*;

##### strengths

* _Windows built-in_:;
* NSB built-in and default;
* DTC support;
* distributed by nature no Single Point of Failure in the system;
* Store & forward support

##### weaknesses

* DTC in heterogeneous network and/or with high latency;
* Cannot be deployed in the cloud;
* no scale out support built-in needs the distributor;
* elevated account to install and create queue(s);
* does not work with Active Directory integration enabled
	
##### IT/Ops Management tools

* Windows management console;
* 3rd party queue explorer;

##### Deployment

* install and configure MSMQ and DTC (PowerShell cmdlets / or the Platform Installer for dev machines);
* Queues automatically created if the endpoint runs with a user with enough permissions, otherwise manual creation and manual ACL setup (!!!)

### SQL Server Transport

Transport package: [NServiceBus.SqlServer](https://www.nuget.org/packages/NServiceBus.SqlServer/)
SQL Server Transport is hybrid design, borrowing some ideas from store-and-forward approach and some from broker design. The queues are maintained in a centralized place (broker) but logic used to manipulate the queues is distributed and deployed to the endpoints (store-and-forward). SQL Server Transport can also work in a federated (broker) mode where clusters of (or single) endpoints use their own instances of SQL Server and messages are routed between them.

##### strengths

* scalable using SQL native solutions;
* supports the DTC;
* avoids DTC escalation if queues, user data and NServiceBus persistence data are in same database (can be used in SQL Azure);
* support for SQL 2014 AlwaysOn;

##### weaknesses

* license cost;
* no-DTC integration with NHibernate persistence requires usage of ambient transactions (`TransactionScope`);

##### IT/Ops Management tools:

* SQL Server Management Studio;

##### Deployment

* Server: available SQL server instance
* Client: no ops
* Queues: automatically created as endpoint startup if the endpoint connect with a user with enough permissions, otherwise manual creation?

### RabbitMQ

NServiceBus Transport package: [NServiceBus.RabbitMQ](https://www.nuget.org/packages/NServiceBus.RabbitMQ/)

##### Strengths

* RabbitMQ has built-in support for [clustering, shard and scale-out](https://www.rabbitmq.com/clustering.html);
* Via [shovels](https://www.rabbitmq.com/shovel.html) and [federation](https://www.rabbitmq.com/federation.html) plugins can be configured to cross network boundaries;

##### Weaknesses

* There is no complete support for [transactions](https://www.rabbitmq.com/semantics.html), RabbitMQ favors [publisher acknowledgements](https://www.rabbitmq.com/confirms.html) over AMQP transactions;
    * off by default 
* To guarantee a transactional-like behavior requires the [NServiceBus Outbox](http://docs.particular.net/nservicebus/no-dtc) feature that requires that all endpoints run at least NServiceBus V5;
* *(...hard HA...)*

##### IT/Ops Management tools

RabbitMQ has a built-in management web application, refer to the official documentation for details regarding the [Management Plugin](https://www.rabbitmq.com/management.html).

##### Deployment

* *Server*: Refer to the official RabbitMQ documentation for a detailed installation guide: [Downloading and Installing RabbitMQ](https://www.rabbitmq.com/download.html);
* *Client*: The RabbitMQ Client is automatically referenced by the endpoint when installing the `NServiceBus.RabbitMQ` transport package;
* *Queues creation*: queues are automatically created at endpoint startup if the endpoint is connected to the RabbitMQ server with a user enough enough permissions; otherwise queues needs to be created manually;

### Azure ServiceBus

Transport package: [NServiceBus.Azure](https://www.nuget.org/packages/NServiceBus.Azure/)

##### strengths

* fully managed by Azure;
* no maintenance;
* fully scaled and HA by Azure;
* local transactions

##### weaknesses

* requires Azure, and to be used from on-premise connectivity to Azure;
* requires on the client side the Azure SDK whose versioning is not always easy

##### IT/Ops Management tools:

* Azure portal;
* PowerShell cmdlets;

##### Deployment

no-ops, all automatic, just configure a namespace via the Azure portal

### Azure Storage Queues

Transport package: [NServiceBus.Azure](https://www.nuget.org/packages/NServiceBus.Azure/)

##### strengths

* fully managed by Azure;
* no maintenance;
* fully scaled and HA by Azure;
* cheap

##### weaknesses

* no transactions;
* requires an Azure subscription, and to be used from on-premise connectivity to Azure;
* requires on the client side the Azure SDK whose versioning is not always easy;

##### IT/Ops Management tools:

* PowerShell cmdlets;
* 3rd party [Cerebrata](http://www.cerebrata.com) tools;

##### Deployment

no-ops, all automatic, just configure a storage account via the Azure portal

### Resources:

Temporary section that list all the external resources I am using

Azure

* http://msdn.microsoft.com/en-us/library/azure/hh767287.aspx

Rabbit

* https://www.rabbitmq.com/access-control.html
* http://www.rabbitmq.com/blog/2012/05/11/some-queuing-theory-throughput-latency-and-bandwidth/
* https://www.rabbitmq.com/confirms.html
* https://www.rabbitmq.com/semantics.html (tx)
* https://www.rabbitmq.com/dlx.html

MSMQ

* http://stackoverflow.com/questions/3001161/what-is-the-maximum-number-of-queues-that-can-be-supported-by-msmq
* http://msdn.microsoft.com/en-us/library/ms811056.aspx
* http://blogs.msdn.com/b/johnbreakwell/archive/2009/08/26/10-connection-limit-for-msmq-clients.aspx
* http://blogs.msdn.com/b/johnbreakwell/archive/2007/02/15/so-what-s-the-limit-on-connections-to-an-msmq-server.aspx
* http://en.wikipedia.org/wiki/Store_and_forward
