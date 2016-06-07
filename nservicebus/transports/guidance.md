---
title: SQL Server Transport usage guidance
summary: Usage guidance for SQL Server transport
tags:
- SQL Server
---

The SQL Server transport implements a message queueing mechanism on top of Microsoft SQL Server. It provides support for sending messages over [SQL Server](http://www.microsoft.com/en-au/server-cloud/products/sql-server/) tables. It does **not** make any use of ServiceBroker, a messaging technology built into the SQL Server.


## How it works

The SQL Server transport is a hybrid queueing system which is neither store-and-forward (like MSMQ) nor a broker (like RabbitMQ). It treats the SQL Server as a storage infrastructure for the queues while the queue implementation logic is executed inside the client process.


### Advantages of SQL Server Transport

 * No additional licensing and training costs, as majority of Microsoft stack organizations already have SQL Server installed and have knowledge required to run it
 * Great general-purpose tooling (SSMS)
 * Throughput on par with MSMQ
 * Scaling out of processing is simple because of support for competing consumers at the queue level

## Deployment considerations

### Introduction
Any NServiceBus endpoint operates and manages different types of data, those are:
 * Business data - data used for implementing business capabilities
 * Persistence data - infrastructure level data including: saga data, timeout manager state and message driven pub/sub information
 * Transport data - messaging infrastructure state

Sql Server Transport manages transport data and puts no constraints on the type and configuration of storage technology used for persistence and business data. It can work with any of available persisters i.e. NHibernate and RavenDB, as well as any storage mechanisms used inside message handlers. However sharing storage technology (MS Sql Server) between transport, persistence and/or business data offers unique advantages (see next section) and as a result this guidance will focus on such scenario.

From data perspective, deployment model chosen by system architect answers the question of how business, persistence and transport data is stored in terms of MS Sql server instances, catalogs and schemas.

NOTE: No matter which deployment option is chosen there is one general rule that should always apply: **All transport data (input, audit and error queues) should reside on a single MS SQL server instance**.  

### Transactionality (Exactly once message delivery)
When sharing single technology for different types of data it is possible to provide `exactly-once` message processing without need for DTC. In all other cases it is required to turn on DTC.

#### Business, Persistence and Transport data in single catalog
Enables `exactly-once` message delivery with native transactions. Multi-catalog with local transactions are possible in principle but currently not supported by Sql Server transport. There is possibility to use different schemas.

#### Business and Persistence data in a single catalog
Enables `exactly-once` message delivery using `Outbox` feature. There is possibility to use different schemas.
  
### Security 

### Performance

### Availability 

### Monitoring (Service Control)

## Sample usage scenarios
### Tiny

The SQL Server transport is an ideal choice for extending an existing web application with asynchronous processing capabilities as an alternative for tedious batch jobs that tend to quickly get out of synch with the main codebase. Assuming the application already uses SQL Server as a database, this scenario does not require any additional infrastructure.

The queue tables are hosted in the same catalog as business data and the NServiceBus runtime can be hosted in the web worker. In some cases there is a need for a separate process for NServiceBus. In most cases these systems consists of 1 to 3 non-scaled out endpoints. Because they consist of a single logical service or bounded context, there is usually no need to create separate schemas for the queues.

### Small

The SQL Server transport is a good choice for a pilot project to prove feasibility of NServiceBus in a given organization as well as for a small, well-defined green field application. It usually requires nothing more than a database. Such a system usually consists of 2-10 fairly independent non-scaled out endpoints.

The best option is to store the queues in the same catalog as the business data. Schemas can be used to make maintenance easier and allow for finer-grained security. 

#### Security

For best results, both in terms of security and maintainability, each endpoint should run in context of a different account. Each such account should own three schemas on the database level
 * the data schema (e.g. `sales`) for which it has exclusive write access. Other accounts might optionally have read access to this schema.
 * the incoming queue schema (e.g. `sales-incoming`) for which it has exclusive `DELETE` access. Other accounts should have `INSERT` permissions to this schema
 * the private queue schema (e.g. `sales-nsb`) for which it has exclusive access. No other accounts should have any permissions to this schema.

This approach ensures that endpoints are allowed to send messages to each other and (optionally) read each other's data. They are not allowed to receive other endpoint's messages nor modify their data directly.

### Medium to large

For larger systems it usually makes more sense to have separate databases for each service or bounded context. It is also not unusual to see NoSQL databases in at least some of the services. The SQL databases might be hosted in a single instance of SQL Server or in separate instances depending on the IT policy.

Such systems consist of tens to hundreds of endpoints. Some of the endpoints might be scaled out for higher throughput. 

The best option is to have a dedicated catalog for the queues. This approach allows to use different scaling up strategies for the queue catalog. It also allows to use a dedicated back up policy for the queues (because data in queues has much more ephemeral). Unfortunately it leaves a problem of how to ensure that data modifications and queue operations are atomic and isolated from each other. 

One option is to use Microsoft Distributed Transaction Coordinator and distributed transactions (NServiceBus with SQL Server transport is DTC-enabled by default). In this mode the message receive transaction and all data modifications that result from processing the message are part of one distributed transaction that spans two SQL catalogs.

Another options is to use Outbox, which is a unique feature of NServiceBus, that provides exactly-once processing semantics over an at-least-once infrastructure. In this mode each individual database has to include the outbox table. Outgoing messages are stored in that table in the same transaction as the business data is modified and, only after that transaction commits successfully, they are dispatched to the destination. 

#### Scaling up

**TODO**

 * Separate file groups for hot queues?
 * In mem OLTP? 

#### Security

For best results both in terms of security and maintainability each endpoint should run in context of a different account. Each such account should own two schemas in the transport catalog
 * the incoming queue schema (e.g. `sales-incoming`) for which it has exclusive `DELETE` access. Other accounts should have `INSERT` permissions to this schema
 * the private queue schema (e.g. `sales-nsb`) for which it has exclusive access. No other accounts should have any permissions to this schema.

This approach ensures that endpoints are allowed to send messages to each other but they are not allowed to receive other endpoint's messages.

### Distributed

**TODO**

 * Describe potential of `INSTEAD OF` triggers
 * Describe potential of linked servers.

### Azure

**TODO**

Seems like it makes sense to cover various options. It looks like having a separate queue DB would be beneficial because we can have scale better (buy higher tier DB only for the queue DB)
