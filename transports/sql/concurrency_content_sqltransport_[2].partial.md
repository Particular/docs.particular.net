## Versions 2.0.x

Each endpoint running SQL Server transport spins up a fixed number of threads (controlled by `MaximumConcurrencyLevel` property of `TransportConfig` section) for each input queue. Each thread runs in a loop, polling the database for messages awaiting processing.

By default, there are 5 input queues created for every endpoint (apart from the main one, there are two for handling timeouts, one for the retries and another one for callbacks). As a consequence, if `MaximumConcurrencyLevel` is set to 10, there are 41 threads running and constantly polling the database. The callback queue has a separate concurrency settings which default to 1 polling thread.


## Versions 2.1 and above

SQL Server transport uses an adaptive concurrency model. The transport adapts the number of polling threads based on the rate of messages coming in. A separate instance of the algorithm is executed by each polling thread. The algorithm counts consecutive successful and failed poll attempts (the attempt succeeds if it finds a message waiting in a queue).

If the number of consecutive successful polls is greater than an internal threshold, a new polling thread is started (provided the `MaximumConcurrencyLevel` is not exceeded). On the other hand, if the number of consecutive failed polls is greater than a threshold, the thread dies.

Read more information about [tuning endpoint message processing](/nservicebus/operations/tuning.md?version=core_5).