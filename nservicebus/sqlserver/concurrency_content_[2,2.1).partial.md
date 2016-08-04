Each endpoint running SQL Server transport spins up a fixed number of threads (controlled by `MaximumConcurrencyLevel` property of `TransportConfig` section) both for each input queue. Each thread runs in a loop, polling the database for messages awaiting processing.

As a consequence, if `MaximumConcurrencyLevel` is set to 10, there are 40 threads running and constantly polling the database (10 threads for the main queue, 10 for each satellite and another 10 for the callback receiver). 

In 2.0 release support for callbacks has been added. Callbacks are implemented by each endpoint instance having a unique [secondary queue](./#secondary-queues). The receive for the secondary queue does not use the `MaximumConcurrencyLevel` and defaults to 1 thread. This value can be adjusted via the configuration API.


### Prior to version 2.0

Prior to 2.0 each endpoint running SQLServer transport spins up a fixed number of threads (controlled by `MaximumConcurrencyLevel` property of `TransportConfig` section) both for input and satellite queues. Each thread runs in loop, polling the database for messages awaiting processing.

The disadvantage of this simple model is the fact that satellites (e.g. [Delayed Retries](/nservicebus/recoverability/#delayed-retries), Timeout Manager) share the same concurrency settings but usually have much lower throughput requirements. If both Delayed Retries and Timeout Manager are enabled, setting `MaximumConcurrencyLevel` to 10 results in 40 threads in total, each polling the database even if there are no messages to be processed.