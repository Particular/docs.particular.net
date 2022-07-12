The Azure ServiceBus transport is capable of processing hundreds of messages per second for all supported transactional modes. If the system requires processing thousands of messages per second, then follow the guidance in this article to optimize performance based on latency and bandwidth.


## Tuning send performance outside the scope of a handler

The following ASB SDK settings impact sending operations outside of the handler context

  * `MessagingFactories().BatchFlushInterval()`: Controls the time that the ASB SDK batches client side requests before sending them out. Optimal values are between 100 and 500 milliseconds. To turn this feature off, provide TimeSpan.Zero.
  * `MessagingFactories().NumberOfMessagingFactoriesPerNamespace()`: Each factory manages a TCP connection to a front-end node in the broker, this connection has throughput limits. Opening more factories can significantly improve send performance. Optimal values are between 2 and 8 for sending.
  * `NumberOfClientsPerEntity()`: Keep this value equal to the number of factories, this ensures there is 1 send client per factory.

The way the send API is invoked has a significant impact on performance. Using `await` for each `Send()` operation will make the ASB SDK wait until the send operation completes and, effectively, it will prevent batching:

Snippet: asb-slow-send

Instead, it's better to maintain a list of send tasks and await the whole group, which allows for batching:

Snippet: asb-fast-send


## Tuning receive performance

The receive performance can be tweaked in all applications by adjusting the following settings:

  * `MessagingFactories().NumberOfMessagingFactoriesPerNamespace()`: Each factory manages a TCP connection to the broker, this connection has throughput limits. The number of factories required depends on how much bandwidth Azure Service Bus is allowed to consume and the size of the messages. Monitor the networks received and sent bytes per second. If the network isn't congested, try to increase the number of factories and clients. The optimal value range is probably somewhere between 16 and a multiple of it, like 64 on partitioned queues (as they consist of 16 partitions).
  * `NumberOfClientsPerEntity()`: Keep this value equal to the number of factories, this ensures there is one internal receive client per factory.

Changing the following settings will also improve application performance; however, their values should depend on the nature of the application:

  * `LimitMessageProcessingConcurrencyTo()`: This is the global concurrency limit across all receive entities. The value should be determined according to the formula: `desired concurrency per receiver` x `number of receivers`. A receiver in the context of the transport is a message pump which takes care of one or more entities (queues and subscriptions) to receive from. Each message pump has its concurrency limitation. The global concurrency limit will be shared across all pumps that are simultaneously receiving messages.
  * `MessageReceivers().PrefetchCount()`: Value is set per receiver. The higher throughput rate for an individual receiver, the more aggressive prefetching should be applied (i.e. the higher value for `PrefetchCount`).
  * `Transactions()`: The transactional guarantees have a significant impact on performance. For example, the application using `ReceiveOnly` will have much better performance than an application using SendsAtomicWithReceive`.

NOTE: If an endpoint is a sender and a receiver the setting `MessagingFactories().NumberOfMessagingFactoriesPerNamespace()` needs to be tweaked accordingly. In general it is recommended to optimize for receive performance since these settings might also improve the send performance.

### Sends Atomic With Receive

To guarantee that sends occur as an atomic operation with the receive confirmation, the ASB SDK requires that both operations are enclosed in a `Serializable` transaction. This implies that:

  * All atomic operations are executed in a "Serial" fashion, i.e. a single operation at a time. Therefore it does not make sense to allow many concurrent operations per client, nor does it make much sense to prefetch a lot of messages, as that will not improve performance.
    * Concurrency: The optimal values are low, e.g. 2 or 4 concurrency limit per client. That will allow one receive operation to occur while another one is completing, potentially adding a few more threads to cover time in between.
    * PrefetchCount: Optimal value is 20. This value is derived from the official [MSDN documentation](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-performance-improvements) when using only one receiver per factory and executing one operation at a time. 
  * Setting up the transactional connection is an expensive operation. Therefore if the application has a high messages load, the endpoint throughput will not be optimal just after starting it. The throughput gradually improves over time, after a few minutes from the start it should reach its maximum based on the configured settings and available network bandwidth.
  * All send operations that occur in the scope of a single receive (inside a handler) must fit into the transaction. The ASB SDK enforces a limit of 100 messages per transaction. Therefore a maximum of 100 messages can be sent this way at a time. Fortunately, that restriction comes with no performance penalty, as the only difference between sending 1 and 100 messages is the size of the operation.

NOTE: When send operations occur inside a handler the default behavior is to add the operation to a batch, which will be executed after the handler function returns. Unless the send options request for immediate dispatch, in such a case the execution is immediate. Therefore using an `await` on the `Send()` operation has a negative impact when used on an immediate dispatch, but has no impact on the default settings. It is advised to always return the task instead of awaiting the task never to be impacted by this difference in behavior, which might lead to subtle issues over time. To learn more about batched dispatch refer to the [Batched message dispatch](/nservicebus/messaging/batched-dispatch.md) article.


### Receive only

`ReceiveOnly` mode has fewer constraints than `SendsAtomicWithReceive`. Therefore it will offer better performance when numbers for concurrency and prefetch count are increased (in the range of thousands of messages per second, even for remote connections).

  * Concurrency: The optimal values are in the range of 128 per client.
  * PrefetchCount: The optimal values are 1x or 2x the per receiver concurrency, so 128 or 256.

The optimal numbers for Concurrency and `PrefetchCount` for receivers are impacted significantly by sends. If the average handler sends roughly the same compound size of messages that it receives, then the numbers recommended above will be optimal. 

However, if the handler sends more messages or sends larger messages (e.g. in the range of 10 to 100 times the total data size per single receive), then Concurrency and `PrefetchCount` values should be lower to reduce incoming bandwidth and give more bandwidth to the senders. In such scenario:

  * Concurrency: The optimal values are the range of 16 per client.
  * PrefetchCount: The optimal values are 1x or 2x the per receiver concurrency, so 16 or 32.


## Notes on bandwidth and other constraints

If throughput is less then ~500 msg/s, even after optimizations described in this article have been applied, then most likely it's caused by network constraints. It is likely that the available local bandwidth isn't sufficient. In such a case one should benchmark the available bandwidth by using a tool like [speedtest.net](https://www.speedtest.net/). The selected location should be as close as possible to the location of the Azure data center that hosts the Azure Service Bus namespace.

If the latency is very high or the bandwidth is too low, then try to move Azure Service Bus namespace to another data center. Another solution might be updating the internet uplink, which also might be a bottleneck.

Note that standard namespaces don't offer any guarantees when it comes to throughput performance. Any numbers measured are true at the specific moment and may vary over time depending on the activity of others on the same infrastructure. If throughput guarantees are required, then it is highly advised to consider using Premium tier namespaces.