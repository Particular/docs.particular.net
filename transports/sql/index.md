---
title: SQL Server Transport
summary: An overview of the NServiceBus SQL Server transport.
reviewed: 2020-01-31
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

The SQL Server transport implements a message queuing mechanism on top of [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/). It provides support for sending messages using SQL Server tables. It does **not** make use of a [service broker](https://technet.microsoft.com/en-us/library/ms166104.aspx).

WARNING: Although this transport will run on the free version of the engine, i.e. [SQL Server Express](https://www.microsoft.com/en-au/sql-server/sql-server-editions-express), it is strongly recommended to use commercial versions for production systems. It is also recommended to ensure that support agreements are in place from [Microsoft Support](https://www.microsoft.com/en-us/microsoftservices/support.aspx), or another third party support provider.

## Transport at a glance

|Feature                    |   |
|:---                       |---
|Transactions |None, ReceiveOnly, SendWithAtomicReceive, TransactionScope
|Pub/Sub                    |Native
|Timeouts                   |Native
|Large message bodies       |SqlServer can handle arbitrary message size within available resources, very large messages via data bus
|Scale-out             |Competing consumer
|Scripted Deployment        |Sql Scripts
|Installers                 |Optional

## Usage

A basic use of the SQL Server transport is as follows:

snippet: usage

See also: [connection settings](/transports/sql/connection-settings.md).


## How it works

SQL Server transport uses SQL Server to store queues and messages. It doesn't use the queuing services provided by SQL Server; the queuing logic is implemented within the transport. The SQL Server transport is best considered as a brokered transport, like RabbitMQ, rather than [store-and-forward](/nservicebus/architecture/principles.md#messaging-versus-rpc-store-and-forward-messaging) transport, such as MSMQ.


## Advantages

 * No additional licensing and training costs; many Microsoft stack organizations have SQL Server installed and have the knowledge required to manage it.
 * Mature tooling, such as [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms).
 * Free to start with the [SQL Server Express edition](https://www.microsoft.com/en-au/sql-server/sql-server-editions-express).
 * Queues support competing consumers (multiple instances of the same endpoint feeding off the same queue) so scale-out doesn't require a [distributor](/transports/msmq/distributor/).
 * Supports [Microsoft Distributed Transaction Coordinator (MSDTC)](https://msdn.microsoft.com/en-us/library/ms684146.aspx).


## Disadvantages

 * No local store-and-forward mechanism without implementing [custom code](/samples/sqltransport/store-and-forward/); when a SQL Server instance is down, the endpoint cannot send nor receive messages.
 * In centralized deployment scenarios, maximum throughput applies for the whole system, not individual endpoints. For example, if SQL Server can handle 2000 msg/s on the given hardware, each one of the 10 endpoints deployed on this machine can only receive a maximum of 200 msg/s (on average).
 * When using SQL Server transport, a database table serves as a queue for the messages for the endpoints. These tables are polled periodically to see if messages need to be processed by the endpoints. Although the polling interval is one second, this may still lead to delays in processing a message. For environments where low latency is required, consider using other transports that use queuing technologies, such as RabbitMQ. Refer to [receiving behavior](design.md#behavior-receiving) documentation for more information about the polling configuration.


## Deployment considerations

The typical process hosting NServiceBus operates and manages three types of data:

 * Transport data - queues and messages managed by the transport.
 * Persistence data - required for correct operation of specific transport features, e.g. saga data, timeout manager state and subscriptions.
 * Business data - application-specific data, independent of NServiceBus, usually managed via code executed from inside message handlers.

SQL Server transport manages transport data, but it puts no constraints on the type and configuration of storage technology used for persistence or business data. It can work with any of the available persisters, e.g. [NHibernate](/persistence/nhibernate) or [RavenDB](/persistence/ravendb/), for storing NServiceBus data, as well as any storage mechanisms for storing business data.

This section explains the factors to consider when choosing technologies for managing business and persistence data to use in combination with the SQL Server transport.

### Security

Security considerations for SQL Server transport should follow [the principle of least privilege](https://en.wikipedia.org/wiki/Principle_of_least_privilege).

Each endpoint should use a dedicated SQL Server principal with `SELECT` and `DELETE` permissions on its input queue tables and `INSERT` permission on input queue tables of endpoints it sends messages to. Each endpoint should also have permissions to insert rows to audit and error queue tables.

[Multi-schema](/transports/sql/deployment-options.md#multi-schema) configuration can be used to manage fine-grained access control to various database objects used by the endpoint, including its queue tables.

partial: alwaysencrypted

### ServiceControl

All deployment options for SQL Server transport described in this section are supported by ServiceControl.


## Sample deployment scenarios


### Background worker replacement

The SQL Server transport is an ideal choice for extending an existing web application with asynchronous processing capabilities as an alternative for batch jobs that tend to quickly get out of sync with the main codebase. Assuming the application already uses SQL Server as a data store, this scenario does not require any additional infrastructure.

The queue tables can be hosted in the same SQL Server catalog as business and persistence data. NServiceBus endpoints can be hosted in an ASP.NET worker process. In some cases there might be a need for a separate process for hosting the NServiceBus endpoint (due to security considerations or IIS worker process lifecycle management). Because the system consists of a single logical service or bounded context, there is usually no need to create a separate schema for the queues.


### Pilot project

The SQL Server transport is a good choice for a pilot project to prove feasibility of NServiceBus in a given organization as well as for a small, well-defined greenfield application. It usually requires only a single shared SQL Server instance.

The best option is to store the queues in the same catalog as the business data. Schemas can be used to make maintenance easier.


### Messaging framework for the enterprise

For larger systems it usually makes more sense to have dedicated catalogs for business and persistence data per service or bounded context. NoSQL data stores can be used in some services depending on data access requirements. The SQL catalogs might be hosted in a single instance of SQL Server or in separate instances depending on the IT policy.

The best option is to have a dedicated catalog for the queues. This approach allows use of different scale-up strategies for the queue catalog. It also allows use of a dedicated backup policy for the queues (because data in queues is much more short-lived).

In this case, local transactions are not enough to ensure `exactly-once` message processing. When required, one option is to use the Microsoft Distributed Transaction Coordinator and distributed transactions (NServiceBus with SQL Server transport is DTC-enabled by default). In this mode the "message receive" transaction and all data modifications that result from processing the message are part of one distributed transaction that spans two SQL catalogs (possibly stored on two different instances).

Another option is to use the [outbox](/nservicebus/outbox/) feature, which provides `exactly-once` processing semantics over an `at-least-once` message delivery infrastructure. In this mode each individual endpoint has to store business data and persistence data in the same catalog. Outgoing messages are stored by persistence infrastructure in a dedicated table. Single local transaction handles outgoing message persistence together with business data modifications. After the local transaction commits successfully the messages are dispatched to the final destination.


## Persistence

When the SQL Server transport is used in combination with [NHibernate persistence](/persistence/nhibernate/), it allows for sharing database connections and optimizing transaction handling to avoid escalating to DTC. However, the SQL Server transport can be used with any available persistence implementation.


## Transactions

SQL Server transport supports all [transaction handling modes](/transports/transactions.md), i.e. Transaction scope, receive only, send atomic with receive, and no transactions.

Refer to [transport transactions](/transports/transactions.md) for a detailed explanation of the supported transaction handling modes and available configuration options.
