## Sagas pessimistic locking  (Requires version 6.4+)

Starting with version 6.4, it's possible to configure sagas persistence to pessimistic locking instead of the default optimistic concurrency control.

RavenDB does not provide pessimistic locking natively. The behavior is based a spin lock that tries to acquire a lease on a resource.

Applying a spin lock over a remote resource sounds expensive but it actually is not. When using optimistic concurrency control the recovery mechanism will result in all message processing to be performed again for each retry including the retrievel of the message from the queue.

Choose pessimistic locking over optimistic locking if the system is experiencing in general is experiencing optimistic concurrency control errors when saga instance are updated. Optimisitic concurrency control is the most efficient form of processing if the system is occasionally experiencing an optimistic concurrency control error.

snippet: ravendb-sagas-pessimistic-lock

The pessimistick locking behavior can be customized using the following options:

### Pessimistic Lease Lock Time: (Requires version 6.4+)

By default the persister locks a saga data document for 60 seconds. It is not recommended to have long running sagas but it might sometimes be required to increase the lease duration.

The lease duration can be adjusted using the following API:

snippet: ravendb-sagas-pessimisticLeaseLockAcquisitionMaximumRefreshDelay

### Pessimistic Lease Lock Acquisition Timeout (Requires version 6.4+)

By default the persister waits 60 seconds to obtain a lease lock. If the lock acquisition fails the message goes through the endpoint configured retry logic.

The behavior of obtaining a lease lock is based on competing on document for update. This can result in a large increase in IO roundtrips. Especially if many instance are competing for this resource.

The pessimistic lease lock acquisition Timeout duration can be adjusted via the following API:

snippet: ravendb-sagas-setPessimisticLeaseLockAcquisitionTimeout

### Pessimistic Lease Lock Acquisition Maximum Refresh Delay (Requires version 6.4+)

To prevent jittering the saga persister waits a random number of milliseconds between lease lock acquisition attempts. By default the random waiting time is between zero and 20 milliseconds. The upper bound can be configured: the supplied value must be greater than zero and smaller or equal to 1 second.

The pessimistic lease lock acquisition maximum refresh delay can be adjusted via the following API:

snippet: ravendb-sagas-setPessimisticLeaseLockTime
