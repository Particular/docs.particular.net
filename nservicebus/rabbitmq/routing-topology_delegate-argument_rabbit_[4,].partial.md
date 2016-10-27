snippet: rabbitmq-config-useroutingtopologyDelegate

The boolean argument supplied to the factory delegate indicates whether the custom routing topology should create durable exchanges and queues on the broker. Read more about durable exchanges and queues in the [AMQP Concepts Guide](https://www.rabbitmq.com/tutorials/amqp-concepts.html).

The type based overload of `UseRoutingTopology` is still supported in Versions 4.x but it is marked as `[Obsolete]` with a warning.
