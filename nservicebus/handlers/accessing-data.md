---
title: Accessing and modifying data
summary: How to access business data in sync with message consumption and modifications to NServiceBus-controlled data.
component: Core
versions: '[5,)'
related:
- nservicebus/nhibernate/accessing-data
- nservicebus/ravendb
tags:
 - NHibernate
 - RavenDB
 - Persistence
 - Data access
 - Saga
---

In most cases [handlers](/nservicebus/handlers/) are meant to modify the internal state of an application based on the received message. There are multiple ways to do this


## Without using NServiceBus persistence

The simplest way to modify the state of the application is to execute the data access code in the handler without using NServiceBus persistence features. Transport transaction mode has to be taken into account when designing such code.


### Transport using native transactions

When the selected transport is configured to use native transactions, either in `ReceiveOnly` or `SendsAtomicWithReceive` mode, the data access code in the handler can be executed multiple times for a single message. This can lead to data corruption if that code is not *idempotent* (which is a fancy word for ensuring side effects of the code are the same, no matter how many times the code is invoked). Here's an example of data access code that is not idempotent:

snippet:BusinessData-Native-NotIdempotent

Should the handler be invoked more than once (e.g. because of a transient problem with the transport while committing the receive transaction) the data will get corrupted (duplicate rows will be inserted). The following code shows how to mitigate the problem:

snippet:BusinessData-Native-Idempotent

The downside of this approach is the fact that the code gets more complex. Sometimes it is hard to get enough information from the incoming message to create correct *idempotent* handling logic. This is especially true for commands. 

### Transport using distributed transactions

When the selected transport is configured to use distributed transactions via Distributed Transaction Coordinator (DTC) service, the handler is executed within an ambient transaction scope. If the data store supports enlisting in a distributed transaction (e.g. SQL Server, Oracle), the state changes are guaranteed to be applied successfully exactly once, atomically with the message receive operation.


## Using NServiceBus persistence

Instead of managing everything themselves, users can delegate the management of data store to NServiceBus persistence. This approach has a number of advantages:
 * NServiceBus guarantees best practices are followed when it comes to managing data store's connection
 * Data access context is automatically shared between all handlers executed for a given message, making it easier to guarantee *idempotency* (no partial successes where one handler managed to commit the changes while other didn't)
 * Data access context is also shared with the [Saga](/nservicebus/sagas) that might participate in handling a given message

The limitation of this approach is that it requires NServiceBus support for a given data store. Out of the box supported stores include SQL Server and Oracle (via NServiceBus.NHibernate persistence) and RavenDB. 

partial:api

Consult the documentation of a particular persistence for details.

 * [NHibernate](/nservicebus/nhibernate/accessing-data.md)
 * [RavenDB](/nservicebus/ravendb/#shared-session)

### Transport using native transactions

In this mode the NServiceBus-managed data store context can be committed multiple times for a single message and it is up to the user to guarantee *idempotency*. Unlike in the basic example, though, the data store context is shared between the handlers so there might be one (possibly generic) handler that takes care of the *idempotency*, allowing others to focus on pure business problem.

snippet:BusinessData-Native-Managed


### Outbox

When using the [Outbox](/nservicebus/outbox) NServiceBus itself guarantees the *idempotency* of data access operations executed via `SynchronizedStorageSession`. The Outbox can be seen as a generic implementation of the *idempotency*-ensuring handler from the previous section.
