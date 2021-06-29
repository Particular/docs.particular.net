## Sagas concurrency control

By default NServiceBus.RavenDB uses optimistic concurrency control. Pessimistic locking can be enabled with the following API:

snippet: ravendb-sagas-pessimistic-lock

NOTE: Starting with NServiceBus.RavenDB version 6.5, pessimistic locking is the default option for concurrency control.

### Pessimistic locking internals

RavenDB does not support pessimistic locking natively and the persister uses a dedicated RavenDB document to enforce exclusive saga updates. In some scenarios, this can lead to an increase in I/O roundtrips, especially if many instances are competing for the same saga. However, in high concurrency scenarios, the overhead is still smaller than the cost of message retry that would be caused by optimistic concurrency failures.

NOTE: It is recommended to choose pessimistic concurrency over optimistic concurrency whenever a saga is experiencing a high number of optimistic concurrency control errors.

### Pessimistic concurrency control settings

The pessimistic concurrency control can be customized using the options mentioned below.

### Pessimistic lease lock time

By default, the persister locks a saga data document for 60 seconds. Although it is not recommended to have sagas execute long-running logic, in some scenarios it might be required to increase the lease duration. The lease duration can be adjusted using the following API:

snippet: ravendb-sagas-setPessimisticLeaseLockTime

### Pessimistic lease lock acquisition timeout

By default, the persister waits 60 seconds to acquire the lock. The value can be adjusted using the following API:

snippet: ravendb-sagas-setPessimisticLeaseLockAcquisitionTimeout

### Pessimistic lease lock acquisition maximum refresh delay

To prevent request synchronization, the persister randomizes the interval between lock acquisition requests. By default, the interval has a value between 0 and 20 milliseconds. The 20ms default upper limit can be changed to any value between 0 and 1000 milliseconds using the following API:

snippet: ravendb-sagas-pessimisticLeaseLockAcquisitionMaximumRefreshDelay
