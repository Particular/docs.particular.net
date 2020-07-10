Non-durable messages are sent using RabbitMQ's non-persistent delivery mode, which means the messages are not persisted to disk by the broker.

If durable messaging has been disabled globally, the exchanges and queues created by the broker will be declared as non-durable as well. If the broker is restarted, all non-durable exchanges and queues will be removed along with any messages in those queues.

NOTE: When using non-durable messaging, the RabbitMQ transport disables [publisher confirms](https://www.rabbitmq.com/confirms.html), to improve sending performance at the expense of reliability.
