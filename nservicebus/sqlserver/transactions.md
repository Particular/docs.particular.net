---
title: Transactions and delivery guarantees
summary: Transactions and delivery guarantees in SQL Server Transport.
tags:
- SQL Server
---

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


