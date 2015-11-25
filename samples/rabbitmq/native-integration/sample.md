---
title: Native integration with RabbitMQ
summary: Shows how to consume messages published by non NServiceBus endpoints
tags:
- RabbitMQ
related:
- nservicebus/rabbitmq
---


## Code walk-through

The sample consists of 2 console apps. A simple NServiceBus receiver and a native sender demonstrating how to have senders on other platforms send messages to your NServiceBus endpoints using the native RabbitMQ Clients available. This could be java clients, Node.js etc.


### Putting the message in the correct queue

When integrating native RabbitMQ sender with your NServiceBus endpoints the first thing you need to do is to make sure the native senders are configured to put the messages in the queue where your endpoint is listening. By default NServiceBus endpoints will listen on a queue with the same name as the endpoint so we just set the endpoint name using:

snippet:ConfigureRabbitQueueName

With this in place we can just have the native sender put a message in this queue using:

snippet:SendMessage


### Message serialization

In this sample we use XML to format our payload since NServiceBus is able to automatically detect the message type based on the root node of the xml. Our native sender will send a `MyMessage` xml string as the message payload. 

Note: The root node is the fully qualified type name (including namespace if it has one) of the message.

snippet:CreateNativePayload

The next step is to define a message contract in our receiver that matches this format. This contract looks like this (Notice the `SomeProperty`)

snippet:DefineNSBMessage


### Uniquely identifying messages

NServiceBus requires all messages to be uniquely identified in order to be able to perform retries in a safe way. Unfortunately RabbitMQ doesn't provide a unique id for messages by default so we need to provide that. By default NServiceBus will look for the message id in the optional AMQP `MessageId` property but you can use your own custom strategy by calling `.CustomMessageIdStrategy` and extract the id from any property, header or event the message payload. 

To set this up all we have to do is to generate a unique id on the sender side and attach it the `MessageId` property.

snippet:GenerateUniqueMessageId
