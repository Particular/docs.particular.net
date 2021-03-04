## Sagas concurrency control

The RavenDB persister allows for both optimistic and pessimistic concurrency options. As of NServiceBus.RavenDB version 7, he default is pessimistic concurrency.


RavenDB does not provide pessimistic locking natively. The behavior is based on a spin lock that tries to acquire a lease on a resource.

Applying a spin lock over a remote resource is not as expensive as it may sound. When using optimistic concurrency control the recovery mechanism will result in all message processing being performed again for each retry including the retrieval of the message from the queue.

The pessimistic locking behavior can be customized using the following options:

### Pessimistic Lease Lock Time:

By default, the persister locks a saga data document for 60 seconds. It is not recommended to have long-running handlers in sagas but it might sometimes be required to increase the lease duration.

The lease duration can be adjusted using the following API:

snippet: ravendb-sagas-pessimisticLeaseLockAcquisitionMaximumRefreshDelay

### Pessimistic Lease Lock Acquisition Timeout

By default the persister waits 60 seconds to obtain a lease lock. If the lock acquisition fails, the message goes through the endpoint configured [retry logic](/nservicebus/recoverability/).

The behavior of obtaining a lease lock is based on competing on the document for update. This can result in a large increase in I/O roundtrips, especially if many instances are competing for this resource.

The pessimistic lease lock acquisition timeout duration can be adjusted with the following API:

snippet: ravendb-sagas-setPessimisticLeaseLockAcquisitionTimeout

### Pessimistic Lease Lock Acquisition Maximum Refresh Delay

To prevent jittering, the saga persister waits a random number of milliseconds between lease lock acquisition attempts. By default, the random waiting time is between zero and 20 milliseconds. The upper bound can be configured: the supplied value must be greater than zero and less than or equal to 1 second.

The pessimistic lease lock acquisition maximum refresh delay can be adjusted via the following API:

snippet: ravendb-sagas-setPessimisticLeaseLockTime
