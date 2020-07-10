Non-durable messages are sent using RabbitMQ's non-persistent delivery mode, which means the messages are not persisted to disk by the broker.

NOTE: When using non-durable messaging, [publisher confirms](connection-settings.md?version=rabbit_5#publisher-confirms) can be disabled to improve sending performance at the expense of reliability.
