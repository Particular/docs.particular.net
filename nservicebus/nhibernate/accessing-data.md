---
title: Accessing Business data
summary: How to access business data in sync with message consumption and modifications to NServiceBus-controlled data.
reviewed: 2016-03-15
tags:
 - NHibernate
 - Saga
---

Regularly [handlers](/nservicebus/handlers/) are meant to modify the internal state of an application. This requires accessing some kind of a data store. Since in NServiceBus messages are durable (unless transactions are explicitly disabled in very rare cases), there are two possible synchronization options between consuming messages and accessing the data:

 * Have an illusion of *exactly-once* message processing either through a distributed transaction or the [outbox](/nservicebus/outbox/).
 * Implement data access code in an *idempotent* way.

The second is much simpler in theory but much harder in practice. In theory all that is required is to create a connection in the handler and execute the *idempotent* data access code. The practice shows that making that code idempotent is a non-trivial task. That is why NServiceBus offers APIs that provide the ability to work on top of *exactly-once* processing illusion.


## Accessing data in the handler

To access the data in an *exactly-once* way is to just lean on the Distributed Transaction Coordinator (DTC) to make sure all the data access, happening while handling the message, is atomic. This approach has two downsides:

 1. The throughput when using DTC is much smaller than without it.
 1. Second, DTC is complex to configure, not mentioning making it Highly Available (HA) via a cluster.

The upside is that the application may use any data store that supports the DTC-coordinated transaction.

NServiceBus [persistence](/nservicebus/persistence/) offers a solution to these problems but limits the data storage choices. The NHibernate persistence supports 'hooking-up' to the data context used by NServiceBus to ensure atomic changes.

snippet:NHibernateAccessingDataViaContext

As shown above, `NHibernateStorageContext` can be used directly to access NHibernate `ISession`.

partial:direct-access

Regardless of how the `ISession` object is accessed, it is fully managed by NServiceBus according to the best practices defined by NServiceBus documentation with regards to transactions.

partial:customizing-session


## Known limitations

Due of the way NServiceBus opens sessions, by passing an existing instance of a database connection, it is currently not possible to use NHibernate's second-level cache. Such behavior of NServiceBus is caused by [still-unresolved bug in NHibernate](https://nhibernate.jira.com/browse/NH-3023).