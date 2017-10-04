---
title: SQL Server Transport
summary: High-level description of NServiceBus SQL Server Transport.
reviewed: 2016-08-31
component: SqlTransport
redirects:
 - nservicebus/sqlserver/usage
 - nservicebus/sqlserver
 - transports/sqlserver
related:
 - samples/sqltransport-sqlpersistence
 - samples/sqltransport-nhpersistence
 - samples/outbox/sql
---

The SQL Server transport implements a message queuing mechanism on top of [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/). It provides support for sending messages over SQL Server tables. It does **not** make any use of [Service Broker](https://technet.microsoft.com/en-us/library/ms166104.aspx).

WARNING: Although this transport will run on the free version of the engine, i.e. [SQL Server Express](https://www.microsoft.com/en-au/sql-server/sql-server-editions-express), it is strongly recommended to use commercial versions for any production system. It is also recommended to ensure that support agreements are in place from [Microsoft Premier Support](https://www.microsoft.com/en-us/microsoftservices/support.aspx), or another third party support provider.

## How it works

SQL Server transport uses SQL Server to store queues and messages. It doesn't use any queuing services provided by SQL Server, the queuing logic is implemented within the transport. The SQL Server transport is best thought of as a brokered transport like RabbitMQ rather than [store-and-forward](/nservicebus/architecture/principles.md#messaging-versus-rpc-store-and-forward-messaging) transport such as MSMQ.


## Advantages and Disadvantages


### Advantages

 * No additional licensing and training costs, as majority of Microsoft stack organizations already have SQL Server installed and have knowledge required to run it.
 * Mature tooling [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms).
 * Free to start [SQL Server Express edition](https://www.microsoft.com/en-au/sql-server/sql-server-editions-express).
 * Queues support competing consumers (multiple instances of same endpoint feeding off of same queue) so scale out doesn't require using [distributor](/transports/msmq/distributor/).
 * Supports [Microsoft Distributed Transaction Coordinator (MSDTC)](https://msdn.microsoft.com/en-us/library/ms684146.aspx).


### Disadvantages

 * No local store-and-forward mechanism, meaning that when SQL Server instance is down the endpoint cannot send nor receive messages.
 * In centralized deployment maximum throughput applies for the whole system, not individual endpoints. If SQL Server on a given hardware can handle 2000 msg/s, each one of the 10 endpoints deployed on this machine can only receive 200 msg/s (on average).
 * When using SQL Server transport, a database table serves as a queue for the messages for the endpoints. These tables are polled periodically to see if messages need to be processed by the endpoints. Although the polling interval is one second, this may still lead to delays in when a message is picked up for processing. For environments where low latency is a requirement, rather than using SQL as a queuing transport, consider using other transports that use queuing technologies like RabbitMQ, MSMQ, etc.


## Deployment considerations

The typical process hosting NServiceBus operates and manages three different kinds of data:

 * Transport data - queues and messages managed by the transport.
 * Persistence data - required for correct operation of specific transport features, e.g. saga data, timeout manager state and subscriptions.
 * Business data - application-specific data, independent of NServiceBus, usually managed via code executed from inside message handlers.

SQL Server Transport manages transport data, but it puts no constraints on the type and configuration of storage technology used for persistence or business data. It can work with any of the available persisters i.e. [NHibernate](/persistence/nhibernate) or [RavenDB](/persistence/ravendb/) for storing NServiceBus data, as well as any storage mechanisms for storing business data.

This section explains the important factors to consider when choosing technologies for managing business and persistence data to use in combination with the SQL Server transport.

NOTE: No matter what deployment options are chosen, there is one general rule that should always apply: **All transport data (input, audit and error queues) should be stored in a single SQL Server catalog**. Multi-instance/multi-catalog deployment topology is still available in version 3 of the SQL Server transport to allow interoperability with version 2, but it is deprecated and not recommended for new projects.


### Security

Security considerations for SQL Server Transport should follow [the principle of least privilege](https://en.wikipedia.org/wiki/Principle_of_least_privilege).

Each endpoint should use a dedicated SQL Server principal with `SELECT` and `DELETE` permissions on its input queue tables and `INSERT` permission on input queue tables of endpoints it sends messages to. Each endpoint should also have permissions to insert rows to audit and error queue tables.

[Multi-schema](/transports/sql/deployment-options.md#modes-overview-multi-schema) configuration can be used to manage fine-grained access control to various database objects used by the endpoint, including its queue tables.


### ServiceControl

All deployment options for SQL Server Transport described in this section are supported by ServiceControl.


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

In such a case local transactions are not enough to ensure `exactly-once` message processing. When required, one option is to use Microsoft Distributed Transaction Coordinator and distributed transactions (NServiceBus with SQL Server transport is DTC-enabled by default). In this mode the message receive transaction and all data modifications that result from processing the message are part of one distributed transaction that spans two SQL catalogs (possibly stored on two different instances).

Another options is to use the [Outbox](/nservicebus/outbox/) feature, that provides `exactly-once` processing semantics over an `at-least-once` message delivery infrastructure. In this mode each individual endpoint has to store business data and persistence data in the same catalog. Outgoing messages are stored by persistence infrastructure in a dedicated table. Single local transaction handles outgoing message persistence together with business data modifications. After the local transaction commits successfully the messages are dispatched to the final destination.


## Persistence

When the SQL Server transport is used in combination [NHibernate persistence](/persistence/nhibernate/) it allows for sharing database connections and optimizing transactions handling to avoid escalating to DTC. However, SQL Server Transport can be used with any other available persistence implementation.


## Transactions

SQL Server transport supports all [transaction handling modes](/transports/transactions.md), i.e. Transaction scope, Receive only, Sends atomic with Receive and No transactions.

Refer to [Transport Transactions](/transports/transactions.md) for detailed explanation of the supported transaction handling modes and available configuration options.
