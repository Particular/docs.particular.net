SQL persistence uses [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when updating or deleting saga data.

Example exception:

```
System.Exception: Optimistic concurrency violation when trying to complete saga OrderSaga 699d0b1a-e2bf-49fd-8f26-aadf01009eaf. Expected version 4.
```

include: saga-concurrency