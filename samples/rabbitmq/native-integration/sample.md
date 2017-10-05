---
title: Native integration with RabbitMQ
summary: Consuming messages published by non-NServiceBus endpoints
reviewed: 2017-10-05
component: Rabbit
related:
- transports/rabbitmq
---


## Code walk-through

The sample consists of two console applications, a simple receiver endpoint and a native sender. These endpoints demonstrate how to have senders on other platforms send messages to NServiceBus endpoints. While the .NET RabbitMQ client is used in this sample, the approach demonstrated can be used with any of the [official](https://www.rabbitmq.com/download.html) or [community](https://www.rabbitmq.com/devtools.html) native RabbitMQ client libraries.


### Putting the message in the correct queue

When integrating native RabbitMQ senders with NServiceBus endpoints, the first thing required is to ensure that the native senders are configured to put the messages in the queue where the endpoint is listening. By default, NServiceBus endpoints will listen on a queue with the same name as the endpoint, so set the endpoint name using:

snippet: ConfigureRabbitQueueName

With this in place, the native sender can place a message in this queue using:

snippet: SendMessage


### Message serialization

In this sample, XML is used to format the payload since NServiceBus is able to automatically detect the message type based on the root node of the XML document. The native sender will send a `MyMessage` XML string as the message payload.

Note: The root node should be the fully qualified type name (including namespace if it has one) of the message.

snippet: CreateNativePayload

The next step is to define a message contract in the receiver that matches the XML-formatted payload. The contract that matches the payload used in this sample looks like this:

snippet: DefineNSBMessage


### Uniquely identifying messages

NServiceBus requires all messages to be uniquely identified in order to be able to perform retries in a safe way. Unfortunately, RabbitMQ doesn't provide a unique ID for messages automatically, so a unique ID will need to be manually generated. By default, NServiceBus will look for this message ID in the optional [AMQP](https://www.rabbitmq.com/amqp-0-9-1-reference.html) `message-id` message header. This behavior can be modified by using a [custom message ID strategy](/transports/rabbitmq/message-id-strategy.md) to tell NServiceBus to look in a different location for the message ID. Using this custom strategy, the ID can be extracted from any message header, or even the message payload itself.

To set this up for this sample, generate a unique ID on the sender side and attach it to the `MessageId` property:

snippet: GenerateUniqueMessageId
