### Pessimistic concurrency

Version 3.3.0 and above offer the option for pessimistic concurrency control in the saga persister.

To configure pessimistic mode, configure the concurrency mode in the `NonDurableSagaOptions`. The pessimistic lock timeout (defaults to 30 seconds) can also be adjusted if necessary.

snippet: ConfiguringNonDurablePessimisticSagaLocking

When one message handler holds a lock on a saga data instance, another message handler will wait asynchronously until the lock is freed or until the `PessimisticLockTimeout` is reached, at which point a `NonDurableSagaLockTimeoutException` will be thrown and the message will be eligible for retry.

Even when pessimistic locking is enabled, the optimistic concurrency version will still be checked before updated data is committed. Pessimistic locks occur only on data reads and do not affect new saga creation, where the optimistic concurrency check still prevents duplicate saga instances from being created.

The saga locking mode is selected when a saga is created and persists for the life of that saga instance.