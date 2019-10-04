---
title: RavenDB Persistence Saga concurrency
reviewed: 2019-06-10
related:
 - nservicebus/sagas/concurrency
redirects:
 - nservicebus/ravendb/saga-concurrency
---

include: dtc-warning

RavenDB honors [concurrency semantics](/nservicebus/sagas/concurrency.md) in the following way.


## Default concurrency behavior

RavenDB persistence uses [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when accessing saga data. See below for examples of the exceptions thrown when conflicts occur. More information about these scenarios is available in _[saga concurrency](/nservicebus/sagas/concurrency.md)_, including guidance on how to reduce the number of conflicts.

WARNING: When a message handler does not change saga data, the RavenDB client will not attempt to write the associated document to storage. If a consistency check is required, a property value must be changed. For example, a counter property may be incremented.

### Creating saga data

```
Raven.Client.Exceptions.ConcurrencyException: Document OrderSagaData/OrderId/316414b3-07f1-40ec-00db-022a4140d517 has change vector A:2-u2LvKAFZTE+972x2hp1gTg, but Put was called with expecting new document. Optimistic concurrency violation, transaction will be aborted.
```


### Updating or deleting saga data

```
Raven.Client.Exceptions.ConcurrencyException: Document OrderSagaDatas/f23921c9-7b53-455d-89be-aad200d98741 has change vector A:93-u2LvKAFZTE+972x2hp1gTg, but Put was called with change vector A:90-u2LvKAFZTE+972x2hp1gTg. Optimistic concurrency violation, transaction will be aborted.
```

include: saga-concurrency
