Starting in version 4.1.1 conflicts cannot occur because the persistence uses pessimistic locking. Pessimistic locking is achieved by performing a `SELECT ... FOR UPDATE` or its dialect-specific equivalent.

Up to and including version 4.1, SQL persistence uses [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when updating or deleting saga data.

Example exception:

```
System.Exception: Optimistic concurrency violation when trying to complete saga OrderSaga 699d0b1a-e2bf-49fd-8f26-aadf01009eaf. Expected version 4.
```

include: saga-concurrency
