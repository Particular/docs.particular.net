## Before scaling out

In NServiceBus 5 and below the default concurrency level is set to 1. That means the messages are processed sequentially. 

Before scaling out via the distributor make sure that the [**MaximumConcurrencyLevel** has been increased in the configuration](/nservicebus/operations/tuning.md#tuning-concurrency) on the distributor endpoint. 

Increasing the concurrency on the workers might not lead to increased performance if the executed code is multi-threaded, e.g. if the worker does CPU-intensive work using all the available cores such as video encoding.