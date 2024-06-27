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

| Feature                                            |                                                                                                                                                                                    |
|:---------------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Transactions                                       | None, ReceiveOnly, SendsAtomicWithReceive                                                                                                                                          |
| Pub/Sub                                            | Native                                                                                                                                                                             |
| Timeouts                                           | Native                                                                                                                                                                             |
| Large message bodies                               | PostgreSQL can handle arbitrary message size  within available resources, very large messages via data bus                                                                         |
| Scale-out                                          | Competing consumer                                                                                                                                                                 |
| Scripted Deployment                                | SQL Scripts                                                                                                                                                                        |
| Installers                                         | Optional                                                                                                                                                                           |
| Native integration                                 | Supported, see [SQL statements used by the transport](https://github.com/Particular/NServiceBus.SqlServer/blob/master/src/NServiceBus.Transport.PostgreSql/PostgreSqlConstants.cs) |
| [time-to-be-received](#time-to-be-received) (TTBR) | Storage reclaimed at most 5 minutes after expiration when receiving endpoint is running                                                                                            |

## Usage

A basic use of the PostgreSQL transport is as follows:

snippet: usage

## How it works

PostgreSQL transport uses tables to represent queues and store messages. The PostgreSQL transport is best considered as a brokered transport, like RabbitMQ, rather than a store-and-forward transport, such as MSMQ.

## Advantages

* No additional licensing and training costs; many organizations have PostgreSQL installed and have the knowledge required to manage it.
* Mature tooling, such as [pgAdmin](https://www.pgadmin.org/).
* Queues support competing consumers.
* Can share database transaction context with [SQL Persistence](/persistence/sql/), [Marten](https://martendb.io/) and EntityFramework to ensure data consistency for [Sagas](/nservicebus/sagas/) and business data.

## Disadvantages

* No local store-and-forward mechanism; when a PostgreSQL instance is down, the endpoint cannot send nor receive messages.
* In centralized deployment scenarios, maximum throughput applies for the whole system, not individual endpoints. For example, if PostgreSQL can handle 2000 msg/s on the given hardware, each one of the 10 endpoints deployed on this machine can only receive a maximum of 200 msg/s (on average).
* When using PostgreSQL transport, a database table serves as a queue for the messages for the endpoints. These tables are polled periodically to see if messages need to be processed by the endpoints. Although the polling interval is one second, this may still lead to delays in processing a message. For environments where low latency is required, consider using other transports that use queuing technologies, such as RabbitMQ.

## Deployment considerations

### Security

Security considerations for PostgreSQL transport should follow [the principle of least privilege](https://en.wikipedia.org/wiki/Principle_of_least_privilege).

Each endpoint should use a dedicated PostgreSQL principal with `SELECT` and `DELETE` permissions on its input queue tables and `INSERT` permission on input queue tables of endpoints it sends messages to. Each endpoint should also have permissions to insert rows to audit and error queue tables.

Dedicated schemas can be used to manage fine-grained access control to various database objects used by the endpoint, including its queue tables.

### Retries

When an exception occurs during message handling, the transaction is already in doubt and must be rolled back. Failure info is stored in memory, and can't influence the next step for that message (immediate retry, delayed retry, or forward to the error queue) until the next time it is processed on that node.

When an endpoint is scaled out to multiple instances, more retries may be observed than configured, as each instance accumulates failures until a single node has observed enough retries to escalate to delayed retries. It is also possible for immediate retries to be observed on a different instance even when [Recoverability](/nservicebus/recoverability/) is configured for zero immediate retries.

## Transactions

PostgreSQL transport supports following [transaction handling modes](/transports/transactions.md): receive only, send atomic with receive, and no transactions. It does not support Transaction scope mode due to the limitations in the design of the PostgreSQL database and the System.Transactions library.

## Time-to-be-received

The SQL transport runs a periodic task that removes expired messages from the queue. The task is first executed when the endpoint starts and is subsequently scheduled to execute 5 minutes after the previous run when the task has been completed. Expired messages are not received from the queue and their disk space will be reclaimed when the periodic task executes.

> [!NOTE]
> The periodic tasks only purges expired messages for endpoint queues that for which an endpoint receives messages. If no receiving endpoint is running expired messages will not be deleted and no storage is not released.
