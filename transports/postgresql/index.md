---
title: PostgreSQL transport
summary: An overview of the NServiceBus PostgreSQL transport.
reviewed: 2024-05-28
component: PostgreSqlTransport
redirects:
related:
---

The PostgreSQL transport implements a message queuing mechanism on top of [PostgreSQL](https://www.postgresql.org/).

## Transport at a glance

|Feature                    |   |
|:---                       |---
|Transactions |None, ReceiveOnly, SendsAtomicWithReceive |
|Pub/Sub                    |Native |
|Timeouts                   |Native |
|Large message bodies       |PostgreSQL can handle arbitrary message size  within available resources, very large messages via data bus |
|Scale-out                  |Competing consumer |
|Scripted Deployment        |SQL Scripts |
|Installers                 |Optional |
|Native integration         |Supported, see [SQL statements used by the transport](https://github.com/Particular/NServiceBus.SqlServer/blob/master/src/NServiceBus.Transport.PostgreSql/PostgreSqlConstants.cs) |
|Case Sensitive             |Depends on the collation

## Usage

A basic use of the transport is as follows:

snippet: usage

## How it works

Tables are used to represent queues and store messages. The transport is best considered as a brokered transport, like RabbitMQ, rather than a store-and-forward transport, such as MSMQ.

## Advantages

* No additional licensing and training costs; many organizations have PostgreSQL installed and have the knowledge required to manage it.
* Mature tooling, such as [pgAdmin](https://www.pgadmin.org/).
* Queues support competing consumers.
* Can share database transaction context with [SQL Persistence](/persistence/sql/), [Marten](https://martendb.io/) and EntityFramework to ensure data consistency for [Sagas](/nservicebus/sagas/) and business data.

## Disadvantages

* No local store-and-forward mechanism; when a PostgreSQL instance is down, the endpoint cannot send or receive messages.
* In centralized deployment scenarios, maximum throughput applies for the whole system, not individual endpoints. For example, if PostgreSQL can handle 2000 msg/s on the given hardware, each one of the 10 endpoints deployed on this machine can only receive a maximum of 200 msg/s (on average).
* When using PostgreSQL transport, a database table serves as a queue for the messages for the endpoints. These tables are polled to see if there are messages to be processed by the endpoints. Although the polling interval is, by default, one second, this may still lead to delays in processing a message. For environments where low latency is required, consider using other transports that use dedicated queuing technologies, such as RabbitMQ.

## Deployment considerations

### Security

Security considerations for PostgreSQL transport should follow [the principle of least privilege](https://en.wikipedia.org/wiki/Principle_of_least_privilege).

Each endpoint should use a dedicated PostgreSQL principal with `SELECT` and `DELETE` permissions on its input queue tables and `INSERT` permission on input queue tables of endpoints it sends messages to. Each endpoint should also have permissions to insert rows to audit and error queue tables.

Dedicated schemas can be used to manage fine-grained access control to various database objects used by the endpoint, including its queue tables.

### Retries

When an exception occurs during message handling, the transaction is already in doubt and must be rolled back. Failure info is stored in memory, and can't influence the next step for that message (immediate retry, delayed retry, or forward to the error queue) until the next time it is processed on that node.

When an endpoint is scaled out to multiple instances, more retries may be observed than configured, as each instance accumulates failures until a single node has observed enough retries to escalate to delayed retries. It is also possible for immediate retries to be observed on a different instance even when [Recoverability](/nservicebus/recoverability/) is configured for zero immediate retries.

### Connection pooling

By default, PostgreSQL [limits the maximum number](https://www.postgresql.org/docs/current/runtime-config-connection.html#RUNTIME-CONFIG-CONNECTION-SETTINGS) of concurrent client connections to `100` per database server. The [client connection pooling mechanism](https://www.npgsql.org/doc/connection-string-parameters.html#pooling) built into the Npgsql library (used internally by the transport) should prevent this constraint from being hit for small system deployments. When the limit is reached, the following options should be considered:

- using [PgBouncer](https://www.pgbouncer.org/), a self-hosted external connection pooling service, and tweaking the connection strings for endpoints and the platform tools according to [the Npgsql compatibility guidelines](https://www.npgsql.org/doc/compatibility.html#pgbouncer)
- using a hosted solution like [Azure Flexible Server](https://learn.microsoft.com/en-us/azure/postgresql/flexible-server/concepts-pgbouncer) or [AWS RDS Proxy](https://aws.amazon.com/rds/proxy/) 
- changing the default value for [the `max_connections` setting](https://www.postgresql.org/docs/current/runtime-config-connection.html#RUNTIME-CONFIG-CONNECTION-SETTINGS) at the database server level. Note that this is not recommended for most systems, since each new connection [spins up a separate process](https://www.postgresql.org/docs/current/connect-estab.html) which can lead to system resource starvation

## Transactions

PostgreSQL transport supports the following [transaction handling modes](/transports/transactions.md): 

- receive only 
- send atomic with receive 
- no transactions. 

It does not support Transaction scope mode due to the limitations in the design of the PostgreSQL database and the System.Transactions library.
