---
title: SQL Server Transport usage guidance
summary: Usage guidance for SQL Server transport
tags:
- SQL Server
---

The SQL Server transport allows endpoints to exchange messages using the Microsoft SQL Server Database Engine. It implements queues on top of database tables and send / receive operations as `INSERT` and `DELETE` operations.

Please note that the SQL Server transport does not use or depend on SQL Server Service Broker.

## How it works

SQL Server provides a central place to store queues and messages but the queue implementation is executed entirely within the endpoint process. As such, SQL Server transport is best thought of as a brokered transport like RabbitMQ rather than store-and-forward transport such as MSMQ.


### Advantages of SQL Server Transport

 * No additional licensing and training costs, as majority of Microsoft stack organizations already have SQL Server installed and have knowledge required to run it
 * Great general-purpose tooling (SSMS)
 * Free to start (Express edition)
 * Scaling out of processing is simple because of support for competing consumers at the queue level

## Deployment considerations

### Introduction
Any NServiceBus endpoint operates and manages different types of data, those are:
 * Business data - data used for implementing business capabilities
 * Persistence data - infrastructure level data including: saga data, timeout manager state and message driven pub/sub information
 * Transport data - queues and messages

SQL Server Transport manages transport data and puts no constraints on the type and configuration of storage technology used for persistence and business data. It can work with any of available persisters i.e. NHibernate and RavenDB, as well as any storage mechanisms used inside message handlers.

Building an NServiceBus system requires that each kind of data is persisted in some way. This section explains what are the important factors to consider when choosing persistence technology to use in combination with the SQL Server transport.
 
NOTE: No matter what deployment options are chosen, there is one general rule that should always apply: **All transport data (input, audit and error queues) should reside on a single SQL Server instance**. Multi-instance deployment topology is deprecated in version 3 of the SQL Server transport and will be removed in version 4. Please see: ###Section### for migration guidance.  

### Transactionality
SQL Server Transport supports all [transaction modes](/nservicebus/transports/transactions.md) in particular `TransactionScope` which enables `exactly once` message processing with usage of distributed transactions. However when transport, persistence and business data are all stored in a single SQL Server catalog it is possible to achieve `exactly-once` message delivery without need for distributed transactions (###Link to Sample###).

NOTE: `Exactly once` message processing without distributed transactions can be achieved with any transport (and SQL Server Transport in particular) using [Outbox](/nservicebus/outbox/). It requires business and persistence data to share the storage mechanism but does not put any requirements on transport data storage.

### Security 
Security considerations for SQL Server Transport should follow the principle of least privilege. For that each endpoint should use dedicated SQL Server principal with SELECT and DELETE permissions on it's input queues as well as INSERT permissions on other endpoint's input queues, audit queue and error queue. 

Multi-schema configuration can be used to ease the maintenance of security configuration because each endpoint may own a number of queues.

### Monitoring (Service Control)
All deployment options for Sql Server Transport described in this section are supported by Service Control. For details on Service Control running on top of Sql Server Transport please ###Link Here###

## Sample usage scenarios
### Tiny

The SQL Server transport is an ideal choice for extending an existing web application with asynchronous processing capabilities as an alternative for tedious batch jobs that tend to quickly get out of synch with the main codebase. Assuming the application already uses SQL Server as a database, this scenario does not require any additional infrastructure.

The queue tables are hosted in the same catalog as business data and the NServiceBus runtime can be hosted in the web worker. In some cases there is a need for a separate process for NServiceBus. Because they consist of a single logical service or bounded context, there is usually no need to create separate schemas for the queues.

### Small

The SQL Server transport is a good choice for a pilot project to prove feasibility of NServiceBus in a given organization as well as for a small, well-defined green field application. It usually requires nothing more than a single shared database.

The best option is to store the queues in the same catalog as the business data. Schemas can be used to make maintenance easier. 

### Medium to large

For larger systems it usually makes more sense to have separate databases for each service or bounded context. It is also not unusual to see NoSQL databases in at least some of the services. The SQL databases might be hosted in a single instance of SQL Server or in separate instances depending on the IT policy.

Such systems consist of tens to hundreds of endpoints. Some of the endpoints might be scaled out for higher throughput. 

The best option is to have a dedicated catalog for the queues. This approach allows to use different scaling up strategies for the queue catalog. It also allows to use a dedicated back up policy for the queues (because data in queues are much more ephemeral). Unfortunately it leaves a problem of how to ensure that data modifications and queue operations are atomic and isolated from each other. 

One option is to use Microsoft Distributed Transaction Coordinator and distributed transactions (NServiceBus with SQL Server transport is DTC-enabled by default). In this mode the message receive transaction and all data modifications that result from processing the message are part of one distributed transaction that spans two SQL catalogs.

Another options is to use Outbox, which is a unique feature of NServiceBus, that provides exactly-once processing semantics over an at-least-once infrastructure. In this mode each individual database has to include the outbox table. Outgoing messages are stored in that table in the same transaction as the business data is modified and, only after that transaction commits successfully, they are dispatched to the destination. 
