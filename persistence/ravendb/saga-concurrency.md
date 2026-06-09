---
title: RavenDB Persistence Saga Concurrency
summary: Explains how concurrency works with sagas in the RavenDB persister
component: raven
reviewed: 2026-06-09
related:
 - nservicebus/sagas/concurrency
redirects:
 - nservicebus/ravendb/saga-concurrency
---

include: cluster-configuration-info

## Default behavior

When simultaneously handling messages, conflicts may occur. See below for examples of the exceptions which are thrown. _[Saga concurrency](/nservicebus/sagas/concurrency.md)_ explains how these conflicts are handled, and contains guidance for high-load scenarios.

### Creating saga data

Example exception:

```
Raven.Client.Exceptions.ConcurrencyException: Document OrderSagaData/OrderId/316414b3-07f1-40ec-00db-022a4140d517 has change vector A:2-u2LvKAFZTE+972x2hp1gTg, but Put was called with expecting new document. Optimistic concurrency violation, transaction will be aborted.
```

### Updating or deleting saga data

By default, RavenDB persistence uses [pessimistic concurrency control](https://en.wikipedia.org/wiki/Pessimistic_concurrency_control) when updating or deleting saga data.

> [!WARNING]
> When a message handler does not change saga data, the RavenDB client will not attempt to write the associated document to storage. If a consistency check is required, a property value must be changed. For example, a counter property may be incremented.

Example exception:

```
Raven.Client.Exceptions.ConcurrencyException: Document OrderSagaDatas/f23921c9-7b53-455d-89be-aad200d98741 has change vector A:93-u2LvKAFZTE+972x2hp1gTg, but Put was called with change vector A:90-u2LvKAFZTE+972x2hp1gTg. Optimistic concurrency violation, transaction will be aborted.
```

include: saga-concurrency

## Sagas concurrency control

The RavenDB persister allows for both optimistic and pessimistic concurrency options. The default is pessimistic concurrency. To enable optimistic concurrency use:

snippet: ravendb-persistence-optimistic-concurrency

RavenDB does not provide pessimistic locking natively. The behavior is based on a spin lock that tries to acquire a lease on a resource.

Applying a spin lock over a remote resource is not as expensive as it may sound. When using optimistic concurrency control the recovery mechanism will result in all message processing being performed again for each retry including the retrieval of the message from the queue.

The pessimistic locking behavior can be customized using the following options:

### Pessimistic Lease Lock Time

By default, the persister locks a saga data document for 60 seconds. It is not recommended to have long-running handlers in sagas but it might sometimes be required to increase the lease duration.

The lease duration can be adjusted using the following API:

snippet: ravendb-sagas-setPessimisticLeaseLockTime

### Pessimistic Lease Lock Acquisition Timeout

By default, the persister waits 60 seconds to obtain a lease lock. If the lock acquisition fails, the message goes through the endpoint configured [retry logic](/nservicebus/recoverability/).

The behavior of obtaining a lease lock is based on competing on the document for update. This can result in a large increase in I/O roundtrips, especially if many instances are competing for this resource.

The pessimistic lease lock acquisition timeout duration can be adjusted with the following API:

snippet: ravendb-sagas-setPessimisticLeaseLockAcquisitionTimeout

### Pessimistic Lease Lock Acquisition Maximum Refresh Delay

To prevent jittering, the saga persister waits a random number of milliseconds between lease lock acquisition attempts. By default, the random waiting time is between zero and 20 milliseconds. The upper bound can be configured: the supplied value must be greater than zero and less than or equal to 1 second.

The pessimistic lease lock acquisition maximum refresh delay can be adjusted via the following API:

snippet: ravendb-sagas-pessimisticLeaseLockAcquisitionMaximumRefreshDelay
