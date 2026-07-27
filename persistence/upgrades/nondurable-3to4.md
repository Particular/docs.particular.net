---
title: Non-Durable Persistence Upgrade from 3 to 4
summary: Instructions on how to upgrade NServiceBus.Persistence.NonDurable from version 3 to version 4
component: NonDurablePersistence
related:
- persistence/non-durable
reviewed: 2026-07-24
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 11
---

## Saga finders using `GetSagaData`

In NServiceBus.Persistence.NonDurable version 3.3.0, the addition of pessimistic locking for saga concurrency required the method used by saga finders to locate sagas by a predicate to transition from a synchronous method to an asynchronous method, and was renamed from `GetSataData` to `FindSagaData`.

Prior to version 3.3.0, a saga finder would use the storage session's synchronous `GetSagaData` method to find a saga:

```csharp
public class OrderSagaFinder : ISagaFinder<OrderSagaData, CompleteOrder>
{
    public Task<OrderSagaData> FindBy(CompleteOrder message, ISynchronizedStorageSession session, IReadOnlyContextBag context, CancellationToken cancellationToken = default)
    {
        var nonDurableSession = session.NonDurablePersistenceSession();

        // Synchronous method
        var sagaData = nonDurableSession.GetSagaData<OrderSagaData>(context,
            data => data.OrderId == message.OrderId,
            cancellationToken);

        // No awaiting, so return the result as a Task<OrderSagaData>
        return Task.FromResult(sagaData);
    }
}
```

To update the saga finder above:

1. Mark the `FindBy` method as `async`.
2. Replace the `GetSagaData` method with `FindSagaData` and await the result.
3. Return the saga data directly instead of wrapping in `Task.FromResult<T>(…)`.

The resulting method looks like this:

```csharp
public class OrderSagaFinder : ISagaFinder<OrderSagaData, CompleteOrder>
{
    // Method now async
    public async Task<OrderSagaData> FindBy(CompleteOrder message, ISynchronizedStorageSession session, IReadOnlyContextBag context, CancellationToken cancellationToken = default)
    {
        var nonDurableSession = session.NonDurablePersistenceSession();

        // Await the new async FindSagaData method
        var sagaData = await nonDurableSession.FindSagaData<OrderSagaData>(context,
            data => data.OrderId == message.OrderId,
            cancellationToken);

        // Within an async method, the result can be returned directly
        return sagaData;
    }
}
```

This upgrade can be made in version 3.3.0 and above to avoid the sync-over-async antipattern used by the implementation of `GetSagaData` that calls `FindSagaData(…).GetAwaiter().GetResult()`.

In version 4.0, `GetSagaData` becomes a build error, and the API is removed in version 5.0.