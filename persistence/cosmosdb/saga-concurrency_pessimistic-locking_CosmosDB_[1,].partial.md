## Sagas concurrency control

By default NServiceBus.CosmosDB uses optimistic concurrency control. Pessimistic locking can be enabled with the following API:

snippet: UsePessimisticLocking

### Pessimistic locking internals

CosmosDB does not support pessimistic locking natively. The behavior is based on a spin lock that tries to acquire a lease on a resource by performing `Container.PatchItemStreamAsync` method.

> [!NOTE]
> It is recommended to choose pessimistic concurrency over optimistic concurrency whenever a saga is experiencing a high number of optimistic concurrency control errors.

> [!NOTE]
> When using pessimistic locking with provisioned throughput it is important to understand the additional patch operation attempts that are issued during the saga loading attempt will lead to higher RU usage. It is important to set the lease lock acquisition minimum and maximum refresh delay according in alignment with the saga contention scenarios to avoid using too much unnecessary RUs.

### Pessimistic concurrency control settings

The pessimistic locking behavior can be customized using the following options:

### Pessimistic lease lock duration

By default, the persister locks a saga data document for 60 seconds. Although it is not recommended to have sagas execute long-running logic, in some scenarios it might be required to increase the lease duration. The lease duration can be adjusted using the following API:

snippet: PessimisticLeaseLockDuration

### Pessimistic lease lock acquisition timeout

By default, the persister waits 60 seconds to acquire the lock. The value can be adjusted using the following API:

snippet: PessimisticLeaseLockAcquisitionTimeout

### Pessimistic lease lock acquisition minimum and maximum refresh delay

To prevent request synchronization, the persister randomizes the interval between lock acquisition requests. By default, the interval has a value between 500 and 1000 milliseconds. These values can be adjusted using the following API:

snippet: PessimisticLeaseLockAcquisitionMinMaxRefreshDelay