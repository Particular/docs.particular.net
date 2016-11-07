The default behavior of sending messages via RabbitMQ is to wait for the publisher confirmation before proceeding. When non-durable messaging is used, the RabbitMQ transport skips the publisher confirmation step.

This effectively means the blocking call to [WaitForConfirmsOrDie](http://www.rabbitmq.com/javadoc/com/rabbitmq/client/Channel.html#waitForConfirmsOrDie%28long%29) is skipped.