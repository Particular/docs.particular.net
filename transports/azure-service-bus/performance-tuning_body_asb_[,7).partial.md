### CPU vs IO-bound processing

The following settings are used in order to tune performance:

 - `MaximumConcurrencyLevel`
 - `BatchSize`
 - `LockDuration`
 - `MaxDeliveryCount`

In scenarios where handlers perform CPU intensive and not IO intensive work (such as in-memory computations), it is recommended to lower the number of threads to one and increase the `BatchSize`. `LockDuration` and `MaxDeliveryCount` might require an adjustment to match the batch size, taking into account the number of messages that end up in the dead letter queue.

In scenarios where handlers perform IO intensive work, it is recommended to set the number of threads to 12 threads per logical core using `MaximumConcurrencyLevel` setting and set the `BatchSize` to a number of messages that takes to process. Take into account the expected (or measured) processing time and IO latency of a single message. Start with a small `BatchSize` and through adjustment and measurement gradually increase it, while adjusting accordingly `LockDuration` and `MaxDeliveryCount`.

NOTE: The number 12 is a good starting point, but might need to be changed in particular systems. It was determined as an optimal value in a series of experiments. The older versions of CLR by default used 25 threads per logical core. When 12 threads are assigned for receiving and processing messages, the other half is dealing with other background operations like transaction completions, sending, GC, etc. In recent versions of the CLR, the number of threads is determined dynamically, so it's necessary to verify if 12 will be still an optimal value empirically.

For more information on those settings, refer to the [Tuning endpoint message processing](/nservicebus/operations/tuning.md), [ASB Batching](/transports/azure-service-bus/batching.md), [ASB Message lock renewal](/transports/azure-service-bus/message-lock-renewal.md) and [ASB Retry behavior](/transports/azure-service-bus/retries.md) articles. 