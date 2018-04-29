## Disable callbacks

Callbacks and callback queues receivers are enabled by default. In order to disable them use the following setting:

snippet: sqlserver-config-disable-secondaries

Secondary queues use the same adaptive concurrency model as the primary queue. Secondary queues (and hence callbacks) are disabled for satellite receivers.


## Callback receiver max concurrency

Changes the number of threads used for the callback receiver. The default is 1 thread.

snippet: sqlserver-CallbackReceiverMaxConcurrency