---
title: NServiceBus Sagas And Concurrency
summary: NServiceBus gives you ACID semantics, using underlying storage so only one worker thread hitting a saga instance can commit.
tags:
- Sagas
---

If your endpoint runs with more than one worker thread, it is possible that multiple messages will hit the same saga instance simultaneously. To give you ACID semantics in this situation, NServiceBus uses the underlying storage to produce consistent behavior, only allowing one of the threads to commit. NServiceBus handles most of this automatically but you should be aware of a few things.

Concurrent access to saga instances is divided into two scenarios;

-   When there is no existing saga instance and multiple threads try to create a new instance of what should be the same saga instance.
-   Where a saga instance already exists in storage and multiple threads try to update that same instance.

Let's look at both scenarios in detail and see your options.

## Concurrent access to non-existing saga instances

Sagas are started by the message types you handle as `IAmStartedByMessages<T>`. If more than one are processed concurrently and are mapped to the same saga instance there is a risk that more than one thread tries to create a new saga instance.

In this case only one thread is allowed to commit. The others roll back and the built-in retries in NServiceBus kick in. On the next retry, the saga instance is found, the race condition is solved, and that saga instance is updated instead. Of course this can result in concurrency problems but they are solved, as mentioned below.

NServiceBus solves this by requiring you to create a unique constraint in your database for the property on which you are correlating.

In NServiceBus V2.X you had to create the constraint yourself but V3.X has the [Unique] attribute. When you put that attribute on one of your saga data properties, NServiceBus creates the constraint for you. This works for both the NHibernate and the RavenDB saga persister.

With this constraint in place, only one thread is allowed to create a new saga instance.

## Concurrent access to existing saga instances

This works predictably due to reliance on the underlying database providing optimistic concurrency support. When more than one thread tries to update the same saga instance, the database detects it and only allows one of them to commit. If this happens the retries will occur and the race condition be solved.

When you use the RavenDB saga persister, you don't have to do anything since the NServiceBus framework (on RavenDB) turns on [optimistic concurrency](http://ravendb.net/kb/16/using-optimistic-concurrency-in-real-world-scenarios).

When running using the NHibernate saga persister, the NServiceBus framework requires you to add a ["Version" property to your saga data](http://ayende.com/blog/3946/nhibernate-mapping-concurrency) so that NHibernate can work its magic.

NServiceBus V4.0 makes this even easier by enabling the optimistic-all option if no Version property is found.

Another option is to use a [transaction isolation level](http://msdn.microsoft.com/en-us/library/system.transactions.isolationlevel.aspx) of serializable but that causes [excessive locking](http://msdn.microsoft.com/en-us/library/ms173763.aspx)so the performance degradation is considerable.

NOTE: "Serializable" is the default isolation level for TransactionScopes.

In NServiceBus V4.0 the default isolation level is
"ReadCommitted", which is a more sensible default.
