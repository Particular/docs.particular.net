---
title: Business data using NHibernate
summary: How to access business data in sync with message consumption and modifications to NServiceBus-controlled data.
tags:
 - NHibernate
 - Saga
---

Most of the time message handlers are meant to modify the internal state of you application. This usually boils down to accessing some kind of a data store. Since in NServiceBus messages are durable (unless transactions are explicitly disabled in very rare cases), there are two possible synchronization options between consuming messages and accessing the data:

 * have an illusion of *exactly-once* message processing either through a distributed transaction or the [outbox](/nservicebus/outbox/)
 * implement data access code in an *idempotent* way

The second is much simpler in theory but much harder in practice. In theory all you need to do is create your own connection in the handler and execute the *idempotent* data access code. The practice shows that making that code idempotent is a non-trivial task. That is why NServiceBus offers APIs that allow you to work on top of *exactly-once* processing illusion. We'll focus on these APIs.


## Accessing data in the handler

The simplest way to access the data in an *exactly-once* way is to just lean on the Distributed Transaction Coordinator (DTC) to make sure all the data access, happening while handling the message, is atomic. This approach has two downsides. First, the throughput when using DTC is much smaller than without it. Second, DTC is not a trivial service to configure, not mentioning making it Highly Available (HA) via a cluster. There is one upside though. For the application data you can use any data store that supports the DTC-coordinated transaction.

NServiceBus persistence APIs offer a solution to these problems but limits the data storage choices. The NHibernate persistence allows you too 'hook-up' to the data context used by NServiceBus to ensure atomic changes.

snippet:NHibernateAccessingDataViaContext

As shown above, `NHibernateStorageContext` can be used directly to access NHibernate `ISession`. 

Note that prior to Version 6 `ISession` could be injected directly into the handlers. This behavior has changed in NServiceBus Version 6 as internal components of NServiceBus are no longer accessible from the IoC container. This approach can still be used on Version 5 and before:

snippet:NHibernateAccessingDataDirectlyConfig

snippet:NHibernateAccessingDataDirectly

The first part tell NServiceBus to inject the `ISession` instance into the handlers and the second part uses standard Property Injection to access the `ISession` object.

Regardless of how you access the `ISession` object, it is fully managed by NServiceBus according to the best practices defined by NServiceBus documentation with regards to transactions.


## Customizing the session

Prior to Version 6 you could customize how the `ISession` object is instantiated. If you needed some special behavior in the `ISession` object managed by NServiceBus, you can hook-up to the session creation process by providing your own delegate.

snippet:CustomSessionCreation

NOTE: Customizing the way session is opened works only for the 'shared' session that is used to access business/user, Saga and Outbox data. It does not work for other persistence concerns such as Timeouts or Subscriptions. Also note that this is no longer possible in NServiceBus Version 6.


## Known limitations

Because of the way NServiceBus opens sessions by passing an existing instance of a database connection, it is currently not possible to use NHibernate's second-level cache. Such behavior of NServiceBus is caused by still-unresolved [bug](https://nhibernate.jira.com/browse/NH-3023) in NHibernate.
