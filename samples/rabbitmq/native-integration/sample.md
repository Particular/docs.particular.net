---
title: RabbitMQ transport native integration sample
summary: Consuming messages published by non-NServiceBus endpoints
reviewed: 2025-03-25
component: Rabbit
isLearningPath: true
related:
- transports/rabbitmq
---


## Code walk-through

The sample consists of two console applications: a simple receiver endpoint and a native sender. These endpoints demonstrate how to have senders on other platforms send messages to NServiceBus endpoints. While the .NET RabbitMQ client is used in this sample, the approach demonstrated can be used with any of the [official](https://www.rabbitmq.com/download.html) or [community](https://www.rabbitmq.com/devtools.html) native RabbitMQ client libraries.

### Putting the message in the correct queue

When integrating native RabbitMQ senders with NServiceBus endpoints, the first thing required is to ensure that the native senders are configured to put the messages in the queue where the endpoint is listening. By default, NServiceBus endpoints will listen on a queue with the same name as the endpoint, so set the endpoint name using:

snippet: ConfigureRabbitQueueName

With this in place, the native sender can send a message to this endpoint using:

snippet: SendMessage

### Message serialization

In this sample, XML is used to format the payload since NServiceBus is able to automatically detect the message type based on the root node of the XML document. The native sender will send a `MyMessage` XML string as the message payload.

> [!NOTE]
> The root node should be the fully qualified type name (including namespace if it has one) of the message.

snippet: CreateNativePayload

The next step is to define a message contract in the receiver that matches the XML-formatted payload. The contract that matches the payload used in this sample looks like this:

snippet: DefineNSBMessage

See [the message type detection documentation](/nservicebus/messaging/message-type-detection.md) for more details.

### Uniquely identifying messages

NServiceBus requires all messages to be uniquely identified in order to be able to perform retries in a safe way. RabbitMQ doesn't provide a unique ID for messages automatically, so a unique ID has to be generated manually. By default, NServiceBus will look for this message ID in the optional [AMQP](https://www.rabbitmq.com/amqp-0-9-1-reference.html) `message-id` message header. This behavior can be modified by using a [custom message ID strategy](/transports/rabbitmq/native-integration.md#custom-message-id-strategy) to tell NServiceBus to look in a different location for the message ID. Using a custom strategy, the ID can be extracted from any message header, or even the message payload itself.

In this sample, a unique ID is generated on the sender side and assigned to the `MessageId` property:

snippet: GenerateUniqueMessageId

### Access to the native RabbitMQ message details

The transport adds the native RabbitMQ client [`BasicDeliverEventArgs`](https://rabbitmq.github.io/rabbitmq-dotnet-client/api/RabbitMQ.Client.Events.BasicDeliverEventArgs.html) to the message processing context. The sample demonstrates this by setting a custom [`AppId`](https://www.rabbitmq.com/consumers.html#message-properties) that is accessed in the message handler using the following syntax:

snippet: AccessToNativeMessageDetails
