* **Example message**: A saga should not implement `IHandleSagaNotFound`, as this catch-all handler will handle messages where *any* saga is not found. Implement `IHandleSagaNotFound` on a separate class instead.

A [saga not found handler](/nservicebus/sagas/saga-not-found.md) provides a way to deal with messages that are not allowed to start a saga but cannot find existing saga data.

Saga not found handlers operate on all saga messages within an endpoint, no matter which saga the message was originally bound for. So it is misleading to implement `IHandleSagaNotFound` on a saga because it creates the impression that it will only handle not found messages for that _specific_ saga, which is false.

Instead, implement `IHandleSagaNotFound` on an independent class.