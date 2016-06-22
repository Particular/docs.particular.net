---
title: SQL Server Transport
summary: High-level description of NServiceBus SQL Server Transport.
reviewed: 2016-06-20
tags:
- SQL Server
redirects:
- nservicebus/sqlserver/usage
related:
- samples/outbox/sqltransport-nhpersistence
- samples/sqltransport-nhpersistence
- samples/outbox/sqltransport-nhpersistence-ef
---

The SQL Server transport implements a message queueing mechanism on top of Microsoft SQL Server. It provides support for sending messages over [SQL Server](http://www.microsoft.com/en-au/server-cloud/products/sql-server/) tables. It does **not** make any use of ServiceBroker, a messaging technology built into the SQL Server.


## How it works

SQL Server transport uses SQL Server to store queues and messages. It doesn't use any queuing services provided by SQL Server, the queuing logic is implemented within the NServiceBus SQL Server transport. The SQL Server transport is best thought of as a brokered transport like RabbitMQ rather than [store-and-forward](/nservicebus/architecture/principles.md#drilling-down-into-details-store-and-forward-messaging) transport such as MSMQ.


## Advantages and Disadvantages of choosing SQL Server Transport


### Advantages

 * No additional licensing and training costs, as majority of Microsoft stack organizations already have SQL Server installed and have knowledge required to run it.
 * Great tooling (SSMS).
 * Maximum throughput for any given endpoint is on par with MSMQ.
 * Free to start (Express edition).
 * Queues support competing consumers (multiple instances of same endpoint feeding off of same queue) so scale-out doesn't require using [distributor](/nservicebus/scalability-and-ha/distributor/).


### Disadvantages

 * No local store-and-forward mechanism, meaning that when SQL Server instance is down the endpoint cannot send nor receive messages.
 * In centralized deployment maximum throughput applies for the whole system, not individual endpoints. If SQL Server on a given hardware can handle 2000 msg/s, each one of the 10 endpoints deployed on this machine can only receive 200 msg/s (on average).
 * Queue tables are polled periodically to check if receive operation should be performed. This may lead to delays in message delivery for endpoints that experience low rate of incoming messages.


## Deployment considerations 

The typical process hosting NServiceBus operates and manages three different kinds of data:
 * Transport data - queues and messages managed by the transport.
 * Persistence data - required for correct operation of specific transport features, e.g. saga data, timeout manager state and subscriptions.
 * Business data - application-specific data, independent of NServiceBus, usually managed via code executed from inside message handlers.

SQL Server Transport manages transport data, but it puts no constraints on the type and configuration of storage technology used for persistence or business data. It can work with any of the available persisters i.e. [NHibernate](/nservicebus/nhibernate) or [RavenDB](/nservicebus/ravendb/) for storing NServiceBus data, as well as any storage mechanisms for storing business datra.

This section explains the important factors to consider when choosing technologies for managing business and persistence data to use in combination with the SQL Server transport.
 
NOTE: No matter what deployment options are chosen, there is one general rule that should always apply: **All transport data (input, audit and error queues) should reside in a single SQL Server catalog**. Multi-instance/multi-catalog deployment topology is still available but is deprecated in version 3 of the SQL Server transport. It will be removed in version 4.

### Transactionality
SQL Server Transport supports all NServiceBus [transaction modes](/nservicebus/transports/transactions.md). `TransactionScope` mode is particularly useful as it enables `exactly once` message processing with usage of distributed transactions. However, when transport, persistence and business data are all stored in a single SQL Server catalog it is possible to achieve `exactly-once` message delivery without distributed transactions. For more details refer to the [SQL Server native integration](/samples/sqltransport/native-integration/) sample.

NOTE: `Exactly once` message processing without distributed transactions can be achieved with any transport using [Outbox](/nservicebus/outbox/). It requires business and persistence data to share the storage mechanism but does not put any requirements on transport data storage.


### Security 

Security considerations for SQL Server Transport should follow [the principle of least privilege](https://en.wikipedia.org/wiki/Principle_of_least_privilege). 

Each endpoint should use a dedicated SQL Server principal with `SELECT` and `DELETE` permissions on its input queue tables and `INSERT` permission on input queue tables of endpoints it sends messages to. Each endpoint should also have permissions to insert rows to audit and error queue tables.

[Multi-schema](/nservicebus/sqlserver/configuration.md#multiple-custom-schemas) configuration can be used to manage fine-grained access control to various database objects used by the endpoint, including its queue tables.


### Service Control

All deployment options for SQL Server Transport described in this section are supported by Service Control.


## Sample deployment scenarios


### Background worker replacement

The SQL Server transport is an ideal choice for extending an existing web application with asynchronous processing capabilities as an alternative for batch jobs that tend to quickly get out of sync with the main codebase. Assuming the application already uses SQL Server as a data store, this scenario does not require any additional infrastructure.

The queue tables can be hosted in the same SQL Server catalog as business and persistence data. NServiceBus endpoint can be hosted in the ASP.NET worker process. In some cases there might be a need for a separate process for hosting the NServiceBus endpoint (due to security considerations or IIS worker process life-cycle management). Because system consists of a single logical service or bounded context, there is usually no need to create separate schema for the queues.


### Pilot project

The SQL Server transport is a good choice for a pilot project to prove feasibility of NServiceBus in a given organization as well as for a small, well-defined green field application. It usually requires nothing more than a single shared SQL Server instance.

The best option is to store the queues in the same catalog as the business data. Schemas can be used to make maintenance easier. 


### Messaging framework for the enterprise

For larger systems it usually makes more sense to have dedicated catalogs for business and persistence data per service or bounded context. NoSQL data stores can be used in some services, depending on data access requirements. The SQL catalogs might be hosted in a single instance of SQL Server or in separate instances depending on the IT policy.

The best option is to have a dedicated catalog for the queues. This approach allows to use different scaling up strategies for the queue catalog. It also allows to use a dedicated backup policy for the queues (because data in queues are much more ephemeral). 

Is such a case local transactions are not enough to ensure `exactly-once` message processing. When required, one option is to use Microsoft Distributed Transaction Coordinator and distributed transactions (NServiceBus with SQL Server transport is DTC-enabled by default). In this mode the message receive transaction and all data modifications that result from processing the message are part of one distributed transaction that spans two SQL catalogs (possibly stored on two different instances).

Another options is to use the [Outbox](/nservicebus/outbox/) feature, that provides `exactly-once` processing semantics over an `at-least-once` message delivery infrastructure. In this mode each individual endpoint has to store bussiness data and persistence data in the same catalog. Outgoing messages are stored by persistece infrastructure in a dedicated table. Single local transaction handles outgoing message persistence together with business data modifications. After the local transaction commits successfully the messages are dispatched to the final destination.
