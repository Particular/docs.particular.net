---
title: Accessing and modifying data
summary: How to access business data in sync with message consumption and modifications to NServiceBus-controlled data.
component: Core
versions: '[5,)'
reviewed: 2016-10-25
related:
 - persistence/nhibernate/accessing-data
 - persistence/ravendb
tags:
 - Persistence
 - Saga
---

In most cases [handlers](/nservicebus/handlers/) are meant to modify the internal state of an application based on the received message. The scenarios below discuss in detail how NServiceBus transaction and persistence settings affect the way business data is stored.


## Without using NServiceBus persistence

The simplest way to modify application data from code running inside NServiceBus (i.e. a message handler) is by using a user-managed connection and transaction. Transport transaction mode has to be taken into account when designing such code.


### Transport in native transaction mode

When the selected transport is configured to use native transactions, either in `ReceiveOnly` or `SendsAtomicWithReceive` mode, the data access code in the handler can be executed multiple times for a single message. This can lead to data corruption if that code is not [idempotent](http://www.enterpriseintegrationpatterns.com/patterns/messaging/IdempotentReceiver.html) (ensuring side effects of message processing are the same, no matter how many times the code is invoked). Here's an example of data access code that is not idempotent:

snippet: BusinessData-Native-NotIdempotent

If the handler with the above code is invoked more than once, for example due to a transient problem with the transport while committing the receive transaction, then the data will get corrupted which might result in duplicate rows to be inserted. The following code shows how to mitigate the problem:

snippet: BusinessData-Native-Idempotent

The downside of this approach is the fact that the code gets more complex. Sometimes it is hard to get enough information from the incoming message to create correct *idempotent* handling logic. This is especially true for commands which don't have natural identity in the same way events have.


### Transport in distributed transaction mode

When the selected transport is configured to use distributed transactions via Distributed Transaction Coordinator (DTC) service, the handler is executed within an ambient transaction scope. If the data store supports enlisting in a distributed transaction (e.g. SQL Server, Oracle), the data modifications are guaranteed to be applied in a single atomic operation together with message receive operation. 


## Using NServiceBus persistence

Instead of managing of connections and transactions themselves, users can delegate the management of data store to NServiceBus persistence. This approach has a number of advantages:

 * NServiceBus guarantees best practices are followed when it comes to managing data store's connection.
 * Data access context is automatically shared between all handlers executed for a given message, making it easier to guarantee *idempotency* (no partial successes where one handler managed to commit the changes while other didn't).
 * Data access context is also shared with the [Saga](/nservicebus/sagas) that might participate in handling a given message.

The downside to this approach is that, in order to share the same data access context across business data transactions and NServiceBus internal database actions, the database technology used must be one of the NServiceBus supported persistence options. NServiceBus supports SQL Server and Oracle via NServiceBus.Persistence.Sql or NServiceBus.NHibernate persistence and RavenDB via NServiceBus.RavenDB persistence.

NOTE: There is support for accessing business data via NServiceBus Azure Storage persistence because Azure data stores support only single-entity operations.

partial: api

The documentation below provides more detail on how to share the same data access context for business data and NServiceBus, when using:

 * [Persistence.Sql](/persistence/sql/accessing-data.md)
 * [NHibernate](/persistence/nhibernate/accessing-data.md)
 * [RavenDB](/persistence/ravendb/#shared-session)


### Transport in native transaction mode

In this mode the NServiceBus-managed data store context can be committed multiple times for a single message and it is up to the user to guarantee *idempotency*. The difference between user-managed connections, though, is the fact that data store context is shared between the handlers so there might be one (possibly generic) handler that takes care of the *idempotency*, allowing others to focus on pure business problem.

snippet: BusinessData-Native-Managed


### Outbox

partial: outbox
