### Access to the native RabbitMQ message details

It can sometimes be useful to access the native RabbitMQ message from behaviors and handlers. When a message is received, the transport adds the native RabbitMQ client [`BasicDeliverEventArgs`](https://rabbitmq.github.io/rabbitmq-dotnet-client/api/RabbitMQ.Client.Events.BasicDeliverEventArgs.html) to the message processing context. Use the code below to access the message details from a [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md):

snippet: rabbitmq-access-to-event-args