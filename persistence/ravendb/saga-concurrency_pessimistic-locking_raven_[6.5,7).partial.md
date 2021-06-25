## Sagas concurrency control

By default NServiceBus.RavenDB uses optimistic concurrency control. Pessimistic locking can be enabled using: 

snippet: ravendb-sagas-pessimistic-lock

NOTE: Pessimistic locking will become the default option starting with NServiceBus.RavenDB version 6.5

### Pessimistic locking internals

RavenDB does not support pessimistic locking natively and the persister uses a dedicated RavenDB document to enforce exclusive saga updates. In some scenarios, this can lead to an increase in I/O roundtrips, especially if many instances are competing for the same saga. However, in the high concurrency scenarios, the overhead is still smaller than the cost of message retry that would be caused by optimistic concurrency failures.   

NOTE: As a result, it is recommended to choose the pessimistic over the optimistic concurrency control whenever a saga is experiencing a significant number of optimistic concurrency control errors. 

### Pessimistic concurrency control settings

The pessimistic concurrency control can be customized using the options mentioned in the sections below.

### Pessimistic Lease Lock Time

By default, the persister locks a saga data document for 60 seconds. Although, it is not recommended to have sagas execute long-running logic, in some scenarios it might be required to increase the lease duration. The lease duration can be adjusted using the following API:

snippet: ravendb-sagas-setPessimisticLeaseLockTime

### Pessimistic Lease Lock Acquisition Timeout

By default, the persister waits 60 seconds to acquire the lock. The value can be adjusted using:

snippet: ravendb-sagas-setPessimisticLeaseLockAcquisitionTimeout

### Pessimistic Lease Lock Acquisition Maximum Refresh Delay

To prevent request synchronization, the persister randomizes the interval between lock acquisition requests. By default, the interval can have a value between 0 and 20 milliseconds. The default, 20 ms upper limit can be changed to any value between 0 and 1000 milliseconds using:

snippet: ravendb-sagas-pessimisticLeaseLockAcquisitionMaximumRefreshDelay
