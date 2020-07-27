## Sagas pessimistic lock

Starting with version 6.4, it's possible to configure sagas persistence to use pessimistic locking instead of the default optimisitc concurrency control.

snippet: sagas-pessimistic-lock

The pessimistick locking behavior can be customized using the following options:

* `SetPessimisticLeaseLockTime(TimeSpan)`: Sets the lease lock time, by default the persister locks a saga data document for 60 seconds.
* `SetPessimisticLeaseLockAcquisitionTimeout(TimeSpan)`: Sets the lease lock acquisition time, by default the persister waits 60 seconds to obtain a lock. If the lock acquisition fails the message goes through the endpoint configured retry logic.
* `SetPessimisticLeaseLockAcquisitionMaximumRefreshDelay(TimeSpan)`: To prevent jittering the saga persister waits a random number of milliseconds between lease lock acquisition attempts. By default the waiting time is between zero and 20 milliseconds. The upper bound can be configured: the supplied value must be greater than zero and smaller or equal to 1 second.
