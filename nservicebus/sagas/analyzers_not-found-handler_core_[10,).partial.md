* **Example message**: A saga should not implement `ISagaNotFoundHandler`, as this gives access to the uninitialized saga data property. Implement `ISagaNotFoundHandler` on a separate class instead..

A [saga not found handler](/nservicebus/sagas/saga-not-found.md) provides a way to deal with messages that are not allowed to start a saga but cannot find existing saga data.

Saga not found handlers may require specific dependencies to be injected into the class and may implement non-trivial handling logic. To seperate those concerns from the saga an independent class should implement `ISagaNotFoundHandler` and be mapped accordingly.