---
title: Concurrency
summary: NServiceBus gives ACID semantics, using underlying storage so only one worker thread hitting a saga instance can commit.
component: Core
tags:
- Saga
redirects:
- nservicebus/nservicebus-sagas-and-concurrency
---

If the endpoint runs with more than one worker thread, it is possible that multiple messages will hit the same saga instance simultaneously. To give ACID semantics in this situation, NServiceBus uses the underlying storage to produce consistent behavior, only allowing one of the threads to commit. NServiceBus handles most of this automatically but there are some caveats.

Concurrent access to saga instances is divided into two scenarios;

 * When there is no existing saga instance and multiple threads try to create a new instance of what should be the same saga instance.
 * Where a saga instance already exists in storage and multiple threads try to update that same instance.


## Concurrent access to non-existing saga instances

Sagas are started by the message types that a handled with `IAmStartedByMessages<T>`. If more than one are processed concurrently and are mapped to the same saga instance there is a risk that more than one thread tries to create a new saga instance.

In this case only one thread is allowed to commit. The others roll back and the built-in retries in NServiceBus kick in. On the next retry, the saga instance is found, the race condition is solved, and that saga instance is updated instead. Of course this can result in concurrency problems but they are solved, as mentioned below.

NServiceBus solves this by automatically creating a unique constraint in the database for the correlation property. With this constraint in place, only one thread is allowed to create a new saga instance.

partial: unique


## Concurrent access to existing saga instances

This works predictably due to reliance on the underlying database providing optimistic concurrency support. When more than one thread tries to update the same saga instance, the database detects it and only allows one of them to commit. If this happens the retries will occur and the race condition be solved.

When using the RavenDB saga persister, no action is required since the NServiceBus framework (on RavenDB) turns on [UseOptimisticConcurrency](https://ravendb.net/docs/search/latest/csharp?searchTerm=how-to%20enable-optimistic-concurrency).

When running using the NHibernate saga persister, the NServiceBus framework requires the addition of a ["Version" property to the saga data](https://ayende.com/blog/3946/nhibernate-mapping-concurrency) so that NHibernate can work its magic.

NServiceBus Version 4 makes this even easier by enabling the optimistic-all option if no Version property is found.

Another option is to use a [transaction isolation level](https://msdn.microsoft.com/en-us/library/system.transactions.isolationlevel.aspx) of serializable but that causes [excessive locking](https://msdn.microsoft.com/en-us/library/ms173763.aspx) with considerable performance degradation.

NOTE: "Serializable" is the default isolation level for TransactionScopes.

In NServiceBus Version 4 the default isolation level is
"ReadCommitted", which is a more sensible default.


### High load scenarios

Under extreme high load like batch processing, trying to access the same Saga's data could lead to a situation where messages ends up in the error queue even though both first and second level retries are enabled.

In that scenario consider re-designing the process.

Take a look at Jimmy Bogard's blog about [Reducing Saga load](https://lostechies.com/jimmybogard/2014/02/27/reducing-nservicebus-saga-load/)
