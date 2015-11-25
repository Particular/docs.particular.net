---
title: SQL Server Transport usage scenarios
summary: The usage scenarios for SQLServer transport
tags:
- SQL Server
---

## Persistence

In theory there is nothing that prevents usage of RavenDB persistence in conjunction with the SQL Server transport but the most common and natural scenario is combination of SQL Server transport and NHibernate persistence. The rest of the article is based on this assumption.


## Transactions & delivery guarantees


### No transactions

The message is received and permanently deleted from the queue table without beginning an explicit transaction. This means it cannot be retried should something go wrong while processing it. Any messages sent as a result of handling the received message are delivered to their destination queues immediately. Should a failure happen between sending one message and another, the first one will be successfully delivered (*partial sends*). The business data updates that happen as part of handler execution are executed in whatever transaction context the user provided, unrelated to the sends. The saga state updates are done on the same connection as the receive but are not related to the receive or sends by means of transactions.


### Native transactions

Because of the limitations of NHibernate connection management infrastructure, there is now was to provide *exactly-once* message processing guarantees solely by means of sharing instances of `SqlConnection` and `SqlTransaction` between the transport and NHibernate. For that reason NServiceBus does not allow that configuration and throws an exception during at start-up. 

Fortunately the [Outbox](/nservicebus/outbox/) feature can be used to mitigate that problem. In such scenario the messages are stored in the same physical store as saga and user data and dispatched after the whole processing is finished. NHibernate persistence detects the status of Outbox and the presence of SQLServer transport and automatically stops reusing the transport connection and transaction. All the data access is done within the Outbox ambient transaction. From the perspective of a particular endpoint this is *exactly-once* processing because of the deduplication that happens on the incoming queue. From a global point of view this is *at-least-once* since on the wire messages can get duplicated.

A sample covering this mode of operation is available [here](/samples/outbox/sqltransport-nhpersistence/).


### Ambient transactions

In this mode the ambient transaction is started before receiving of the message and encompasses the whole processing process including user data access and saga data access. If all the logical data stores (transport, user data, saga data) use the same physical store there is no Distributed Transaction Coordinator (DTC) escalation. 

snippet:OutboxSqlServerConnectionStrings

A sample covering this mode of operation is available [here](/samples/sqltransport-nhpersistence/).


## Scenarios


### Messaging framework for the enterprise

The SQL Server transport throughput is on par with the Microsoft Message Queueing (MSMQ) service. The actual value is very much dependent on the hardware and software configuration but is in the range few thousands messages per second per endpoint instance. If the enterprise has a solid high-performance SQL Server infrastructure, taking advantage of that infrastructure to implement messaging backbone is natural choice when moving from isolated applications to connected services. In the enterprise scenario there are probably multiple systems and these systems have their own databases. SQL Server transport [multi database](multiple-databases.md) has been designed exactly for such scenarios. It can be used in two transaction modes
 * **DTC** The receive operation, all data access and all the send operations potentially targeting different databases are conducted in a single distributed transaction coordinated by the DTC. This mode requires DTC service to be configured properly (including the high availability configuration matching the one of the SQL Server). The downside is that if a destination database is brought down for maintenance, the transactions that try to send messages to it are going to fail and this might cause the domino effect in other systems.
 * **Outbox** The *exactly-once* delivery guarantee is provided by the outbox. No DTC service is required. Messages addressed to a database that is temporarily down will by retried by means of [Second-Level Retries](/nservicebus/errors/automatic-retries.md) so SLR timeout larger than maintenance windows should be configured. The downside is that all the systems need to have Outbox enabled which requires them to use NServiceBus 5.x or later.


### Background worker replacement

SQL Server transport is ideal for small web applications that grew to require reliable background processing capabilities. In this case the web app, instead of firing a background thread or doing `asyc` processing, would send a message on the bus. The message would be picked up by the backend service, processed and the response returned. SQL Server transport will work out-of-the-box in that scenario without requiring DTC.


### Pilot project

The SQL Server transport is ideal for a pilot project which goal is to prove feasibility of of NServiceBus in a given organization. It does not require anything more than a database. Such a project would usually be small so it is not likely to use more than one physical database in which case no DTC is required.


### Messaging framework for an application using SQL Azure

The SQL Azure's limitations include no support distributed transactions and relatively small database size. This makes it an ideal candidate for a multi-database mode SQL Server transport with outbox. Each endpoint can have its own database that fits in the limit and outbox guarantees *exactly-once* processing of messages.
