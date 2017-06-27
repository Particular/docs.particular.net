---
title: Saga Concurrency
summary: NServiceBus gives ACID semantics, using underlying storage so only one worker thread hitting a saga instance can commit.
component: Core
tags:
- Saga
redirects:
- nservicebus/nservicebus-sagas-and-concurrency
reviewed: 2017-05-16
related:
- persistence/nhibernate/saga-concurrency
- persistence/ravendb/saga-concurrency
- persistence/sql/saga-concurrency
---

If the endpoint is configured to allow concurrent processing of messages (default) or is scaled out, it is possible that multiple messages will hit the same saga instance simultaneously. To give ACID semantics in this situation, NServiceBus uses the underlying storage to produce consistent behavior, only allowing one of messages to complete. NServiceBus handles most of this automatically but there are some caveats.

Concurrent access to saga instances is divided into two scenarios;

 * Concurrently trying to create the same instance of a new saga.
 * Concurrently trying to update the same instance of an existing saga.

Each saga persister will honor the following semantics, see the specific persister documentation for implementation details.

## Concurrent access to non-existing saga instances

Sagas are started by the message types that a handled with `IAmStartedByMessages<T>`. If messages mapped to the same saga instance are processed concurrently there is a risk that duplicates of the instance will be created.

In this case only one message is allowed to complete processing. The others roll back and the built-in retries in NServiceBus kick in. On the next retry, the saga instance is found, the race condition is solved, and that saga instance is updated instead, see below.

partial: unique

## Concurrent access to existing saga instances

When messages concurrently tries to update the same saga instance the storage will either detect and throw a concurrency exception or serialize access to the instance. Concurrency exceptions will be automatically resolved by the NServiceBus retries will.

Another option is to use a [transaction isolation level](https://msdn.microsoft.com/en-us/library/system.transactions.isolationlevel.aspx) of serializable but that causes [excessive locking](https://docs.microsoft.com/en-us/sql/t-sql/statements/set-transaction-isolation-level-transact-sql) with considerable performance degradation.

NOTE: While `Serializable` is the default isolation level for TransactionScopes NServiceBus Version 4 and higher will default the default isolation level to `ReadCommitted`.


### High load scenarios

Under extreme high load like batch processing, concurrent access to saga instance might lead to messages being moved to the error queue due to the NServiceBus retries being exhausted.

In that scenario consider re-designing the process.

Take a look at Jimmy Bogard's blog about [Reducing Saga load](https://lostechies.com/jimmybogard/2014/02/27/reducing-nservicebus-saga-load/)