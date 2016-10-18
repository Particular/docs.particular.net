### CPU vs IO bound processing

The following settings are used in order to tune performance:

 - `MaximumConcurrencyLevel`
 - `BatchSize`
 - `LockDuration`
 - `MaxDeliveryCount`

In scenarios where handlers perform CPU intensive and not IO intensive work (such as in memory computations), it is recommended to lower the number of threads to one and increase the `BatchSize`. `LockDuration` and `MaxDeliveryCount` might require an adjustment to match the batch size, taking into account the number of messages that end up in the dead letter queue.

In scenarios where handlers perform IO intensive work, it is recommended to set the number of threads  to 12 threads per logical core using `MaximumConcurrencyLevel` setting, and set the `BatchSize` to a number of messages that takes to process. Take into account the expected (or measured) processing time and IO latency of a single message. Start with a small `BatchSize` and through adjustment and measurement gradually increase it, while adjusting accordingly `LockDuration` and `MaxDeliveryCount`.

NOTE: The magic number 12 comes from the following reasoning. In older versions of the CLR it would default itself to 25 threads per logical core and 12 is about half of that. So 12 threads for receiving and processing and the rest for other background operations like transaction completions, sending, GC, etc... In recent versions of the CLR this logic doesn't hold though as it now dynamically adjusts, but 12 is still a decent starting point.

For more information on those settings, refer to the [Tuning endpoint message processing](/nservicebus/operations/tuning.md), [ASB Batching](/nservicebus/azure-service-bus/batching.md), [ASB Message lock renewal](/nservicebus/azure-service-bus/message-lock-renewal.md) and [ASB Retry behavior](/nservicebus/azure-service-bus/retries.md) articles. 