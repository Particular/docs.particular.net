Each endpoint running SQL Server transport spins up a fixed number of threads (controlled by `MaximumConcurrencyLevel` property of `TransportConfig` section) for each input queue. Each thread runs in a loop, using the `delete` command to poll the database for messages awaiting processing.

By default, there are 4 input queues created for every endpoint (apart from the main one, there are two for handling timeouts and one for the retries). As a consequence, if `MaximumConcurrencyLevel` is set to 10, there are 40 threads running and constantly polling the database.

Read more information about [tuning endpoint message processing](/nservicebus/operations/tuning.md).