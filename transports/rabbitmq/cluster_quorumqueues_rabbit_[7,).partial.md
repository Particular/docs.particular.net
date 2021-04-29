### Quorum queues

The input queue for an endpoint can be created as a [Quorum queue](https://www.rabbitmq.com/quorum-queues.html).

There are some important caveats when using Quorum queues:

1. An existing queue cannot be automatically converted to a quorum queue.
2. Expired TTBR messages are not discarded and will be received by the quorum queue handler.
3. Quorum queues do not support delayed delivery.

WARN: Using delayed delivery with Quorum queues could result in message loss.