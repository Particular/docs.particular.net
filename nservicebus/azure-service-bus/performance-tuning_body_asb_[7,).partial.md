
All transaction modes are capable or processing at least 500 small (less then 1KB) messages per second. We recognize that for certain type of customers this is not enough.

If throughput is less then ~500 msg/s then this is probably due to network constraints. It is likely that the available bandwidth isn't sufficient. Benchmark the available bandwidth by using an (online) tool like [speedtest.net](http://www.speedtest.net/). Select a location that is near to the location of the Azure data center that hosts the Azure Service Bus namespace.

Try to move to another Azure data center if the latency is very high and/or the bandwidth is too low and/or upgrade the internet uplink if that is the bottleneck.

If the above is sufficient then the following guidance can help to optimize performance based on either latency and/or bandwidth.

## Tuning send performance outside the scope of a handler

The following ASB SDK settings impact sending operations outside of the handler context

  * `MessagingFactories().BatchFlushInterval()`: Controls the time that the ASB SDK batches client side requests before sending them out. Optimal values are between 100 and 500 milliseconds. To turn this feature off, provide TimeSpan.Zero.
  * `MessagingFactories().NumberOfMessagingFactoriesPerNamespace()`: Each factory manages a TCP connection to a front end node in the broker, this connection has throughput limits. Opening more factories significantly improves send performance. Optimal values are between 2 and 8 for sending.
  * `NumberOfClientsPerEntity()`: Keep this value equal to the number of factories, this ensures there is 1 send client per factory.

The way the send API is invoked has significant impact on performance. Using await on each Send() operation will make the ASB SDK wait until the send operation completes and effectively will prevent batching the sends. 

Snippet: asb-slow-send

Instead, it's better to maintain a list of send tasks and await them all:

Snippet: asb-fast-send

## Tuning receive performance

The receive performance can be tweaked in all applications by adjusting the following settings:

  * `MessagingFactories().NumberOfMessagingFactoriesPerNamespace()`: Each factory manages a TCP connection to the broker, this connection has throughput limits. The number of factories depends on how much bandwidth Azure Service Bus is allowed to consume and the size of the messages. Monitor the networks received and sent bytes per second. If the network isn't congested increase the number of factories and clients. The optimal value range is probably somewhere between 16 and a multiple of it, like 64 on partitioned queues (as they consist of 16 partitions).
  * `NumberOfClientsPerEntity()`: Keep this value equal to the number of factories, this ensures there is 1 internal receive client per factory.

Changing the following settings will also improve application performance, but their values depend on the nature of the application:

  * `LimitMessageProcessingConcurrencyTo()`: This is the global concurrency limit across all receive entities. The value should be determined according to the formula: `desired concurrency per receiver` x `number of receivers`.
  * `MessageReceivers().PrefetchCount()`: Value is per receiver, in general the following rule applies: The lower the throughput rate of an individual receiver, the lower this value should be.
  * `Transactions()`: The most significant performance impact comes from the transactional guarantees needed, `SendsAtomicWithReceive` will offer less performance than `ReceiveOnly`.

### Sends Atomic With Receive

In order to guarantee that sends occur as an atomic operation with the receive confirmation, the ASB SDK requires that both operations are enclosed in a `Serializable` transaction. This implies that:

  * All atomic operations are executed in a 'Serial' fashion, i.e. 1 by 1, therefore it does not make sense to allow many concurrent operations per client, nor does it make much sense to prefetch a lot of messages.
	- Concurrency: Optimal values are 2, or maybe 4, per client. Allowing for 1 receive to occur while 1 complete is executing, potentially adding a bit more to cover time in between.
	- PrefetchCount: Optimal value is 20. This value is derived from the official [MSDN documentation](https://azure.microsoft.com/en-us/documentation/articles/service-bus-performance-improvements/), using only 1 receiver per factory and 1 operation at a time. 
  * Setting up the transactional connection takes up quite some time. If the application requires a large number of messages per second than the endpoint throughput will not be optimal just after starting the endpoint. Throughput slowly ramps up and after a few minutes the throughput should reach its maximum based on the configured settings and available network bandwidth and should remain stable from there on.
  * All send operations that occur in the scope of a single receive (inside a handler), must fit into the transaction. The ASB SDK enforces a limit of 100 messages per transaction. Therefore a maximum of 100 messages can be sent this way. Fortunately, that restriction comes with no performance penalty, as the only difference between sending 1 and 100 messages is the size of the operation.

NOTE: When send operations occur inside a handler the default behavior is to add the operation to a batch, which will be executed after the handler function returns. Unless the send options request for immediate dispatch, then execution is immediate. Using an `await` on the Send operation therefore has a negative impact when used on an immediate dispatch, but not on the default. It is advised to return the task instead of awaiting the task to never be impacted by this difference in behavior. To learn more about batched dispatch, refer to [the following document](/nservicebus/messaging/batched-dispatch.md)

### Receive only

This mode has fewer constraints and will work better with high numbers for concurrency and prefetch count, which should result in a pretty high throughput (likely over 1000/s even for remote connections)

  * Concurrency: Optimal values are in the range of 128 per client.
  * PrefetchCount: Set this to 1x or 2x the per receiver concurrency, so 128 or 256.

The optimal numbers for Concurrency and PrefetchCount for receivers are impacted significantly by sends. If the average handler sends roughly the same compound size of messages that it receives, then the numbers recommended above will be optimal. However, if the handler sends more messages or sends larger messages (e.g. in the range of 10 to 100 times the total data size per single receive), then concurrency and prefetchcount should have lower values to reduce incoming bandwidth and give more bandwidth to the senders.

  * Concurrency: Optimal values in the range of 16 per client in this scenario.
  * PrefetchCount: Set this to 1x or 2x the per receiver concurrency, so 16 or 32.

## Performance Tuning Samples

Check out the [performance tuning samples](/samples/azure/performance-tuning-asb/) to experiment with settings in the described scenarios.

NOTE: Standard namespaces have no guaranteed throughput performance, any numbers measured are recordings at that specific moment and may vary over time depending on activity of others on the same infrstructure. If throughput guarantees are required it is highly advised to by Premium tier namespaces.