### Quorum queues

Starting with RabbitMQ version 3.8.0, [quorum queues](https://www.rabbitmq.com/quorum-queues.html) are an alternative to mirrored queues when data safety is a high priority.

The input queue for an endpoint can be created as a [quorum queue](https://www.rabbitmq.com/quorum-queues.html) using the `QueueMode.Quorum` setting with the `RabbitMQClusterTransport`:

snippet: quorum-queue-config

WARN: An existing queue cannot be automatically converted to a quorum queue. Endpoints are not able to detect a queue-mode configuration mismatch on existing queues and will ignore the configured queue mode when the queue already exists.

WARN: Quorum queues do not support [time-to-be-received](/nservicebus/messaging/discard-old-messages.md). Messages with a time-to-be-received configured won't be discarded if the destination is a quorum queue.

NOTE: Endpoints must be updated to NServiceBus.Transport.RabbitMQ version 6.1 or higher in order to send messages to endpoints using quorum queues or to shared error and audit queues that are configured as quorum queues.

### Delayed delivery

The [delayed delivery infrastructure](/transports/rabbitmq/delayed-delivery.md) provided by the RabbitMQ transport is not supported by quorum queues. The delayed delivery infrastructure continues to use classic queues and [dead letter exchanges](https://www.rabbitmq.com/dlx.html) which can be exposed to message loss in a RabbitMQ cluster. Therefore, it is recommended to disable delayed delivery features (e.g. saga timeouts, delayed retries, and delayed messages) by using the `DelayedDeliverySupport.Disabled` option when configuring the transport:

snippet: disable-delayed-delivery

To enable delayed delivery regardless of the risk of message loss, configure the `DelayedDeliverySupport.UnsafeEnabled` option:

snippet: enable-delayed-delivery
