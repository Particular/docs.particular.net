---
title: Sagas And Concurrency
summary: NServiceBus gives you ACID semantics, using underlying storage so only one worker thread hitting a saga instance can commit.
tags:
- Saga
redirects:
- nservicebus/nservicebus-sagas-and-concurrency
---

If your endpoint runs with more than one worker thread, it is possible that multiple messages will hit the same saga instance simultaneously. To give you ACID semantics in this situation, NServiceBus uses the underlying storage to produce consistent behavior, only allowing one of the threads to commit. NServiceBus handles most of this automatically but you should be aware of a few things.

Concurrent access to saga instances is divided into two scenarios;

-   When there is no existing saga instance and multiple threads try to create a new instance of what should be the same saga instance.
-   Where a saga instance already exists in storage and multiple threads try to update that same instance.

Let's look at both scenarios in detail and see the options.


## Concurrent access to non-existing saga instances

Sagas are started by the message types you handle as `IAmStartedByMessages<T>`. If more than one are processed concurrently and are mapped to the same saga instance there is a risk that more than one thread tries to create a new saga instance.

In this case only one thread is allowed to commit. The others roll back and the built-in retries in NServiceBus kick in. On the next retry, the saga instance is found, the race condition is solved, and that saga instance is updated instead. Of course this can result in concurrency problems but they are solved, as mentioned below.

NServiceBus solves this by automatically creating a unique constraint in the database for the property on which you are correlating. With this constraint in place, only one thread is allowed to create a new saga instance.

NOTE: In NServiceBus Version 2 you had to create the constraint yourself in the selected data store. Version 3 to 5 provided a `[Unique]` attribute. When you put that attribute on one of your saga data properties, NServiceBus creates the constraint for you. This works for both the NHibernate and the RavenDB saga persister.


## Concurrent access to existing saga instances

This works predictably due to reliance on the underlying database providing optimistic concurrency support. When more than one thread tries to update the same saga instance, the database detects it and only allows one of them to commit. If this happens the retries will occur and the race condition be solved.

### SQL Server
When running using NHibernate persistance, the NServiceBus framework enables pessimistic locking, resulting in saga data being locked for a single thread. Adding ["Version" property to the saga data](http://docs.particular.net/nservicebus/nhibernate/saga-concurrency) gives NHibernate the option to work its magic and gain additional performance, especially in high volume messaging environments.

Another option is to use a [transaction isolation level](https://msdn.microsoft.com/en-us/library/system.transactions.isolationlevel.aspx) of serializable but that can cause [excessive locking](https://msdn.microsoft.com/en-us/library/ms173763.aspx) with considerable performance degradation, if not handled with care. Since NServiceBus 4 the default isolation level is "ReadCommitted".

NOTE: "Serializable" is the default isolation level for distributed transactions.

### RavenDb
When you use the RavenDB saga persister, you don't have to do anything since the NServiceBus framework (on RavenDB) turns on [UseOptimisticConcurrency](http://ravendb.net/docs/search/latest/csharp?searchTerm=how-to%20enable-optimistic-concurrency).

### High load scenarios

Under extreme high load like batch processing, trying to access the same Saga's data could lead to a situation where messages ends up in the error queue even though you have both first and second level retries enabled.

In that scenario you may need to look at re-designing the process.

Take a look at Jimmy Bogard's blog about [Reducing Saga load](https://lostechies.com/jimmybogard/2014/02/27/reducing-nservicebus-saga-load/)
