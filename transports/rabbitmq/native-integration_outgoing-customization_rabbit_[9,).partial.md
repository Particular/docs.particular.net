### Access to the native RabbitMQ message details prior to sending

When integrating with other software systems it might be necessary to customize the native RabbitMQ message immediately before sending it to the broker. This can be done by registering a callback that gets invoked for each message as a last step before handing the message to the RabbitMQ client SDK.

snippet: rabbitmq-customize-outgoing-message