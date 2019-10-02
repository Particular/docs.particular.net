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


The RavenDB persister applies [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when saga instance are updated. It does not support locking. If optimistic concurrency control conflicts occur the message needs to be reprocessed via [recoverability](/nservicebus/recoverability/).

NOTE: When the state of the saga instance is unchanged after the handler completes the document will not be written back to storage. If a consistency check is required when the transaction commits a property value must manually be updated. For example, an incrementing counter property to force the RavenDB client to store the updated document.

Please read the guidance on [saga concurrency](/nservicebus/sagas/concurrency.md) on potential improvements.


### Concurrent access to non-existing saga instances

RavenDB lacks support for unique indexes, the persister mimics this by creating a separate document for each saga instance to serve this purpose. The key of this document is set to the value of the correlation property, this causes RavenDB to throw an exception should two documents with the same correlation value be created at the same time.

When this happens the following error is logged:
```
Raven.Client.Exceptions.ConcurrencyException: Document OrderSagaData/OrderId/316414b3-07f1-40ec-00db-022a4140d517 has change vector A:2-u2LvKAFZTE+972x2hp1gTg, but Put was called with expecting new document. Optimistic concurrency violation, transaction will be aborted.
```


### Concurrent access to existing saga instances

The persister makes use of RavenDB optimistic locking causing concurrency exceptions to be throw should the same saga instance be updated concurrently.

When this happens the following error is logged:
```
Raven.Client.Exceptions.ConcurrencyException: Document OrderSagaDatas/f23921c9-7b53-455d-89be-aad200d98741 has change vector A:93-u2LvKAFZTE+972x2hp1gTg, but Put was called with change vector A:90-u2LvKAFZTE+972x2hp1gTg. Optimistic concurrency violation, transaction will be aborted.
```

include: saga-concurrency
