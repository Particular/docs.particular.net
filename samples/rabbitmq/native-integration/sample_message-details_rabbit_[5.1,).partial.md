### Access to the native RabbitMQ message details

The transport adds the native RabbitMQ client [`BasicDeliverEventArgs`](https://rabbitmq.github.io/rabbitmq-dotnet-client/api/RabbitMQ.Client.Events.BasicDeliverEventArgs.html) to the message processing context. The sample demonstrates this by setting a custom [`AppId`](https://www.rabbitmq.com/consumers.html#message-properties) that is accessed in the message handler using the following syntax:

snippet: AccessToNativeMessageDetails